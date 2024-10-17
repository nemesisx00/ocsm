using Ocsm.Nodes;
using System;
using System.Linq;

namespace Ocsm.Cofd.Ctl.Nodes;

public partial class ActionOptionButton : FixedOption
{
	public enum Action
	{
		Reflexive = 1,
		Instant,
		Extended,
		Simple,
		Contested,
		Resisted
	}
	
	protected override void refreshMetadata() => replaceItems(Enum.GetValues<Action>()
		.Select(a => Enum.GetName(a))
		.ToList());
}
