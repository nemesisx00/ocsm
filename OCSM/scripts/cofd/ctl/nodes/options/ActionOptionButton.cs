using System;
using System.Linq;
using Ocsm.Nodes;

namespace Ocsm.Cofd.Ctl.Nodes;

public partial class ActionOptionButton : CustomOption
{
	public enum Action
	{
		Reflexive = 1,
		Instant,
		Extended,
		Simple,
		Contested,
		Resisted,
	}
	
	public override void _Ready() => refreshMetadata();
	
	protected override void refreshMetadata() => replaceItems(Enum.GetValues<Action>()
		.ToList()
		.Select(a => Enum.GetName(a))
		.ToList());
}
