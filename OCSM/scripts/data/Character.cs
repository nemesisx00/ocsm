using System;

namespace Ocsm;

public class Character
{
	public string GameSystem { get; set; }
	public string Name { get; set; }
	public string Player { get; set; }
	
	public Character()
	{
		GameSystem = String.Empty;
		Name = String.Empty;
		Player = String.Empty;
	}
	
	public Character(string gameSystem) : this()
	{
		GameSystem = gameSystem;
	}
	
	public Character(string name, string gameSystem) : this(gameSystem)
	{
		Name = name;
	}
}
