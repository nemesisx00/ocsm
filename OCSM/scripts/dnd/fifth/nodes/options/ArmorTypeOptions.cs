using Godot;
using Ocsm.Dnd.Fifth.Inventory;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class ArmorTypeOptions : OptionButton
{
	public override void _Ready()
	{
		Clear();
		AddItem(string.Empty);
		AddItem(ItemArmor.ArmorType.Light.GetLabel());
		AddItem(ItemArmor.ArmorType.Medium.GetLabel());
		AddItem(ItemArmor.ArmorType.Heavy.GetLabel());
		AddItem(ItemArmor.ArmorType.Shield.GetLabel());
	}
}
