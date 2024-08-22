using Godot;
using System.Linq;
using System.Collections.Generic;
using Ocsm.Nodes;

namespace Ocsm.Cofd.Nodes;

public partial class MeritList : ItemDotsList
{
	[Signal]
	public delegate void MeritChangedEventHandler(Transport<List<Merit>> transport);
	
	public new List<Merit> Values { get; set; } = [];
	
	public override void _Ready()
	{
		ItemLabel = "Merit";
		
		Refresh();
	}
	
	public override void Refresh()
	{
		foreach(var child in GetChildren())
			child.QueueFree();
		
		foreach(var merit in Values.Where(m => m is not null))
			addInput(merit.Name, merit.Value);
		
		if(sortItems)
			sortChildren();
		
		addInput();
	}
	
	protected override void updateValues()
	{
		removeEmpties();
		
		Values = [.. GetChildren()
			.Select(node => new Merit()
			{
				Name = node.GetChild<TextEdit>(0).Text,
				Value = node.GetChild<TrackSimple>(1).Value
			})
			.OrderBy(m => m)];
		
		EmitSignal(SignalName.ValueChanged, new Transport<List<Merit>>(Values));
		
		if(sortItems)
			sortChildren();
		
		addInput();
	}
}
