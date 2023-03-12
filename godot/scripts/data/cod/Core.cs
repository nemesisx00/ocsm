using System;
using System.Collections.Generic;
using OCSM.Nodes;

namespace OCSM.CoD
{
	public class CodCore : Character
	{
		public long Beats { get; set; }
		public long Experience { get; set; }
		public Advantages Advantages { get; set; }
		public List<string> Aspirations { get; set; }
		public List<Attribute> Attributes { get; set; }
		public List<string> Conditions { get; set; }
		public Details Details { get; set; }
		public List<Merit> Merits { get; set; }
		public List<Skill> Skills { get; set; }
		public List<Pair<string, string>> Specialties { get; set; }
		
		public CodCore() : base()
		{
			Beats = 0;
			Experience = 0;
			
			Advantages = new Advantages();
			Aspirations = new List<string>();
			Attributes = Attribute.asList();
			Conditions = new List<string>();
			Details = new Details();
			Merits = new List<Merit>();
			Skills = Skill.asList();
			Specialties = new List<Pair<string, string>>();
		}
		
		public CodCore(string gameSystem) : this()
		{
			GameSystem = gameSystem;
		}
	}
}
