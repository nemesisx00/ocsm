using System;
using System.Collections.Generic;
using EnumsNET;

namespace OCSM.DnD.Fifth.Inventory
{
	public class ItemWeapon : ItemEquippable, IComparable<ItemWeapon>, IEquatable<ItemWeapon>
	{
		public enum WeaponType
		{
			None = 0,
			Simple = SimpleMelee | SimpleRanged,
			SimpleMelee = 1,
			SimpleRanged = 2,
			Martial = MartialMelee | MartialRanged,
			MartialMelee = 4,
			MartialRanged = 8,
			Improvised = 16,
		}
		
		public enum WeaponProperties
		{
			None = 0,
			Ammunition,
			Finesse,
			Heavy,
			Light,
			Loading,
			Range,
			Reach,
			Silvered,
			Special,
			Thrown,
			TwoHanded,
			Versatile,
		}
		
		public static string FormatProperties(List<WeaponProperties> properties)
		{
			var str = "";
			
			properties.Sort();
			foreach(var prop in properties)
			{
				if(str.Length > 0)
					str += ", ";
				str += Enums.GetName(prop);
			}
			
			return str;
		}
		
		public Dictionary<DamageDie, int> DamageDice { get; set; }
		public List<WeaponProperties> Properties { get; set; }
		public Range Range { get; set; }
		public WeaponType Type { get; set; }
		
		public ItemWeapon() : base()
		{
			DamageDice = new Dictionary<DamageDie, int>();
			Properties = new List<WeaponProperties>();
			Range = Range.Melee;
			Type = WeaponType.None;
		}
		
		public int CompareTo(ItemWeapon other)
		{
			var ret = Type.CompareTo(other.Type);
			if(other is ItemWeapon)
			{
				if(ret.Equals(0))
					ret = base.CompareTo(other);
				if(ret.Equals(0))
					ret = Range.CompareTo(other.Range);
				if(ret.Equals(0))
					ret = DamageDice.Keys.Count.CompareTo(other.DamageDice.Keys.Count);
				if(ret.Equals(0))
					ret = Properties.Count.CompareTo(other.Properties.Count);
				if(ret.Equals(0))
					ret = ItemWeapon.FormatProperties(Properties).CompareTo(ItemWeapon.FormatProperties(other.Properties));
			}
			return ret;
		}
		
		public bool Equals(ItemWeapon item)
		{
			return base.Equals(item)
				&& Properties.Equals(item.Properties)
				&& Type.Equals(item.Type);
		}
	}
}
