using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Ocsm.Api;
using Ocsm.Nodes;

namespace Ocsm.Wod.VtmV5;

public class Health() : IComparable<Health>, IEmptiable, IEquatable<Health>
{
	public int Max { get; set; } = 6;
	public int Superficial { get; set; } = 0;
	public int Aggravated { get; set; } = 0;
	
	[JsonIgnore]
	public bool Empty => Superficial == 0
		&& Aggravated == 0;
	
	public int CompareTo(Health other)
	{
		var ret = Aggravated.CompareTo(other?.Aggravated);
		
		if(ret == 0)
			ret = Superficial.CompareTo(other?.Superficial);
		
		if(ret == 0)
			ret = Max.CompareTo(other?.Max);
		
		return ret;
	}
	
	public bool Equals(Health other) => Max == other?.Max
		&& Superficial == other?.Superficial
		&& Aggravated == other?.Aggravated;
	
	public void SetValue(StatefulButton.States state, int value)
	{
		switch(state)
		{
			case StatefulButton.States.One:
				Superficial = value;
				break;
			
			case StatefulButton.States.Two:
				Aggravated = value;
				break;
		}
	}
	
	public void FromTrackComplex(Dictionary<StatefulButton.States, int> values)
	{
		foreach(var pair in values)
		{
			SetValue(pair.Key, pair.Value);
		}
	}
	
	public Dictionary<StatefulButton.States, int> ToTrackComplex() => new()
	{
		{ StatefulButton.States.One, Superficial },
		{ StatefulButton.States.Two, Aggravated }
	};
	
	public override bool Equals(object obj) => Equals(obj as Health);
	public override int GetHashCode() => HashCode.Combine(Max, Superficial, Aggravated);

	internal void FromTrackComplex(Dictionary<string, long> value)
	{
		throw new NotImplementedException();
	}
}
