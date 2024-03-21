using System;
using System.Text.Json.Serialization;
using Ocsm.Api;

namespace Ocsm;

public struct Pair<K, V>(K key = default, V value = default) : IComparable<Pair<K, V>>, IEmptiable, IEquatable<Pair<K, V>>
	where K: IComparable<K>, IEquatable<K>
	where V: IComparable<V>, IEquatable<V>
{
	public static bool operator ==(Pair<K, V> left, Pair<K, V> right) => left.Equals(right);
	public static bool operator !=(Pair<K, V> left, Pair<K, V> right) => !(left == right);
	
	public K Key { get; set; } = key;
	public V Value { get; set; } = value;
	
	[JsonIgnore]
	public readonly bool Empty => (Key is null || Key is string skey && string.IsNullOrEmpty(skey))
		&& (Value is null || (Value is string sval && string.IsNullOrEmpty(sval)));
	
	public readonly int CompareTo(Pair<K, V> pair)
	{
		var ret = Key.CompareTo(pair.Key);
		
		if(ret == 0)
			ret = Value.CompareTo(pair.Value);
		
		return ret;
	}
	
	public readonly bool Equals(Pair<K, V> pair) => Key.Equals(pair.Key) && Value.Equals(pair.Value);
	public override readonly bool Equals(object obj) => obj is Pair<K, V> pair && Equals(pair);
	public override readonly int GetHashCode() => HashCode.Combine(Key, Value);
}
