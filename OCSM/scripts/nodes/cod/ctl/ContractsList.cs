using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
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
			metadataManager.MetadataLoaded += refresh;
			
			refresh();
		}
		
		public void refresh()
		{
			foreach(Node c in GetChildren())
			{
				c.QueueFree();
			}
			
			Values.Sort();
			Values.Where(v => v is OCSM.CoD.CtL.Contract)
				.ToList()
				.ForEach(v => addInput(v));
			
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
			Values.Sort();
			EmitSignal(nameof(ValueChanged), new Transport<List<OCSM.CoD.CtL.Contract>>(Values));
			NodeUtilities.rearrangeNodes(this, children.Where(n => !(n as Contract).getData().Empty)
														.OrderBy(n => (n as Contract).getData())
														.ToList());
			
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
				var actionNode = instance.GetNode<ActionOptionButton>(Contract.NodePath.ActionInput);
				actionNode.SelectItemByText(value.Action);
				instance.actionChanged(actionNode.Selected);
				
				if(value.Attribute is OCSM.CoD.Attribute)
				{
					var node = instance.GetNode<AttributeOptionButton>(Contract.NodePath.AttributeInput);
					node.SelectItemByText(value.Attribute.Name);
					instance.attributeChanged(node.Selected);
				}
				
				if(value.AttributeResisted is OCSM.CoD.Attribute)
					instance.GetNode<AttributeOptionButton>(Contract.NodePath.Attribute2Input).SelectItemByText(value.AttributeResisted.Name);
				
				if(value.AttributeContested is OCSM.CoD.Attribute)
				{
					var node = instance.GetNode<AttributeOptionButton>(Contract.NodePath.Attribute3Input);
					node.SelectItemByText(value.AttributeContested.Name);
					instance.contestedAttributeChanged(node.Selected);
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
				
				if(value.SeemingBenefits is System.Collections.Generic.List<Pair<string, string>>)
				{
					instance.SeemingBenefits = value.SeemingBenefits;
					instance.refreshSeemingBenefits();
				}
				
				if(value.Skill is Skill)
					instance.GetNode<SkillOptionButton>(Contract.NodePath.SkillInput).SelectItemByText(value.Skill.Name);
				
				if(metadataManager.Container is CoDChangelingContainer ccc)
				{
					if(value.Regalia is ContractRegalia)
						instance.GetNode<ContractRegaliaOptionButton>(Contract.NodePath.RegaliaInput).SelectItemByText(value.Regalia.Name);
					if(value.ContractType is ContractType)
						instance.GetNode<OptionButton>(Contract.NodePath.ContractTypeInput).SelectItemByText(value.ContractType.Name);
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
			instance.GetNode<ContractRegaliaOptionButton>(Contract.NodePath.RegaliaInput).ItemSelected += optionSelected;
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
