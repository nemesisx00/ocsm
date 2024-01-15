using Godot;
using System.Collections.Generic;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Dnd.Fifth.Inventory;
using Ocsm.Nodes.Autoload;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class DndFifthAddEditMetadata : BaseAddEditMetadata
{
	private sealed class NodePaths
	{
		public static readonly NodePath ArmorName = new("%Armor");
		public static readonly NodePath BackgroundsName = new("%Backgrounds");
		public static readonly NodePath ClassesName = new("%Classes");
		public static readonly NodePath FeaturesName = new("%FeatureEntry");
		public static readonly NodePath RacesName = new("%Races");
	}
	
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		
		CloseRequested += QueueFree;
		
		var armorEntry = GetNode<ArmorEntry>(NodePaths.ArmorName);
		armorEntry.SaveClicked += saveArmor;
		armorEntry.DeleteConfirmed += deleteArmor;
		
		var backgroundEntry = GetNode<BackgroundEntry>(NodePaths.BackgroundsName);
		backgroundEntry.FeaturefulSaveClicked += saveBackground;
		backgroundEntry.DeleteConfirmed += deleteBackground;
		
		var classEntry = GetNode<ClassEntry>(NodePaths.ClassesName);
		classEntry.FeaturefulSaveClicked += saveClass;
		classEntry.DeleteConfirmed += deleteClass;
		
		var featureEntry = GetNode<FeatureEntry>(NodePaths.FeaturesName);
		featureEntry.SaveClicked += saveFeature;
		featureEntry.DeleteConfirmed += deleteFeature;
		
		var raceEntry = GetNode<RaceEntry>(NodePaths.RacesName);
		raceEntry.FeaturefulSaveClicked += saveRace;
		raceEntry.DeleteConfirmed += deleteRace;
	}
	
	protected void deleteArmor(string name)
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Armors.Find(a => a.Name.Equals(name)) is ItemArmor armor)
			{
				dfc.Armors.Remove(armor);
				EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
			}
		}
	}
	
	protected void deleteBackground(string name)
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Backgrounds.Find(b => b.Name.Equals(name)) is Background background)
			{
				dfc.Backgrounds.Remove(background);
				EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
			}
		}
	}
	
	protected void deleteClass(string name)
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Classes.Find(c => c.Name.Equals(name)) is Class clazz)
			{
				dfc.Classes.Remove(clazz);
				EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
			}
		}
	}
	
	protected void deleteFeature(string name)
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Features.Find(f => f.Name.Equals(name)) is Fifth.Feature feature)
			{
				dfc.Features.Remove(feature);
				EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
			}
		}
	}
	
	protected void deleteRace(string name)
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Races.Find(r => r.Name.Equals(name)) is Race race)
			{
				dfc.Races.Remove(race);
				EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
			}
		}
	}
	
	protected void saveArmor(Transport<ItemArmor> transport)
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var armor = transport.Value;
			
			if(dfc.Armors.Find(a => a.Name.Equals(armor.Name)) is ItemArmor existingArmor)
				dfc.Armors.Remove(existingArmor);
			
			dfc.Armors.Add(armor);
			dfc.Armors.Sort();
			EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
		}
	}
	
	protected void saveBackground(string name, string description, Transport<List<FeatureSection>> sections, Transport<List<Fifth.Feature>> features)
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Backgrounds.Find(b => b.Name.Equals(name)) is Background background)
				dfc.Backgrounds.Remove(background);
			
			List<FeatureSection> sectionList = [];
			sections.Value.ForEach(fs => sectionList.Add(fs));
			
			List<Fifth.Feature> featureList = [];
			features.Value.ForEach(f => featureList.Add(f));
			
			dfc.Backgrounds.Add(new()
			{
				Description = description,
				Features = featureList,
				Name = name,
				Sections = sectionList,
			});
			
			dfc.Backgrounds.Sort();
			EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
		}
	}
	
	protected void saveClass(string name, string description, Transport<List<FeatureSection>> sections, Transport<List<Fifth.Feature>> features)
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Classes.Find(c => c.Name.Equals(name)) is Class clazz)
				dfc.Classes.Remove(clazz);
			
			List<FeatureSection> sectionList = [];
			sections.Value.ForEach(fs => sectionList.Add(fs));
			
			List<Fifth.Feature> featureList = [];
			features.Value.ForEach(f => featureList.Add(f));
			
			dfc.Classes.Add(new()
			{
				Description = description,
				Features = featureList,
				HitDie = Die.Ten,
				Name = name,
				Sections = sectionList,
			});
			
			dfc.Classes.Sort();
			EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
		}
	}
	
	protected void saveFeature(Transport<Fifth.Feature> transport)
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Features.Find(f => f.Name.Equals(transport.Value.Name)) is Fifth.Feature feature)
				dfc.Features.Remove(feature);
			
			dfc.Features.Add(transport.Value);
			dfc.Features.Sort();
			EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
		}
	}
	
	protected void saveRace(string name, string description, Transport<List<FeatureSection>> sections, Transport<List<Fifth.Feature>> features)
	{
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			if(dfc.Races.Find(r => r.Name.Equals(name)) is Race race)
				dfc.Races.Remove(race);
			
			List<FeatureSection> sectionList = [];
			sections.Value.ForEach(fs => sectionList.Add(fs));
			
			List<Fifth.Feature> featureList = [];
			features.Value.ForEach(f => featureList.Add(f));
			
			dfc.Races.Add(new()
			{
				Description = description,
				Features = featureList,
				Name = name,
				Sections = sectionList,
			});
			
			dfc.Races.Sort();
			EmitSignal(BaseAddEditMetadata.SignalName.MetadataChanged);
		}
	}
}
