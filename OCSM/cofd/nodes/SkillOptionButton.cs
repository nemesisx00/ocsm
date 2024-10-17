using System;
using System.Linq;
using Ocsm.Nodes;

namespace Ocsm.Cofd.Nodes;

public partial class SkillOptionButton : FixedOption
{
	protected override void refreshMetadata() => replaceItems(Enum.GetValues<Traits>()
		.Where(t => t.GetTraitType() == Trait.Type.Skill)
		.Select(t => t.GetLabel())
		.ToList());
}
