using Ocsm.Meta;

namespace Ocsm;

public class Character(GameSystem gameSystem, string name = "")
{
	public GameSystem GameSystem { get; set; } = gameSystem;
	public string Name { get; set; } = name;
	public string Player { get; set; } = string.Empty;
}
