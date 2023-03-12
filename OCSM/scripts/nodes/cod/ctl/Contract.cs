using Godot;
using System;
using System.Collections.Generic;
using OCSM.CoD;
using OCSM.CoD.CtL;
using OCSM.CoD.CtL.Meta;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes.CoD.CtL
{
	public partial class Contract : MarginContainer
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
		
		public List<Pair<string, string>> SeemingBenefits { get; set; } = new List<Pair<string, string>>();
		
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
			attribute3Input.ItemSelected += contestedAttributeChanged;
			
			refreshSeemingBenefits();
		}
		
		public void clearInputs()
		{
			GetNode<AttributeOptionButton>(NodePath.AttributeInput).Deselect();
			attribute2Input.Deselect();
			attribute3Input.Deselect();
			skillInput.Deselect();
			GetNode<ContractRegaliaOptionButton>(NodePath.RegaliaInput).Deselect();
			GetNode<OptionButton>(NodePath.ContractTypeInput).Deselect();
			
			GetNode<LineEdit>(NodePath.NameInput).Text = String.Empty;
			GetNode<OptionButton>(NodePath.ActionInput).Deselect();
			GetNode<LineEdit>(NodePath.CostInput).Text = String.Empty;
			GetNode<TextEdit>(NodePath.DescriptionInput).Text = String.Empty;
			GetNode<LineEdit>(NodePath.DurationInput).Text = String.Empty;
			GetNode<TextEdit>(NodePath.EffectsInput).Text = String.Empty;
			GetNode<TextEdit>(NodePath.FailureInput).Text = String.Empty;
			GetNode<TextEdit>(NodePath.FailureDramaticInput).Text = String.Empty;
			GetNode<TextEdit>(NodePath.SuccessInput).Text = String.Empty;
			GetNode<TextEdit>(NodePath.SuccessExceptionalInput).Text = String.Empty;
			GetNode<TextEdit>(NodePath.LoopholeInput).Text = String.Empty;
			
			SeemingBenefits.Clear();
			
			actionChanged(0);
			attributeChanged(0);
			contestedAttributeChanged(0);
			refreshSeemingBenefits();
		}
		
		public OCSM.CoD.CtL.Contract getData()
		{
			var attr1Node = GetNode<AttributeOptionButton>(NodePath.AttributeInput);
			var regaliaNode = GetNode<ContractRegaliaOptionButton>(NodePath.RegaliaInput);
			var contractTypeNode = GetNode<ContractTypeButton>(NodePath.ContractTypeInput);
			
			var name = GetNode<LineEdit>(NodePath.NameInput).Text;
			var cost = GetNode<LineEdit>(NodePath.CostInput).Text;
			var description = GetNode<TextEdit>(NodePath.DescriptionInput).Text;
			var duration = GetNode<LineEdit>(NodePath.DurationInput).Text;
			var effects = GetNode<TextEdit>(NodePath.EffectsInput).Text;
			var failure = GetNode<TextEdit>(NodePath.FailureInput).Text;
			var FailureDramatic = GetNode<TextEdit>(NodePath.FailureDramaticInput).Text;
			var success = GetNode<TextEdit>(NodePath.SuccessInput).Text;
			var successExceptional = GetNode<TextEdit>(NodePath.SuccessExceptionalInput).Text;
			var loophole = GetNode<TextEdit>(NodePath.LoopholeInput).Text;
			
			var actionNode = GetNode<OptionButton>(NodePath.ActionInput);
			var action = actionNode.GetSelectedItemText();
			
			var attribute = OCSM.CoD.Attribute.byName(attr1Node.GetSelectedItemText());
			var attributeResisted = OCSM.CoD.Attribute.byName(attribute2Input.GetSelectedItemText());
			var attributeContested = OCSM.CoD.Attribute.byName(attribute3Input.GetSelectedItemText());
			var skill = Skill.byName(skillInput.GetSelectedItemText());
			var regalia = regaliaNode.GetSelectedItemText();
			var contractType = contractTypeNode.GetSelectedItemText();
			
			var container = metadataManager.Container;
			
			ContractRegalia regaliaObj = null;
			ContractType contractTypeObj = null;
			if(metadataManager.Container is CoDChangelingContainer ccc2)
			{
				if(ccc2.Regalias.Find(r => r.Name.Equals(regalia)) is Regalia r)
					regaliaObj = ContractRegalia.From(r);
				else if(ccc2.Courts.Find(c => c.Name.Equals(regalia)) is Court c)
					regaliaObj = ContractRegalia.From(c);
				
				contractTypeObj = ccc2.ContractTypes.Find(ct => ct.Name.Equals(contractType));
			}
			
			if(!(regaliaObj is ContractRegalia) && regalia.Equals(ContractRegalia.Goblin.Name))
				regaliaObj = ContractRegalia.Goblin;
			
			SeemingBenefits.Sort();
			
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
				SeemingBenefits = SeemingBenefits,
				Skill = skill,
			};
		}
		
		public void setData(OCSM.CoD.CtL.Contract contract)
		{
			if(metadataManager.Container is CoDChangelingContainer ccc)
			{
				var attribute1 = GetNode<AttributeOptionButton>(NodePath.AttributeInput);
				if(contract.Attribute is OCSM.CoD.Attribute)
					attribute1.SelectItemByText(contract.Attribute.Name);
				if(contract.AttributeContested is OCSM.CoD.Attribute)
					attribute2Input.SelectItemByText(contract.AttributeContested.Name);
				if(contract.AttributeResisted is OCSM.CoD.Attribute)
					attribute3Input.SelectItemByText(contract.AttributeResisted.Name);
				if(contract.Skill is OCSM.CoD.Skill)
					skillInput.SelectItemByText(contract.Skill.Name);
				if(contract.Regalia is ContractRegalia)
					GetNode<ContractRegaliaOptionButton>(NodePath.RegaliaInput).SelectItemByText(contract.Regalia.Name);
				if(contract.ContractType is ContractType)
					GetNode<ContractTypeButton>(NodePath.ContractTypeInput).SelectItemByText(contract.ContractType.Name);
				
				var actionOption = GetNode<ActionOptionButton>(NodePath.ActionInput);
				GetNode<LineEdit>(NodePath.NameInput).Text = contract.Name;
				actionOption.SelectItemByText(contract.Action);
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
				actionChanged(actionOption.Selected, false);
				attributeChanged(attribute1.Selected);
				contestedAttributeChanged(attribute2Input.Selected);
			}
		}
		
		public void refreshSeemingBenefits()
		{
			foreach(Node c in seemingBenefitsRow.GetChildren())
			{
				if(c is HBoxContainer)
					c.QueueFree();
			}
			
			SeemingBenefits.Sort();
			SeemingBenefits.ForEach(sb => addSeemingBenefitInput(sb.Key, sb.Value));
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
				skillInput.Deselect();
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
			var benefits = new List<Pair<string, string>>();
			var children = seemingBenefitsRow.GetChildren();
			var lastIndex = children.Count - 1;
			foreach(Node c in children)
			{
				if(c is HBoxContainer)
				{
					var seemingNode = c.GetNode<SeemingOptionButton>(NodePath.SeemingInput);
					var seeming = seemingNode.GetSelectedItemText();
					var benefit = c.GetNode<TextEdit>(NodePath.BenefitInput).Text;
					
					if(!children.IndexOf(c).Equals(lastIndex) && String.IsNullOrEmpty(seeming) && String.IsNullOrEmpty(benefit))
						c.QueueFree();
					else if(!String.IsNullOrEmpty(seeming) || !String.IsNullOrEmpty(benefit))
						benefits.Add(new Pair<string, string>() { Key = seeming, Value = benefit });
				}
			}
			
			SeemingBenefits = benefits;
			SeemingBenefits.Sort();
			
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
				instance.GetChild<SeemingOptionButton>(0).SelectItemByText(seeming);
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
