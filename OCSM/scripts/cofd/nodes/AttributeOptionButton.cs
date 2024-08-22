using System;
using System.Linq;
using Ocsm.Nodes;

namespace Ocsm.Cofd.Nodes;

public partial class AttributeOptionButton : CustomOption
{
	public override void _Ready() => refreshMetadata();
	
	protected override void refreshMetadata() => replaceItems(Enum.GetValues<Traits>()
		.Where(t => t.GetTraitType() == Trait.Type.Attribute)
		.Select(t => t.ToString()));
}
