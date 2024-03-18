using System;
using System.Text;
using System.Collections.Generic;

namespace Ocsm.Dnd.Fifth.Inventory;

public class ItemWeapon() : ItemEquippable(), IComparable<ItemWeapon>, IEquatable<ItemWeapon>
{
	public enum WeaponTypes
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
	
	public enum WeaponProperties
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
	
	public static string FormatProperties(List<WeaponProperties> properties)
	{
		var str = new StringBuilder();
		
		properties?.Sort();
		properties?.ForEach(prop => {
			if(str.Length > 0)
				str.Append(", ");
			str.Append(prop.GetLabel());
		});
		
		return str.ToString();
	}
	
	public Dictionary<DamageDie, int> DamageDice { get; set; } = [];
	public List<WeaponProperties> Properties { get; set; } = [];
	public Range Range { get; set; } = Range.Melee;
	public WeaponTypes WeaponType { get; set; }
	
	public int CompareTo(ItemWeapon other)
	{
		var ret = WeaponType.CompareTo(other.WeaponType);
		
		if(ret == 0)
			ret = base.CompareTo(other);
		
		if(ret == 0)
			ret = Range.CompareTo(other?.Range);
		
		if(ret == 0)
			ret = DamageDice.Keys.Count.CompareTo(other?.DamageDice.Keys.Count);
		
		if(ret == 0)
			ret = Properties.Count.CompareTo(other?.Properties.Count);
		
		if(ret == 0)
			ret = FormatProperties(Properties).CompareTo(FormatProperties(other?.Properties));
		
		return ret;
	}
	
	public bool Equals(ItemWeapon other) => base.Equals(other)
		&& Properties == other?.Properties
		&& WeaponType == other?.WeaponType;
	
	public override bool Equals(object obj) => Equals(obj as ItemWeapon);
	
	public override int GetHashCode() => HashCode.Combine(
		base.GetHashCode(),
		DamageDice,
		Properties,
		Range,
		WeaponType
	);
}
