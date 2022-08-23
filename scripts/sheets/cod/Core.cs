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
		public Attributes Attributes { get; set; }
		public List<string> Conditions { get; set; }
		public Dictionary<string, int> HealthCurrent { get; set; }
		public List<TextValueItem> Merits { get; set; }
		public Skills Skills { get; set; }
		public List<Skill.Specialty> Specialties { get; set; }
		
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
			Attributes = new Attributes();
			Conditions = new List<string>();
			HealthCurrent = new Dictionary<string, int>();
			HealthCurrent.Add(BoxComplex.State.Three, 0);
			HealthCurrent.Add(BoxComplex.State.Two, 0);
			HealthCurrent.Add(BoxComplex.State.One, 0);
			Merits = new List<TextValueItem>();
			Skills = new Skills();
			Specialties = new List<Skill.Specialty>();
		}
		
		public override string ToString()
		{
			var output = "{ ";
			output += "Age: " + Age.ToString() + ", ";
			output += "Aspirations: { '" + String.Join("', '", Aspirations) + "' }, ";
			output += "Chronicle: '" + Chronicle + "', ";
			output += "Concept: '" + Concept + "', ";
			output += "Conditions: { '" + String.Join("', '", Conditions) + "' }, ";
			output += "Name: '" + Name + "', ";
			output += "Merits: { ";
			Merits.ForEach(merit => output += merit.ToString());
			output += " }, ";
			output += "Player: '" + Player + "', ";
			output += "Size: " + Size.ToString() + ", ";
			output += "Beats: " + Beats.ToString() + ", ";
			output += "Experience: " + Experience.ToString() + ", ";
			output += "Health: (" + HealthCurrent[BoxComplex.State.Three] + ", " + HealthCurrent[BoxComplex.State.Two] + ", " + HealthCurrent[BoxComplex.State.One] + ") / " + HealthMax + ", ";
			output += "Willpower: " + WillpowerSpent + " / " + WillpowerMax + ", ";
			output += "Attributes: " + Attributes.ToString() + ", ";
			output += "Skills: " + Skills.ToString() + ", ";
			output += "Specialties: { " + String.Join("', '", Specialties) + " }, ";
			output += " }";
			return output;
		}
	}
}
