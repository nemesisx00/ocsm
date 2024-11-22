using System.Linq;
using Godot;
using Godot.Collections;
using Ocsm.Meta;
using Ocsm.Nodes.Autoload;
using Ocsm.Nodes.Meta;
using Ocsm.Wod.VtmV5.Meta;

namespace Ocsm.Wod.VtmV5.Nodes.Meta;

public partial class WodVtmV5AddEditMetadata : Container, IAddEditMetadata
{
	
	private static class AnimationName
	{
		public static readonly StringName SlideUp = new("slideUp");
	}
	
	private static class NodePaths
	{
		public static readonly NodePath AnimationPlayer = new("%AnimationPlayer");
		public static readonly NodePath CloseButton = new("%CloseButton");
		public readonly static NodePath MetadataSelector = new("%MetadataSelector");
		public readonly static NodePath PredatorType = new("%Predator Type");
		public readonly static NodePath TabContainer = new("%TabContainer");
	}
	
	private MetadataManager metadataManager;
	
	private AnimationPlayer animPlayer;
	private MetadataEntry predatorTypeEntry;
	
	public override void _ExitTree()
	{
		predatorTypeEntry.SaveClicked -= saveMetadata;
		predatorTypeEntry.DeleteConfirmed -= deleteMetadata;
		
		base._ExitTree();
	}
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		
		animPlayer = GetNode<AnimationPlayer>(NodePaths.AnimationPlayer);
		
		predatorTypeEntry = GetNode<MetadataEntry>(NodePaths.PredatorType);
		predatorTypeEntry.SaveClicked += saveMetadata;
		predatorTypeEntry.DeleteConfirmed += deleteMetadata;
		
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
	
	private void deleteMetadata(string name, Array<string> types)
	{
		if(metadataManager.Container is WodVtmV5Container container
			&& container.Metadata.Where(m => m.Types == types.ToList() && m.Name == name).FirstOrDefault() is Metadata entry)
		{
			container.Metadata.Remove(entry);
			metadataManager.SaveGameSystemMetadata();
		}
	}
	
	private void saveMetadata(string name, string description, Array<string> types)
	{
		if(metadataManager.Container is WodVtmV5Container container)
		{
			if(container.Metadata.Where(m => m.Types == types.ToList() && m.Name == name).FirstOrDefault() is Metadata entry)
				container.Metadata.Remove(entry);
			
			container.Metadata.Add(new()
			{
				Description = description,
				Name = name,
				Types = [.. types],
			});
			
			metadataManager.SaveGameSystemMetadata();
		}
	}
}
