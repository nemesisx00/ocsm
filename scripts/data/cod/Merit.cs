using System;

namespace OCSM.CoD
{
	public class Merit : IEquatable<Merit>
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int Value { get; set; }
		
		public bool Equals(Merit merit)
		{
			return merit.Name.Equals(Name)
				&& merit.Description.Equals(Description)
				&& merit.Value.Equals(Value);
		}
	}
}
