using System.Collections.Generic;
using OCSM.Meta;

namespace OCSM.DnD.Fifth.Meta
{
	public class Race : Metadata
	{
		public List<Feature> Features { get; set; }
		
		public Race() : base()
		{
			Features = new List<Feature>();
		}
	}
}
