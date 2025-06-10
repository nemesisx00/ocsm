using System;
using System.Text;
using System.Collections.Generic;

namespace Ocsm.Dnd.Fifth.Inventory;

public class WeaponData() : IComparable<WeaponData>, IEquatable<WeaponData>
{
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
	public Range Range { get; set; } = new();
	public WeaponTypes WeaponType { get; set; }
	
	public int CompareTo(WeaponData other)
	{
		var ret = WeaponType.CompareTo(other.WeaponType);
		
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
	
	public bool Equals(WeaponData other) => base.Equals(other)
		&& Properties == other?.Properties
		&& WeaponType == other?.WeaponType;
	
	public override bool Equals(object obj) => Equals(obj as WeaponData);
	public string FormatProperties() => FormatProperties(Properties);
	
	public override int GetHashCode() => HashCode.Combine(
		base.GetHashCode(),
		DamageDice,
		Properties,
		Range,
		WeaponType
	);
}
