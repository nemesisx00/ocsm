using System;

namespace Ocsm.Dnd.Fifth;

public class FeatureSection(string section, string text) : IComparable<FeatureSection>, IEquatable<FeatureSection>
{
	public string Section { get; set; } = section;
	public string Text { get; set; } = text;
	
	public int CompareTo(FeatureSection featureSection)
	{
		var ret = Section.CompareTo(featureSection?.Section);
		
		if(ret == 0)
			ret = Text.CompareTo(featureSection?.Text);
		
		return ret;
	}
	
	public bool Equals(FeatureSection section) => Section.Equals(section.Section)
		&& Text.Equals(section.Text);
	
	public override bool Equals(object obj) => Equals(obj as FeatureSection);
	public override int GetHashCode() => HashCode.Combine(Section, Text);
}
