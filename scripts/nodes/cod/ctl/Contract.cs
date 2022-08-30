using Godot;
using System;
using System.Collections.Generic;
using OCSM;
using OCSM.CoD;
using OCSM.CoD.CtL;
using OCSM.CoD.CtL.Meta;

public class Contract : VBoxContainer
{
	public const string ActionInput = "Action";
	private const int ActionContestedIndex = 4;
	private const int ActionResistedIndex = 5;
	public const string AttributeInput = "Attribute";
	public const string Attribute2Input = "Attribute2";
	public const string Attribute3Input = "Attribute3";
	private const string Attribute2Minus = "Attribute2Minus";
	private const string BenefitInput = "Benefit";
	public const string ContractTypeInput = "ContractType";
	public const string CostInput = "Cost";
	public const string DetailsInput = "Details";
	public const string DescriptionInput = "Description";
	public const string DurationInput = "Duration";
	public const string EffectsInput = "Effects";
	public const string FailureInput = "Failure";
	public const string FailureDramaticInput = "DramaticFailure";
	public const string LoopholeInput = "Loophole";
	public const string NameInput = "Name";
	public const string RegaliaInput = "Regalia";
	private const string SeemingInput = "Seeming";
	private const string SeemingBenefitsRow = "SeemingBenefitsRow";
	public const string SkillInput = "Skill";
	private const string SkillPlus = "SkillPlus";
	public const string SuccessInput = "Success";
	public const string SuccessExceptionalInput = "ExceptionalSuccess";
	private const string ToggleDetails = "ToggleDetails";
	private const string Versus = "Vs";
	private const string Wyrd = "Wyrd";
	private const string Wyrd2 = "Wyrd2";
	
