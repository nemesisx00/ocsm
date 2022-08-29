using Godot;
using System;
using OCSM;
using OCSM.Meta;

public class MetadataMenu : MenuButton
{
	public enum CoDChanglingItem { Contract, Kit, Seeming }
	public enum DnDFifthItem { Background, Class, Feature, Race }
	
	private const string AddMenuLabel = "Add";
	private const string AddMenuName = "addMenu";
	
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		metadataManager.Connect(nameof(MetadataManager.GameSystemChanged), this, nameof(gameSystemChanged));
		
		refreshMenuItems();
	}
	
	private void refreshMenuItems()
	{
		var popup = GetPopup();
		
		var oldMenuItem = popup.GetNodeOrNull<PopupMenu>("addMenu");
		if(oldMenuItem is PopupMenu)
		{
			popup.RemoveItem(1);
			popup.RemoveChild(oldMenuItem);
			oldMenuItem.QueueFree();
		}
		
		var addMenu = new PopupMenu();
		addMenu.Name = AddMenuName;
		
		switch(metadataManager.CurrentGameSystem)
		{
			case GameSystem.CoD.Changeling:
				addMenu.AddItem("Contract");
				addMenu.AddItem("Kith");
				addMenu.AddItem("Seeming");
				break;
			case GameSystem.DnD.Fifth:
				addMenu.AddItem("Background");
				addMenu.AddItem("Class");
				addMenu.AddItem("Feature");
				addMenu.AddItem("Race");
				break;
			case GameSystem.CoD.Mortal:
			default:
				break;
		}
		
		
		addMenu.Connect(Constants.Signal.IdPressed, this, nameof(handleAddMenuItem));
		
		if(addMenu.GetItemCount() > 0)
		{
			popup.AddChild(addMenu);
			popup.AddSubmenuItem(AddMenuLabel, AddMenuName);
		}
		else
			addMenu.QueueFree();
	}
	
	private void gameSystemChanged(string gameSystem)
	{
		refreshMenuItems();
	}
	
	private void handleAddMenuItem(int id)
	{
		switch(metadataManager.CurrentGameSystem)
		{
			case GameSystem.CoD.Changeling:
				switch((CoDChanglingItem)id)
				{
					case CoDChanglingItem.Contract:
						GD.Print("Add Contract not implemented");
						break;
					case CoDChanglingItem.Kit:
						GD.Print("Add Kit not implemented");
						break;
					case CoDChanglingItem.Seeming:
						GD.Print("Add Seeming not implemented");
						break;
					default:
						break;
				}
				break;
			case GameSystem.DnD.Fifth:
				switch((DnDFifthItem)id)
				{
					case DnDFifthItem.Background:
						GD.Print("Add Background not implemented");
						break;
					case DnDFifthItem.Class:
						GD.Print("Add Class not implemented");
						break;
					case DnDFifthItem.Feature:
						showAddDndFeature();
						break;
					case DnDFifthItem.Race:
						GD.Print("Add Race not implemented");
						break;
					default:
						break;
				}
				break;
			case GameSystem.CoD.Mortal:
			default:
				break;
		}
	}
	
	private void showAddDndFeature()
	{
		var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.DnD.Fifth.NewFeature);
		var instance = resource.Instance<NewFeature>();
		GetTree().CurrentScene.AddChild(instance);
		NodeUtilities.centerControl(instance, GetViewportRect().GetCenter());
		instance.Popup_();
	}
}
