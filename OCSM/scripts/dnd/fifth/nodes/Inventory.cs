using Godot;
using System.Collections.Generic;
using Ocsm.Dnd.Fifth.Inventory;
using Ocsm.Dnd.Fifth.Meta;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class Inventory : VBoxContainer
{
	public sealed class NodePaths
	{
		public static readonly NodePath AddItem = new("%AddItem");
		public static readonly NodePath ItemList = new("%ItemList");
		public static readonly NodePath SelectedItem = new("%SelectedItem");
	}
	
	[Signal]
	public delegate void ItemsChangedEventHandler(Transport<List<Item>> items);
	
	public List<Item> Items { get; set; } = [];
	public Ability Strength { get; set; }
	public Ability Dexterity { get; set; }
	
	private VBoxContainer itemList;
	private InventoryItemOptions options;
	
	public override void _Ready()
	{
		itemList = GetNode<VBoxContainer>(NodePaths.ItemList);
		options = GetNode<InventoryItemOptions>(NodePaths.SelectedItem);
		
		GetNode<Button>(NodePaths.AddItem).Pressed += addItemHandler;
		
		RegenerateItems();
	}
	
	public void RegenerateItems()
	{
		foreach(Node c in itemList.GetChildren())
			c.QueueFree();
		
		var resource = GD.Load<PackedScene>(ScenePaths.Dnd.Fifth.InventoryItem);
		Items.ForEach(i => instantiateItem(i, resource));
	}
	
	private void addItemHandler()
	{
		var metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		if(metadataManager.Container is DndFifthContainer dfc)
		{
			var itemName = options.GetSelectedItemText();
			
			if(dfc.AllItems.Find(i => i.Name.Equals(itemName)) is Item item)
			{
				Items.Add(item);
				_ = EmitSignal(SignalName.ItemsChanged, new Transport<List<Item>>(Items));
				
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
		if(Items.Find(i => i.Equals(transport.Value)) is ItemEquippable item && transport.Value is ItemEquippable changed)
		{
			item.Equipped = changed.Equipped;
			_ = EmitSignal(SignalName.ItemsChanged, new Transport<List<Item>>(Items));
		}
	}
}
