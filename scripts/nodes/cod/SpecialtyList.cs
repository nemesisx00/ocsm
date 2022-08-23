using Godot;
using System;
using System.Collections.Generic;
using OCSM;

public class SpecialtyList : ScrollContainer
{
	private const string InputContainer = "Grid";
	
	[Signal]
	public delegate void ValueChanged(List<Skill.Specialty> values);
	
	public List<Skill.Specialty> Values { get; set; } = new List<Skill.Specialty>();
	
	public override void _Ready()
	{
		refresh();
	}
	
	public void refresh()
	{
		foreach(Node c in GetNode<GridContainer>(InputContainer).GetChildren())
		{
			c.QueueFree();
		}
		
		foreach(var v in Values)
		{
			if(v is Skill.Specialty)
				addInput(v);
		}
		
		addInput();
	}
	
	private void skillChanged(int index) { updateValues(); }
	private void valueChanged(string text) { updateValues(); }
	
	private void updateValues()
	{
		var values = new List<Skill.Specialty>();
		var children = GetNode<GridContainer>(InputContainer).GetChildren();
		foreach(HBoxContainer row in children)
		{
			var optButton = row.GetChild<OptionButton>(0);
			var skill = Skill.byName(optButton.GetItemText(optButton.Selected));
			var value = row.GetChild<LineEdit>(1).Text;
			
			if(!String.IsNullOrEmpty(value))
				values.Add(new Skill.Specialty(skill, value));
			else if(children.IndexOf(row) != children.Count - 1)
				row.QueueFree();
		}
		
		EmitSignal(nameof(ValueChanged), values);
		
		if(children.Count <= values.Count)
		{
			addInput();
		}
	}
	
	private void addInput(Skill.Specialty value = null)
	{
		var container = GetNode<GridContainer>(InputContainer);
		
		var row = new HBoxContainer();
		row.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
		row.SizeFlagsVertical = (int)Control.SizeFlags.ShrinkCenter;
		row.RectMinSize = new Vector2(0, 25);
		
		var skill = new OptionButton();
		skill.SizeFlagsHorizontal = (int)Control.SizeFlags.Fill;
		skill.SizeFlagsVertical = (int)Control.SizeFlags.ShrinkCenter;
		skill.SizeFlagsStretchRatio = 1;
		skill.RectMinSize = new Vector2(150, 0);
		
		var skillList = OCSM.Skill.toList();
		skillList.ForEach(s => {
			var index = skillList.IndexOf(s);
			skill.AddItem(s.Name, index);
			if(value is Skill.Specialty)
				skill.Selected = index;
		});
		row.AddChild(skill);
		
		var node = new LineEdit();
		node.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
		node.SizeFlagsVertical = (int)Control.SizeFlags.ExpandFill;
		node.SizeFlagsVertical = 10;
		node.RectMinSize = new Vector2(0, 25);
		node.HintTooltip = "Enter a new Specialty";
		if(value is Skill.Specialty)
			node.Text = value.Value;
		row.AddChild(node);
		
		container.AddChild(row);
		skill.Connect(Constants.Signal.ItemSelected, this, nameof(skillChanged));
		node.Connect(Constants.Signal.TextChanged, this, nameof(valueChanged));
	}
}
