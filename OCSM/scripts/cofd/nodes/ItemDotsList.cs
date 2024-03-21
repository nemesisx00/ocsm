using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Ocsm.Nodes.Cofd;

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
		GetChildren().ToList()
			.ForEach(n => n.QueueFree());
		
		Values.ToList()
			.ForEach(pair => addInput(pair.Key, pair.Value));
		
		if(sortItems)
			sortChildren();
		addInput();
	}
	
	protected void removeEmpties() => GetChildren()
		.Where(node => String.IsNullOrEmpty(node.GetChild<TextEdit>(0).Text))
		.ToList()
		.ForEach(node => node.QueueFree());
	
	protected void sortChildren() => NodeUtilities.rearrangeNodes(
			this,
			[.. GetChildren()
				.OrderBy(node => node.GetChild<TextEdit>(0).Text)
				.ThenBy(node => node.GetChild<TrackSimple>(1).Value)]
		);
	
	protected virtual void addInput(string text = "", int dots = 0)
	{
		var resource = GD.Load<PackedScene>(Constants.Scene.Cofd.ItemDots);
		var node = resource.Instantiate();
		var textEdit = node.GetChild<TextEdit>(0);
		textEdit.Text = text;
		textEdit.TooltipText = string.Format(TooltipFormat, ItemLabel);
		
		var track = node.GetChild<TrackSimple>(1);
		track.Value = dots;
		
		AddChild(node);
		textEdit.TextChanged += () => updateValues();
		track.NodeChanged += n => updateValues();
	}
	
	protected virtual void updateValues()
	{
		removeEmpties();
		
		Dictionary<string, int> values = [];
		GetChildren()
			.Select(node => new { text = node.GetChild<TextEdit>(0).Text, dots = node.GetChild<TrackSimple>(1).Value })
			.OrderBy(o => o.text)
			.ToList()
			.ForEach(o => values.Add(o.text, o.dots));
		
		Values = values;
		EmitSignal(SignalName.ValueChanged, new Transport<Dictionary<string, int>>(Values));
		
		if(sortItems)
			sortChildren();
		addInput();
	}
}
