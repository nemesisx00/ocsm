using System;
using System.Text;
using System.Collections.Generic;

namespace OCSM.DnD.Fifth.Inventory
{
	public class ItemWeapon : ItemEquippable, IComparable<ItemWeapon>, IEquatable<ItemWeapon>
	{
		public enum WeaponType
		{
			None = 0,
			[Label("Simple")]
			Simple = SimpleMelee | SimpleRanged,
			[Label("Simple Melee")]
			SimpleMelee = 1,
			[Label("Simple Ranged")]
			SimpleRanged = 2,
			[Label("Martial")]
			Martial = MartialMelee | MartialRanged,
			[Label("Martial Melee")]
			MartialMelee = 4,
			[Label("Martial Ranged")]
			MartialRanged = 8,
			[Label("Improvised")]
			Improvised = 16,
		}
		
		public enum WeaponProperty
		{
			None = 0,
			[Label("Ammunition")]
			Ammunition,
			[Label("Finesse")]
			Finesse,
			[Label("Heavy")]
			Heavy,
			[Label("Light")]
			Light,
			[Label("Loading")]
			Loading,
			[Label("Range")]
			Range,
			[Label("Reach")]
			Reach,
			[Label("Silvered")]
			Silvered,
			[Label("Special")]
			Special,
			[Label("Thrown")]
			Thrown,
			[Label("TwoHanded")]
			TwoHanded,
			[Label("Versatile")]
			Versatile,
		}
		
		public static string FormatProperties(List<WeaponProperty> properties)
		{
			var str = new StringBuilder();
			
			properties.Sort();
			properties.ForEach(prop => {
				if(str.Length > 0)
					str.Append(", ");
				str.Append(prop.GetLabel());
			});
			
			return str.ToString();
		}
		
		public Dictionary<DamageDie, int> DamageDice { get; set; }
		public List<WeaponProperty> Properties { get; set; }
		public Range Range { get; set; }
		public WeaponType Type { get; set; }
		
		public ItemWeapon() : base()
		{
			DamageDice = new Dictionary<DamageDie, int>();
			Properties = new List<WeaponProperty>();
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
