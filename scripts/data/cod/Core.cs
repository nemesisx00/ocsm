using System;
using System.Collections.Generic;
using OCSM.Nodes;

namespace OCSM.CoD
{
	public class CodCore : Character
	{
		public long Beats { get; set; }
		public long Experience { get; set; }
		public long HealthMax { get; set; }
		public long Size { get; set; }
		public long WillpowerMax { get; set; }
		public long WillpowerSpent { get; set; }
		public string Chronicle { get; set; }
		public string Concept { get; set; }
		
		public List<string> Aspirations { get; set; }
		public List<Attribute> Attributes { get; set; }
		public List<string> Conditions { get; set; }
		public Dictionary<string, long> HealthCurrent { get; set; }
		public List<Merit> Merits { get; set; }
		public List<Skill> Skills { get; set; }
		public List<Specialty> Specialties { get; set; }
		
		public CodCore() : base()
		{
			Beats = 0;
			Experience = 0;
			HealthMax = 6;
			Size = 5;
			WillpowerMax = 2;
			WillpowerSpent = 0;
			
			Chronicle = String.Empty;
			Concept = String.Empty;
			
			Aspirations = new List<string>();
			Attributes = Attribute.asList();
			Conditions = new List<string>();
			HealthCurrent = new Dictionary<string, long>();
			HealthCurrent.Add(StatefulButton.State.Three, 0);
			HealthCurrent.Add(StatefulButton.State.Two, 0);
			HealthCurrent.Add(StatefulButton.State.One, 0);
			Merits = new List<Merit>();
			Skills = Skill.asList();
			Specialties = new List<Specialty>();
		}
		
		public CodCore(string gameSystem) : this()
		{
			GameSystem = gameSystem;
		}
	}
}
