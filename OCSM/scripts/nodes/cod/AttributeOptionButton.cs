using System;
using System.Linq;
using Ocsm.Cofd;

namespace Ocsm.Nodes.Cofd;

public partial class AttributeOptionButton : CustomOption
{
	public override void _Ready() => refreshMetadata();
	
	protected override void refreshMetadata() => replaceItems(Enum.GetValues<Traits>()
		.Where(t => t.GetTraitType() == Trait.Type.Attribute)
		.Select(t => t.ToString())
		.ToList());
}
