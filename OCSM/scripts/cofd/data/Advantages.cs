
namespace Ocsm.Cofd;

public struct Advantages()
{
	public const int DefaultIntegrity = 7;
	public const int DefaultPower = 1;
	public const int DefaultResourceMax = 10;
	public const int DefaultSize = 5;
	public const int DefaultWillpowerMax = 2;
	
	public int Armor { get; set; }
	public int Defense { get; set; }
	public Health Health { get; set; } = new();
	public int Initiative { get; set; }
	public int Integrity { get; set; } = DefaultIntegrity;
	public int Power { get; set; } = DefaultPower;
	public int ResourceSpent { get; set; }
	public int ResourceMax { get; set; } = DefaultResourceMax;
	public int Size { get; set; } = DefaultSize;
	public int Speed { get; set; }
	public int WillpowerSpent { get; set; }
	public int WillpowerMax { get; set; } = DefaultWillpowerMax;
}
