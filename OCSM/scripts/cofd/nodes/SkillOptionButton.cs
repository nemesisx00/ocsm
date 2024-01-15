using System;
using System.Linq;
using Ocsm.Nodes;

namespace Ocsm.Cofd.Nodes;

public partial class SkillOptionButton : CustomOption
{
	public override void _Ready() => refreshMetadata();
	protected override void refreshMetadata() => replaceItems(Enum.GetValues<Skill.EnumValues>()
		.Select(s => s.GetLabelOrName())
		.ToList());
}
