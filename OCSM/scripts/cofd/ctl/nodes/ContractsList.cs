using Godot;
using System.Collections.Generic;
using System.Linq;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Cofd.Nodes;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Cofd.Ctl.Nodes;

public partial class ContractsList : Container
{
	[Signal]
	public delegate void ValueChangedEventHandler(Transport<List<Contract>> values);
	
	public List<Contract> Values { get; set; } = [];
	
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		metadataManager.MetadataLoaded += Refresh;
		
		Refresh();
	}
	
	public void Refresh()
	{
		GetChildren().ToList()
			.ForEach(n => n.QueueFree());
		
		Values.Where(v => v is not null)
			.OrderBy(v => v)
			.ToList()
			.ForEach(v => addInput(v));
		
		addInput();
	}
	
	private void updateValues()
	{
		var values = new List<Contract>();
		
		GetChildren()
			.Select(n => (n as ContractNode).GetData())
			.Where(c => !c.Empty)
			.OrderBy(c => c)
			.ToList()
			.ForEach(c => values.Add(c));
		
		var last = GetChildren().Count - 1;
		GetChildren()
			.Where(n => (n as ContractNode).GetData().Empty && GetChildren().IndexOf(n) != last)
			.ToList()
			.ForEach(n => n.QueueFree());
		
		Values = values;
		_ = EmitSignal(SignalName.ValueChanged, new Transport<List<Contract>>(Values));
		
		if(GetChildren().Count <= Values.Count)
			addInput();
	}
	
	private void addInput(Contract value = null)
	{
		var resource = GD.Load<PackedScene>(ScenePaths.Cofd.Changeling.ContractNode);
		var instance = resource.Instantiate<ContractNode>();
		
		AddChild(instance);
		
		if(value is not null)
		{
			var actionNode = instance.GetNode<ActionOptionButton>(ContractNode.NodePaths.ActionInput);
			actionNode.SelectItemByText(value.Action);
			instance.ActionChanged(actionNode.Selected);
			
			if(value.Attribute is Attribute.EnumValues attribute)
			{
				var node = instance.GetNode<AttributeOptionButton>(ContractNode.NodePaths.AttributeInput);
				node.SelectItemByText(attribute.ToString());
				instance.AttributeChanged(node.Selected);
			}
			
			if(value.AttributeResisted is Attribute.EnumValues attributeResisted)
				instance.GetNode<AttributeOptionButton>(ContractNode.NodePaths.Attribute2Input).SelectItemByText(attributeResisted.ToString());
			
			if(value.AttributeContested is Attribute.EnumValues attributeContested)
			{
				var node = instance.GetNode<AttributeOptionButton>(ContractNode.NodePaths.Attribute3Input);
				node.SelectItemByText(attributeContested.ToString());
				instance.ContestedAttributeChanged(node.Selected);
			}
			
			if(!string.IsNullOrEmpty(value.Cost))
				instance.GetNode<LineEdit>(ContractNode.NodePaths.CostInput).Text = value.Cost;
			
			if(!string.IsNullOrEmpty(value.Description))
				instance.GetNode<TextEdit>(ContractNode.NodePaths.DescriptionInput).Text = value.Description;
			
			if(!string.IsNullOrEmpty(value.Duration))
				instance.GetNode<LineEdit>(ContractNode.NodePaths.DurationInput).Text = value.Duration;
			
			if(!string.IsNullOrEmpty(value.Effects))
				instance.GetNode<TextEdit>(ContractNode.NodePaths.EffectsInput).Text = value.Effects;
			
			if(!string.IsNullOrEmpty(value.Loophole))
				instance.GetNode<TextEdit>(ContractNode.NodePaths.LoopholeInput).Text = value.Loophole;
			
			if(!string.IsNullOrEmpty(value.Name))
				instance.GetNode<LineEdit>(ContractNode.NodePaths.NameInput).Text = value.Name;
			
			if(value.SeemingBenefits is not null)
			{
				instance.SeemingBenefits = value.SeemingBenefits;
				instance.RefreshSeemingBenefits();
			}
			
			if(value.Skill is Skill.EnumValues skill)
				instance.GetNode<SkillOptionButton>(ContractNode.NodePaths.SkillInput).SelectItemByText(skill.GetLabelOrName());
			
			if(metadataManager.Container is CofdChangelingContainer ccc)
			{
				if(value.Regalia is not null)
					instance.GetNode<ContractRegaliaOptionButton>(ContractNode.NodePaths.RegaliaInput).SelectItemByText(value.Regalia.Name);
				
				if(value.ContractType is not null)
					instance.GetNode<OptionButton>(ContractNode.NodePaths.ContractTypeInput).SelectItemByText(value.ContractType.Name);
			}
			
			instance.GetNode<TextureButton>(ContractNode.NodePaths.ToggleResults).ButtonPressed = value.ShowResults;
			instance.GetNode<VBoxContainer>(ContractNode.NodePaths.RollResultsRow).Visible = value.ShowResults;
			
			if(!string.IsNullOrEmpty(value.RollFailure))
				instance.GetNode<TextEdit>(ContractNode.NodePaths.FailureInput).Text = value.RollFailure;
			
			if(!string.IsNullOrEmpty(value.RollFailureDramatic))
				instance.GetNode<TextEdit>(ContractNode.NodePaths.FailureDramaticInput).Text = value.RollFailureDramatic;
			
			if(!string.IsNullOrEmpty(value.RollSuccess))
				instance.GetNode<TextEdit>(ContractNode.NodePaths.SuccessInput).Text = value.RollSuccess;
			
			if(!string.IsNullOrEmpty(value.RollSuccessExceptional))
				instance.GetNode<TextEdit>(ContractNode.NodePaths.SuccessExceptionalInput).Text = value.RollSuccessExceptional;
		}
		
		instance.GetNode<OptionButton>(ContractNode.NodePaths.ActionInput).ItemSelected += optionSelected;
		instance.GetNode<AttributeOptionButton>(ContractNode.NodePaths.AttributeInput).ItemSelected += optionSelected;
		instance.GetNode<AttributeOptionButton>(ContractNode.NodePaths.Attribute2Input).ItemSelected += optionSelected;
		instance.GetNode<AttributeOptionButton>(ContractNode.NodePaths.Attribute3Input).ItemSelected += optionSelected;
		instance.GetNode<SkillOptionButton>(ContractNode.NodePaths.SkillInput).ItemSelected += optionSelected;
		instance.GetNode<ContractRegaliaOptionButton>(ContractNode.NodePaths.RegaliaInput).ItemSelected += optionSelected;
		instance.GetNode<ContractTypeButton>(ContractNode.NodePaths.ContractTypeInput).ItemSelected += optionSelected;
		instance.GetNode<LineEdit>(ContractNode.NodePaths.CostInput).TextChanged += textChanged;
		instance.GetNode<LineEdit>(ContractNode.NodePaths.DurationInput).TextChanged += textChanged;
		instance.GetNode<LineEdit>(ContractNode.NodePaths.NameInput).TextChanged += textChanged;
		instance.GetNode<TextEdit>(ContractNode.NodePaths.DescriptionInput).TextChanged += textChanged;
		instance.GetNode<TextEdit>(ContractNode.NodePaths.EffectsInput).TextChanged += textChanged;
		instance.GetNode<TextEdit>(ContractNode.NodePaths.LoopholeInput).TextChanged += textChanged;
		instance.GetNode<TextEdit>(ContractNode.NodePaths.FailureInput).TextChanged += textChanged;
		instance.GetNode<TextEdit>(ContractNode.NodePaths.FailureDramaticInput).TextChanged += textChanged;
		instance.GetNode<TextEdit>(ContractNode.NodePaths.SuccessInput).TextChanged += textChanged;
		instance.GetNode<TextEdit>(ContractNode.NodePaths.SuccessExceptionalInput).TextChanged += textChanged;
	}
	
	private void optionSelected(long index) => updateValues();
	private void textChanged(string newText) => updateValues();
	private void textChanged() => updateValues();
}