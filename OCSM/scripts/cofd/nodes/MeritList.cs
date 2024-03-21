using Godot;
using System.Linq;
using System.Collections.Generic;
using Ocsm.Cofd;

namespace Ocsm.Nodes.Cofd;

public partial class MeritList : ItemDotsList
{
	[Signal]
	public new delegate void ValueChangedEventHandler(Transport<List<Merit>> transport);
	
	public new List<Merit> Values { get; set; } = [];
	
	public override void _Ready()
	{
		ItemLabel = "Merit";
		
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
		
		var values = new List<Merit>();
		var list = GetChildren()
			.Select(node => new Merit() { Name = node.GetChild<TextEdit>(0).Text, Value = node.GetChild<TrackSimple>(1).Value })
			.OrderBy(m => m)
			.ToList();
		
		list.ForEach(m => values.Add(m));
		
		Values = values;
		EmitSignal(SignalName.ValueChanged, new Transport<List<Merit>>(Values));
		
		if(sortItems)
			sortChildren();
		addInput();
	}
}
