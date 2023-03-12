
namespace OCSM.DnD.Fifth
{
	public class HitPoints
	{
		public int Current { get; set; }
		public int Max { get; set; }
		public int Temp { get; set; }
		
		public HitPoints(int max = 1)
		{
			Current = max;
			Max = max;
			Temp = 0;
		}
	}
}
