using Godot;
using System.Collections.Generic;
using System.Linq;
using Ocsm.Dnd.Fifth;
using Ocsm.Dnd.Fifth.Inventory;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes.Dnd.Fifth;

public partial class Inventory : VBoxContainer
{
	public static class NodePaths
	{
		public static readonly NodePath AddItem = new("%AddItem");
		public static readonly NodePath ItemList = new("%ItemList");
		public static readonly NodePath SelectedItem = new("%SelectedItem");
	}
	
	[Signal]
	public delegate void ItemsChangedEventHandler(Transport<List<Item>> items);
	
	public List<Item> Items { get; set; }
	public AbilityInfo Strength { get; set; }
	public AbilityInfo Dexterity { get; set; }
	
	private VBoxContainer itemList;
	private InventoryItemOptions options;
	
	public override void _Ready()
	{
		Items ??= [];
		
		itemList = GetNode<VBoxContainer>(NodePaths.ItemList);
		options = GetNode<InventoryItemOptions>(NodePaths.SelectedItem);
		
		GetNode<Button>(NodePaths.AddItem).Pressed += addItemHandler;
		
		RegenerateItems();
	}
	
	public void RegenerateItems()
	{
		foreach(Node c in itemList.GetChildren())
		{
			c.QueueFree();
		}
		
		var resource = GD.Load<PackedScene>(Constants.Scene.Dnd.Fifth.InventoryItem);
		Items.ForEach(i => instantiateItem(i, resource));
	}
	
	private void addItemHandler()
	{
		var metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		if(metadataManager.Container is DndFifthContainer container)
		{
			var itemName = options.GetSelectedItemText();
			if(container.AllItems.Find(i => i.Name.Equals(itemName)) is Item item)
			{
				Items.Add(item);
				EmitSignal(SignalName.ItemsChanged, new Transport<List<Item>>(Items));
				
				RegenerateItems();
			}
			options.Deselect();
		}
	}
	
	private void instantiateItem(Item item, PackedScene resource)
	{
		var instance = resource.Instantiate<InventoryItem>();
		instance.Item = item;
		instance.Strength = Strength;
		instance.Dexterity = Dexterity;
		instance.Equipped += itemEquipped;
		
		itemList.AddChild(instance);
		instance.Refresh();
		instance.Visible = true;
	}
	
	private void itemEquipped(Transport<Item> transport)
	{
		if(transport.Value is ItemEquippable changed
			&& Items.Where(i => i == transport.Value).FirstOrDefault() is ItemEquippable item)
		{
			item.Equipped = changed.Equipped;
			EmitSignal(SignalName.ItemsChanged, new Transport<List<Item>>(Items));
		}
	}
}
