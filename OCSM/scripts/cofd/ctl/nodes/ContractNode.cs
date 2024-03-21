using Godot;
using System.Collections.Generic;
using System.Linq;
using Ocsm.Cofd;
using Ocsm.Cofd.Ctl;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Nodes.Autoload;
using Ocsm.Meta;

namespace Ocsm.Nodes.Cofd.Ctl;

public partial class ContractNode : MarginContainer
{
	public static class NodePaths
	{
		public static readonly NodePath ActionInput = new("%Action");
		public static readonly NodePath AttributeInput = new("%Attribute");
		public static readonly NodePath Attribute2Input = new("%Attribute2");
		public static readonly NodePath Attribute3Input = new("%Attribute3");
		public static readonly NodePath Attribute2Minus = new("%Attribute2Minus");
		public static readonly NodePath BenefitInput = new("%Benefit");
		public static readonly NodePath ContractTypeInput = new("%ContractType");
		public static readonly NodePath CostInput = new("%Cost");
		public static readonly NodePath DetailsInput = new("%Details");
		public static readonly NodePath DescriptionInput = new("%Description");
		public static readonly NodePath DurationInput = new("%Duration");
		public static readonly NodePath EffectsInput = new("%Effects");
		public static readonly NodePath FailureInput = new("%Failure");
		public static readonly NodePath FailureDramaticInput = new("%DramaticFailure");
		public static readonly NodePath LoopholeInput = new("%Loophole");
		public static readonly NodePath NameInput = new("%Name");
		public static readonly NodePath RegaliaInput = new("%Regalia");
		public static readonly NodePath RollResultsRow = new("%RollResultsRow");
		public static readonly NodePath SeemingInput = new("%Seeming");
		public static readonly NodePath SeemingBenefitsRow = new("%SeemingBenefitsRow");
		public static readonly NodePath SkillInput = new("%Skill");
		public static readonly NodePath SkillPlus = new("%SkillPlus");
		public static readonly NodePath SuccessInput = new("%Success");
		public static readonly NodePath SuccessExceptionalInput = new("%ExceptionalSuccess");
		public static readonly NodePath ToggleDetails = new("%ToggleDetails");
		public static readonly NodePath ToggleResults = new("%ToggleResults");
		public static readonly NodePath Versus = new("%Vs");
		public static readonly NodePath Wyrd = new("%Wyrd");
		public static readonly NodePath Wyrd2 = new("%Wyrd2");
	}
	
	public Dictionary<string, string> SeemingBenefits { get; set; } = [];
	
	private MetadataManager metadataManager;
	
	private AttributeOptionButton attribute2Input;
	private Control attribute2Minus;
	private AttributeOptionButton attribute3Input;
	private VBoxContainer detailsInput;
	private VBoxContainer rollResultsRow;
	private VBoxContainer seemingBenefitsRow;
	private SkillOptionButton skillInput;
	private Control skillPlus;
	private TextureButton toggleResultsNode;
	private Label versus;
	private Control wyrd1;
	private Control wyrd2;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		
		attribute2Input = GetNode<AttributeOptionButton>(NodePaths.Attribute2Input);
		attribute2Minus = GetNode<Control>(NodePaths.Attribute2Minus);
		attribute3Input = GetNode<AttributeOptionButton>(NodePaths.Attribute3Input);
		detailsInput = GetNode<VBoxContainer>(NodePaths.DetailsInput);
		rollResultsRow = GetNode<VBoxContainer>(NodePaths.RollResultsRow);
		seemingBenefitsRow = GetNode<VBoxContainer>(NodePaths.SeemingBenefitsRow);
		skillInput = GetNode<SkillOptionButton>(NodePaths.SkillInput);
		skillPlus = GetNode<Control>(NodePaths.SkillPlus);
		toggleResultsNode = GetNode<TextureButton>(NodePaths.ToggleResults);
		versus = GetNode<Label>(NodePaths.Versus);
		wyrd1 = GetNode<Control>(NodePaths.Wyrd);
		wyrd2 = GetNode<Control>(NodePaths.Wyrd2);
		
		toggleResultsNode.Pressed += ToggleResults;
		GetNode<OptionButton>(NodePaths.ActionInput).ItemSelected += actionChanged;
		GetNode<TextureButton>(NodePaths.ToggleDetails).Pressed += ToggleDetails;
		GetNode<AttributeOptionButton>(NodePaths.AttributeInput).ItemSelected += AttributeChanged;
		attribute3Input.ItemSelected += ContestedAttributeChanged;
		
