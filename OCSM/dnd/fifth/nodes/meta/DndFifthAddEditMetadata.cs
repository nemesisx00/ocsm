using Godot;
using System.Linq;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Dnd.Fifth.Inventory;
using Ocsm.Meta;
using Ocsm.Nodes.Autoload;
using Ocsm.Nodes.Meta;

namespace Ocsm.Dnd.Fifth.Nodes.Meta;

public partial class DndFifthAddEditMetadata : Container, IAddEditMetadata
{
	private static class AnimationName
	{
		public static readonly StringName SlideUp = new("slideUp");
	}
	
	private static class NodePaths
	{
		public static readonly NodePath AnimationPlayer = new("%AnimationPlayer");
		public static readonly NodePath ArmorName = new("%Armor");
		public static readonly NodePath BackgroundsName = new("%Backgrounds");
		public static readonly NodePath ClassesName = new("%Classes");
		public static readonly NodePath CloseButton = new("%CloseButton");
		public static readonly NodePath FeaturesName = new("%FeatureEntry");
		public static readonly NodePath SpeciesName = new("%Species");
	}
	
	private MetadataManager metadataManager;
	
	private AnimationPlayer animPlayer;
	private ArmorEntry armorEntry;
	private MetadataEntry backgroundEntry;
	private MetadataEntry classEntry;
	private FeatureEntry featureEntry;
	private MetadataEntry speciesEntry;
	
	public override void _ExitTree()
	{
		armorEntry.SaveClicked += saveArmor;
		armorEntry.DeleteConfirmed += deleteArmor;
		backgroundEntry.SaveClicked += saveMetadata;
		backgroundEntry.DeleteConfirmed += deleteMetadata;
		classEntry.SaveClicked += saveMetadata;
		classEntry.DeleteConfirmed += deleteMetadata;
		featureEntry.SaveClicked += saveFeature;
		featureEntry.DeleteConfirmed += deleteFeature;
		speciesEntry.SaveClicked += saveMetadata;
		speciesEntry.DeleteConfirmed += deleteMetadata;
		
		base._ExitTree();
	}
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		
		animPlayer = GetNode<AnimationPlayer>(NodePaths.AnimationPlayer);
		
		armorEntry = GetNode<ArmorEntry>(NodePaths.ArmorName);
		armorEntry.SaveClicked += saveArmor;
		armorEntry.DeleteConfirmed += deleteArmor;
		
		backgroundEntry = GetNode<MetadataEntry>(NodePaths.BackgroundsName);
		backgroundEntry.SaveClicked += saveMetadata;
		backgroundEntry.DeleteConfirmed += deleteMetadata;
		
		classEntry = GetNode<MetadataEntry>(NodePaths.ClassesName);
		classEntry.SaveClicked += saveMetadata;
		classEntry.DeleteConfirmed += deleteMetadata;
		
		featureEntry = GetNode<FeatureEntry>(NodePaths.FeaturesName);
		featureEntry.SaveClicked += saveFeature;
		featureEntry.DeleteConfirmed += deleteFeature;
		
		speciesEntry = GetNode<MetadataEntry>(NodePaths.SpeciesName);
		speciesEntry.SaveClicked += saveMetadata;
		speciesEntry.DeleteConfirmed += deleteMetadata;
		
		GetNode<Button>(NodePaths.CloseButton).Pressed += Close;
		
		animPlayer.Play(AnimationName.SlideUp);
	}
	
	public override void _UnhandledKeyInput(InputEvent evt)
	{
		if(evt.IsActionReleased(Actions.Cancel))
			Close();
	}
	
	public void Close()
	{
		animPlayer.PlayBackwards(AnimationName.SlideUp);
		animPlayer.AnimationFinished += anim => QueueFree();
	}
	
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
	
	protected void deleteMetadata(string name, MetadataType type)
	{
		if(metadataManager.Container is DndFifthContainer container
			&& container.Metadata.Where(m => m.Type == type && m.Name == name).FirstOrDefault() is Metadata entry)
		{
			container.Metadata.Remove(entry);
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
	
	protected void saveMetadata(string name, string description, MetadataType type)
	{
		if(metadataManager.Container is DndFifthContainer container)
		{
			if(container.Metadata.Where(m => m.Type == type && m.Name == name).FirstOrDefault() is Metadata entry)
				container.Metadata.Remove(entry);
			
			container.Metadata.Add(new() {
				Description = description,
				Name = name,
				Type = type,
			});
			
			metadataManager.SaveGameSystemMetadata();
		}
	}
}
