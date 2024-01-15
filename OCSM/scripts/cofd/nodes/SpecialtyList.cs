using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Ocsm.Cofd.Nodes;

public partial class SpecialtyList : Container
{
	private sealed class NodePaths
	{
		public static readonly NodePath Skill = new("%Skill");
		public static readonly NodePath Specialty = new("%Specialty");
	}
	
	[Signal]
	public delegate void ValueChangedEventHandler(Transport<Dictionary<Skill.EnumValues, string>> values);
	
	public Dictionary<Skill.EnumValues, string> Values { get; set; } = [];
	
	public override void _Ready()
	{
		Refresh();
	}
	
	public void Refresh()
	{
		foreach(Node c in GetChildren())
		{
			c.QueueFree();
		}
		
		Values.ToList().ForEach(entry => {
			if(entry.Key is Skill.EnumValues skill && !string.IsNullOrEmpty(entry.Value))
				addInput(skill, entry.Value);
		});
		
		addInput();
	}
	
	private void removeEmpties() => GetChildren()
		.Where(row => Skill.KindFromString(row.GetChild<OptionButton>(0).GetSelectedItemText()) is null
			&& string.IsNullOrEmpty(row.GetChild<TextEdit>(1).Text))
		.ToList()
		.ForEach(row => row.QueueFree());
	
	private void sortChildren() => NodeUtilities.RearrangeNodes(
		this,
		[.. GetChildren()
			.OrderBy(row => Skill.KindFromString(row.GetChild<OptionButton>(0).GetSelectedItemText()))]
	);
	
	private void updateValues()
	{
		removeEmpties();
		
		var values = new Dictionary<Skill.EnumValues, string>();
		// Get all skill/text pairs, even if text is empty
		var list = GetChildren()
			.Select(row => new {
				option = row.GetChild<OptionButton>(0),
				skill = Skill.KindFromString(row.GetChild<OptionButton>(0).GetSelectedItemText()),
				text = row.GetChild<TextEdit>(1).Text
			})
			.Where(o => o.skill is Skill.EnumValues)
			.OrderBy(o => o.skill)
			.ThenBy(o => o.text)
			.ToList();
		
		// Update the values
		list.ForEach(o => {
			if(o.skill is Skill.EnumValues s)
				values.Add(s, o.text);
		});
		
		Values = values;
		_ = EmitSignal(SignalName.ValueChanged, new Transport<Dictionary<Skill.EnumValues, string>>(values));
		
		// Update the option buttons' disabled items
		list.ForEach(o => o.option
			.SetDisabled(
				Values.Keys.Select(s => s.GetLabelOrName()).ToList(),
				true,
				[o.skill.GetLabelOrName()]
			)
		);
		
		sortChildren();
		// Make sure we always have one empty available
		addInput();
	}
	
	private void addInput(Skill.EnumValues? skill = null, string specialty = "")
	{
		var resource = GD.Load<PackedScene>(Constants.Scene.Cofd.Specialty);
		var instance = resource.Instantiate<HBoxContainer>();
		AddChild(instance);
		
		var option = instance.GetChild<SkillOptionButton>(0);
		var value = instance.GetChild<TextEdit>(1);
		
		Values.Keys.ToList()
			.ForEach(s => option.SetDisabledByText(s.GetLabelOrName(), true));
		
		if(skill is Skill.EnumValues s && !string.IsNullOrEmpty(specialty))
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
