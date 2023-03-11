using System;
using System.Text.Json.Serialization;
using OCSM.API;

namespace OCSM
{
	public struct Pair : IComparable<Pair>, IEmptiable, IEquatable<Pair>
	{
		public string Key { get; set; }
		public string Value { get; set; }
		
		[JsonIgnore]
		public bool Empty { get { return String.IsNullOrEmpty(Key) && String.IsNullOrEmpty(Value); } }
		
		public int CompareTo(Pair pair)
		{
			var ret = 0;
			if(!Equals(pair))
			{
				ret = Key.CompareTo(pair.Key);
				if(ret.Equals(0))
					ret = Value.CompareTo(pair.Value);
			}
			return ret;
		}
		
		public bool Equals(Pair pair) { return Key.Equals(pair.Key) && Value.Equals(pair.Value); }
	}
}
