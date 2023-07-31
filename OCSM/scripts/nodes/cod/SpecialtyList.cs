using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Ocsm.Cofd;

namespace Ocsm.Nodes.Cofd;

public partial class SpecialtyList : Container
{
	private sealed class NodePath
	{
		public const string Skill = "%Skill";
		public const string Specialty = "%Specialty";
	}
	
	[Signal]
	public delegate void ValueChangedEventHandler(Transport<Dictionary<Skill.Enum, string>> values);
	
	public Dictionary<Skill.Enum, string> Values { get; set; } = new Dictionary<Skill.Enum, string>();
	
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
		
		Values.ToList().ForEach(entry => {
			if(entry.Key is Skill.Enum skill && !String.IsNullOrEmpty(entry.Value))
				addInput(skill, entry.Value);
		});
		
		addInput();
	}
	
	private void removeEmpties() => GetChildren()
										.Where(row => Skill.KindFromString(row.GetChild<OptionButton>(0).GetSelectedItemText()) is null
											&& String.IsNullOrEmpty(row.GetChild<TextEdit>(1).Text))
										.ToList()
										.ForEach(row => row.QueueFree());
	
	private void sortChildren() => NodeUtilities.rearrangeNodes(
										this,
										GetChildren()
											.OrderBy(row => Skill.KindFromString(row.GetChild<OptionButton>(0).GetSelectedItemText()))
											.ToList()
									);
	
	private void updateValues()
	{
		removeEmpties();
		
		var values = new Dictionary<Skill.Enum, string>();
		// Get all skill/text pairs, even if text is empty
		var list = GetChildren()
					.Select(row => new {
						option = row.GetChild<OptionButton>(0),
						skill = Skill.KindFromString(row.GetChild<OptionButton>(0).GetSelectedItemText()),
						text = row.GetChild<TextEdit>(1).Text
					})
					.Where(o => o.skill is Skill.Enum)
					.OrderBy(o => o.skill)
					.ThenBy(o => o.text)
					.ToList();
		
		// Update the values
		list.ForEach(o => {
			if(o.skill is Skill.Enum s)
				values.Add(s, o.text);
		});
		
		Values = values;
		EmitSignal(nameof(ValueChanged), new Transport<Dictionary<Skill.Enum, string>>(values));
		
		// Update the option buttons' disabled items
		list.ForEach(o => o.option
			.SetDisabled(
				Values.Keys.Select(s => s.GetLabelOrName()).ToList(),
				true,
				new List<string>() { o.skill.GetLabelOrName() }
			)
		);
		
		sortChildren();
		// Make sure we always have one empty available
		addInput();
	}
	
	private void addInput(Skill.Enum? skill = null, string specialty = "")
	{
		var resource = GD.Load<PackedScene>(Constants.Scene.Cofd.Specialty);
		var instance = resource.Instantiate<HBoxContainer>();
		AddChild(instance);
		
		var option = instance.GetChild<SkillOptionButton>(0);
		var value = instance.GetChild<TextEdit>(1);
		
		Values.Keys.ToList()
			.ForEach(s => option.SetDisabledByText(s.GetLabelOrName(), true));
		
		if(skill is Skill.Enum s && !String.IsNullOrEmpty(specialty))
		{
			var text = s.GetLabelOrName();
			option.SetDisabledByText(text, false);
			option.SelectItemByText(text);
			value.Text = specialty;
		}
		
		option.ItemSelected += i => updateValues();
		value.TextChanged += () => updateValues();
	}
}
