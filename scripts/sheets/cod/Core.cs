using System;
using System.Collections.Generic;

namespace OCSM
{
	public class CodCore : Character
	{
		public int Age { get; set; }
		public int Beats { get; set; }
		public int Experience { get; set; }
		public int HealthMax { get; set; }
		public int Size { get; set; }
		public int WillpowerMax { get; set; }
		public int WillpowerSpent { get; set; }
		public string Chronicle { get; set; }
		public string Concept { get; set; }
		public string Player { get; set; }
		
		public List<string> Aspirations { get; set; }
		public List<Attribute> Attributes { get; set; }
		public List<string> Conditions { get; set; }
		public Dictionary<string, int> Flaws { get; set; }
		public Dictionary<string, int> HealthCurrent { get; set; }
		public Dictionary<string, int> Merits { get; set; }
		public List<Skill> Skills { get; set; }
		public Dictionary<string, string> Specialties { get; set; }
		
		public CodCore()
		{
			Age = -1;
			Beats = 0;
			Experience = 0;
			HealthMax = 6;
			Size = 5;
			WillpowerMax = 2;
			WillpowerSpent = 0;
			
			Chronicle = String.Empty;
			Concept = String.Empty;
			Name = String.Empty;
			Player = String.Empty;
			
			Aspirations = new List<string>();
			Attributes = Attribute.asList();
			Conditions = new List<string>();
			Flaws = new Dictionary<string, int>();
			HealthCurrent = new Dictionary<string, int>();
			HealthCurrent.Add(BoxComplex.State.Three, 0);
			HealthCurrent.Add(BoxComplex.State.Two, 0);
			HealthCurrent.Add(BoxComplex.State.One, 0);
			Merits = new Dictionary<string, int>();
			Skills = Skill.asList();
			Specialties = new Dictionary<string, string>();
		}
	}
}
