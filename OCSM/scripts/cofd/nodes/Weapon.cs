using Godot;

namespace Ocsm.Cofd.Nodes;

public partial class Weapon : VBoxContainer
{
	private static class NodePaths
	{
		public static readonly NodePath Name = new("%Name");
		public static readonly NodePath Type = new("%Type");
		public static readonly NodePath Availability = new("%Availability");
		public static readonly NodePath Damage = new("%Damage");
		public static readonly NodePath Initiative = new("%Initiative");
		public static readonly NodePath Strength = new("%Strength");
		public static readonly NodePath Size = new("%Size");
		public static readonly NodePath Ranged = new("%Ranged");
		public static readonly NodePath Capacity = new("%Capacity");
		public static readonly NodePath ShortRange = new("%ShortRange");
		public static readonly NodePath MediumRange = new("%MediumRange");
		public static readonly NodePath LongRange = new("%LongRange");
		public static readonly NodePath Special = new("%Special");
	}
	
	private HBoxContainer ranged;
	private OptionButton type;
	
	public override void _Ready()
	{
		ranged = GetNode<HBoxContainer>(NodePaths.Ranged);
		type = GetNode<OptionButton>(NodePaths.Type);
	}
	
	private void typeChanged(long index)
	{
		var text = type.GetItemText((int)index);
		var weaponType = Enums.FromName<WeaponType>(text);
	}
}
