using System.Collections.Generic;
using Ocsm.Meta;

namespace Ocsm.Wod.VtmV5;

public class Vampire(GameSystem gameSystem, string name = "") : Character(gameSystem, name)
{
	public List<MetadataNumber> Advantages { get; set; } = [];
	public string Ambition { get; set; }
	public List<TraitDots> Attributes { get; set; } = TraitDots.Attributes;
	public int BloodPotency { get; set; }
	public string Chronicle { get; set; }
	public List<string> ChronicleTenets { get; set; } = [];
	public Clan Clan { get; set; }
	public string Compulsion { get; set; }
	public List<string> Convictions { get; set; } = [];
	public string Desire { get; set; }
	public Dictionary<Discipline, List<DisciplinePower>> Disciplines { get; set; } = [];
	public int Experience { get; set; }
	public int ExperienceTotal { get; set; }
	public List<MetadataNumber> Flaws { get; set; } = [];
	public int Generation { get; set; }
	public Health Health { get; set; }
	public int Humanity { get; set; }
	public int Hunger { get; set; }
	public Metadata PredatorType { get; set; }
	public Metadata Resonance { get; set; }
	public string Sire { get; set; }
	public List<TraitDots> Skills { get; set; } = TraitDots.Skills;
	public Dictionary<Traits, string> Specialties { get; set; } = [];
	public List<string> Touchstones { get; set; } = [];
	public Health Willpower { get; set; }
}
