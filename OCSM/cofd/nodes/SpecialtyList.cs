using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Ocsm.Cofd.Nodes;

public partial class SpecialtyList : Container
{
	private static class NodePaths
	{
		public static readonly NodePath Skill = new("%Skill");
		public static readonly NodePath Specialty = new("%Specialty");
	}
	
	[Signal]
	public delegate void ValueChangedEventHandler(Transport<Dictionary<Traits, string>> values);
	
	public Dictionary<Traits, string> Values { get; set; } = [];
	
	public override void _Ready() => Refresh();
	
	public void Refresh()
	{
		foreach(Node c in GetChildren())
		{
			c.QueueFree();
		}
		
		Values.ToList().ForEach(entry => {
			if(entry.Key is Traits skill && !string.IsNullOrEmpty(entry.Value))
				addInput(skill, entry.Value);
		});
		
		addInput();
	}
	
	private void removeEmpties() => GetChildren()
		.Where(row => TraitDots.KindFromString(row.GetChild<OptionButton>(0).GetSelectedItemText()) is null
			&& string.IsNullOrEmpty(row.GetChild<TextEdit>(1).Text))
		.ToList()
		.ForEach(row => row.QueueFree());
	
	private void sortChildren() => NodeUtilities.RearrangeNodes(
			this,
			[.. GetChildren()
				.OrderBy(row => TraitDots.KindFromString(row.GetChild<OptionButton>(0).GetSelectedItemText()))]
		);
	
	private void updateValues()
	{
		removeEmpties();
		
		var values = new Dictionary<Traits, string>();
		// Get all skill/text pairs, even if text is empty
		var list = GetChildren()
					.Select(row => new {
						option = row.GetChild<OptionButton>(0),
						skill = TraitDots.KindFromString(row.GetChild<OptionButton>(0).GetSelectedItemText()),
						text = row.GetChild<TextEdit>(1).Text
					})
					.Where(o => o.skill is not null)
					.OrderBy(o => o.skill)
					.ThenBy(o => o.text)
					.ToList();
		
		// Update the values
		list.ForEach(o => {
			if(o.skill is Traits s)
				values.Add(s, o.text);
		});
		
		Values = values;
		EmitSignal(SignalName.ValueChanged, new Transport<Dictionary<Traits, string>>(values));
		
		// Update the option buttons' disabled items
		list.ForEach(o => o.option
			.SetDisabled(
				Values.Keys.Select(s => s.GetLabel()).ToList(),
				true,
				[o.skill.GetLabel()]
			)
		);
		
		sortChildren();
		// Make sure we always have one empty available
		addInput();
	}
	
	private void addInput(Traits? skill = null, string specialty = "")
	{
		var resource = GD.Load<PackedScene>(ResourcePaths.Specialty);
		var instance = resource.Instantiate<HBoxContainer>();
		AddChild(instance);
		
		var option = instance.GetChild<SkillOptionButton>(0);
		var value = instance.GetChild<TextEdit>(1);
		
		Values.Keys.ToList()
			.ForEach(s => option.SetDisabledByText(s.GetLabel(), true));
		
		if(skill is Traits s && !string.IsNullOrEmpty(specialty))
		{
			var text = s.GetLabel();
			option.SetDisabledByText(text, false);
			option.SelectItemByText(text);
			value.Text = specialty;
		}
		
		option.ItemSelected += i => updateValues();
		value.TextChanged += () => updateValues();
	}
}
