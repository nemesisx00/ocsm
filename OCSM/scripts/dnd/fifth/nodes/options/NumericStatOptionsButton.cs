using System;
using System.Linq;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class NumericStatOptionsButton : CustomOption
{
	public override void _Ready() => refreshMetadata();
	
	protected override void refreshMetadata() => replaceItems(Enum.GetValues<NumericStats>()
		.Where(ns => !string.IsNullOrEmpty(ns.GetLabel()))
		.Select(ns => ns.GetLabel())
		.ToList());
}
