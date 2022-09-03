using System;
using System.Collections.Generic;
using OCSM.Nodes;
using OCSM.Nodes.CoD;

namespace OCSM.CoD
{
	public class CodCore : Character
	{
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
		public Dictionary<string, int> HealthCurrent { get; set; }
		public List<Merit> Merits { get; set; }
		public List<Skill> Skills { get; set; }
		public Dictionary<string, string> Specialties { get; set; }
		
		public CodCore()
		{
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
			HealthCurrent = new Dictionary<string, int>();
			HealthCurrent.Add(StatefulButton.State.Three, 0);
			HealthCurrent.Add(StatefulButton.State.Two, 0);
			HealthCurrent.Add(StatefulButton.State.One, 0);
			Merits = new List<Merit>();
			Skills = Skill.asList();
			Specialties = new Dictionary<string, string>();
		}
	}
}
