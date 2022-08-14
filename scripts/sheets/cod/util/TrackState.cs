using System;

public class TrackTwoState
{
	public int One { get; set; }
	public int Two { get; set; }
	
	public override string ToString()
	{
		return String.Format("[ One: {0}, Two: {1} ]", One, Two);
	}
}

public class TrackThreeState : TrackTwoState
{
	public int Three { get; set; }
	
	public TrackThreeState() : base()
	{
		Three = 0;
	}
	
	public string toDamage()
	{
		return String.Format("[ Bashing: {0}, Lethal: {1}, Aggravated: {2} ]", One, Two, Three);
	}
	
	public override string ToString()
	{
		return String.Format("[ One: {0}, Two: {1}, Three: {2} ]", One, Two, Three);
	}
}
