using System;
using System.Linq;
using Godot;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class NumericStatOptionsButton : OptionButton
{
	[Export]
	public bool EmptyOption { get; set; }
	
	public override void _Ready()
	{
		if(EmptyOption)
			AddItem(string.Empty);
		
		foreach(var label in Enum.GetValues<NumericStats>()
			.Where(ns => !string.IsNullOrEmpty(ns.GetLabel()))
			.Select(ns => ns.GetLabel()))
		{
			AddItem(label);
		}
	}
}
