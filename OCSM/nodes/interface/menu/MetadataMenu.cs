using System.Reflection;
using Godot;
using Ocsm.Cofd.Ctl.Nodes.Meta;
using Ocsm.Dnd.Fifth.Nodes.Meta;
using Ocsm.Nodes.Autoload;
using Ocsm.Nodes.Meta;
using Ocsm.Wod.VtmV5.Nodes.Meta;

namespace Ocsm.Nodes;

public partial class MetadataMenu : MenuButton
{
	private static class ItemNames
	{
		public static readonly StringName ManageMetadata = new("Manage Metadata");
	}
	
	public enum MenuItem
	{
		ManageMetadata,
	}
	
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		
		var popup = GetPopup();
		popup.AddItem(ItemNames.ManageMetadata, (int)MenuItem.ManageMetadata);
		popup.IdPressed += handleMenuItem;
	}
	
	private void handleMenuItem(long id)
	{
		switch((MenuItem)id)
		{
			case MenuItem.ManageMetadata:
				showAddEditMetadata();
				break;
		}
	}
	
	private void showAddEditMetadata()
	{
		var node = metadataManager.CurrentGameSystem.FactoryType
			.GetMethod("GenerateAddEditMetadata", BindingFlags.Public | BindingFlags.Static)?
			.Invoke(null, null) as Node;
		
		if(node is not null)
			GetTree().CurrentScene.AddChild(node);
	}
}
