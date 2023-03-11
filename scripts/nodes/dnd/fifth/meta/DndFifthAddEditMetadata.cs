using Godot;
using System.Collections.Generic;
using OCSM.DnD.Fifth;
using OCSM.DnD.Fifth.Meta;
using OCSM.DnD.Fifth.Inventory;
using OCSM.Nodes.Autoload;
using OCSM.Nodes.Meta;

namespace OCSM.Nodes.DnD.Fifth.Meta
{
	public partial class DndFifthAddEditMetadata : BaseAddEditMetadata
	{
		private sealed class NodePath
		{
			public const string ArmorName = "%Armor";
			public const string BackgroundsName = "%Backgrounds";
			public const string ClassesName = "%Classes";
			public const string FeaturesName = "%FeatureEntry";
			public const string RacesName = "%Races";
		}
		
		private MetadataManager metadataManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			
			CloseRequested += closeHandler;
			
			var armorEntry = GetNode<ArmorEntry>(NodePath.ArmorName);
			armorEntry.SaveClicked += saveArmor;
			armorEntry.DeleteConfirmed += deleteArmor;
			
			var backgroundEntry = GetNode<BackgroundEntry>(NodePath.BackgroundsName);
			backgroundEntry.SaveClicked += saveBackground;
			backgroundEntry.DeleteConfirmed += deleteBackground;
			
			var classEntry = GetNode<ClassEntry>(NodePath.ClassesName);
			classEntry.SaveClicked += saveClass;
			classEntry.DeleteConfirmed += deleteClass;
			
			var featureEntry = GetNode<FeatureEntry>(NodePath.FeaturesName);
			featureEntry.SaveClicked += saveFeature;
			featureEntry.DeleteConfirmed += deleteFeature;
			
			var raceEntry = GetNode<RaceEntry>(NodePath.RacesName);
			raceEntry.SaveClicked += saveRace;
			raceEntry.DeleteConfirmed += deleteRace;
		}
		
		private void closeHandler()
		{
			QueueFree();
		}
		
		protected void deleteArmor(string name)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Armors.Find(a => a.Name.Equals(name)) is ItemArmor armor)
				{
					dfc.Armors.Remove(armor);
					EmitSignal(nameof(MetadataChanged));
				}
			}
		}
		
		protected void deleteBackground(string name)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Backgrounds.Find(b => b.Name.Equals(name)) is Background background)
				{
					dfc.Backgrounds.Remove(background);
					EmitSignal(nameof(MetadataChanged));
				}
			}
		}
		
		protected void deleteClass(string name)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Classes.Find(c => c.Name.Equals(name)) is Class clazz)
				{
					dfc.Classes.Remove(clazz);
					EmitSignal(nameof(MetadataChanged));
				}
			}
		}
		
		protected void deleteFeature(string name)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Features.Find(f => f.Name.Equals(name)) is OCSM.DnD.Fifth.Feature feature)
				{
					dfc.Features.Remove(feature);
					EmitSignal(nameof(MetadataChanged));
				}
			}
		}
		
		protected void deleteRace(string name)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Races.Find(r => r.Name.Equals(name)) is Race race)
				{
					dfc.Races.Remove(race);
					EmitSignal(nameof(MetadataChanged));
				}
			}
		}
		
		protected void saveArmor(Transport<ItemArmor> transport)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				var armor = transport.Value;
				
				if(dfc.Armors.Find(a => a.Name.Equals(armor.Name)) is ItemArmor existingArmor)
					dfc.Armors.Remove(existingArmor);
				
				dfc.Armors.Add(armor);
				dfc.Armors.Sort();
				EmitSignal(nameof(MetadataChanged));
			}
		}
		
		protected void saveBackground(string name, string description, Transport<List<OCSM.DnD.Fifth.FeatureSection>> sections, Transport<List<OCSM.DnD.Fifth.Feature>> features)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Backgrounds.Find(b => b.Name.Equals(name)) is Background background)
					dfc.Backgrounds.Remove(background);
				
				var sectionList = new List<OCSM.DnD.Fifth.FeatureSection>();
				sections.Value.ForEach(fs => sectionList.Add(fs));
				
				var featureList = new List<OCSM.DnD.Fifth.Feature>();
				features.Value.ForEach(f => featureList.Add(f));
				
				dfc.Backgrounds.Add(new Background() { Description = description, Features = featureList, Name = name, Sections = sectionList, });
				dfc.Backgrounds.Sort();
				EmitSignal(nameof(MetadataChanged));
			}
		}
		
		protected void saveClass(string name, string description, Transport<List<OCSM.DnD.Fifth.FeatureSection>> sections, Transport<List<OCSM.DnD.Fifth.Feature>> features)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Classes.Find(c => c.Name.Equals(name)) is Class clazz)
					dfc.Classes.Remove(clazz);
				
				var sectionList = new List<OCSM.DnD.Fifth.FeatureSection>();
				sections.Value.ForEach(fs => sectionList.Add(fs));
				
				var featureList = new List<OCSM.DnD.Fifth.Feature>();
				features.Value.ForEach(f => featureList.Add(f));
				
				dfc.Classes.Add(new Class() { Description = description, Features = featureList, HitDie = OCSM.DnD.Fifth.Die.d10, Name = name, Sections = sectionList, });
				dfc.Classes.Sort();
				EmitSignal(nameof(MetadataChanged));
			}
		}
		
		protected void saveFeature(Transport<OCSM.DnD.Fifth.Feature> transport)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Features.Find(f => f.Name.Equals(transport.Value.Name)) is OCSM.DnD.Fifth.Feature feature)
					dfc.Features.Remove(feature);
				
				dfc.Features.Add(transport.Value);
				dfc.Features.Sort();
				EmitSignal(nameof(MetadataChanged));
			}
		}
		
		protected void saveRace(string name, string description, Transport<List<OCSM.DnD.Fifth.FeatureSection>> sections, Transport<List<OCSM.DnD.Fifth.Feature>> features)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Races.Find(r => r.Name.Equals(name)) is Race race)
					dfc.Races.Remove(race);
				
				var sectionList = new List<OCSM.DnD.Fifth.FeatureSection>();
				sections.Value.ForEach(fs => sectionList.Add(fs));
				
				var featureList = new List<OCSM.DnD.Fifth.Feature>();
				features.Value.ForEach(f => featureList.Add(f));
				
				dfc.Races.Add(new Race() { Description = description, Features = featureList, Name = name, Sections = sectionList, });
				dfc.Races.Sort();
				EmitSignal(nameof(MetadataChanged));
			}
		}
	}
}
