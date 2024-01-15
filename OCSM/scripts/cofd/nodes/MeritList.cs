using Godot;
using System.Linq;
using System.Collections.Generic;
using Ocsm.Nodes;

namespace Ocsm.Cofd.Nodes;

public partial class MeritList : ItemDotsList
{
	[Signal]
	public delegate void MeritValueChangedEventHandler(Transport<List<Merit>> transport);
	
	public new List<Merit> Values { get; set; } = [];
	
	private const string DefaultItemLabel = "Merit";
	
	public override void _Ready()
	{
		ItemLabel = DefaultItemLabel;
		
		Refresh();
	}
	
	public override void Refresh()
	{
		GetChildren().ToList()
			.ForEach(n => n.QueueFree());
		
		Values.Where(m => m is not null)
			.ToList()
			.ForEach(m => addInput(m.Name, m.Value));
		
		if(sortItems)
			sortChildren();
		
		addInput();
	}
	
	protected override void updateValues()
	{
		removeEmpties();
		
		var values = GetChildren()
			.Select(node => new Merit() { Name = node.GetChild<TextEdit>(0).Text, Value = node.GetChild<TrackSimple>(1).Value })
			.OrderBy(m => m)
			.ToList();
		
		Values = values;
		_ = EmitSignal(SignalName.MeritValueChanged, new Transport<List<Merit>>(Values));
		
		if(sortItems)
			sortChildren();
		
		addInput();
	}
}
