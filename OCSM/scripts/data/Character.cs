
namespace Ocsm;

public class Character()
{
	public GameSystems GameSystem { get; set; }
	public string Name { get; set; }
	public string Player { get; set; }

	public Character(GameSystems gameSystem) : this() => GameSystem = gameSystem;
	public Character(string name, GameSystems gameSystem) : this(gameSystem) => Name = name;
}
