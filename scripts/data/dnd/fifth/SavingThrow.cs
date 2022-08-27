using System;
using System.Collections.Generic;

namespace OCSM.DnD.Fifth
{
	public sealed class SavingThrow : IEquatable<SavingThrow>
	{
		public static List<SavingThrow> generateNewSavingThrow()
		{
			var list = new List<SavingThrow>();
			list.Add(new SavingThrow(AbilityScore.Names.Constitution, Proficiency.Enum.NoProficiency));
			list.Add(new SavingThrow(AbilityScore.Names.Charisma, Proficiency.Enum.NoProficiency));
			list.Add(new SavingThrow(AbilityScore.Names.Dexterity, Proficiency.Enum.NoProficiency));
			list.Add(new SavingThrow(AbilityScore.Names.Intelligence, Proficiency.Enum.NoProficiency));
			list.Add(new SavingThrow(AbilityScore.Names.Strength, Proficiency.Enum.NoProficiency));
			list.Add(new SavingThrow(AbilityScore.Names.Wisdom, Proficiency.Enum.NoProficiency));
			return list;
		}
		
		public string Name { get; set; }
		public Proficiency.Enum Proficient { get; set; }
		
		public SavingThrow(string name, Proficiency.Enum proficient)
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
