using Godot;
using OCSM.CoD;

namespace OCSM.Nodes.CoD
{
	public partial class Weapon : VBoxContainer
	{
		private sealed class NodePath
		{
			public const string Name = "%Name";
			public const string Type = "%Type";
			public const string Availability = "%Availability";
			public const string Damage = "%Damage";
			public const string Initiative = "%Initiative";
			public const string Strength = "%Strength";
			public const string Size = "%Size";
			public const string Ranged = "%Ranged";
			public const string Capacity = "%Capacity";
			public const string ShortRange = "%ShortRange";
			public const string MediumRange = "%MediumRange";
			public const string LongRange = "%LongRange";
			public const string Special = "%Special";
		}
		
		private HBoxContainer ranged;
		private OptionButton type;
		
		public override void _Ready()
		{
			ranged = GetNode<HBoxContainer>(NodePath.Ranged);
			type = GetNode<OptionButton>(NodePath.Type);
		}
		
		private void typeChanged(long index)
		{
			var text = type.GetItemText((int)index);
			var weaponType = Enums.FromName<WeaponType>(text);
		}
	}
}
