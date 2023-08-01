
namespace Ocsm.Cofd;

public class Advantages
{
	public long Armor { get; set; } = 0;
	public long Defense { get; set; }
	public Health Health { get; set; } = new Health();
	public long Initiative { get; set; }
	public long Integrity { get; set; } = 7;
	public long Power { get; set; } = 1;
	public long ResourceSpent { get; set; } = 0;
	public long ResourceMax { get; set; } = 10;
	public long Size { get; set; } = 5;
	public long Speed { get; set; }
	public long WillpowerSpent { get; set; } = 0;
	public long WillpowerMax { get; set; } = 2;
	
	public Advantages() {}
}
