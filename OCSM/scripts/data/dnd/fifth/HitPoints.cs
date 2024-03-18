
namespace Ocsm.Dnd.Fifth;

public struct HitPoints(int max = 1)
{
	public int Current { get; set; } = max;
	public int Max { get; set; } = max;
	public int Temp { get; set; }
}
