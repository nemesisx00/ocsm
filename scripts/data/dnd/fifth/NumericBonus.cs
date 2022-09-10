using System;
using System.Collections.Generic;

namespace OCSM.DnD.Fifth
{
	public enum NumericStat { None, AbilityScore, ArmorClass, Initiative, MaxHP, Speed, TempHP }
	public sealed class NumericStatNames
	{
		public const string AbilityScore = "Ability Score";
		public const string ArmorClass = "Armor Class";
		public const string Initiative = "Initiative";
		public const string MaxHP = "Maximum Hit Points";
		public const string Speed = "Walking Speed";
		public const string TempHP = "Temporary Hit Points";
		
		public static List<string> asList()
		{
			var list = new List<string>();
			list.Add(String.Empty);
			list.Add(NumericStatNames.AbilityScore);
			list.Add(NumericStatNames.ArmorClass);
			list.Add(NumericStatNames.Initiative);
			list.Add(NumericStatNames.MaxHP);
			list.Add(NumericStatNames.Speed);
			list.Add(NumericStatNames.TempHP);
			return list;
		}
		
		public static string forNumericStat(NumericStat stat)
		{
			switch(stat)
			{
				case NumericStat.AbilityScore:
					return NumericStatNames.AbilityScore;
				case NumericStat.ArmorClass:
					return NumericStatNames.ArmorClass;
				case NumericStat.Initiative:
					return NumericStatNames.Initiative;
				case NumericStat.MaxHP:
					return NumericStatNames.MaxHP;
				case NumericStat.Speed:
					return NumericStatNames.Speed;
				case NumericStat.TempHP:
					return NumericStatNames.TempHP;
				case NumericStat.None:
				default:
					return String.Empty;
			}
		}
	}
	
	public class NumericBonus : IEquatable<NumericBonus>
	{
		public string AbilityName { get; set; }
		public bool Add { get; set; }
		public string Name { get; set; }
		public NumericStat Type { get; set; }
		public int Value { get; set; }
		
		public NumericBonus()
		{
			AbilityName = String.Empty;
			Add = false;
			Name = String.Empty;
			Type = NumericStat.None;
			Value = 0;
		}
		
		public bool Equals(NumericBonus numericBonus)
		{
			return numericBonus.AbilityName.Equals(AbilityName)
				&& numericBonus.Add.Equals(Add)
				&& numericBonus.Name.Equals(Name)
				&& numericBonus.Type.Equals(Type)
				&& numericBonus.Value.Equals(Value);
		}
	}
}
