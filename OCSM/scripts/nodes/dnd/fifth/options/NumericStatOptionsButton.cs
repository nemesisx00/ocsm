using System;
using System.Linq;
using OCSM.DnD.Fifth;

namespace OCSM.Nodes.DnD.Fifth
{
	public partial class NumericStatOptionsButton : CustomOption
	{
		public override void _Ready()
		{
			refreshMetadata();
		}
		
		protected override void refreshMetadata()
		{
			replaceItems(Enum.GetValues<NumericStat>()
				.Where(ns => !String.IsNullOrEmpty(ns.GetLabel()))
				.Select(ns => ns.GetLabel())
				.ToList());
		}
	}
}
