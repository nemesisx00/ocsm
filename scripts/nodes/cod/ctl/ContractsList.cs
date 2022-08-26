using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using OCSM;

public class ContractsList : Container
{
	[Signal]
	public delegate void ValueChanged(SignalPayload<List<OCSM.Contract>> values);
	
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
	
	private void updateValues()
	{
		var values = new List<OCSM.Contract>();
		var children = GetChildren();
		foreach(Contract contract in children)
		{
			var attr1Node = contract.GetNode<AttributeOptionButton>(PathBuilder.SceneUnique(Contract.Attribute));
			var attr2Node = contract.GetNode<AttributeOptionButton>(PathBuilder.SceneUnique(Contract.Attribute2));
			var attr3Node = contract.GetNode<AttributeOptionButton>(PathBuilder.SceneUnique(Contract.Attribute3));
			var skillNode = contract.GetNode<SkillOptionButton>(PathBuilder.SceneUnique(Contract.Skill));
			var regaliaNode = contract.GetNode<RegaliaOptionButton>(PathBuilder.SceneUnique(Contract.Regalia));
			var contractTypeNode = contract.GetNode<OptionButton>(PathBuilder.SceneUnique(Contract.ContractType));
			
			var name = contract.GetNode<LineEdit>(PathBuilder.SceneUnique(Contract.NameName)).Text;
			var action = contract.GetNode<OptionButton>(PathBuilder.SceneUnique(Contract.Action)).Selected;
			var cost = contract.GetNode<LineEdit>(PathBuilder.SceneUnique(Contract.Cost)).Text;
			var description = contract.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.Description)).Text;
			var duration = contract.GetNode<LineEdit>(PathBuilder.SceneUnique(Contract.Duration)).Text;
			var effects = contract.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.Effects)).Text;
			var seemingBenefits = contract.SeemingBenefits;
			var failure = contract.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.Failure)).Text;
			var FailureDramatic = contract.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.FailureDramatic)).Text;
			var success = contract.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.Success)).Text;
			var successExceptional = contract.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.SuccessExceptional)).Text;
			var loophole = contract.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.Loophole)).Text;
			
			var attribute = OCSM.Attribute.byName(attr1Node.GetItemText(attr1Node.Selected));
			var attributeResisted = OCSM.Attribute.byName(attr2Node.GetItemText(attr2Node.Selected));
			var attributeContested = OCSM.Attribute.byName(attr3Node.GetItemText(attr3Node.Selected));
			var skill = OCSM.Skill.byName(skillNode.GetItemText(skillNode.Selected));
			var regalia = regaliaNode.GetItemText(regaliaNode.Selected);
			var contractType = contractTypeNode.GetItemText(contractTypeNode.Selected);
			
			var shouldRemove = String.IsNullOrEmpty(name) && action < 1 && String.IsNullOrEmpty(description)
				&& String.IsNullOrEmpty(effects) && !(attribute is OCSM.Attribute) && !(attributeResisted is OCSM.Attribute) && !(attributeContested is OCSM.Attribute)
				&& seemingBenefits.Count < 1 && String.IsNullOrEmpty(failure) && String.IsNullOrEmpty(FailureDramatic)
				&& !OCSM.Regalia.asList().Contains(regalia) && !OCSM.ContractType.asList().Contains(contractType)
				&& String.IsNullOrEmpty(success) && String.IsNullOrEmpty(successExceptional) && String.IsNullOrEmpty(loophole);
			
			if(!shouldRemove)
			{
				values.Add(new OCSM.Contract()
				{
					Action = action,
					Attribute = attribute,
					AttributeResisted = attributeResisted,
					AttributeContested = attributeContested,
					Cost = cost,
					Description = description,
					Duration = duration,
					Effects = effects,
					Loophole = loophole,
					Name = name,
					Regalia = regalia,
					ContractType = contractType,
					RollFailure = failure,
					RollFailureDramatic = FailureDramatic,
					RollSuccess = success,
					RollSuccessExceptional = successExceptional,
					SeemingBenefits = seemingBenefits,
					Skill = skill,
				});
			}
			else if(children.IndexOf(contract) != children.Count - 1)
				contract.QueueFree();
		}
		
		Values = values;
		EmitSignal(nameof(ValueChanged), new SignalPayload<List<OCSM.Contract>>() { Payload = values });
		
		if(GetChildren().Count <= Values.Count)
		{
			addInput();
		}
	}
	
	private void addInput(OCSM.Contract value = null)
	{
		var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.CoD.Changeling.Contract);
		var instance = resource.Instance<Contract>();
		
		AddChild(instance);
		
		if(value is OCSM.Contract)
		{
			if(value.Action > -1)
				instance.GetNode<OptionButton>(PathBuilder.SceneUnique(Contract.Action)).Selected = value.Action;
			if(value.Attribute is OCSM.Attribute)
				instance.GetNode<AttributeOptionButton>(PathBuilder.SceneUnique(Contract.Attribute)).Selected = OCSM.Attribute.asList().FindIndex(a => a.Name.Equals(value.Attribute.Name)) + 1;
			if(value.AttributeResisted is OCSM.Attribute)
				instance.GetNode<AttributeOptionButton>(PathBuilder.SceneUnique(Contract.Attribute2)).Selected = OCSM.Attribute.asList().FindIndex(a => a.Name.Equals(value.AttributeResisted.Name)) + 1;
			if(value.AttributeContested is OCSM.Attribute)
				instance.GetNode<AttributeOptionButton>(PathBuilder.SceneUnique(Contract.Attribute3)).Selected = OCSM.Attribute.asList().FindIndex(a => a.Name.Equals(value.AttributeContested.Name)) + 1;
			if(!String.IsNullOrEmpty(value.Cost))
				instance.GetNode<LineEdit>(PathBuilder.SceneUnique(Contract.Cost)).Text = value.Cost;
			if(!String.IsNullOrEmpty(value.Description))
				instance.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.Description)).Text = value.Description;
			if(!String.IsNullOrEmpty(value.Duration))
				instance.GetNode<LineEdit>(PathBuilder.SceneUnique(Contract.Duration)).Text = value.Duration;
			if(!String.IsNullOrEmpty(value.Effects))
				instance.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.Effects)).Text = value.Effects;
			if(!String.IsNullOrEmpty(value.Loophole))
				instance.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.Loophole)).Text = value.Loophole;
			if(!String.IsNullOrEmpty(value.Name))
				instance.GetNode<LineEdit>(PathBuilder.SceneUnique(Contract.NameName)).Text = value.Name;
			if(value.SeemingBenefits is Dictionary<string, string>)
			{
				instance.SeemingBenefits = value.SeemingBenefits;
				instance.refreshSeemingBenefits();
			}
			if(value.Skill is Skill)
				instance.GetNode<SkillOptionButton>(PathBuilder.SceneUnique(Contract.Skill)).Selected = Skill.asList().IndexOf(value.Skill) + 1;
			if(!String.IsNullOrEmpty(value.Regalia))
				instance.GetNode<RegaliaOptionButton>(PathBuilder.SceneUnique(Contract.Regalia)).Selected = Regalia.asList().FindIndex(r => r.Equals(value.Regalia)) + 1;
			if(!String.IsNullOrEmpty(value.ContractType))
				instance.GetNode<OptionButton>(PathBuilder.SceneUnique(Contract.ContractType)).Selected = ContractType.asList().FindIndex(r => r.Equals(value.ContractType)) + 1;
			if(!String.IsNullOrEmpty(value.RollFailure))
				instance.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.Failure)).Text = value.RollFailure;
			if(!String.IsNullOrEmpty(value.RollFailureDramatic))
				instance.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.FailureDramatic)).Text = value.RollFailureDramatic;
			if(!String.IsNullOrEmpty(value.RollSuccess))
				instance.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.Success)).Text = value.RollSuccess;
			if(!String.IsNullOrEmpty(value.RollSuccessExceptional))
				instance.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.SuccessExceptional)).Text = value.RollSuccessExceptional;
		}
		
		instance.GetNode<OptionButton>(PathBuilder.SceneUnique(Contract.Action)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
		instance.GetNode<AttributeOptionButton>(PathBuilder.SceneUnique(Contract.Attribute)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
		instance.GetNode<AttributeOptionButton>(PathBuilder.SceneUnique(Contract.Attribute2)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
		instance.GetNode<AttributeOptionButton>(PathBuilder.SceneUnique(Contract.Attribute3)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
		instance.GetNode<SkillOptionButton>(PathBuilder.SceneUnique(Contract.Skill)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
		instance.GetNode<RegaliaOptionButton>(PathBuilder.SceneUnique(Contract.Regalia)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
		instance.GetNode<OptionButton>(PathBuilder.SceneUnique(Contract.ContractType)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
		instance.GetNode<LineEdit>(PathBuilder.SceneUnique(Contract.Cost)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		instance.GetNode<LineEdit>(PathBuilder.SceneUnique(Contract.Duration)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		instance.GetNode<LineEdit>(PathBuilder.SceneUnique(Contract.NameName)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		instance.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.Description)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		instance.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.Effects)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		instance.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.Loophole)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		instance.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.Failure)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		instance.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.FailureDramatic)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		instance.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.Success)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		instance.GetNode<TextEdit>(PathBuilder.SceneUnique(Contract.SuccessExceptional)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
	}
	
	private void optionSelected(int index) { updateValues(); }
	private void textChanged(string newText) { textChanged(); }
	private void textChanged() { updateValues(); }
}
