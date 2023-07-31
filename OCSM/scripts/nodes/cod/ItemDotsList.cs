using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Ocsm.Nodes.Cofd;

public partial class ItemDotsList : Container
{
	[Export]
	protected bool SortItems = true;
	
	[Signal]
	public delegate void ValueChangedEventHandler(Transport<Dictionary<string, long>> transport);
	
	protected const string TooltipFormat = "Enter a new {0}";
	
	public Dictionary<string, long> Values { get; set; } = new Dictionary<string, long>();
	
	protected string ItemLabel { get; set; }
	
	public override void _Ready()
	{
		ItemLabel = Name;
		refresh();
	}
	
	public virtual void refresh()
	{
		GetChildren().ToList()
			.ForEach(n => n.QueueFree());
		
		Values.ToList()
			.ForEach(pair => addInput(pair.Key, pair.Value));
		
		if(SortItems)
			sortChildren();
		addInput();
	}
	
	protected void removeEmpties() => GetChildren()
										.Where(node => String.IsNullOrEmpty(node.GetChild<TextEdit>(0).Text))
										.ToList()
										.ForEach(node => node.QueueFree());
	
	protected void sortChildren() => NodeUtilities.rearrangeNodes(
										this,
										GetChildren()
											.OrderBy(node => node.GetChild<TextEdit>(0).Text)
											.ThenBy(node => node.GetChild<TrackSimple>(1).Value)
											.ToList()
									);
	
	protected virtual void addInput(string text = "", long dots = 0)
	{
		var resource = GD.Load<PackedScene>(Constants.Scene.Cofd.ItemDots);
		var node = resource.Instantiate();
		var textEdit = node.GetChild<TextEdit>(0);
		textEdit.Text = text;
		textEdit.TooltipText = String.Format(TooltipFormat, ItemLabel);
		
		var track = node.GetChild<TrackSimple>(1);
		track.Value = dots;
		
		AddChild(node);
		textEdit.TextChanged += () => updateValues();
		track.NodeChanged += n => updateValues();
	}
	
	protected virtual void updateValues()
	{
		removeEmpties();
		
		var values = new Dictionary<string, long>();
		GetChildren()
			.Select(node => new { text = node.GetChild<TextEdit>(0).Text, dots = node.GetChild<TrackSimple>(1).Value })
			.OrderBy(o => o.text)
			.ToList()
			.ForEach(o => values.Add(o.text, o.dots));
		
		Values = values;
		EmitSignal(nameof(ValueChanged), new Transport<Dictionary<string, long>>(Values));
		
		if(SortItems)
			sortChildren();
		addInput();
	}
}
