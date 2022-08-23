using Godot;
using System;
using OCSM;

public class Contract : VBoxContainer
{
	private const string ToggleDetails = "ToggleDetails";
	private const string Details = "Details";
	private const string Description = "Description";
	private const string Attribute = "Attribute";
	private const string Attribute2 = "Attribute2";
	private const string Attribute2Minus = "Attribute2Minus";
	private const string Skill = "Skill";
	private const string SkillPlus = "SkillPlus";
	private const string Wyrd = "Wyrd";
	
	public override void _Ready()
	{
		GetNode<TextureButton>(PathBuilder.SceneUnique(ToggleDetails)).Connect(Constants.Signal.Pressed, this, nameof(toggleDetails));
		GetNode<TextEdit>(PathBuilder.SceneUnique(Description)).Connect(Constants.Signal.TextChanged, this, nameof(descriptionChanged));
		GetNode<AttributeOptionButton>(PathBuilder.SceneUnique(Attribute)).Connect(Constants.Signal.ItemSelected, this, nameof(attributeChanged));
	}
	
	private void toggleDetails()
	{
		var node = GetNode<VBoxContainer>(PathBuilder.SceneUnique(Details));
		if(node.Visible)
			node.Hide();
		else
			node.Show();
	}
	
	private void attributeChanged(int index)
	{
		var attr2 = GetNode<AttributeOptionButton>(PathBuilder.SceneUnique(Attribute2));
		var skill = GetNode<SkillOptionButton>(PathBuilder.SceneUnique(Skill));
		
		if(index > 0)
		{
			attr2.Show();
			GetNode<Control>(PathBuilder.SceneUnique(Attribute2Minus)).Show();
			skill.Show();
			GetNode<Control>(PathBuilder.SceneUnique(SkillPlus)).Show();
			GetNode<Control>(PathBuilder.SceneUnique(Wyrd)).Show();
		}
		else
		{
			attr2.Hide();
			attr2.Selected = 0;
			GetNode<Control>(PathBuilder.SceneUnique(Attribute2Minus)).Hide();
			skill.Hide();
			skill.Selected = 0;
			GetNode<Control>(PathBuilder.SceneUnique(SkillPlus)).Hide();
			GetNode<Control>(PathBuilder.SceneUnique(Wyrd)).Hide();
		}
	}
	
	private void descriptionChanged()
	{
		TextEditUtilities.autoSize(GetNode<TextEdit>(PathBuilder.SceneUnique(Description)), Constants.TextInputMinHeight);
	}
}
