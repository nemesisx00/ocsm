using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Ocsm.API;
using Ocsm.Nodes;

namespace Ocsm.Cofd
{
	public class Health : IComparable<Health>, IEmptiable, IEquatable<Health>
	{
		public long Max { get; set; } = 6;
		public long Bashing { get; set; } = 0;
		public long Lethal { get; set; } = 0;
		public long Aggravated { get; set; } = 0;
		
		[JsonIgnore]
		public bool Empty
		{
			get
			{
				return Bashing.Equals(0)
					&& Lethal.Equals(0)
					&& Aggravated.Equals(0);
			}
		}
		
		public Health() {}
		
		public int CompareTo(Health other)
		{
			var ret = 0;
			if(other is Health)
			{
				ret = Aggravated.CompareTo(other.Aggravated);
				if(ret.Equals(0))
					ret = Lethal.CompareTo(other.Lethal);
				if(ret.Equals(0))
					ret = Bashing.CompareTo(other.Bashing);
				if(ret.Equals(0))
					ret = Max.CompareTo(other.Max);
			}
			return ret;
		}
		
		public bool Equals(Health other)
		{
			return Max.Equals(other.Max)
				&& Bashing.Equals(other.Bashing)
				&& Lethal.Equals(other.Lethal)
				&& Aggravated.Equals(other.Aggravated);
		}
		
		public void setValue(string state, long value)
		{
			switch(state)
			{
				case StatefulButton.State.One:
					Bashing = value;
					break;
				case StatefulButton.State.Two:
					Lethal = value;
					break;
				case StatefulButton.State.Three:
					Aggravated = value;
					break;
			}
		}
		
		public void fromTrackComplex(Dictionary<string, long> values)
		{
			values.ToList()
				.ForEach(pair => setValue(pair.Key, pair.Value));
		}
		
		public Dictionary<string, long> toTrackComplex()
		{
			return new Dictionary<string, long>()
			{
				{ StatefulButton.State.One, Bashing },
				{ StatefulButton.State.Two, Lethal },
				{ StatefulButton.State.Three, Aggravated }
			};
		}
	}
}
