using Godot;
using System;
using System.Collections.Generic;
using OCSM.CoD;
using OCSM.CoD.CtL;
using OCSM.CoD.CtL.Meta;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes.CoD.CtL
{
	public partial class Contract : VBoxContainer
	{
		public sealed class NodePath
		{
			public const string ActionInput = "%Action";
			public const string AttributeInput = "%Attribute";
			public const string Attribute2Input = "%Attribute2";
			public const string Attribute3Input = "%Attribute3";
			public const string Attribute2Minus = "%Attribute2Minus";
			public const string BenefitInput = "%Benefit";
			public const string ContractTypeInput = "%ContractType";
			public const string CostInput = "%Cost";
			public const string DetailsInput = "%Details";
			public const string DescriptionInput = "%Description";
			public const string DurationInput = "%Duration";
			public const string EffectsInput = "%Effects";
			public const string FailureInput = "%Failure";
			public const string FailureDramaticInput = "%DramaticFailure";
			public const string LoopholeInput = "%Loophole";
			public const string NameInput = "%Name";
			public const string RegaliaInput = "%Regalia";
			public const string SeemingInput = "%Seeming";
			public const string SeemingBenefitsRow = "%SeemingBenefitsRow";
			public const string SkillInput = "%Skill";
			public const string SkillPlus = "%SkillPlus";
			public const string SuccessInput = "%Success";
			public const string SuccessExceptionalInput = "%ExceptionalSuccess";
			public const string ToggleDetails = "%ToggleDetails";
			public const string Versus = "%Vs";
			public const string Wyrd = "%Wyrd";
			public const string Wyrd2 = "%Wyrd2";
		}
		
		private const int ActionContestedIndex = 4;
		private const int ActionResistedIndex = 5;
		
		public Dictionary<string, string> SeemingBenefits { get; set; } = new Dictionary<string, string>();
		
		private MetadataManager metadataManager;
		
		private AttributeOptionButton attribute2Input;
		private Control attribute2Minus;
		private AttributeOptionButton attribute3Input;
		private VBoxContainer detailsInput;
		private VBoxContainer seemingBenefitsRow;
		private SkillOptionButton skillInput;
		private Control skillPlus;
		private Label versus;
		private Control wyrd1;
		private Control wyrd2;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			
			attribute2Input = GetNode<AttributeOptionButton>(NodePath.Attribute2Input);
			attribute2Minus = GetNode<Control>(NodePath.Attribute2Minus);
			attribute3Input = GetNode<AttributeOptionButton>(NodePath.Attribute3Input);
			detailsInput = GetNode<VBoxContainer>(NodePath.DetailsInput);
			seemingBenefitsRow = GetNode<VBoxContainer>(NodePath.SeemingBenefitsRow);
			skillInput = GetNode<SkillOptionButton>(NodePath.SkillInput);
			skillPlus = GetNode<Control>(NodePath.SkillPlus);
			versus = GetNode<Label>(NodePath.Versus);
			wyrd1 = GetNode<Control>(NodePath.Wyrd);
			wyrd2 = GetNode<Control>(NodePath.Wyrd2);
			
			GetNode<OptionButton>(NodePath.ActionInput).ItemSelected += actionChanged;
			GetNode<TextureButton>(NodePath.ToggleDetails).Pressed += toggleDetails;
			GetNode<AttributeOptionButton>(NodePath.AttributeInput).ItemSelected += attributeChanged;
			GetNode<AttributeOptionButton>(NodePath.Attribute3Input).ItemSelected += contestedAttributeChanged;
			
			refreshSeemingBenefits();
		}
		
		public void clearInputs()
		{
			GetNode<AttributeOptionButton>(NodePath.AttributeInput).Selected = 0;
			GetNode<AttributeOptionButton>(NodePath.Attribute2Input).Selected = 0;
			GetNode<AttributeOptionButton>(NodePath.Attribute3Input).Selected = 0;
			GetNode<SkillOptionButton>(NodePath.SkillInput).Selected = 0;
			GetNode<RegaliaOptionButton>(NodePath.RegaliaInput).Selected = 0;
			GetNode<OptionButton>(NodePath.ContractTypeInput).Selected = 0;
			
			GetNode<LineEdit>(NodePath.NameInput).Text = String.Empty;
			GetNode<OptionButton>(NodePath.ActionInput).Selected = 0;
			GetNode<LineEdit>(NodePath.CostInput).Text = String.Empty;
			GetNode<TextEdit>(NodePath.DescriptionInput).Text = String.Empty;
			GetNode<LineEdit>(NodePath.DurationInput).Text = String.Empty;
			GetNode<TextEdit>(NodePath.EffectsInput).Text = String.Empty;
			SeemingBenefits = new Dictionary<string, string>();
			GetNode<TextEdit>(NodePath.FailureInput).Text = String.Empty;
			GetNode<TextEdit>(NodePath.FailureDramaticInput).Text = String.Empty;
			GetNode<TextEdit>(NodePath.SuccessInput).Text = String.Empty;
			GetNode<TextEdit>(NodePath.SuccessExceptionalInput).Text = String.Empty;
			GetNode<TextEdit>(NodePath.LoopholeInput).Text = String.Empty;
			
			actionChanged(0);
			attributeChanged(0);
			contestedAttributeChanged(0);
			refreshSeemingBenefits();
		}
		
		public OCSM.CoD.CtL.Contract getData()
		{
			var attr1Node = GetNode<AttributeOptionButton>(NodePath.AttributeInput);
			var attr2Node = GetNode<AttributeOptionButton>(NodePath.Attribute2Input);
			var attr3Node = GetNode<AttributeOptionButton>(NodePath.Attribute3Input);
			var skillNode = GetNode<SkillOptionButton>(NodePath.SkillInput);
			var regaliaNode = GetNode<RegaliaOptionButton>(NodePath.RegaliaInput);
			var contractTypeNode = GetNode<ContractTypeButton>(NodePath.ContractTypeInput);
			
			var name = GetNode<LineEdit>(NodePath.NameInput).Text;
			var action = GetNode<OptionButton>(NodePath.ActionInput).Selected;
			var cost = GetNode<LineEdit>(NodePath.CostInput).Text;
			var description = GetNode<TextEdit>(NodePath.DescriptionInput).Text;
			var duration = GetNode<LineEdit>(NodePath.DurationInput).Text;
			var effects = GetNode<TextEdit>(NodePath.EffectsInput).Text;
			var seemingBenefits = SeemingBenefits;
			var failure = GetNode<TextEdit>(NodePath.FailureInput).Text;
			var FailureDramatic = GetNode<TextEdit>(NodePath.FailureDramaticInput).Text;
			var success = GetNode<TextEdit>(NodePath.SuccessInput).Text;
			var successExceptional = GetNode<TextEdit>(NodePath.SuccessExceptionalInput).Text;
			var loophole = GetNode<TextEdit>(NodePath.LoopholeInput).Text;
			
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
			
			return new OCSM.CoD.CtL.Contract()
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
				SeemingBenefits = seemingBenefits,
				Skill = skill,
			};
		}
		
		public void setData(OCSM.CoD.CtL.Contract contract)
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				var attributeIndex = 0;
				var contestedIndex = 0;
				if(contract.Attribute is OCSM.CoD.Attribute)
					attributeIndex = OCSM.CoD.Attribute.asList().FindIndex(a => a.Name.Equals(contract.Attribute.Name)) + 1;
				if(contract.AttributeContested is OCSM.CoD.Attribute)
					contestedIndex = OCSM.CoD.Attribute.asList().FindIndex(a => a.Name.Equals(contract.AttributeContested.Name)) + 1;
				
				GetNode<AttributeOptionButton>(NodePath.AttributeInput).Selected = attributeIndex;
				GetNode<AttributeOptionButton>(NodePath.Attribute2Input).Selected = contestedIndex;
				GetNode<AttributeOptionButton>(NodePath.Attribute3Input).Selected = OCSM.CoD.Attribute.asList().IndexOf(contract.AttributeResisted) + 1;
				GetNode<SkillOptionButton>(NodePath.SkillInput).Selected = OCSM.CoD.Skill.asList().IndexOf(contract.Skill) + 1;
				GetNode<RegaliaOptionButton>(NodePath.RegaliaInput).Selected = ccc.Regalias.IndexOf(contract.Regalia) + 1;
				GetNode<ContractTypeButton>(NodePath.ContractTypeInput).Selected = ccc.ContractTypes.IndexOf(contract.ContractType) + 1;
				
				GetNode<LineEdit>(NodePath.NameInput).Text = contract.Name;
				GetNode<OptionButton>(NodePath.ActionInput).Selected = contract.Action;
				GetNode<LineEdit>(NodePath.CostInput).Text = contract.Cost;
				GetNode<TextEdit>(NodePath.DescriptionInput).Text = contract.Description;
				GetNode<LineEdit>(NodePath.DurationInput).Text = contract.Duration;
				GetNode<TextEdit>(NodePath.EffectsInput).Text = contract.Effects;
				GetNode<TextEdit>(NodePath.FailureInput).Text = contract.RollFailure;
				GetNode<TextEdit>(NodePath.FailureDramaticInput).Text = contract.RollFailureDramatic;
				GetNode<TextEdit>(NodePath.SuccessInput).Text = contract.RollSuccess;
				GetNode<TextEdit>(NodePath.SuccessExceptionalInput).Text = contract.RollSuccessExceptional;
				GetNode<TextEdit>(NodePath.LoopholeInput).Text = contract.Loophole;
				
				SeemingBenefits = contract.SeemingBenefits;
				
				refreshSeemingBenefits();
				actionChanged(contract.Action, false);
				attributeChanged(attributeIndex);
				contestedAttributeChanged(contestedIndex);
			}
		}
		
		public void refreshSeemingBenefits()
		{
			foreach(Node c in seemingBenefitsRow.GetChildren())
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
			if(detailsInput.Visible)
				detailsInput.Hide();
			else
				detailsInput.Show();
		}
		
		public void actionChanged(long index)
		{
			actionChanged(index, true);
		}
		
		public void actionChanged(long index, bool reset = true)
		{
			if(ActionContestedIndex.Equals(index))
			{
				versus.Show();
				attribute3Input.Show();
			}
			else
			{
				versus.Hide();
				attribute3Input.Hide();
				if(reset)
					attribute3Input.Selected = 0;
				wyrd2.Hide();
			}
			
			if(ActionResistedIndex.Equals(index))
			{
				attribute2Input.Show();
				attribute2Minus.Show();
			}
			else
			{
				attribute2Input.Hide();
				if(reset)
					attribute2Input.Selected = 0;
				attribute2Minus.Hide();
			}
		}
		
		public void attributeChanged(long index)
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
				skillInput.Selected = 0;
				skillPlus.Hide();
				wyrd1.Hide();
			}
		}
		
		public void contestedAttributeChanged(long index)
		{
			if(index > 0)
				wyrd2.Show();
			else
				wyrd2.Hide();
		}
		
		private void updateSeemingBenefits()
		{
			var benefits = new Dictionary<string, string>();
			var children = seemingBenefitsRow.GetChildren();
			foreach(Node c in children)
			{
				if(c is HBoxContainer)
				{
					var seemingNode = c.GetNode<SeemingOptionButton>(NodePath.SeemingInput);
					var seeming = String.Empty;
					if(seemingNode.Selected > -1)
						seeming = seemingNode.GetItemText(seemingNode.Selected);
					var benefit = c.GetNode<TextEdit>(NodePath.BenefitInput).Text;
					
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
			var resource = GD.Load<PackedScene>(Constants.Scene.CoD.Changeling.SeemingBenefit);
			var instance = resource.Instantiate<HBoxContainer>();
			seemingBenefitsRow.AddChild(instance);
			
			//Set the values after adding the child, as we need the _Ready() function to populate the SeemingOptionButton before the index will match a given item.
			if(!String.IsNullOrEmpty(seeming) && !String.IsNullOrEmpty(benefit))
			{
				if(metadataManager.Container is CoDChangelingContainer ccc)
				{
					instance.GetChild<SeemingOptionButton>(0).Selected = ccc.Seemings.FindIndex(s => s.Name.Equals(seeming)) + 1;
				}
				var text = instance.GetChild<TextEdit>(1);
				text.Text = benefit;
			}
			instance.GetChild<SeemingOptionButton>(0).ItemSelected += seemingChanged;
			instance.GetChild<TextEdit>(1).TextChanged += benefitChanged;
		}
		
		private void seemingChanged(long index) { updateSeemingBenefits(); }
		private void benefitChanged() { updateSeemingBenefits(); }
	}
}
