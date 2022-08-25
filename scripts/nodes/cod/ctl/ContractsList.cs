using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using OCSM;

public class ContractsList : Container
{
	[Signal]
	public delegate void ValueChanged(List<Contract> values);
	
	public List<OCSM.Contract> Values { get; set; } = new List<OCSM.Contract>();
	
	public override void _Ready()
	{
		refresh();
	}
	
	public void refresh()
	{
		foreach(Node c in GetChildren())
		{
			c.QueueFree();
		}
		
		foreach(var v in Values)
		{
			if(v is OCSM.Contract)
				addInput(v);
		}
		
		addInput();
	}
	
	private void skillChanged(int index) { updateValues(); }
	private void valueChanged(string text) { updateValues(); }
	
	private void updateValues()
	{
		var values = new List<OCSM.Contract>();
		var children = GetChildren();
		foreach(HBoxContainer row in children)
		{
			var optButton = row.GetChild<OptionButton>(0);
			var skill = Skill.byName(optButton.GetItemText(optButton.Selected));
			var value = row.GetChild<LineEdit>(1).Text;
			
			if(!String.IsNullOrEmpty(value))
			{
				//values.Add(new Skill.Specialty(skill, value));
			}
			else if(children.IndexOf(row) != children.Count - 1)
				row.QueueFree();
		}
		
		EmitSignal(nameof(ValueChanged), values);
		
		if(children.Count <= values.Count)
		{
			addInput();
		}
	}
	
	private void addInput(OCSM.Contract value = null)
	{
		var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.CoD.Changeling.Contract);
		var instance = resource.Instance<Contract>();
		if(value is OCSM.Contract)
		{
			if(!String.IsNullOrEmpty(value.Name))
				instance.GetNode<LineEdit>(Contract.NameName).Text = value.Name;
			if(value.Action > -1)
				instance.GetNode<OptionButton>(Contract.Action).Selected = value.Action;
			if(!String.IsNullOrEmpty(value.Description))
				instance.GetNode<TextEdit>(Contract.Description).Text = value.Description;
			if(!String.IsNullOrEmpty(value.Effects))
				instance.GetNode<TextEdit>(Contract.Effects).Text = value.Effects;
			if(value.Attribute is OCSM.Attribute)
				instance.GetNode<AttributeOptionButton>(Contract.Attribute).Selected = OCSM.Attribute.asList().FindIndex(a => a.Name.Equals(value.Attribute.Name)) + 1;
			if(value.AttributeResisted is OCSM.Attribute)
				instance.GetNode<AttributeOptionButton>(Contract.Attribute2).Selected = OCSM.Attribute.asList().FindIndex(a => a.Name.Equals(value.AttributeResisted.Name)) + 1;
			if(value.AttributeContested is OCSM.Attribute)
				instance.GetNode<AttributeOptionButton>(Contract.Attribute3).Selected = OCSM.Attribute.asList().FindIndex(a => a.Name.Equals(value.AttributeContested.Name)) + 1;
			if(value.Skill is Skill)
				instance.GetNode<SkillOptionButton>(Contract.Skill).Selected = Skill.asList().IndexOf(value.Skill) + 1;
			if(value.SeemingBenefits is Dictionary<string, string>)
				instance.SeemingBenefits = value.SeemingBenefits;
			if(!String.IsNullOrEmpty(value.RollFailure))
				instance.GetNode<TextEdit>(Contract.Failure).Text = value.RollFailure;
			if(!String.IsNullOrEmpty(value.RollFailureExceptional))
				instance.GetNode<TextEdit>(Contract.FailureExceptional).Text = value.RollFailureExceptional;
			if(!String.IsNullOrEmpty(value.RollSuccess))
				instance.GetNode<TextEdit>(Contract.Success).Text = value.RollSuccess;
			if(!String.IsNullOrEmpty(value.RollSuccessExceptional))
				instance.GetNode<TextEdit>(Contract.SuccessExceptional).Text = value.RollSuccessExceptional;
			if(!String.IsNullOrEmpty(value.Loophole))
				instance.GetNode<TextEdit>(Contract.Loophole).Text = value.Loophole;
		}
		
		AddChild(instance);
		//Wire up
	}
}
