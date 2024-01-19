
namespace Ocsm.Wod.Vtm.V5.Data;

public class Track(int max = 1)
{
	public int Aggravated { get; set; }
	public int Max { get; set; } = max;
	public int Superficial { get; set; }
	
	public void Normalize()
	{
		if(Superficial > Max)
		{
			var diff = Superficial - Max;
			Superficial = Max;
			Aggravated += diff;
		}
		
		if(Aggravated > Max)
			Aggravated = Max;
	}
}
