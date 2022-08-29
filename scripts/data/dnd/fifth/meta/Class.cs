using System.Collections.Generic;
using OCSM.Meta;

namespace OCSM.DnD.Fifth.Meta
{
	public class Class : Metadata
	{
		public List<Feature> Features { get; set; }
		
		public Class() : base()
		{
			Features = new List<Feature>();
		}
	}
}
