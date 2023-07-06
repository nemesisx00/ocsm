using System;
using System.Linq;

namespace Ocsm.Nodes.Cofd
{
	public partial class SkillOptionButton : CustomOption
	{
		public override void _Ready()
		{
			refreshMetadata();
		}
		
		protected override void refreshMetadata()
		{
			replaceItems(Enum.GetValues<Ocsm.Cofd.Skill.Enum>()
				.Select(s => s.GetLabelOrName())
				.ToList());
		}
	}
}
