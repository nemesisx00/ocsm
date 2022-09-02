using System;
using System.Collections.Generic;
using OCSM.Meta;

namespace OCSM.DnD.Fifth
{
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
	
	public class Feature : Metadata, IEquatable<Feature>
	{
		//public FeatureRequirement Requirement { get; set; }
		public List<FeatureSection> Sections { get; set; }
		public string Source { get; set; }
		public string Text { get; set; }
		public string Type { get; set; }
		
		public Feature() : base()
		{
			//Requirement = null;
			Sections = new List<FeatureSection>();
			Source = String.Empty;
			Text = String.Empty;
			Type = String.Empty;
		}
		
		public Feature(string name, string description = "") : base(name, description)
		{
			Sections = new List<FeatureSection>();
			Source = String.Empty;
			Text = String.Empty;
			Type = String.Empty;
		}
		
		public bool Equals(Feature feature)
		{
			return base.Equals(feature)
				//&& feature.Requirement.Equals(Requirement)
				&& feature.Sections.Equals(Sections)
				&& feature.Source.Equals(Source)
				&& feature.Text.Equals(Text)
				&& feature.Type.Equals(Type);
		}
	}
	/*
	public class FeatureRequirement : IEquatable<FeatureRequirement>
	{
		public Background Background { get; set; } = null;
		public Class Class { get; set; } = null;
		public int Level { get; set; }
		public Race Race { get; set; } = null;
		
		public FeatureRequirement()
		{
			Background = null;
			Level = 0;
		}
		
		public bool Equals(FeatureRequirement requirement)
		{
			var output = requirement.Level.Equals(Level);
			
			if(requirement.Background is Background)
				output = output && requirement.Background.Equals(Background);
			else
				output = output && !(Background is Background);
			
			if(requirement.Class is Class)
				output = output && requirement.Class.Equals(Class);
			else
				output = output && !(Class is Class);
			
			if(requirement.Race is Race)
				output = output && requirement.Race.Equals(Race);
			else
				output = output && !(Race is Race);
			
			return output;
		}
	}
	*/
	
	public class FeatureSection : IEquatable<FeatureSection>
	{
		public string Section { get; set; }
		public string Text { get; set; }
		
		public FeatureSection(string section, string text)
		{
			Section = section;
			Text = text;
		}
		
		public bool Equals(FeatureSection section)
		{
			return section.Section.Equals(Section)
				&& section.Text.Equals(Text);
		}
	}
}
