using Godot;
using System.Collections.Generic;
using OCSM.DnD.Fifth;
using OCSM.DnD.Fifth.Inventory;
using OCSM.DnD.Fifth.Meta;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes.DnD.Fifth
{
	public partial class Inventory : VBoxContainer
	{
		public sealed class NodePath
		{
			public const string AddItem = "%AddItem";
			public const string ItemList = "%ItemList";
			public const string SelectedItem = "%SelectedItem";
		}
		
		[Signal]
		public delegate void ItemsChangedEventHandler(Transport<List<Item>> items);
		
		public List<Item> Items { get; set; }
		public Ability Strength { get; set; }
		public Ability Dexterity { get; set; }
		
		private VBoxContainer itemList;
		private InventoryItemOptions options;
		
		public override void _Ready()
		{
			if(!(Items is List<Item>))
				Items = new List<Item>();
			
			itemList = GetNode<VBoxContainer>(NodePath.ItemList);
			options = GetNode<InventoryItemOptions>(NodePath.SelectedItem);
			
			GetNode<Button>(NodePath.AddItem).Pressed += addItemHandler;
			
			regenerateItems();
		}
		
		public void regenerateItems()
		{
			foreach(Node c in itemList.GetChildren())
			{
				c.QueueFree();
			}
			
			var resource = GD.Load<PackedScene>(Constants.Scene.DnD.Fifth.InventoryItem);
			Items.ForEach(i => instantiateItem(i, resource));
		}
		
		private void addItemHandler()
		{
			var metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				var itemName = options.GetSelectedItemText();
				if(dfc.AllItems.Find(i => i.Name.Equals(itemName)) is Item item)
				{
					Items.Add(item);
					EmitSignal(nameof(ItemsChanged), new Transport<List<Item>>(Items));
					
					regenerateItems();
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
			instance.refresh();
			instance.Visible = true;
		}
		
		private void itemEquipped(Transport<Item> transport)
		{
			if(Items.Find(i => i.Equals(transport.Value)) is ItemEquippable item && transport.Value is ItemEquippable changed)
			{
				item.Equipped = changed.Equipped;
				EmitSignal(nameof(ItemsChanged), new Transport<List<Item>>(Items));
			}
		}
	}
}
