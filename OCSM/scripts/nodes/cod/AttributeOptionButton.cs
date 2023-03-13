using System;
using System.Linq;

namespace OCSM.Nodes.CoD
{
	public partial class AttributeOptionButton : CustomOption
	{
		public override void _Ready()
		{
			refreshMetadata();
		}
		
		protected override void refreshMetadata()
		{
			replaceItems(Enum.GetValues<OCSM.CoD.Attribute.Enum>()
				.Select(a => a.GetLabelOrName())
				.ToList());
		}
	}
}
