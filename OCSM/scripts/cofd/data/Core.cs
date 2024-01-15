using System.Collections.Generic;

namespace Ocsm.Cofd;

public class CofdCore() : Character()
{
	public int Beats { get; set; } = 0;
	public int Experience { get; set; } = 0;
	public Advantages Advantages { get; set; } = new();
	public List<string> Aspirations { get; set; } = [];
	public List<Attribute> Attributes { get; set; } = Attribute.Attributes;
	public List<string> Conditions { get; set; } = [];
	public Details Details { get; set; } = new Details();
	public List<Merit> Merits { get; set; } = [];
	public List<Skill> Skills { get; set; } = Skill.Skills;
	public Dictionary<Skill.EnumValues, string> Specialties { get; set; } = [];
	
	public CofdCore(GameSystems gameSystem) : this() => GameSystem = gameSystem;
}
