using System.Collections.Generic;
using System.Linq;
using Godot;
using Ocsm.Dnd.Fifth;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Dnd.Fifth.Inventory;
using Ocsm.Nodes.Autoload;
using Ocsm.Nodes.Meta;

namespace Ocsm.Nodes.Dnd.Fifth.Meta;

public partial class DndFifthAddEditMetadata : Window, IAddEditMetadata
{
	private static class NodePaths
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
		metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		
		CloseRequested += closeHandler;
		
		var armorEntry = GetNode<ArmorEntry>(NodePaths.ArmorName);
		armorEntry.SaveClicked += saveArmor;
		armorEntry.DeleteConfirmed += deleteArmor;
		
		var backgroundEntry = GetNode<FeaturefulEntry>(NodePaths.BackgroundsName);
		backgroundEntry.SaveClicked += saveFeatureful;
		backgroundEntry.DeleteConfirmed += deleteFeatureful;
		
		var classEntry = GetNode<FeaturefulEntry>(NodePaths.ClassesName);
		classEntry.SaveClicked += saveFeatureful;
		classEntry.DeleteConfirmed += deleteFeatureful;
		
		var featureEntry = GetNode<FeatureEntry>(NodePaths.FeaturesName);
		featureEntry.SaveClicked += saveFeature;
		featureEntry.DeleteConfirmed += deleteFeature;
		
		var raceEntry = GetNode<FeaturefulEntry>(NodePaths.RacesName);
		raceEntry.SaveClicked += saveFeatureful;
		raceEntry.DeleteConfirmed += deleteFeatureful;
	}
	
	private void closeHandler() => QueueFree();
	
	protected void deleteArmor(string name)
	{
		if(metadataManager.Container is DndFifthContainer container
			&& container.Armors.Where(a => a.Name == name).FirstOrDefault() is ItemArmor armor)
		{
			container.Armors.Remove(armor);
			metadataManager.SaveGameSystemMetadata();
		}
	}
	
	protected void deleteFeature(string name)
	{
		if(metadataManager.Container is DndFifthContainer container
			&& container.Features.Where(f => f.Name == name).FirstOrDefault() is Feature feature)
		{
			container.Features.Remove(feature);
			metadataManager.SaveGameSystemMetadata();
		}
	}
	
	protected void deleteFeatureful(MetadataType type, string name)
	{
		if(metadataManager.Container is DndFifthContainer container
			&& container.Featurefuls.Where(f => f.Type == type && f.Name == name).FirstOrDefault() is Featureful entry)
		{
			container.Featurefuls.Remove(entry);
			metadataManager.SaveGameSystemMetadata();
		}
	}
	
	protected void saveArmor(Transport<ItemArmor> transport)
	{
		if(metadataManager.Container is DndFifthContainer container)
		{
			if(container.Armors.Where(a => a.Name == transport.Value.Name).FirstOrDefault() is ItemArmor existingArmor)
				container.Armors.Remove(existingArmor);
			
			container.Armors.Add(transport.Value);
			container.Armors.Sort();
			metadataManager.SaveGameSystemMetadata();
		}
	}
	
	protected void saveFeature(Transport<Feature> transport)
	{
		if(metadataManager.Container is DndFifthContainer container)
		{
			if(container.Features.Find(f => f.Name == transport.Value.Name) is Feature feature)
				container.Features.Remove(feature);
			
			container.Features.Add(transport.Value);
			container.Features.Sort();
			metadataManager.SaveGameSystemMetadata();
		}
	}
	
	protected void saveFeatureful(MetadataType type, string name, string description, Transport<List<Ocsm.Dnd.Fifth.FeatureSection>> sections, Transport<List<Ocsm.Dnd.Fifth.Feature>> features)
	{
		if(metadataManager.Container is DndFifthContainer container)
		{
			if(container.Featurefuls.Where(f => f.Type == type && f.Name == name).FirstOrDefault() is Featureful entry)
				container.Featurefuls.Remove(entry);
			
			List<FeatureSection> sectionList = [];
			List<Feature> featureList = [];
			
			sections.Value.ForEach(fs => sectionList.Add(fs));
			features.Value.ForEach(f => featureList.Add(f));
			
			container.Featurefuls.Add(new() {
				Description = description,
				Features = featureList,
				Name = name,
				Sections = sectionList,
				Type = type,
			});
			
			container.Featurefuls.Sort();
			metadataManager.SaveGameSystemMetadata();
		}
	}
}
