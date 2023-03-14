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
		public delegate void ValueChangedEventHandler(Transport<List<Contract>> values);
		
		public List<Contract> Values { get; set; } = new List<Contract>();
		
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
			Values.Where(v => v is Contract)
				.ToList()
				.ForEach(v => addInput(v));
			
			addInput();
		}
		
		private void updateValues()
		{
			var values = new List<Contract>();
			
			GetChildren().Select(n => (n as ContractNode).getData())
				.Where(c => !c.Empty)
				.OrderBy(c => c)
				.ToList()
				.ForEach(c => values.Add(c));
			
			GetChildren().Where(n => (n as ContractNode).getData().Empty && !GetChildren().IndexOf(n).Equals(GetChildren().Count - 1))
				.ToList()
				.ForEach(n => n.QueueFree());
			
			Values = values;
			EmitSignal(nameof(ValueChanged), new Transport<List<Contract>>(Values));
			
			if(GetChildren().Count <= Values.Count)
			{
				addInput();
			}
		}
		
		private void addInput(Contract value = null)
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.CoD.Changeling.ContractNode);
			var instance = resource.Instantiate<ContractNode>();
			
			AddChild(instance);
			
			if(value is Contract)
			{
				var actionNode = instance.GetNode<ActionOptionButton>(ContractNode.NodePath.ActionInput);
				actionNode.SelectItemByText(value.Action);
				instance.actionChanged(actionNode.Selected);
				
				if(value.Attribute is OCSM.CoD.Attribute.Enum attribute)
				{
					var node = instance.GetNode<AttributeOptionButton>(ContractNode.NodePath.AttributeInput);
					node.SelectItemByText(attribute.ToString());
					instance.attributeChanged(node.Selected);
				}
				
				if(value.AttributeResisted is OCSM.CoD.Attribute.Enum attributeResisted)
					instance.GetNode<AttributeOptionButton>(ContractNode.NodePath.Attribute2Input).SelectItemByText(attributeResisted.ToString());
				
				if(value.AttributeContested is OCSM.CoD.Attribute.Enum attributeContested)
				{
					var node = instance.GetNode<AttributeOptionButton>(ContractNode.NodePath.Attribute3Input);
					node.SelectItemByText(attributeContested.ToString());
					instance.contestedAttributeChanged(node.Selected);
				}
				
				if(!String.IsNullOrEmpty(value.Cost))
					instance.GetNode<LineEdit>(ContractNode.NodePath.CostInput).Text = value.Cost;
				if(!String.IsNullOrEmpty(value.Description))
					instance.GetNode<TextEdit>(ContractNode.NodePath.DescriptionInput).Text = value.Description;
				if(!String.IsNullOrEmpty(value.Duration))
					instance.GetNode<LineEdit>(ContractNode.NodePath.DurationInput).Text = value.Duration;
				if(!String.IsNullOrEmpty(value.Effects))
					instance.GetNode<TextEdit>(ContractNode.NodePath.EffectsInput).Text = value.Effects;
				if(!String.IsNullOrEmpty(value.Loophole))
					instance.GetNode<TextEdit>(ContractNode.NodePath.LoopholeInput).Text = value.Loophole;
				if(!String.IsNullOrEmpty(value.Name))
					instance.GetNode<LineEdit>(ContractNode.NodePath.NameInput).Text = value.Name;
				
				if(value.SeemingBenefits is System.Collections.Generic.Dictionary<string, string>)
				{
					instance.SeemingBenefits = value.SeemingBenefits;
					instance.refreshSeemingBenefits();
				}
				
				if(value.Skill is Skill.Enum skill)
					instance.GetNode<SkillOptionButton>(ContractNode.NodePath.SkillInput).SelectItemByText(skill.GetLabelOrName());
				
				if(metadataManager.Container is CoDChangelingContainer ccc)
				{
					if(value.Regalia is ContractRegalia)
						instance.GetNode<ContractRegaliaOptionButton>(ContractNode.NodePath.RegaliaInput).SelectItemByText(value.Regalia.Name);
					if(value.ContractType is ContractType)
						instance.GetNode<OptionButton>(ContractNode.NodePath.ContractTypeInput).SelectItemByText(value.ContractType.Name);
				}
				
				instance.GetNode<TextureButton>(ContractNode.NodePath.ToggleResults).ButtonPressed = value.ShowResults;
				instance.GetNode<VBoxContainer>(ContractNode.NodePath.RollResultsRow).Visible = value.ShowResults;
				if(!String.IsNullOrEmpty(value.RollFailure))
					instance.GetNode<TextEdit>(ContractNode.NodePath.FailureInput).Text = value.RollFailure;
				if(!String.IsNullOrEmpty(value.RollFailureDramatic))
					instance.GetNode<TextEdit>(ContractNode.NodePath.FailureDramaticInput).Text = value.RollFailureDramatic;
				if(!String.IsNullOrEmpty(value.RollSuccess))
					instance.GetNode<TextEdit>(ContractNode.NodePath.SuccessInput).Text = value.RollSuccess;
				if(!String.IsNullOrEmpty(value.RollSuccessExceptional))
					instance.GetNode<TextEdit>(ContractNode.NodePath.SuccessExceptionalInput).Text = value.RollSuccessExceptional;
			}
			
			instance.GetNode<OptionButton>(ContractNode.NodePath.ActionInput).ItemSelected += optionSelected;
			instance.GetNode<AttributeOptionButton>(ContractNode.NodePath.AttributeInput).ItemSelected += optionSelected;
			instance.GetNode<AttributeOptionButton>(ContractNode.NodePath.Attribute2Input).ItemSelected += optionSelected;
			instance.GetNode<AttributeOptionButton>(ContractNode.NodePath.Attribute3Input).ItemSelected += optionSelected;
			instance.GetNode<SkillOptionButton>(ContractNode.NodePath.SkillInput).ItemSelected += optionSelected;
			instance.GetNode<ContractRegaliaOptionButton>(ContractNode.NodePath.RegaliaInput).ItemSelected += optionSelected;
			instance.GetNode<ContractTypeButton>(ContractNode.NodePath.ContractTypeInput).ItemSelected += optionSelected;
			instance.GetNode<LineEdit>(ContractNode.NodePath.CostInput).TextChanged += textChanged;
			instance.GetNode<LineEdit>(ContractNode.NodePath.DurationInput).TextChanged += textChanged;
			instance.GetNode<LineEdit>(ContractNode.NodePath.NameInput).TextChanged += textChanged;
			instance.GetNode<TextEdit>(ContractNode.NodePath.DescriptionInput).TextChanged += textChanged;
			instance.GetNode<TextEdit>(ContractNode.NodePath.EffectsInput).TextChanged += textChanged;
			instance.GetNode<TextEdit>(ContractNode.NodePath.LoopholeInput).TextChanged += textChanged;
			instance.GetNode<TextEdit>(ContractNode.NodePath.FailureInput).TextChanged += textChanged;
			instance.GetNode<TextEdit>(ContractNode.NodePath.FailureDramaticInput).TextChanged += textChanged;
			instance.GetNode<TextEdit>(ContractNode.NodePath.SuccessInput).TextChanged += textChanged;
			instance.GetNode<TextEdit>(ContractNode.NodePath.SuccessExceptionalInput).TextChanged += textChanged;
		}
		
		private void optionSelected(long index) { updateValues(); }
		private void textChanged(string newText) { textChanged(); }
		private void textChanged() { updateValues(); }
	}
}
