using Godot;
using System.Collections.Generic;
using OCSM.DnD.Fifth.Inventory;

namespace OCSM.Nodes.DnD.Fifth
{
	public class Inventory : VBoxContainer
	{
		public sealed class Names
		{
			public const string AddItem = "AddItem";
			public const string ItemList = "ItemList";
		}
		
		[Signal]
		public delegate void ItemsChanged(Transport<List<Item>> items);
		
		public List<Item> Items { get; set; }
		
		public override void _Ready()
		{
			if(!(Items is List<Item>))
				Items = new List<Item>();
			
			GetNode<Button>(NodePathBuilder.SceneUnique(Names.AddItem)).Connect(Constants.Signal.Pressed, this, nameof(addItemHandler));
			
			regenerateItems();
		}
		
		public void regenerateItems()
		{
			var itemList = GetNode<VBoxContainer>(NodePathBuilder.SceneUnique(Names.ItemList));
			foreach(Node c in itemList.GetChildren())
			{
				c.QueueFree();
			}
			
			var itemScene = GD.Load<PackedScene>(Constants.Scene.DnD.Fifth.InventoryItem);
			foreach(Item i in Items)
			{
				var instance = itemScene.Instance<InventoryItem>();
				instance.Item = i;
				instance.Connect(nameof(InventoryItem.Equipped), this, nameof(itemEquipped));
				
				itemList.AddChild(instance);
				instance.refresh();
				instance.Visible = true;
			}
		}
		
		private void addItemHandler()
		{
			/*
			var item = get item from option input
			Items.Add(item);
			EmitSignal(nameof(ItemsChanged), new Transport<List<Item>>(Items));
			
			regenerateItems();
			*/
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
