using System;
using System.Linq;

namespace Ocsm.Nodes.Cofd
{
	public partial class AttributeOptionButton : CustomOption
	{
		public override void _Ready()
		{
			refreshMetadata();
		}
		
		protected override void refreshMetadata()
		{
			replaceItems(Enum.GetValues<Ocsm.Cofd.Attribute.Enum>()
				.Select(a => a.GetLabelOrName())
				.ToList());
		}
	}
}
