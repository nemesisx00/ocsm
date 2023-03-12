using System;

namespace OCSM.DnD.Fifth
{
	public enum NumericStat
	{
		None,
		[Label("Ability Score")]
		AbilityScore,
		[Label("Armor Class")]
		ArmorClass,
		[Label("Initiative")]
		Initiative,
		[Label("Maximum Hit Points")]
		MaxHP,
		[Label("Walking Speed")]
		Speed,
		[Label("Temporary Hit Points")]
		TempHP
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
