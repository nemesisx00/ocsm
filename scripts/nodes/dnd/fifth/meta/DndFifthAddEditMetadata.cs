using Godot;
using System.Collections.Generic;
using OCSM.Nodes.Autoload;
using OCSM.DnD.Fifth;
using OCSM.DnD.Fifth.Meta;
using OCSM.DnD.Fifth.Inventory;

namespace OCSM.Nodes.DnD.Fifth.Meta
{
	public class DndFifthAddEditMetadata : WindowDialog
	{
		private const string ArmorName = "Armor";
		private const string BackgroundsName = "Backgrounds";
		private const string ClassesName = "Classes";
		private const string FeaturesName = "FeatureEntry";
		private const string RacesName = "Races";
		
		[Signal]
		public delegate void MetadataChanged();
		
		private MetadataManager metadataManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			GetCloseButton().Connect(Constants.Signal.Pressed, this, nameof(closeHandler));
			
			var armorEntry = GetNode<ArmorEntry>(NodePathBuilder.SceneUnique(ArmorName));
			armorEntry.Connect(nameof(ArmorEntry.SaveClicked), this, nameof(saveArmor));
			armorEntry.Connect(nameof(ArmorEntry.DeleteConfirmed), this, nameof(deleteArmor));
			
			var backgroundEntry = GetNode<BackgroundEntry>(NodePathBuilder.SceneUnique(BackgroundsName));
			backgroundEntry.Connect(nameof(BackgroundEntry.SaveClicked), this, nameof(saveBackground));
			backgroundEntry.Connect(nameof(BackgroundEntry.DeleteConfirmed), this, nameof(deleteBackground));
			
			var classEntry = GetNode<ClassEntry>(NodePathBuilder.SceneUnique(ClassesName));
			classEntry.Connect(nameof(ClassEntry.SaveClicked), this, nameof(saveClass));
			classEntry.Connect(nameof(ClassEntry.DeleteConfirmed), this, nameof(deleteClass));
			
			var featureEntry = GetNode<FeatureEntry>(NodePathBuilder.SceneUnique(FeaturesName));
			featureEntry.Connect(nameof(FeatureEntry.SaveClicked), this, nameof(saveFeature));
			featureEntry.Connect(nameof(FeatureEntry.DeleteConfirmed), this, nameof(deleteFeature));
			
			var raceEntry = GetNode<RaceEntry>(NodePathBuilder.SceneUnique(RacesName));
			raceEntry.Connect(nameof(RaceEntry.SaveClicked), this, nameof(saveRace));
			raceEntry.Connect(nameof(RaceEntry.DeleteConfirmed), this, nameof(deleteRace));
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
		
		protected void saveBackground(string name, string description, List<Transport<OCSM.DnD.Fifth.FeatureSection>> sections, List<Transport<OCSM.DnD.Fifth.Feature>> features)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Backgrounds.Find(b => b.Name.Equals(name)) is Background background)
					dfc.Backgrounds.Remove(background);
				
				var sectionList = new List<OCSM.DnD.Fifth.FeatureSection>();
				sections.ForEach(t => sectionList.Add(t.Value));
				
				var featureList = new List<OCSM.DnD.Fifth.Feature>();
				features.ForEach(t => featureList.Add(t.Value));
				
				dfc.Backgrounds.Add(new Background() { Description = description, Features = featureList, Name = name, Sections = sectionList, });
				dfc.Backgrounds.Sort();
				EmitSignal(nameof(MetadataChanged));
			}
		}
		
		protected void saveClass(string name, string description, List<Transport<OCSM.DnD.Fifth.FeatureSection>> sections, List<Transport<OCSM.DnD.Fifth.Feature>> features)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Classes.Find(c => c.Name.Equals(name)) is Class clazz)
					dfc.Classes.Remove(clazz);
				
				var sectionList = new List<OCSM.DnD.Fifth.FeatureSection>();
				sections.ForEach(t => sectionList.Add(t.Value));
				
				var featureList = new List<OCSM.DnD.Fifth.Feature>();
				features.ForEach(t => featureList.Add(t.Value));
				
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
		
		protected void saveRace(string name, string description, List<Transport<OCSM.DnD.Fifth.FeatureSection>> sections, List<Transport<OCSM.DnD.Fifth.Feature>> features)
		{
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Races.Find(r => r.Name.Equals(name)) is Race race)
					dfc.Races.Remove(race);
				
				var sectionList = new List<OCSM.DnD.Fifth.FeatureSection>();
				sections.ForEach(t => sectionList.Add(t.Value));
				
				var featureList = new List<OCSM.DnD.Fifth.Feature>();
				features.ForEach(t => featureList.Add(t.Value));
				
				dfc.Races.Add(new Race() { Description = description, Features = featureList, Name = name, Sections = sectionList, });
				dfc.Races.Sort();
				EmitSignal(nameof(MetadataChanged));
			}
		}
	}
}
