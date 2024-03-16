
namespace Ocsm.Cofd;

public class Advantages()
{
	public int Armor { get; set; }
	public int Defense { get; set; }
	public Health Health { get; set; } = new Health();
	public int Initiative { get; set; }
	public int Integrity { get; set; } = 7;
	public int Power { get; set; } = 1;
	public int ResourceSpent { get; set; }
	public int ResourceMax { get; set; } = 10;
	public int Size { get; set; } = 5;
	public int Speed { get; set; }
	public int WillpowerSpent { get; set; }
	public int WillpowerMax { get; set; } = 2;
}
