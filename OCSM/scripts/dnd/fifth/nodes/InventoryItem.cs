using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class InventoryItem : HBoxContainer
{
	public static class NodePaths
	{
		public static readonly NodePath Details = new("%Details");
		public static readonly NodePath Equipped = new("%Equipped");
		public static readonly NodePath Name = new("%Name");
		public static readonly NodePath Weight = new("%Weight");
	}
	
	[Signal]
	public delegate void EquippedEventHandler(Transport<Item> transport);
	
	public Item Item { get; set; }
	public AbilityInfo Strength { get; set; }
	public AbilityInfo Dexterity { get; set; }
	
	public override void _Ready()
	{
		GetNode<CheckBox>(NodePaths.Equipped).Pressed += toggleEquipped;
		
		Refresh();
	}
	
	public void Refresh()
	{
		GetNode<Label>(NodePaths.Name).Text = Item.Name;
		GetNode<Label>(NodePaths.Weight).Text = Item.Weight + " lbs";
		
		var details = GetNode<HBoxContainer>(NodePaths.Details);
		foreach(Node node in details.GetChildren())
		{
			node.QueueFree();
		}
		
		if(Item.ArmorData is not null)
			generateDetails(Item.ArmorData).ForEach(n => details.AddChild(n));
		else if(Item.WeaponData is not null)
			generateDetails(Item.WeaponData).ForEach(n => details.AddChild(n));
		
		var equipped = GetNode<CheckBox>(NodePaths.Equipped);
		if(Item.Equipped is not null)
		{
			equipped.ButtonPressed = (bool)Item.Equipped;
			equipped.Visible = true;
		}
		else
			equipped.Visible = false;
	}
	
	private void toggleEquipped()
	{
		var checkbox = GetNode<CheckBox>(NodePaths.Equipped);
		if(Item.Equipped is not null)
		{
			Item.Equipped = checkbox.ButtonPressed;
			EmitSignal(SignalName.Equipped, new Transport<Item>(Item));
		}
	}
	
	private List<Node> generateDetails(ArmorData armor)
	{
		var nodes = new List<Node>();
		
		StringBuilder ac = new($"AC {armor.BaseArmorClass}");
		
		if((armor.AllowDexterityBonus && Dexterity.Modifier > 0) || Dexterity.Modifier < 0)
		{
			ac.Append(' ');
			
			if(Dexterity.Modifier > 0)
				ac.Append('+');
			
			ac.Append(Dexterity.Modifier);
			
			if(armor.LimitDexterityBonus && armor.DexterityBonusLimit > 0)
				ac.Append($" (Max {armor.DexterityBonusLimit})");
		}
		
		nodes.Add(NodeUtilities.CreateCenteredLabel(ac.ToString()));
		
		//TODO: Should this always display or hide when the character's strength meets/exceeds the requirements?
		if(armor.MinimumStrength > 0 && Strength.TotalScore < armor.MinimumStrength)
			nodes.Add(NodeUtilities.CreateCenteredLabel($"{armor.MinimumStrength} Str Required"));
		
		if(armor.StealthDisadvantage)
			nodes.Add(NodeUtilities.CreateCenteredLabel("Disadvantage on Stealth Checks"));
		
		return nodes;
	}
	
	private static List<Node> generateDetails(WeaponData weapon)
	{
		var damage = new StringBuilder();
		weapon.DamageDice.ToList()
			.ForEach(d => {
				if(damage.Length > 0)
					damage.Append(" + ");
				damage.Append(d.Key.ToString(d.Value));
				damage.Append(' ');
				damage.Append(d.Key.DamageType.GetLabel());
			});
		
		return [NodeUtilities.CreateCenteredLabel(damage.ToString())];
	}
}
