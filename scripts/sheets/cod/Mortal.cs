using Godot;
using System;
using System.Collections.Generic;

namespace OCSM
{
	public class Mortal : Character
	{
		public int Age { get; set; }
		public int Beats { get; set; }
		public int Experience { get; set; }
		public int HealthMax { get; set; }
		public int Integrity { get; set; }
		public int Size { get; set; }
		public int WillpowerMax { get; set; }
		public int WillpowerSpent { get; set; }
		public string Chronicle { get; set; }
		public string Concept { get; set; }
		public string Faction { get; set; }
		public string GroupName { get; set; }
		public string Name { get; set; }
		public string Player { get; set; }
		public string Vice { get; set; }
		public string Virtue { get; set; }
		
		public Attributes Attributes { get; private set; }
		public List<string> Conditions { get; private set; }
		public Dictionary<string, int> HealthCurrent { get; set; }
		public Dictionary<string, int> Merits { get; private set; }
		public Skills Skills { get; private set; }
		
		public Mortal()
		{
			Age = -1;
			Beats = 0;
			Experience = 0;
			HealthMax = 6;
			Integrity = 7;
			Size = 5;
			WillpowerMax = 2;
			WillpowerSpent = 0;
			
			Chronicle = String.Empty;
			Concept = String.Empty;
			Faction = String.Empty;
			GroupName = String.Empty;
			Name = String.Empty;
			Player = String.Empty;
			Vice = String.Empty;
			Virtue = String.Empty;
			
			Attributes = new Attributes();
			Conditions = new List<string>();
			HealthCurrent = new Dictionary<string, int>();
			HealthCurrent.Add(BoxComplex.State.Three, 0);
			HealthCurrent.Add(BoxComplex.State.Two, 0);
			HealthCurrent.Add(BoxComplex.State.One, 0);
			Merits = new Dictionary<string, int>();
			Skills = new Skills();
		}

		public override string ToString()
		{
			var output = "{ ";
			output += "Age: " + Age.ToString() + ", ";
			output += "Chronicle: '" + Chronicle + "', ";
			output += "Concept: '" + Concept + "', ";
			output += "Faction: '" + Faction + "', ";
			output += "GroupName: '" + GroupName + "', ";
			output += "Name: '" + Name + "', ";
			output += "Player: '" + Player + "', ";
			output += "Vice: '" + Vice + "', ";
			output += "Virtue: '" + Virtue + "', ";
			output += "Size: " + Size.ToString() + ", ";
			output += "Beats: " + Beats.ToString() + ", ";
			output += "Experience: " + Experience.ToString() + ", ";
			output += "Health: a) " + HealthCurrent[BoxComplex.State.Three] + " l) " + HealthCurrent[BoxComplex.State.Two] + " b) " + HealthCurrent[BoxComplex.State.One] + " / " + HealthMax + ", ";
			output += "Integrity: " + Integrity + ", ";
			output += "Willpower: " + WillpowerSpent + " / " + WillpowerMax + ", ";
			output += "Attributes: " + Attributes.ToString() + ", ";
			output += "Skills: " + Skills.ToString() + ", ";
			output += " }";
			return output;
		}
	}
}
