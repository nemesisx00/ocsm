using System;
using System.Linq;

namespace OCSM.Nodes.CoD
{
	public partial class SkillOptionButton : CustomOption
	{
		public override void _Ready()
		{
			refreshMetadata();
		}
		
		protected override void refreshMetadata()
		{
			replaceItems(Enum.GetValues<OCSM.CoD.Skill.Enum>()
				.Select(s => s.GetLabelOrName())
				.ToList());
		}
	}
}
