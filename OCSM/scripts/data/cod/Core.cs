using System.Collections.Generic;

namespace Ocsm.Cofd
{
	public class CodCore : Character
	{
		public long Beats { get; set; } = 0;
		public long Experience { get; set; } = 0;
		public Advantages Advantages { get; set; } = new Advantages();
		public List<string> Aspirations { get; set; } = new List<string>();
		public List<Attribute> Attributes { get; set; } = Attribute.Attributes;
		public List<string> Conditions { get; set; } = new List<string>();
		public Details Details { get; set; } = new Details();
		public List<Merit> Merits { get; set; } = new List<Merit>();
		public List<Skill> Skills { get; set; } = Skill.Skills;
		public Dictionary<Skill.Enum, string> Specialties { get; set; } = new Dictionary<Skill.Enum, string>();
		
		public CodCore() : base() {}
		
		public CodCore(string gameSystem) : this()
		{
			GameSystem = gameSystem;
		}
	}
}
