using Godot;
using System;
using System.Collections.Generic;
using OCSM;

public class ContractsList : Container
{
	[Signal]
	public delegate void ValueChanged(List<Contract> values);
	
	public List<Contract> Values { get; set; } = new List<Contract>();
	
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
			if(v is Contract)
				addInput(v);
		}
		
		addInput();
	}
	
	private void skillChanged(int index) { updateValues(); }
	private void valueChanged(string text) { updateValues(); }
	
	private void updateValues()
	{
		var values = new List<Contract>();
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
	
	private void addInput(Contract value = null)
	{
		var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.CoD.Changeling.Contract);
		var instance = resource.Instance<VBoxContainer>();
		if(value is Contract)
		{
			/*
			instance.GetChild<SkillOptionButton>(0).Selected = OCSM.Skill.asList().FindIndex(s => s.Equals(value)) + 1;
			instance.GetChild<LineEdit>(1).Text = value.Value;
			*/
		}
		
		AddChild(instance);
		instance.GetChild<SkillOptionButton>(0).Connect(Constants.Signal.ItemSelected, this, nameof(skillChanged));
		instance.GetChild<LineEdit>(1).Connect(Constants.Signal.TextChanged, this, nameof(valueChanged));
	}
}
