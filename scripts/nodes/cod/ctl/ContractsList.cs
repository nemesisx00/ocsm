using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using OCSM;
using OCSM.CoD;
using OCSM.CoD.CtL;
using OCSM.CoD.CtL.Meta;

public class ContractsList : Container
{
	[Signal]
	public delegate void ValueChanged(SignalPayload<List<OCSM.CoD.CtL.Contract>> values);
	
	public List<OCSM.CoD.CtL.Contract> Values { get; set; } = new List<OCSM.CoD.CtL.Contract>();
	
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		
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
			if(v is OCSM.CoD.CtL.Contract)
				addInput(v);
		}
		
		addInput();
	}
	
	private void updateValues()
	{
		var values = new List<OCSM.CoD.CtL.Contract>();
		var children = GetChildren();
		foreach(Contract contract in children)
		{
			var attr1Node = contract.GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Contract.Attribute));
			var attr2Node = contract.GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Contract.Attribute2));
			var attr3Node = contract.GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Contract.Attribute3));
			var skillNode = contract.GetNode<SkillOptionButton>(NodePathBuilder.SceneUnique(Contract.Skill));
			var regaliaNode = contract.GetNode<RegaliaOptionButton>(NodePathBuilder.SceneUnique(Contract.Regalia));
			var contractTypeNode = contract.GetNode<OptionButton>(NodePathBuilder.SceneUnique(Contract.ContractType));
			
			var name = contract.GetNode<LineEdit>(NodePathBuilder.SceneUnique(Contract.NameName)).Text;
			var action = contract.GetNode<OptionButton>(NodePathBuilder.SceneUnique(Contract.Action)).Selected;
			var cost = contract.GetNode<LineEdit>(NodePathBuilder.SceneUnique(Contract.Cost)).Text;
			var description = contract.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.Description)).Text;
			var duration = contract.GetNode<LineEdit>(NodePathBuilder.SceneUnique(Contract.Duration)).Text;
			var effects = contract.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.Effects)).Text;
			var seemingBenefits = contract.SeemingBenefits;
			var failure = contract.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.Failure)).Text;
			var FailureDramatic = contract.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.FailureDramatic)).Text;
			var success = contract.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.Success)).Text;
			var successExceptional = contract.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.SuccessExceptional)).Text;
			var loophole = contract.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.Loophole)).Text;
			
			var attribute = OCSM.CoD.Attribute.byName(attr1Node.GetItemText(attr1Node.Selected));
			var attributeResisted = OCSM.CoD.Attribute.byName(attr2Node.GetItemText(attr2Node.Selected));
			var attributeContested = OCSM.CoD.Attribute.byName(attr3Node.GetItemText(attr3Node.Selected));
			var skill = Skill.byName(skillNode.GetItemText(skillNode.Selected));
			var regalia = regaliaNode.GetItemText(regaliaNode.Selected);
			var contractType = contractTypeNode.GetItemText(contractTypeNode.Selected);
			
			var container = metadataManager.Container;
			var regaliaAndContractTypeExist = true;
			if(container is CoDChangelingContainer ccc)
			{
				regaliaAndContractTypeExist = ccc.Regalias.Find(r => r.Name.Equals(regalia)) is Regalia
					&& ccc.ContractTypes.Find(ct => ct.Name.Equals(contractType)) is ContractType;
			}
			
			var shouldRemove = String.IsNullOrEmpty(name) && action < 1 && String.IsNullOrEmpty(description)
				&& String.IsNullOrEmpty(effects) && !(attribute is OCSM.CoD.Attribute) && !(attributeResisted is OCSM.CoD.Attribute) && !(attributeContested is OCSM.CoD.Attribute)
				&& seemingBenefits.Count < 1 && String.IsNullOrEmpty(failure) && String.IsNullOrEmpty(FailureDramatic)
				&& !regaliaAndContractTypeExist
				&& String.IsNullOrEmpty(success) && String.IsNullOrEmpty(successExceptional) && String.IsNullOrEmpty(loophole);
			
			if(!shouldRemove)
			{
				Regalia regaliaObj = null;
				ContractType contractTypeObj = null;
				if(metadataManager.Container is CoDChangelingContainer ccc2)
				{
					regaliaObj = ccc2.Regalias.Find(r => r.Name.Equals(regalia));
					contractTypeObj = ccc2.ContractTypes.Find(ct => ct.Name.Equals(contractType));
				}
				
				values.Add(new OCSM.CoD.CtL.Contract(name, description)
				{
					Action = action,
					Attribute = attribute,
					AttributeResisted = attributeResisted,
					AttributeContested = attributeContested,
					Cost = cost,
					Duration = duration,
					Effects = effects,
					Loophole = loophole,
					Regalia = regaliaObj,
					ContractType = contractTypeObj,
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
		EmitSignal(nameof(ValueChanged), new SignalPayload<List<OCSM.CoD.CtL.Contract>>(values));
		
		if(GetChildren().Count <= Values.Count)
		{
			addInput();
		}
	}
	
	private void addInput(OCSM.CoD.CtL.Contract value = null)
	{
		var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.CoD.Changeling.Contract);
		var instance = resource.Instance<Contract>();
		
		AddChild(instance);
		
		if(value is OCSM.CoD.CtL.Contract)
		{
			if(value.Action > -1)
			{
				instance.GetNode<OptionButton>(NodePathBuilder.SceneUnique(Contract.Action)).Selected = value.Action;
				instance.actionChanged(value.Action);
			}
			
			if(value.Attribute is OCSM.CoD.Attribute)
			{
				var index = OCSM.CoD.Attribute.asList().FindIndex(a => a.Equals(value.Attribute)) + 1;
				instance.GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Contract.Attribute)).Selected = index;
				instance.attributeChanged(index);
			}
			
			if(value.AttributeResisted is OCSM.CoD.Attribute)
				instance.GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Contract.Attribute2)).Selected = OCSM.CoD.Attribute.asList().FindIndex(a => a.Equals(value.AttributeResisted)) + 1;
			
			if(value.AttributeContested is OCSM.CoD.Attribute)
			{
				var index = OCSM.CoD.Attribute.asList().FindIndex(a => a.Equals(value.AttributeContested)) + 1;
				instance.GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Contract.Attribute3)).Selected = index;
				instance.contestedAttributeChanged(index);
			}
			
			if(!String.IsNullOrEmpty(value.Cost))
				instance.GetNode<LineEdit>(NodePathBuilder.SceneUnique(Contract.Cost)).Text = value.Cost;
			if(!String.IsNullOrEmpty(value.Description))
				instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.Description)).Text = value.Description;
			if(!String.IsNullOrEmpty(value.Duration))
				instance.GetNode<LineEdit>(NodePathBuilder.SceneUnique(Contract.Duration)).Text = value.Duration;
			if(!String.IsNullOrEmpty(value.Effects))
				instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.Effects)).Text = value.Effects;
			if(!String.IsNullOrEmpty(value.Loophole))
				instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.Loophole)).Text = value.Loophole;
			if(!String.IsNullOrEmpty(value.Name))
				instance.GetNode<LineEdit>(NodePathBuilder.SceneUnique(Contract.NameName)).Text = value.Name;
			
			if(value.SeemingBenefits is Dictionary<string, string>)
			{
				instance.SeemingBenefits = value.SeemingBenefits;
				instance.refreshSeemingBenefits();
			}
			
			if(value.Skill is Skill)
				instance.GetNode<SkillOptionButton>(NodePathBuilder.SceneUnique(Contract.Skill)).Selected = Skill.asList().IndexOf(value.Skill) + 1;
			
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				if(value.Regalia is Regalia)
					instance.GetNode<RegaliaOptionButton>(NodePathBuilder.SceneUnique(Contract.Regalia)).Selected = ccc.Regalias.FindIndex(r => r.Equals(value.Regalia)) + 1;
				if(value.ContractType is ContractType)
					instance.GetNode<OptionButton>(NodePathBuilder.SceneUnique(Contract.ContractType)).Selected = ccc.ContractTypes.FindIndex(r => r.Equals(value.ContractType)) + 1;
			}
			
			if(!String.IsNullOrEmpty(value.RollFailure))
				instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.Failure)).Text = value.RollFailure;
			if(!String.IsNullOrEmpty(value.RollFailureDramatic))
				instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.FailureDramatic)).Text = value.RollFailureDramatic;
			if(!String.IsNullOrEmpty(value.RollSuccess))
				instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.Success)).Text = value.RollSuccess;
			if(!String.IsNullOrEmpty(value.RollSuccessExceptional))
				instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.SuccessExceptional)).Text = value.RollSuccessExceptional;
		}
		
		instance.GetNode<OptionButton>(NodePathBuilder.SceneUnique(Contract.Action)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
		instance.GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Contract.Attribute)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
		instance.GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Contract.Attribute2)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
		instance.GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Contract.Attribute3)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
		instance.GetNode<SkillOptionButton>(NodePathBuilder.SceneUnique(Contract.Skill)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
		instance.GetNode<RegaliaOptionButton>(NodePathBuilder.SceneUnique(Contract.Regalia)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
		instance.GetNode<ContractTypeButton>(NodePathBuilder.SceneUnique(Contract.ContractType)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
		instance.GetNode<LineEdit>(NodePathBuilder.SceneUnique(Contract.Cost)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		instance.GetNode<LineEdit>(NodePathBuilder.SceneUnique(Contract.Duration)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		instance.GetNode<LineEdit>(NodePathBuilder.SceneUnique(Contract.NameName)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.Description)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.Effects)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.Loophole)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.Failure)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.FailureDramatic)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.Success)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.SuccessExceptional)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
	}
	
	private void optionSelected(int index) { updateValues(); }
	private void textChanged(string newText) { textChanged(); }
	private void textChanged() { updateValues(); }
}
