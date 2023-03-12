using System;
using Godot;
using OCSM.DnD.Fifth.Inventory;

namespace OCSM.Nodes.DnD.Fifth
{
	public partial class ArmorTypeOptions : OptionButton
	{
		public override void _Ready()
		{
			Clear();
			AddItem(String.Empty);
			AddItem(ItemArmor.ArmorType.Light.GetLabel());
			AddItem(ItemArmor.ArmorType.Medium.GetLabel());
			AddItem(ItemArmor.ArmorType.Heavy.GetLabel());
			AddItem(ItemArmor.ArmorType.Shield.GetLabel());
		}
	}
}
