using System;
using System.Linq;
using Ocsm.Cofd;

namespace Ocsm.Nodes.Cofd;

public partial class SkillOptionButton : CustomOption
{
	public override void _Ready() => refreshMetadata();
	
	protected override void refreshMetadata() => replaceItems(Enum.GetValues<Traits>()
		.Where(t => t.GetTraitType() == Trait.Type.Skill)
		.Select(t => t.GetLabel())
		.ToList());
}
