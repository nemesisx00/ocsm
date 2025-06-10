namespace Ocsm;

public class Character(string gameSystem, string name = "")
{
	public string GameSystem { get; set; } = gameSystem;
	public string Name { get; set; } = name;
	public string Player { get; set; } = string.Empty;
}