		RefreshSeemingBenefits();
	}
	
	public void ClearInputs()
	{
		GetNode<AttributeOptionButton>(NodePaths.AttributeInput).Deselect();
		attribute2Input.Deselect();
		attribute3Input.Deselect();
		skillInput.Deselect();
		GetNode<MetadataOption>(NodePaths.RegaliaInput).Deselect();
		GetNode<OptionButton>(NodePaths.ContractTypeInput).Deselect();
		
		GetNode<LineEdit>(NodePaths.NameInput).Text = string.Empty;
		GetNode<OptionButton>(NodePaths.ActionInput).Deselect();
		GetNode<LineEdit>(NodePaths.CostInput).Text = string.Empty;
		GetNode<TextEdit>(NodePaths.DescriptionInput).Text = string.Empty;
		GetNode<LineEdit>(NodePaths.DurationInput).Text = string.Empty;
		GetNode<TextEdit>(NodePaths.EffectsInput).Text = string.Empty;
		GetNode<TextEdit>(NodePaths.FailureInput).Text = string.Empty;
		GetNode<TextEdit>(NodePaths.FailureDramaticInput).Text = string.Empty;
		GetNode<TextEdit>(NodePaths.SuccessInput).Text = string.Empty;
		GetNode<TextEdit>(NodePaths.SuccessExceptionalInput).Text = string.Empty;
		GetNode<TextEdit>(NodePaths.LoopholeInput).Text = string.Empty;
		
		SeemingBenefits.Clear();
		
		actionChanged(0);
		AttributeChanged(0);
		ContestedAttributeChanged(0);
		RefreshSeemingBenefits();
	}
	
	public Contract GetData()
	{
		var attr1Node = GetNode<AttributeOptionButton>(NodePaths.AttributeInput);
		var regaliaNode = GetNode<MetadataOption>(NodePaths.RegaliaInput);
		var contractTypeNode = GetNode<MetadataOption>(NodePaths.ContractTypeInput);
		
		var name = GetNode<LineEdit>(NodePaths.NameInput).Text;
		var cost = GetNode<LineEdit>(NodePaths.CostInput).Text;
		var description = GetNode<TextEdit>(NodePaths.DescriptionInput).Text;
		var duration = GetNode<LineEdit>(NodePaths.DurationInput).Text;
		var effects = GetNode<TextEdit>(NodePaths.EffectsInput).Text;
		var failure = GetNode<TextEdit>(NodePaths.FailureInput).Text;
		var FailureDramatic = GetNode<TextEdit>(NodePaths.FailureDramaticInput).Text;
		var success = GetNode<TextEdit>(NodePaths.SuccessInput).Text;
		var successExceptional = GetNode<TextEdit>(NodePaths.SuccessExceptionalInput).Text;
		var loophole = GetNode<TextEdit>(NodePaths.LoopholeInput).Text;
		
		var actionNode = GetNode<OptionButton>(NodePaths.ActionInput);
		var action = actionNode.GetSelectedItemText();
		
		var attribute = TraitDots.KindFromString(attr1Node.GetSelectedItemText());
		var attributeContested = TraitDots.KindFromString(attribute3Input.GetSelectedItemText());
		var attributeResisted = TraitDots.KindFromString(attribute2Input.GetSelectedItemText());
		var skill = TraitDots.KindFromString(skillInput.GetSelectedItemText());
		var regalia = regaliaNode.GetSelectedItemText();
		var contractType = contractTypeNode.GetSelectedItemText();
		
		Metadata regaliaObj = null;
		Metadata contractTypeObj = null;
		if(metadataManager.Container is CofdChangelingContainer container)
		{
			if(container.Metadata.Where(m => m.Type == MetadataType.CofdChangelingContractRegalia && m.Name == regalia).FirstOrDefault() is Metadata r)
				regaliaObj = r;
			
			if(container.Metadata.Where(m => m.Type == MetadataType.CofdChangelingContractType && m.Name == contractType).FirstOrDefault() is Metadata ct)
				contractTypeObj = ct;
		}
		
		return new Ocsm.Cofd.Ctl.Contract()
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
			Regalia = regaliaObj,
			ContractType = contractTypeObj,
			RollFailure = failure,
			RollFailureDramatic = FailureDramatic,
			RollSuccess = success,
			RollSuccessExceptional = successExceptional,
			SeemingBenefits = SeemingBenefits,
			Skill = skill,
		};
	}
	
	public void SetData(Contract contract)
	{
		if(metadataManager.Container is CofdChangelingContainer)
		{
			var attribute1 = GetNode<AttributeOptionButton>(NodePaths.AttributeInput);
			if(contract.Attribute is not null)
				attribute1.SelectItemByText(contract.Attribute.ToString());
			
			if(contract.AttributeContested is not null)
				attribute2Input.SelectItemByText(contract.AttributeContested.ToString());
			
			if(contract.AttributeResisted is not null)
				attribute3Input.SelectItemByText(contract.AttributeResisted.ToString());
			
			if(contract.Skill is not null)
				skillInput.SelectItemByText(contract.Skill.GetLabel());
			
			if(contract.Regalia is not null)
				GetNode<MetadataOption>(NodePaths.RegaliaInput).SelectItemByText(contract.Regalia.Name);
			
			if(contract.ContractType is not null)
				GetNode<MetadataOption>(NodePaths.ContractTypeInput).SelectItemByText(contract.ContractType.Name);
			
			var actionOption = GetNode<ActionOptionButton>(NodePaths.ActionInput);
			GetNode<LineEdit>(NodePaths.NameInput).Text = contract.Name;
			actionOption.SelectItemByText(contract.Action);
			GetNode<LineEdit>(NodePaths.CostInput).Text = contract.Cost;
			GetNode<TextEdit>(NodePaths.DescriptionInput).Text = contract.Description;
			GetNode<LineEdit>(NodePaths.DurationInput).Text = contract.Duration;
			GetNode<TextEdit>(NodePaths.EffectsInput).Text = contract.Effects;
			GetNode<TextEdit>(NodePaths.FailureInput).Text = contract.RollFailure;
			GetNode<TextEdit>(NodePaths.FailureDramaticInput).Text = contract.RollFailureDramatic;
			GetNode<TextEdit>(NodePaths.SuccessInput).Text = contract.RollSuccess;
			GetNode<TextEdit>(NodePaths.SuccessExceptionalInput).Text = contract.RollSuccessExceptional;
			GetNode<TextEdit>(NodePaths.LoopholeInput).Text = contract.Loophole;
			
			toggleResultsNode.ButtonPressed = contract.ShowResults;
			rollResultsRow.Visible = contract.ShowResults;
			SeemingBenefits = contract.SeemingBenefits;
			
			RefreshSeemingBenefits();
			ActionChanged(actionOption.Selected, false);
			AttributeChanged(attribute1.Selected);
			ContestedAttributeChanged(attribute2Input.Selected);
		}
	}
	
	public void RefreshSeemingBenefits()
	{
		seemingBenefitsRow.GetChildren()
			.Where(n => n is HBoxContainer)
			.ToList()
			.ForEach(n => n.QueueFree());
		
		SeemingBenefits.OrderBy(e => e.Key)
			.ToList()
			.ForEach(e => addSeemingBenefitInput(e.Key, e.Value));
		
		addSeemingBenefitInput();
	}
	
	public void ToggleDetails()
	{
		if(detailsInput.Visible)
			detailsInput.Hide();
		else
			detailsInput.Show();
	}
	
	public void ToggleResults()
	{
		if(rollResultsRow.Visible)
			rollResultsRow.Hide();
		else
			rollResultsRow.Show();
	}
	
	private void actionChanged(long index) => ActionChanged(index, true);
	public void ActionChanged(long index, bool reset = true)
	{
		if(index.Equals((long)ActionOptionButton.Action.Contested))
		{
			versus.Show();
			attribute3Input.Show();
		}
		else
		{
			versus.Hide();
			attribute3Input.Hide();
			if(reset)
				attribute3Input.Deselect();
			wyrd2.Hide();
		}
		
		if(index.Equals((long)ActionOptionButton.Action.Resisted))
		{
			attribute2Input.Show();
			attribute2Minus.Show();
		}
		else
		{
			attribute2Input.Hide();
			if(reset)
				attribute2Input.Deselect();
			attribute2Minus.Hide();
		}
	}
	
	public void AttributeChanged(long index)
	{
		if(index > 0)
		{
			skillInput.Show();
			skillPlus.Show();
			wyrd1.Show();
		}
		else
		{
			skillInput.Hide();
			skillInput.Deselect();
			skillPlus.Hide();
			wyrd1.Hide();
		}
	}
	
	public void ContestedAttributeChanged(long index)
	{
		if(index > 0)
			wyrd2.Show();
		else
			wyrd2.Hide();
	}
	
	private void updateSeemingBenefits()
	{
		GetChildren()
			.Where(node => string.IsNullOrEmpty(node.GetNode<MetadataOption>(NodePaths.SeemingInput).GetSelectedItemText())
				&& string.IsNullOrEmpty(node.GetNode<TextEdit>(NodePaths.BenefitInput).Text))
			.ToList()
			.ForEach(node => node.QueueFree());
		
		SeemingBenefits = GetChildren()
					.Where(node => node is HBoxContainer)
					.Select(node => new {
						seeming = node.GetNode<MetadataOption>(NodePaths.SeemingInput).GetSelectedItemText(),
						benefit = node.GetNode<TextEdit>(NodePaths.BenefitInput).Text
					})
					.OrderBy(o => o.seeming)
					.ToDictionary(o => o.seeming, o => o.benefit);
		
		addSeemingBenefitInput();
	}
	
	private void addSeemingBenefitInput(string seeming = null, string benefit = "")
	{
		var resource = GD.Load<PackedScene>(Constants.Scene.Cofd.Changeling.SeemingBenefit);
		var instance = resource.Instantiate<HBoxContainer>();
		seemingBenefitsRow.AddChild(instance);
		
		//Set the values after adding the child, as we need the _Ready() function to populate the SeemingOptionButton before the index will match a given item.
		if(!string.IsNullOrEmpty(seeming) && !string.IsNullOrEmpty(benefit))
		{
			instance.GetChild<MetadataOption>(0).SelectItemByText(seeming);
			var text = instance.GetChild<TextEdit>(1);
			text.Text = benefit;
		}
		
		instance.GetChild<MetadataOption>(0).ItemSelected += i => updateSeemingBenefits();
		instance.GetChild<TextEdit>(1).TextChanged += () => updateSeemingBenefits();
	}
}
