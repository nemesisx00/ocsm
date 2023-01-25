using Godot;
using OCSM.DnD.Fifth.Inventory;

namespace OCSM.Nodes.DnD.Fifth
{
	public class InventoryItem : HBoxContainer
	{
		public sealed class Names
		{
			public const string Name = "Name";
			public const string Equipped = "Equipped";
		}
		
		[Signal]
		public delegate void Equipped(Transport<Item> transport);
		
		public Item Item { get; set; }
		
		public override void _Ready()
		{
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(Names.Equipped)).Connect(Constants.Signal.Pressed, this, nameof(toggleEquipped));
			
			refresh();
		}
		
		public void refresh()
		{
			GetNode<Label>(NodePathBuilder.SceneUnique(Names.Name)).Text = Item.Name;
			if(Item is ItemEquippable ie)
			{
				GetNode<CheckBox>(NodePathBuilder.SceneUnique(Names.Equipped)).Pressed = ie.Equipped;
			}
		}
		
		private void toggleEquipped()
		{
			var checkbox = GetNode<CheckBox>(NodePathBuilder.SceneUnique(Names.Equipped));
			if(Item is ItemEquippable ie)
			{
				ie.Equipped = checkbox.Pressed;
				EmitSignal(nameof(Equipped), new Transport<Item>(ie));
			}
		}
	}
}
