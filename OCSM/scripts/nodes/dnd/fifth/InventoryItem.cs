using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OCSM.DnD.Fifth;
using OCSM.DnD.Fifth.Inventory;

namespace OCSM.Nodes.DnD.Fifth
{
	public partial class InventoryItem : HBoxContainer
	{
		public sealed class NodePath
		{
			public const string Details = "%Details";
			public const string Equipped = "%Equipped";
			public const string Name = "%Name";
			public const string Weight = "%Weight";
		}
		
		[Signal]
		public delegate void EquippedEventHandler(Transport<Item> transport);
		
		public Item Item { get; set; }
		public Ability Strength { get; set; }
		public Ability Dexterity { get; set; }
		
		public override void _Ready()
		{
			GetNode<CheckBox>(NodePath.Equipped).Pressed += toggleEquipped;
			
			refresh();
		}
		
		public void refresh()
		{
			GetNode<Label>(NodePath.Name).Text = Item.Name;
			GetNode<Label>(NodePath.Weight).Text = Item.Weight + " lbs";
			
			var details = GetNode<HBoxContainer>(NodePath.Details);
			foreach(Node node in details.GetChildren())
			{
				node.QueueFree();
			}
			
			if(Item is ItemArmor ia)
				generateDetails(ia).ForEach(n => details.AddChild(n));
			else if(Item is ItemWeapon iw)
				generateDetails(iw).ForEach(n => details.AddChild(n));
			
			var equipped = GetNode<CheckBox>(NodePath.Equipped);
			if(Item is ItemEquippable ie)
			{
				equipped.ButtonPressed = ie.Equipped;
				equipped.Visible = true;
			}
			else
				equipped.Visible = false;
		}
		
		private void toggleEquipped()
		{
			var checkbox = GetNode<CheckBox>(NodePath.Equipped);
			if(Item is ItemEquippable ie)
			{
				ie.Equipped = checkbox.ButtonPressed;
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
			var damage = new StringBuilder();
			weapon.DamageDice.ToList()
				.ForEach(d => {
					if(damage.Length > 0)
						damage.Append(" + ");
					damage.Append(d.Key.ToString(d.Value));
					damage.Append(" ");
					damage.Append(d.Key.Type.GetLabel());
				});
			
			return new List<Node>()
			{
				NodeUtilities.createCenteredLabel(damage.ToString())
			};
		}
	}
}
