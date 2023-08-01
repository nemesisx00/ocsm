using System;
using System.Text.Json.Serialization;
using Ocsm.API;

namespace Ocsm;

public struct Pair<K, V> : IComparable<Pair<K, V>>, IEmptiable, IEquatable<Pair<K, V>>
	where K: IComparable<K>, IEquatable<K>
	where V: IComparable<V>, IEquatable<V>
{
	public K Key { get; set; }
	public V Value { get; set; }
	
	[JsonIgnore]
	public bool Empty
	{
		get
		{
			var ret = Key is K && Value is V;
			if(Key is string skey)
				ret = ret && String.IsNullOrEmpty(skey);
			if(Value is string sval)
				ret = ret && String.IsNullOrEmpty(sval);
			return ret;
		}
	}
	
	public int CompareTo(Pair<K, V> pair)
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
	
	public bool Equals(Pair<K, V> pair) { return Key.Equals(pair.Key) && Value.Equals(pair.Value); }
}
