using System.Collections.Generic;
using System.Linq;
using Godot;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Dnd.Fifth.Inventory;
using Ocsm.Meta;
using Ocsm.Nodes.Autoload;
using Ocsm.Nodes.Meta;

namespace Ocsm.Dnd.Fifth.Nodes.Meta;

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
	
	private ArmorEntry armorEntry;
	private FeaturefulEntry backgroundEntry;
	private FeaturefulEntry classEntry;
	private FeatureEntry featureEntry;
	private FeaturefulEntry raceEntry;
	
	public override void _ExitTree()
	{
		armorEntry.SaveClicked += saveArmor;
		armorEntry.DeleteConfirmed += deleteArmor;
		backgroundEntry.SaveClicked += saveFeatureful;
		backgroundEntry.DeleteConfirmed += deleteFeatureful;
		classEntry.SaveClicked += saveFeatureful;
		classEntry.DeleteConfirmed += deleteFeatureful;
		featureEntry.SaveClicked += saveFeature;
		featureEntry.DeleteConfirmed += deleteFeature;
		raceEntry.SaveClicked += saveFeatureful;
		raceEntry.DeleteConfirmed += deleteFeatureful;
		
		base._ExitTree();
	}
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		
		CloseRequested += closeHandler;
		
		armorEntry = GetNode<ArmorEntry>(NodePaths.ArmorName);
		armorEntry.SaveClicked += saveArmor;
		armorEntry.DeleteConfirmed += deleteArmor;
		
		backgroundEntry = GetNode<FeaturefulEntry>(NodePaths.BackgroundsName);
		backgroundEntry.SaveClicked += saveFeatureful;
		backgroundEntry.DeleteConfirmed += deleteFeatureful;
		
		classEntry = GetNode<FeaturefulEntry>(NodePaths.ClassesName);
		classEntry.SaveClicked += saveFeatureful;
		classEntry.DeleteConfirmed += deleteFeatureful;
		
		featureEntry = GetNode<FeatureEntry>(NodePaths.FeaturesName);
		featureEntry.SaveClicked += saveFeature;
		featureEntry.DeleteConfirmed += deleteFeature;
		
		raceEntry = GetNode<FeaturefulEntry>(NodePaths.RacesName);
		raceEntry.SaveClicked += saveFeatureful;
		raceEntry.DeleteConfirmed += deleteFeatureful;
	}
	
	private void closeHandler() => QueueFree();
	
	protected void deleteArmor(string name)
	{
		if(metadataManager.Container is DndFifthContainer container
			&& container.Items.Where(i => i.Name == name).FirstOrDefault() is Item item)
		{
			container.Items.Remove(item);
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
	
	protected void saveArmor(Transport<Item> transport)
	{
		if(metadataManager.Container is DndFifthContainer container)
		{
			if(container.Items.Where(i => i.Name == transport.Value.Name).FirstOrDefault() is Item existingItem)
				container.Items.Remove(existingItem);
			
			container.Items.Add(transport.Value);
			container.Items.Sort();
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
	
	protected void saveFeatureful(MetadataType type, string name, string description, Transport<List<FeatureSection>> sections, Transport<List<Feature>> features)
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
