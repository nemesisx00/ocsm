using System.Collections.Generic;

namespace Ocsm.Cofd;

public class CofdCore(GameSystem gameSystem, string name = "") : Character(gameSystem, name)
{
	public int Beats { get; set; }
	public int Experience { get; set; }
	public Advantages Advantages { get; set; } = new();
	public List<string> Aspirations { get; set; } = [];
	public List<TraitDots> Attributes { get; set; } = TraitDots.Attributes;
	public List<string> Conditions { get; set; } = [];
	public Details Details { get; set; } = new();
	public List<Merit> Merits { get; set; } = [];
	public List<TraitDots> Skills { get; set; } = TraitDots.Skills;
	public Dictionary<Traits, string> Specialties { get; set; } = [];
}
