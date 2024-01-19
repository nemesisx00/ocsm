
namespace Ocsm.Wod.Vtm.V5.Data;

public class V5Kindred() : BaseV5Sheet()
{
	public const int DefaultHumanity = 8;
	public const int MaxHumanity = 10;
	
	public string Ambition { get; set; }
	public Clans Clan { get; set; }
	public DisciplineValues Disciplines { get; set; } = new();
	public int Generation { get; set; }
	public int Humanity { get; set; } = DefaultHumanity;
	public PredatorTypes PredatorType { get; set; }
	public string Sire { get; set; }
}
