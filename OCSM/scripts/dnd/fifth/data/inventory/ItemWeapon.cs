using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Ocsm.Dnd.Fifth.Inventory;

public class ItemWeapon() : ItemEquippable(), IComparable<ItemWeapon>, IEquatable<ItemWeapon>
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
		
		properties?.Sort();
		properties?.ForEach(prop => {
			if(str.Length > 0)
				str.Append(Comma);
			str.Append(prop.GetLabel());
		});
		
		return str.ToString();
	}
	
	private const string Comma = ", ";
	
	public Dictionary<DamageDie, int> DamageDice { get; set; } = [];
	public List<WeaponProperty> Properties { get; set; } = [];
	public Range Range { get; set; } = Range.Melee;
	public WeaponType Type { get; set; }
	
	public int CompareTo(ItemWeapon other)
	{
		var ret = Type.CompareTo(other?.Type);
		
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
	
	public override bool Equals(object other) => Equals(other as ItemWeapon);
	
	public bool Equals(ItemWeapon other) => base.Equals(other)
		&& DamageDice.Equals(other?.DamageDice)
		&& Properties.Equals(other?.Properties)
		&& Range.Equals(other?.Range)
		&& Type.Equals(other?.Type);
	
	public override int GetHashCode()
	{
		HashCode hash = new();
		hash.Add(base.GetHashCode());
		
		DamageDice.ToList()
			.ForEach(pair => {
				hash.Add(pair.Key);
				hash.Add(pair.Value);
			});
		
		Properties.ForEach(p => hash.Add(p));
		hash.Add(Range);
		hash.Add(Type);
		return hash.ToHashCode();
	}
}
