using Godot;
using System;
using System.Collections.Generic;
using OCSM;

public class Contract : VBoxContainer
{
	public const string Action = "Action";
	private const int ActionContested = 4;
	private const int ActionResisted = 5;
	public const string Attribute = "Attribute";
	public const string Attribute2 = "Attribute2";
	public const string Attribute3 = "Attribute3";
	private const string Attribute2Minus = "Attribute2Minus";
	private const string Benefit = "Benefit";
	public const string ContractType = "ContractType";
	public const string Cost = "Cost";
	public const string Details = "Details";
	public const string Description = "Description";
	public const string Duration = "Duration";
	public const string Effects = "Effects";
	public const string Failure = "Failure";
	public const string FailureDramatic = "DramaticFailure";
	public const string Loophole = "Loophole";
	public const string NameName = "Name";
	public const string Regalia = "Regalia";
	private const string Seeming = "Seeming";
	private const string SeemingBenefitsRow = "SeemingBenefitsRow";
	public const string Skill = "Skill";
	private const string SkillPlus = "SkillPlus";
	public const string Success = "Success";
	public const string SuccessExceptional = "ExceptionalSuccess";
	private const string ToggleDetails = "ToggleDetails";
	private const string Versus = "Vs";
	private const string Wyrd = "Wyrd";
	private const string Wyrd2 = "Wyrd2";
	
	public Dictionary<string, string> SeemingBenefits { get; set; } = new Dictionary<string, string>();
	
	public override void _Ready()
	{
		GetNode<OptionButton>(PathBuilder.SceneUnique(Action)).Connect(Constants.Signal.ItemSelected, this, nameof(actionChanged));
		GetNode<TextureButton>(PathBuilder.SceneUnique(ToggleDetails)).Connect(Constants.Signal.Pressed, this, nameof(toggleDetails));
		GetNode<AttributeOptionButton>(PathBuilder.SceneUnique(Attribute)).Connect(Constants.Signal.ItemSelected, this, nameof(attributeChanged));
		GetNode<AttributeOptionButton>(PathBuilder.SceneUnique(Attribute3)).Connect(Constants.Signal.ItemSelected, this, nameof(contestedAttributeChanged));
		
		refreshSeemingBenefits();
	}
	
	public void refreshSeemingBenefits()
	{
		var row = GetNode<VBoxContainer>(PathBuilder.SceneUnique(SeemingBenefitsRow));
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
	
	private void toggleDetails()
	{
		var node = GetNode<VBoxContainer>(PathBuilder.SceneUnique(Details));
		if(node.Visible)
			node.Hide();
		else
			node.Show();
	}
	
	private void actionChanged(int index)
	{
		var attr3 = GetNode<AttributeOptionButton>(PathBuilder.SceneUnique(Attribute3));
		if(ActionContested.Equals(index))
		{
			GetNode<Label>(PathBuilder.SceneUnique(Versus)).Show();
			attr3.Show();
		}
		else
		{
			GetNode<Label>(PathBuilder.SceneUnique(Versus)).Hide();
			attr3.Hide();
			attr3.Selected = 0;
			GetNode<Control>(PathBuilder.SceneUnique(Wyrd2)).Hide();
		}
		
		var attr2 = GetNode<AttributeOptionButton>(PathBuilder.SceneUnique(Attribute2));
		if(ActionResisted.Equals(index))
		{
			attr2.Show();
			GetNode<Control>(PathBuilder.SceneUnique(Attribute2Minus)).Show();
		}
		else
		{
			attr2.Hide();
			attr2.Selected = 0;
			GetNode<Control>(PathBuilder.SceneUnique(Attribute2Minus)).Hide();
		}
	}
	
	private void attributeChanged(int index)
	{
		var skill = GetNode<SkillOptionButton>(PathBuilder.SceneUnique(Skill));
		
		if(index > 0)
		{
			skill.Show();
			GetNode<Control>(PathBuilder.SceneUnique(SkillPlus)).Show();
			GetNode<Control>(PathBuilder.SceneUnique(Wyrd)).Show();
		}
		else
		{
			skill.Hide();
			skill.Selected = 0;
			GetNode<Control>(PathBuilder.SceneUnique(SkillPlus)).Hide();
			GetNode<Control>(PathBuilder.SceneUnique(Wyrd)).Hide();
		}
	}
	
	private void contestedAttributeChanged(int index)
	{
		if(index > 0)
		{
			GetNode<Control>(PathBuilder.SceneUnique(Wyrd2)).Show();
		}
		else
		{
			GetNode<Control>(PathBuilder.SceneUnique(Wyrd2)).Hide();
		}
	}
	
	private void updateSeemingBenefits()
	{
		var row = GetNode<VBoxContainer>(PathBuilder.SceneUnique(SeemingBenefitsRow));
		var benefits = new Dictionary<string, string>();
		var children = row.GetChildren();
		foreach(Node c in children)
		{
			if(c is HBoxContainer)
			{
				var seemingNode = c.GetNode<SeemingOptionButton>(PathBuilder.SceneUnique(Seeming));
				var seeming = seemingNode.GetItemText(seemingNode.Selected);
				var benefit = c.GetNode<TextEdit>(PathBuilder.SceneUnique(Benefit)).Text;
				
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
		var row = GetNode<VBoxContainer>(PathBuilder.SceneUnique(SeemingBenefitsRow));
		var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.CoD.Changeling.SeemingBenefit);
		var instance = resource.Instance<HBoxContainer>();
		row.AddChild(instance);
		
		//Set the values after adding the child, as we need the _Ready() function to populate the SeemingOptionButton before the index will match a given item.
		if(!String.IsNullOrEmpty(seeming) && !String.IsNullOrEmpty(benefit))
		{
			instance.GetChild<SeemingOptionButton>(0).Selected = OCSM.Seeming.asList().FindIndex(s => s.Equals(seeming)) + 1;
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
