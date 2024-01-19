
namespace Ocsm.Wod.Vtm.V5.Data;

public abstract class BaseV5Sheet() : Character(GameSystems.WodVtmV5)
{
	public const int BaseHealth = 3;
	
	public Attributes Attributes { get; set; } = new();
	public Track Health { get; set; } = new(BaseHealth);
	public Skills Skills { get; set; } = new();
	public Track Willpower { get; set; } = new();
}
