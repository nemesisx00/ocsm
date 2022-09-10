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
	
	public class NumericBonus : IComparable<NumericBonus>, IEquatable<NumericBonus>
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
		
		public int CompareTo(NumericBonus numericBonus)
		{
			var ret = Type.CompareTo(numericBonus.Type);
			if(ret.Equals(0))
				ret = Name.CompareTo(numericBonus.Name);
			if(ret.Equals(0))
				ret = AbilityName.CompareTo(numericBonus.AbilityName);
			if(ret.Equals(0))
				ret = Add.CompareTo(numericBonus.Add);
			return ret;
		}
		
		public bool Equals(NumericBonus numericBonus)
		{
			return AbilityName.Equals(numericBonus.AbilityName)
				&& Add.Equals(numericBonus.Add)
				&& Name.Equals(numericBonus.Name)
				&& Type.Equals(numericBonus.Type)
				&& Value.Equals(numericBonus.Value);
		}
	}
}
