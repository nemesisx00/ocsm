using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Ocsm.API;
using Ocsm.Nodes;

namespace Ocsm.Cofd;

public class Health() : IComparable<Health>, IEmptiable, IEquatable<Health>
{
	public int Max { get; set; } = 6;
	public int Bashing { get; set; } = 0;
	public int Lethal { get; set; } = 0;
	public int Aggravated { get; set; } = 0;
	
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
	
	public bool Equals(Health other) => Max == other?.Max
		&& Bashing == other?.Bashing
		&& Lethal == other?.Lethal
		&& Aggravated == other?.Aggravated;
	
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
	
	public void FromTrackComplex(Dictionary<StatefulButton.States, int> values) => values.ToList()
		.ForEach(pair => SetValue(pair.Key, pair.Value));
	
	public Dictionary<StatefulButton.States, int> ToTrackComplex() => new()
	{
		{ StatefulButton.States.One, Bashing },
		{ StatefulButton.States.Two, Lethal },
		{ StatefulButton.States.Three, Aggravated }
	};
	
	public override bool Equals(object obj) => Equals(obj as Health);
	public override int GetHashCode() => HashCode.Combine(Max, Bashing, Lethal, Aggravated);

	internal void FromTrackComplex(Dictionary<string, long> value)
	{
		throw new NotImplementedException();
	}
}
