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
			
			var itemScene = GD.Load<PackedScene>(Constants.Scene.DnD.Fifth.InventoryItem);
			foreach(Item i in Items)
			{
				var instance = itemScene.Instantiate<InventoryItem>();
				instance.Item = i;
				instance.Strength = Strength;
				instance.Dexterity = Dexterity;
				instance.Equipped += itemEquipped;
				
				itemList.AddChild(instance);
				instance.refresh();
				instance.Visible = true;
			}
			
			//Dynamically update rect.min_x for everything here, based on largest values
		}
		
		private void addItemHandler()
		{
			var metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(options.Selected > 0 && dfc.AllItems.Find(i => i.Name.Equals(options.GetItemText(options.Selected))) is Item item)
				{
					Items.Add(item);
					EmitSignal(nameof(ItemsChanged), new Transport<List<Item>>(Items));
					
					regenerateItems();
				}
				options.Selected = 0;
			}
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
