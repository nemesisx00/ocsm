using System;
using System.Collections.Generic;
using Ocsm.Meta;

namespace Ocsm.Dnd.Fifth;

public sealed class FeatureType
{
	public const string Background = "Background";
	public const string Class = "Class";
	public const string Race = "Race";
	
	public static List<string> asList()
	{
		var list = new List<string>();
		list.Add(Background);
		list.Add(Class);
		list.Add(Race);
		return list;
	}
}

public class Feature : Metadata, IComparable<Feature>, IEquatable<Feature>
{
	public string ClassName { get; set; }
	public List<NumericBonus> NumericBonuses { get; set; }
	public int RequiredLevel { get; set; }
	public List<FeatureSection> Sections { get; set; }
	public string Source { get; set; }
	public string Text { get; set; }
	public string Type { get; set; }
	
	public Feature() : base()
	{
		ClassName = String.Empty;
		NumericBonuses = new List<NumericBonus>();
		RequiredLevel = 0;
		Sections = new List<FeatureSection>();
		Source = String.Empty;
		Text = String.Empty;
		Type = String.Empty;
	}
	
	public int CompareTo(Feature feature)
	{
		var ret = 0;
		if(feature is Feature)
		{
			ret = Type.CompareTo(feature.Type);
			if(ret.Equals(0))
				ret = RequiredLevel.CompareTo(feature.RequiredLevel);
			if(ret.Equals(0))
				ret = Name.CompareTo(feature.Name);
			if(ret.Equals(0))
				ret = ClassName.CompareTo(feature.ClassName);
		}
		return ret;
	}
	
	public bool Equals(Feature feature)
	{
		return base.Equals(feature)
			&& NumericBonuses.Equals(feature.NumericBonuses)
			&& RequiredLevel.Equals(feature.RequiredLevel)
			&& Sections.Equals(feature.Sections)
			&& Source.Equals(feature.Source)
			&& Text.Equals(feature.Text)
			&& Type.Equals(feature.Type);
	}
}

public class FeatureSection : IComparable<FeatureSection>, IEquatable<FeatureSection>
{
	public string Section { get; set; }
	public string Text { get; set; }
	
	public FeatureSection(string section, string text)
	{
		Section = section;
		Text = text;
	}
	
	public int CompareTo(FeatureSection featureSection)
	{
		var ret = 0;
		if(featureSection is FeatureSection)
		{
			ret = Section.CompareTo(featureSection.Section);
			if(ret.Equals(0))
				ret = Text.CompareTo(featureSection.Text);
		}
		return ret;
	}
	
	public bool Equals(FeatureSection section)
	{
		return Section.Equals(section.Section)
			&& Text.Equals(section.Text);
	}
}
