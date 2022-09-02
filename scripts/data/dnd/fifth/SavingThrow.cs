using System;
using System.Collections.Generic;

namespace OCSM.DnD.Fifth
{
	public sealed class SavingThrow : IEquatable<SavingThrow>
	{
		public static List<SavingThrow> generateBaseSavingThrows()
		{
			var list = new List<SavingThrow>();
			list.Add(new SavingThrow(AbilityScore.Names.Constitution, Proficiency.NoProficiency));
			list.Add(new SavingThrow(AbilityScore.Names.Charisma, Proficiency.NoProficiency));
			list.Add(new SavingThrow(AbilityScore.Names.Dexterity, Proficiency.NoProficiency));
			list.Add(new SavingThrow(AbilityScore.Names.Intelligence, Proficiency.NoProficiency));
			list.Add(new SavingThrow(AbilityScore.Names.Strength, Proficiency.NoProficiency));
			list.Add(new SavingThrow(AbilityScore.Names.Wisdom, Proficiency.NoProficiency));
			return list;
		}
		
		public string Name { get; set; }
		public Proficiency Proficient { get; set; }
		
		public SavingThrow(string name, Proficiency proficient)
		{
			Name = name;
			Proficient = proficient;
		}
		
		public bool Equals(SavingThrow savingThrow)
		{
			return savingThrow.Name.Equals(Name)
				&& savingThrow.Proficient.Equals(Proficient);
		}
	}
}
