using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Ocsm.Api;
using Ocsm.Nodes;

namespace Ocsm.Cofd;

public class Health() : IComparable<Health>, IEmptiable, IEquatable<Health>
{
	public const int DefaultMax = 6;
	
	public int Max { get; set; } = DefaultMax;
	public int Bashing { get; set; }
	public int Lethal { get; set; }
	public int Aggravated { get; set; }
	
	[JsonIgnore]
	public bool Empty => Bashing == 0
		&& Lethal == 0
		&& Aggravated == 0;
	
	public int CompareTo(Health other)
	{
		var ret = Aggravated.CompareTo(other?.Aggravated);
		
		if(ret == 0)
			ret = Lethal.CompareTo(other?.Lethal);
		
		if(ret == 0)
			ret = Bashing.CompareTo(other?.Bashing);
		
		if(ret == 0)
			ret = Max.CompareTo(other?.Max);
		
		return ret;
	}
	
	public override bool Equals(object other) => Equals(other as Health);
	
	public bool Equals(Health other) => Max.Equals(other?.Max)
		&& Bashing.Equals(other?.Bashing)
		&& Lethal.Equals(other?.Lethal)
		&& Aggravated.Equals(other?.Aggravated);
	
	public override int GetHashCode() => HashCode.Combine(
		Max,
		Bashing,
		Lethal,
		Aggravated
	);
	
	public void FromTrackComplex(Dictionary<StatefulButton.States, int> values)
	{
		values?.ToList()
			.ForEach(pair => SetValue(pair.Key, pair.Value));
	}
	
	public void SetValue(StatefulButton.States state, int value)
	{
		switch(state)
		{
			case StatefulButton.States.One:
				Bashing = value;
				break;
			
			case StatefulButton.States.Two:
				Lethal = value;
				break;
			
			case StatefulButton.States.Three:
				Aggravated = value;
				break;
		}
	}
	
	public Dictionary<StatefulButton.States, int> ToTrackComplex() => new()
	{
		{ StatefulButton.States.One, Bashing },
		{ StatefulButton.States.Two, Lethal },
		{ StatefulButton.States.Three, Aggravated }
	};
}