	public Dictionary<string, string> SeemingBenefits { get; set; } = new Dictionary<string, string>();
	
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		
		GetNode<OptionButton>(NodePathBuilder.SceneUnique(ActionInput)).Connect(Constants.Signal.ItemSelected, this, nameof(actionChanged));
		GetNode<TextureButton>(NodePathBuilder.SceneUnique(ToggleDetails)).Connect(Constants.Signal.Pressed, this, nameof(toggleDetails));
		GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(AttributeInput)).Connect(Constants.Signal.ItemSelected, this, nameof(attributeChanged));
		GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Attribute3Input)).Connect(Constants.Signal.ItemSelected, this, nameof(contestedAttributeChanged));
		
		refreshSeemingBenefits();
	}
	
	public void clearInputs()
	{
		GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(AttributeInput)).Selected = 0;
		GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Attribute2Input)).Selected = 0;
		GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Attribute3Input)).Selected = 0;
		GetNode<SkillOptionButton>(NodePathBuilder.SceneUnique(SkillInput)).Selected = 0;
		GetNode<RegaliaOptionButton>(NodePathBuilder.SceneUnique(RegaliaInput)).Selected = 0;
		GetNode<OptionButton>(NodePathBuilder.SceneUnique(ContractTypeInput)).Selected = 0;
		
		GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text = String.Empty;
		GetNode<OptionButton>(NodePathBuilder.SceneUnique(ActionInput)).Selected = 0;
		GetNode<LineEdit>(NodePathBuilder.SceneUnique(CostInput)).Text = String.Empty;
		GetNode<TextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text = String.Empty;
		GetNode<LineEdit>(NodePathBuilder.SceneUnique(DurationInput)).Text = String.Empty;
		GetNode<TextEdit>(NodePathBuilder.SceneUnique(EffectsInput)).Text = String.Empty;
		SeemingBenefits = new Dictionary<string, string>();
		GetNode<TextEdit>(NodePathBuilder.SceneUnique(FailureInput)).Text = String.Empty;
		GetNode<TextEdit>(NodePathBuilder.SceneUnique(FailureDramaticInput)).Text = String.Empty;
		GetNode<TextEdit>(NodePathBuilder.SceneUnique(SuccessInput)).Text = String.Empty;
		GetNode<TextEdit>(NodePathBuilder.SceneUnique(SuccessExceptionalInput)).Text = String.Empty;
		GetNode<TextEdit>(NodePathBuilder.SceneUnique(LoopholeInput)).Text = String.Empty;
		
		actionChanged(0);
		attributeChanged(0);
		contestedAttributeChanged(0);
		refreshSeemingBenefits();
	}
	
	public OCSM.CoD.CtL.Contract getData()
	{
		var attr1Node = GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(AttributeInput));
		var attr2Node = GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Attribute2Input));
		var attr3Node = GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Attribute3Input));
		var skillNode = GetNode<SkillOptionButton>(NodePathBuilder.SceneUnique(SkillInput));
		var regaliaNode = GetNode<RegaliaOptionButton>(NodePathBuilder.SceneUnique(RegaliaInput));
		var contractTypeNode = GetNode<ContractTypeButton>(NodePathBuilder.SceneUnique(ContractTypeInput));
		
		var name = GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text;
		var action = GetNode<OptionButton>(NodePathBuilder.SceneUnique(ActionInput)).Selected;
		var cost = GetNode<LineEdit>(NodePathBuilder.SceneUnique(CostInput)).Text;
		var description = GetNode<TextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text;
		var duration = GetNode<LineEdit>(NodePathBuilder.SceneUnique(DurationInput)).Text;
		var effects = GetNode<TextEdit>(NodePathBuilder.SceneUnique(EffectsInput)).Text;
		var seemingBenefits = SeemingBenefits;
		var failure = GetNode<TextEdit>(NodePathBuilder.SceneUnique(FailureInput)).Text;
		var FailureDramatic = GetNode<TextEdit>(NodePathBuilder.SceneUnique(FailureDramaticInput)).Text;
		var success = GetNode<TextEdit>(NodePathBuilder.SceneUnique(SuccessInput)).Text;
		var successExceptional = GetNode<TextEdit>(NodePathBuilder.SceneUnique(SuccessExceptionalInput)).Text;
		var loophole = GetNode<TextEdit>(NodePathBuilder.SceneUnique(LoopholeInput)).Text;
		
		var attribute = OCSM.CoD.Attribute.byName(attr1Node.GetItemText(attr1Node.Selected));
		var attributeResisted = OCSM.CoD.Attribute.byName(attr2Node.GetItemText(attr2Node.Selected));
		var attributeContested = OCSM.CoD.Attribute.byName(attr3Node.GetItemText(attr3Node.Selected));
		var skill = Skill.byName(skillNode.GetItemText(skillNode.Selected));
		var regalia = String.Empty;
		if(regaliaNode.Selected >= 0)
			regalia = regaliaNode.GetItemText(regaliaNode.Selected);
		var contractType = String.Empty;
		if(contractTypeNode.Selected >= 0)
			contractType = contractTypeNode.GetItemText(contractTypeNode.Selected);
		
		var container = metadataManager.Container;
		var regaliaAndContractTypeExist = true;
		if(container is CoDChangelingContainer ccc)
		{
			regaliaAndContractTypeExist = ccc.Regalias.Find(r => r.Name.Equals(regalia)) is Regalia
				&& ccc.ContractTypes.Find(ct => ct.Name.Equals(contractType)) is ContractType;
		}
		
		Regalia regaliaObj = null;
		ContractType contractTypeObj = null;
		if(metadataManager.Container is CoDChangelingContainer ccc2)
		{
			regaliaObj = ccc2.Regalias.Find(r => r.Name.Equals(regalia));
			contractTypeObj = ccc2.ContractTypes.Find(ct => ct.Name.Equals(contractType));
		}
		
		return new OCSM.CoD.CtL.Contract(name, description)
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
		};
	}
	
	public void setData(OCSM.CoD.CtL.Contract contract)
	{
		if(metadataManager.Container is CoDChangelingContainer ccc)
		{
			GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(AttributeInput)).Selected = OCSM.CoD.Attribute.asList().IndexOf(contract.Attribute) + 1;
			GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Attribute2Input)).Selected = OCSM.CoD.Attribute.asList().IndexOf(contract.AttributeContested) + 1;
			GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Attribute3Input)).Selected = OCSM.CoD.Attribute.asList().IndexOf(contract.AttributeResisted) + 1;
			GetNode<SkillOptionButton>(NodePathBuilder.SceneUnique(SkillInput)).Selected = OCSM.CoD.Skill.asList().IndexOf(contract.Skill) + 1;
			GetNode<RegaliaOptionButton>(NodePathBuilder.SceneUnique(RegaliaInput)).Selected = ccc.Regalias.IndexOf(contract.Regalia) + 1;
			GetNode<ContractTypeButton>(NodePathBuilder.SceneUnique(ContractTypeInput)).Selected = ccc.ContractTypes.IndexOf(contract.ContractType) + 1;
			
			GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text = contract.Name;
			GetNode<OptionButton>(NodePathBuilder.SceneUnique(ActionInput)).Selected = contract.Action;
			GetNode<LineEdit>(NodePathBuilder.SceneUnique(CostInput)).Text = contract.Cost;
			GetNode<TextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text = contract.Description;
			GetNode<LineEdit>(NodePathBuilder.SceneUnique(DurationInput)).Text = contract.Duration;
			GetNode<TextEdit>(NodePathBuilder.SceneUnique(EffectsInput)).Text = contract.Effects;
			GetNode<TextEdit>(NodePathBuilder.SceneUnique(FailureInput)).Text = contract.RollFailure;
			GetNode<TextEdit>(NodePathBuilder.SceneUnique(FailureDramaticInput)).Text = contract.RollFailureDramatic;
			GetNode<TextEdit>(NodePathBuilder.SceneUnique(SuccessInput)).Text = contract.RollSuccess;
			GetNode<TextEdit>(NodePathBuilder.SceneUnique(SuccessExceptionalInput)).Text = contract.RollSuccessExceptional;
			GetNode<TextEdit>(NodePathBuilder.SceneUnique(LoopholeInput)).Text = contract.Loophole;
			
			SeemingBenefits = contract.SeemingBenefits;
		}
	}
	
	public void refreshSeemingBenefits()
	{
		var row = GetNode<VBoxContainer>(NodePathBuilder.SceneUnique(SeemingBenefitsRow));
		foreach(Node c in row.GetChildren())
		{
			if(c is HBoxContainer)
				c.QueueFree();
		}
		
		foreach(var seeming in SeemingBenefits.Keys)
		{
			addSeemingBenefitInput(seeming, SeemingBenefits[seeming]);
		}
		
		addSeemingBenefitInput();
	}
	
	public void toggleDetails()
	{
		var node = GetNode<VBoxContainer>(NodePathBuilder.SceneUnique(DetailsInput));
		if(node.Visible)
			node.Hide();
		else
			node.Show();
	}
	
	public void actionChanged(int index)
	{
		var attr3 = GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Attribute3Input));
		if(ActionContestedIndex.Equals(index))
		{
			GetNode<Label>(NodePathBuilder.SceneUnique(Versus)).Show();
			attr3.Show();
		}
		else
		{
			GetNode<Label>(NodePathBuilder.SceneUnique(Versus)).Hide();
			attr3.Hide();
			attr3.Selected = 0;
			GetNode<Control>(NodePathBuilder.SceneUnique(Wyrd2)).Hide();
		}
		
		var attr2 = GetNode<AttributeOptionButton>(NodePathBuilder.SceneUnique(Attribute2Input));
		if(ActionResistedIndex.Equals(index))
		{
			attr2.Show();
			GetNode<Control>(NodePathBuilder.SceneUnique(Attribute2Minus)).Show();
		}
		else
		{
			attr2.Hide();
			attr2.Selected = 0;
			GetNode<Control>(NodePathBuilder.SceneUnique(Attribute2Minus)).Hide();
		}
	}
	
	public void attributeChanged(int index)
	{
		var skill = GetNode<SkillOptionButton>(NodePathBuilder.SceneUnique(SkillInput));
		
		if(index > 0)
		{
			skill.Show();
			GetNode<Control>(NodePathBuilder.SceneUnique(SkillPlus)).Show();
			GetNode<Control>(NodePathBuilder.SceneUnique(Wyrd)).Show();
		}
		else
		{
			skill.Hide();
			skill.Selected = 0;
			GetNode<Control>(NodePathBuilder.SceneUnique(SkillPlus)).Hide();
			GetNode<Control>(NodePathBuilder.SceneUnique(Wyrd)).Hide();
		}
	}
	
	public void contestedAttributeChanged(int index)
	{
		if(index > 0)
		{
			GetNode<Control>(NodePathBuilder.SceneUnique(Wyrd2)).Show();
		}
		else
		{
			GetNode<Control>(NodePathBuilder.SceneUnique(Wyrd2)).Hide();
		}
	}
	
	private void updateSeemingBenefits()
	{
		var row = GetNode<VBoxContainer>(NodePathBuilder.SceneUnique(SeemingBenefitsRow));
		var benefits = new Dictionary<string, string>();
		var children = row.GetChildren();
		foreach(Node c in children)
		{
			if(c is HBoxContainer)
			{
				var seemingNode = c.GetNode<SeemingOptionButton>(NodePathBuilder.SceneUnique(SeemingInput));
				var seeming = seemingNode.GetItemText(seemingNode.Selected);
				var benefit = c.GetNode<TextEdit>(NodePathBuilder.SceneUnique(BenefitInput)).Text;
				
				if(!String.IsNullOrEmpty(seeming) && !String.IsNullOrEmpty(benefit) && !benefits.ContainsKey(seeming))
					benefits.Add(seeming, benefit);
				else if(String.IsNullOrEmpty(seeming) && String.IsNullOrEmpty(benefit) && children.IndexOf(c) != children.Count - 1)
					c.QueueFree();
			}
		}
		
		SeemingBenefits = benefits;
		
		if(children.Count <= SeemingBenefits.Count + 1)
		{
			addSeemingBenefitInput();
		}
	}
	
	private void addSeemingBenefitInput(string seeming = null, string benefit = "")
	{
		var row = GetNode<VBoxContainer>(NodePathBuilder.SceneUnique(SeemingBenefitsRow));
		var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.CoD.Changeling.SeemingBenefit);
		var instance = resource.Instance<HBoxContainer>();
		row.AddChild(instance);
		
		//Set the values after adding the child, as we need the _Ready() function to populate the SeemingOptionButton before the index will match a given item.
		if(!String.IsNullOrEmpty(seeming) && !String.IsNullOrEmpty(benefit))
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				instance.GetChild<SeemingOptionButton>(0).Selected = ccc.Seemings.FindIndex(s => s.Name.Equals(seeming)) + 1;
			}
			var text = instance.GetChild<TextEdit>(1);
			text.Text = benefit;
			NodeUtilities.autoSize(text, Constants.TextInputMinHeight);
		}
		instance.GetChild<SeemingOptionButton>(0).Connect(Constants.Signal.ItemSelected, this, nameof(seemingChanged));
		instance.GetChild<TextEdit>(1).Connect(Constants.Signal.TextChanged, this, nameof(benefitChanged));
	}
	
	private void seemingChanged(int index) { updateSeemingBenefits(); }
	private void benefitChanged() { updateSeemingBenefits(); }
}
