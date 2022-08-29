using System.Collections.Generic;
using OCSM.Meta;

namespace OCSM.DnD.Fifth.Meta
{
	public class Background : Metadata
	{
		public List<Feature> Features { get; set; }
		
		public Background() : base()
		{
			Features = new List<Feature>();
		}
	}
}
