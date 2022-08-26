using Godot;
using System;
using System.Collections.Generic;
using OCSM;

public class SpecialtyList : Container
{
	[Signal]
	public delegate void ValueChanged(Dictionary<string, string> values);
	
	public Dictionary<string, string> Values { get; set; } = new Dictionary<string, string>();
	
	public override void _Ready()
	{
		refresh();
	}
	
	public void refresh()
	{
		foreach(Node c in GetChildren())
		{
			c.QueueFree();
		}
		
		foreach(var key in Values.Keys)
		{
			var skill = Skill.byName(key);
			if(skill is Skill)
				addInput(skill, Values[key]);
		}
		
		addInput();
	}
	
	private void skillChanged(int index) { updateValues(); }
	private void valueChanged(string text) { updateValues(); }
	
	private void updateValues()
	{
		var values = new Dictionary<string, string>();
		var children = GetChildren();
		foreach(HBoxContainer row in children)
		{
			var optButton = row.GetChild<OptionButton>(0);
			var skill = Skill.byName(optButton.GetItemText(optButton.Selected));
			var value = row.GetChild<LineEdit>(1).Text;
			
			if(!String.IsNullOrEmpty(value))
				values.Add(skill.Name, value);
			else if(children.IndexOf(row) != children.Count - 1)
				row.QueueFree();
		}
		
		EmitSignal(nameof(ValueChanged), values);
		
		if(children.Count <= values.Count)
		{
			addInput();
		}
	}
	
	private void addInput(Skill skill = null, string specialty = "")
	{
		var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.CoD.Specialty);
		var instance = resource.Instance<HBoxContainer>();
		AddChild(instance);
		
		if(skill is Skill && !String.IsNullOrEmpty(specialty))
		{
			instance.GetChild<SkillOptionButton>(0).Selected = OCSM.Skill.asList().FindIndex(s => s.Equals(skill)) + 1;
			instance.GetChild<LineEdit>(1).Text = specialty;
		}
		
		instance.GetChild<SkillOptionButton>(0).Connect(Constants.Signal.ItemSelected, this, nameof(skillChanged));
		instance.GetChild<LineEdit>(1).Connect(Constants.Signal.TextChanged, this, nameof(valueChanged));
	}
}
