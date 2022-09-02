using Godot;
using System;
using System.Collections.Generic;
using OCSM.CoD;
using OCSM.CoD.CtL;
using OCSM.CoD.CtL.Meta;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes.CoD.CtL
{
	public class ContractsList : Container
	{
		[Signal]
		public delegate void ValueChanged(List<Transport<OCSM.CoD.CtL.Contract>> values);
		
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
				var data = contract.getData();
				
				var shouldRemove = String.IsNullOrEmpty(data.Name) && data.Action < 1 && String.IsNullOrEmpty(data.Description)
					&& String.IsNullOrEmpty(data.Effects) && !(data.Attribute is OCSM.CoD.Attribute) && !(data.AttributeResisted is OCSM.CoD.Attribute) && !(data.AttributeContested is OCSM.CoD.Attribute)
					&& data.SeemingBenefits.Count < 1 && String.IsNullOrEmpty(data.RollFailure) && String.IsNullOrEmpty(data.RollFailureDramatic)
					&& !(data.Regalia is Regalia) && !(data.ContractType is ContractType)
					&& String.IsNullOrEmpty(data.RollSuccess) && String.IsNullOrEmpty(data.RollSuccessExceptional) && String.IsNullOrEmpty(data.Loophole);
				
				if(!shouldRemove)
					values.Add(data);
				else if(children.IndexOf(contract) != children.Count - 1)
					contract.QueueFree();
			}
			
			Values = values;
			
			if(GetChildren().Count <= Values.Count)
			{
				addInput();
			}
		}
		
		private void doEmitSignal()
		{
			var list = new List<Transport<OCSM.CoD.CtL.Contract>>();
			foreach(var c in Values)
			{
				list.Add(new Transport<OCSM.CoD.CtL.Contract>(c));
			}
			EmitSignal(nameof(ValueChanged), list);
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
					instance.GetNode<OptionButton>(NodePathBuilder.SceneUnique(Contract.ActionInput)).Selected = value.Action;
					instance.actionChanged(value.Action);
				}
				
				if(value.Attribute is OCSM.CoD.Attribute)
				{
					var index = OCSM.CoD.Attribute.asList().FindIndex(a => a.Equals(value.Attribute)) + 1;
					instance.GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Contract.AttributeInput)).Selected = index;
					instance.attributeChanged(index);
				}
				
				if(value.AttributeResisted is OCSM.CoD.Attribute)
					instance.GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Contract.Attribute2Input)).Selected = OCSM.CoD.Attribute.asList().FindIndex(a => a.Equals(value.AttributeResisted)) + 1;
				
				if(value.AttributeContested is OCSM.CoD.Attribute)
				{
					var index = OCSM.CoD.Attribute.asList().FindIndex(a => a.Equals(value.AttributeContested)) + 1;
					instance.GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Contract.Attribute3Input)).Selected = index;
					instance.contestedAttributeChanged(index);
				}
				
				if(!String.IsNullOrEmpty(value.Cost))
					instance.GetNode<LineEdit>(NodePathBuilder.SceneUnique(Contract.CostInput)).Text = value.Cost;
				if(!String.IsNullOrEmpty(value.Description))
					instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.DescriptionInput)).Text = value.Description;
				if(!String.IsNullOrEmpty(value.Duration))
					instance.GetNode<LineEdit>(NodePathBuilder.SceneUnique(Contract.DurationInput)).Text = value.Duration;
				if(!String.IsNullOrEmpty(value.Effects))
					instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.EffectsInput)).Text = value.Effects;
				if(!String.IsNullOrEmpty(value.Loophole))
					instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.LoopholeInput)).Text = value.Loophole;
				if(!String.IsNullOrEmpty(value.Name))
					instance.GetNode<LineEdit>(NodePathBuilder.SceneUnique(Contract.NameInput)).Text = value.Name;
				
				if(value.SeemingBenefits is Dictionary<string, string>)
				{
					instance.SeemingBenefits = value.SeemingBenefits;
					instance.refreshSeemingBenefits();
				}
				
				if(value.Skill is Skill)
					instance.GetNode<SkillOptionButton>(NodePathBuilder.SceneUnique(Contract.SkillInput)).Selected = Skill.asList().IndexOf(value.Skill) + 1;
				
				if(metadataManager.Container is CoDChangelingContainer ccc)
				{
					if(value.Regalia is Regalia)
						instance.GetNode<RegaliaOptionButton>(NodePathBuilder.SceneUnique(Contract.RegaliaInput)).Selected = ccc.Regalias.FindIndex(r => r.Equals(value.Regalia)) + 1;
					if(value.ContractType is ContractType)
						instance.GetNode<OptionButton>(NodePathBuilder.SceneUnique(Contract.ContractTypeInput)).Selected = ccc.ContractTypes.FindIndex(r => r.Equals(value.ContractType)) + 1;
				}
				
				if(!String.IsNullOrEmpty(value.RollFailure))
					instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.FailureInput)).Text = value.RollFailure;
				if(!String.IsNullOrEmpty(value.RollFailureDramatic))
					instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.FailureDramaticInput)).Text = value.RollFailureDramatic;
				if(!String.IsNullOrEmpty(value.RollSuccess))
					instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.SuccessInput)).Text = value.RollSuccess;
				if(!String.IsNullOrEmpty(value.RollSuccessExceptional))
					instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.SuccessExceptionalInput)).Text = value.RollSuccessExceptional;
			}
			
			instance.GetNode<OptionButton>(NodePathBuilder.SceneUnique(Contract.ActionInput)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
			instance.GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Contract.AttributeInput)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
			instance.GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Contract.Attribute2Input)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
			instance.GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Contract.Attribute3Input)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
			instance.GetNode<SkillOptionButton>(NodePathBuilder.SceneUnique(Contract.SkillInput)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
			instance.GetNode<RegaliaOptionButton>(NodePathBuilder.SceneUnique(Contract.RegaliaInput)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
			instance.GetNode<ContractTypeButton>(NodePathBuilder.SceneUnique(Contract.ContractTypeInput)).Connect(Constants.Signal.ItemSelected, this, nameof(optionSelected));
			instance.GetNode<LineEdit>(NodePathBuilder.SceneUnique(Contract.CostInput)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
			instance.GetNode<LineEdit>(NodePathBuilder.SceneUnique(Contract.DurationInput)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
			instance.GetNode<LineEdit>(NodePathBuilder.SceneUnique(Contract.NameInput)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
			instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.DescriptionInput)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
			instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.EffectsInput)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
			instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.LoopholeInput)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
			instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.FailureInput)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
			instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.FailureDramaticInput)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
			instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.SuccessInput)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
			instance.GetNode<TextEdit>(NodePathBuilder.SceneUnique(Contract.SuccessExceptionalInput)).Connect(Constants.Signal.TextChanged, this, nameof(textChanged));
		}
		
		private void optionSelected(int index) { updateValues(); }
		private void textChanged(string newText) { textChanged(); }
		private void textChanged() { updateValues(); }
	}
}
