using System;
using System.Linq;
using Ocsm.Dnd.Fifth;

namespace Ocsm.Nodes.Dnd.Fifth;

public partial class NumericStatOptionsButton : CustomOption
{
	public override void _Ready() => refreshMetadata();
	
	protected override void refreshMetadata() => replaceItems(Enum.GetValues<NumericStats>()
		.Where(ns => !string.IsNullOrEmpty(ns.GetLabel()))
		.Select(ns => ns.GetLabel())
		.ToList());
}
