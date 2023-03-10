using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using OCSM.CoD;
using OCSM.CoD.CtL;
using OCSM.CoD.CtL.Meta;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes.CoD.CtL
{
	public partial class ContractsList : Container
	{
		[Signal]
		public delegate void ValueChangedEventHandler(Transport<List<OCSM.CoD.CtL.Contract>> values);
		
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
			var lastIndex = children.Count - 1;
			foreach(Contract contract in children)
			{
				var data = contract.getData();
				if(!data.Empty)
					values.Add(data);
				else if(!children.IndexOf(contract).Equals(lastIndex))
					contract.QueueFree();
			}
			
			Values = values;
			EmitSignal(nameof(ValueChanged), new Transport<List<OCSM.CoD.CtL.Contract>>(Values));
			
			if(GetChildren().Count <= Values.Count)
			{
				addInput();
			}
		}
		
		private void addInput(OCSM.CoD.CtL.Contract value = null)
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.CoD.Changeling.Contract);
			var instance = resource.Instantiate<Contract>();
			
			AddChild(instance);
			
			if(value is OCSM.CoD.CtL.Contract)
			{
				if(value.Action > -1)
				{
					instance.GetNode<OptionButton>(Contract.NodePath.ActionInput).Selected = value.Action;
					instance.actionChanged(value.Action);
				}
				
				if(value.Attribute is OCSM.CoD.Attribute)
				{
					var index = OCSM.CoD.Attribute.asList().FindIndex(a => a.Equals(value.Attribute)) + 1;
					instance.GetNode<AttributeOptionButton>(Contract.NodePath.AttributeInput).Selected = index;
					instance.attributeChanged(index);
				}
				
				if(value.AttributeResisted is OCSM.CoD.Attribute)
					instance.GetNode<AttributeOptionButton>(Contract.NodePath.Attribute2Input).Selected = OCSM.CoD.Attribute.asList().FindIndex(a => a.Equals(value.AttributeResisted)) + 1;
				
				if(value.AttributeContested is OCSM.CoD.Attribute)
				{
					var index = OCSM.CoD.Attribute.asList().FindIndex(a => a.Equals(value.AttributeContested)) + 1;
					instance.GetNode<AttributeOptionButton>(Contract.NodePath.Attribute3Input).Selected = index;
					instance.contestedAttributeChanged(index);
				}
				
				if(!String.IsNullOrEmpty(value.Cost))
					instance.GetNode<LineEdit>(Contract.NodePath.CostInput).Text = value.Cost;
				if(!String.IsNullOrEmpty(value.Description))
					instance.GetNode<TextEdit>(Contract.NodePath.DescriptionInput).Text = value.Description;
				if(!String.IsNullOrEmpty(value.Duration))
					instance.GetNode<LineEdit>(Contract.NodePath.DurationInput).Text = value.Duration;
				if(!String.IsNullOrEmpty(value.Effects))
					instance.GetNode<TextEdit>(Contract.NodePath.EffectsInput).Text = value.Effects;
				if(!String.IsNullOrEmpty(value.Loophole))
					instance.GetNode<TextEdit>(Contract.NodePath.LoopholeInput).Text = value.Loophole;
				if(!String.IsNullOrEmpty(value.Name))
					instance.GetNode<LineEdit>(Contract.NodePath.NameInput).Text = value.Name;
				
				if(value.SeemingBenefits is System.Collections.Generic.Dictionary<string, string>)
				{
					instance.SeemingBenefits = value.SeemingBenefits;
					instance.refreshSeemingBenefits();
				}
				
				if(value.Skill is Skill)
					instance.GetNode<SkillOptionButton>(Contract.NodePath.SkillInput).Selected = Skill.asList().IndexOf(value.Skill) + 1;
				
				if(metadataManager.Container is CoDChangelingContainer ccc)
				{
					if(value.Regalia is Regalia)
						instance.GetNode<RegaliaOptionButton>(Contract.NodePath.RegaliaInput).Selected = ccc.Regalias.FindIndex(r => r.Equals(value.Regalia)) + 1;
					if(value.ContractType is ContractType)
						instance.GetNode<OptionButton>(Contract.NodePath.ContractTypeInput).Selected = ccc.ContractTypes.FindIndex(r => r.Equals(value.ContractType)) + 1;
				}
				
				if(!String.IsNullOrEmpty(value.RollFailure))
					instance.GetNode<TextEdit>(Contract.NodePath.FailureInput).Text = value.RollFailure;
				if(!String.IsNullOrEmpty(value.RollFailureDramatic))
					instance.GetNode<TextEdit>(Contract.NodePath.FailureDramaticInput).Text = value.RollFailureDramatic;
				if(!String.IsNullOrEmpty(value.RollSuccess))
					instance.GetNode<TextEdit>(Contract.NodePath.SuccessInput).Text = value.RollSuccess;
				if(!String.IsNullOrEmpty(value.RollSuccessExceptional))
					instance.GetNode<TextEdit>(Contract.NodePath.SuccessExceptionalInput).Text = value.RollSuccessExceptional;
			}
			
			instance.GetNode<OptionButton>(Contract.NodePath.ActionInput).ItemSelected += optionSelected;
			instance.GetNode<AttributeOptionButton>(Contract.NodePath.AttributeInput).ItemSelected += optionSelected;
			instance.GetNode<AttributeOptionButton>(Contract.NodePath.Attribute2Input).ItemSelected += optionSelected;
			instance.GetNode<AttributeOptionButton>(Contract.NodePath.Attribute3Input).ItemSelected += optionSelected;
			instance.GetNode<SkillOptionButton>(Contract.NodePath.SkillInput).ItemSelected += optionSelected;
			instance.GetNode<RegaliaOptionButton>(Contract.NodePath.RegaliaInput).ItemSelected += optionSelected;
			instance.GetNode<ContractTypeButton>(Contract.NodePath.ContractTypeInput).ItemSelected += optionSelected;
			instance.GetNode<LineEdit>(Contract.NodePath.CostInput).TextChanged += textChanged;
			instance.GetNode<LineEdit>(Contract.NodePath.DurationInput).TextChanged += textChanged;
			instance.GetNode<LineEdit>(Contract.NodePath.NameInput).TextChanged += textChanged;
			instance.GetNode<TextEdit>(Contract.NodePath.DescriptionInput).TextChanged += textChanged;
			instance.GetNode<TextEdit>(Contract.NodePath.EffectsInput).TextChanged += textChanged;
			instance.GetNode<TextEdit>(Contract.NodePath.LoopholeInput).TextChanged += textChanged;
			instance.GetNode<TextEdit>(Contract.NodePath.FailureInput).TextChanged += textChanged;
			instance.GetNode<TextEdit>(Contract.NodePath.FailureDramaticInput).TextChanged += textChanged;
			instance.GetNode<TextEdit>(Contract.NodePath.SuccessInput).TextChanged += textChanged;
			instance.GetNode<TextEdit>(Contract.NodePath.SuccessExceptionalInput).TextChanged += textChanged;
		}
		
		private void optionSelected(long index) { updateValues(); }
		private void textChanged(string newText) { textChanged(); }
		private void textChanged() { updateValues(); }
	}
}
