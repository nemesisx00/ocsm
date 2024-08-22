using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using Ocsm.Nodes;

namespace Ocsm.Cofd.Nodes;

public partial class ItemDotsList : Container
{
	[Export]
	protected bool sortItems = true;
	
	[Signal]
	public delegate void ValueChangedEventHandler(Transport<Dictionary<string, int>> transport);
	
	protected const string TooltipFormat = "Enter a new {0}";
	
	public Dictionary<string, int> Values { get; set; } = [];
	
	protected string ItemLabel { get; set; }
	
	public override void _Ready()
	{
		ItemLabel = Name;
		Refresh();
	}
	
	public virtual void Refresh()
	{
		foreach(var child in GetChildren())
		{
			child.GetChild<TrackSimple>(1).NodeChanged -= handleTrackNodeChanged;
			child.QueueFree();
		}
		
		foreach(var pair in Values)
			addInput(pair.Key, pair.Value);
		
		if(sortItems)
			sortChildren();
		
		addInput();
	}
	
	protected void removeEmpties() => GetChildren()
		.Where(node => String.IsNullOrEmpty(node.GetChild<TextEdit>(0).Text))
		.ToList()
		.ForEach(node => node.QueueFree());
	
	protected void sortChildren() => NodeUtilities.RearrangeNodes(
			this,
			[.. GetChildren()
				.OrderBy(node => node.GetChild<TextEdit>(0).Text)
				.ThenBy(node => node.GetChild<TrackSimple>(1).Value)]
		);
	
	protected virtual void addInput(string text = "", int dots = 0)
	{
		var resource = GD.Load<PackedScene>(ScenePaths.Cofd.ItemDots);
		var node = resource.Instantiate();
		var textEdit = node.GetChild<TextEdit>(0);
		textEdit.Text = text;
		textEdit.TooltipText = string.Format(TooltipFormat, ItemLabel);
		
		var track = node.GetChild<TrackSimple>(1);
		track.Value = dots;
		
		AddChild(node);
		textEdit.TextChanged += () => updateValues();
		track.NodeChanged += handleTrackNodeChanged;
	}
	
	protected void handleTrackNodeChanged(TrackSimple node) => updateValues();
	
	protected virtual void updateValues()
	{
		removeEmpties();
		
		Values = GetChildren()
			.Select(node => new KeyValuePair<string, int>(
				node.GetChild<TextEdit>(0).Text,
				node.GetChild<TrackSimple>(1).Value
			))
			.OrderBy(o => o.Key)
			.ToDictionary();
		
		EmitSignal(SignalName.ValueChanged, new Transport<Dictionary<string, int>>(Values));
		
		if(sortItems)
			sortChildren();
		
		addInput();
	}
}
