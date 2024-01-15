using System;

namespace Ocsm.Dnd.Fifth;

public class FeatureSection(string section, string text) : IComparable<FeatureSection>, IEquatable<FeatureSection>
{
	public string Section { get; set; } = section;
	public string Text { get; set; } = text;
	
	public int CompareTo(FeatureSection other)
	{
		var ret = Section.CompareTo(other?.Section);
		
		if(ret == 0)
			ret = Text.CompareTo(other?.Text);
		
		return ret;
	}
	
	public override bool Equals(object other) => Equals(other as FeatureSection);
	
	public bool Equals(FeatureSection other) => Section.Equals(other?.Section)
		&& Text.Equals(other?.Text);
	
	public override int GetHashCode() => HashCode.Combine(Section, Text);
}
