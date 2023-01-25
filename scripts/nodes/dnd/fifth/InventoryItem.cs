using Godot;
using System.Collections.Generic;
using EnumsNET;
using OCSM.DnD.Fifth;
using OCSM.DnD.Fifth.Inventory;

namespace OCSM.Nodes.DnD.Fifth
{
	public class InventoryItem : HBoxContainer
	{
		public sealed class Names
		{
			public const string Details = "Details";
			public const string Equipped = "Equipped";
			public const string Name = "Name";
			public const string Weight = "Weight";
		}
		
		[Signal]
		public delegate void Equipped(Transport<Item> transport);
		
		public Item Item { get; set; }
		public Ability Strength { get; set; }
		public Ability Dexterity { get; set; }
		
		public override void _Ready()
		{
			GetNode<CheckBox>(NodePathBuilder.SceneUnique(Names.Equipped)).Connect(Constants.Signal.Pressed, this, nameof(toggleEquipped));
			
			refresh();
		}
		
		public void refresh()
		{
			GetNode<Label>(NodePathBuilder.SceneUnique(Names.Name)).Text = Item.Name;
			GetNode<Label>(NodePathBuilder.SceneUnique(Names.Weight)).Text = Item.Weight + " lbs";
			
			var details = GetNode<HBoxContainer>(NodePathBuilder.SceneUnique(Names.Details));
			foreach(Node node in details.GetChildren())
			{
				node.QueueFree();
			}
			
			if(Item is ItemArmor ia)
				generateDetails(ia).ForEach(n => details.AddChild(n));
			else if(Item is ItemWeapon iw)
				generateDetails(iw).ForEach(n => details.AddChild(n));
			
			var equipped = GetNode<CheckBox>(NodePathBuilder.SceneUnique(Names.Equipped));
			if(Item is ItemEquippable ie)
			{
				equipped.Pressed = ie.Equipped;
				equipped.Visible = true;
			}
			else
				equipped.Visible = false;
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
		
		private List<Node> generateDetails(ItemArmor armor)
		{
			var nodes = new List<Node>();
			
			var ac = "AC " + armor.BaseArmorClass;
			if((armor.AllowDexterityBonus && Dexterity.Modifier > 0) || Dexterity.Modifier < 0)
			{
				ac += " ";
				if(Dexterity.Modifier > 0)
					ac += "+";
				ac += Dexterity.Modifier;
				if(armor.LimitDexterityBonus && armor.DexterityBonusLimit > 0)
					ac += " (Max " + armor.DexterityBonusLimit + ")";
			}
			
			nodes.Add(NodeUtilities.createCenteredLabel(ac));
			
			//TODO: Should this always display or hide when the character's strength meets/exceeds the requirements?
			if(armor.MinimumStrength > 0 && Strength.Score < armor.MinimumStrength)
				nodes.Add(NodeUtilities.createCenteredLabel(armor.MinimumStrength + " Str Required"));
			
			if(armor.StealthDisadvantage)
				nodes.Add(NodeUtilities.createCenteredLabel("Disadvantage on Stealth Checks"));
			
			return nodes;
		}
		
		private List<Node> generateDetails(ItemWeapon weapon)
		{
			var nodes = new List<Node>();
			
			var damage = "";
			foreach(var entry in weapon.DamageDice)
			{
				if(damage.Length > 0)
					damage += " + ";
				damage += entry.Key.ToString(entry.Value) + " " + Enums.GetName(entry.Key.Type);
			}
			nodes.Add(NodeUtilities.createCenteredLabel(damage));
			
			return nodes;
		}
	}
}
