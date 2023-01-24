using Godot;
using System;
using OCSM.Nodes.Autoload;
using OCSM.DnD.Fifth.Meta;
using OCSM.DnD.Fifth.Inventory;

namespace OCSM.Nodes.DnD.Fifth
{
	public class EquipmentNode : VBoxContainer
	{
		private sealed class Names
		{
			public const string ArmorEquipped = "ArmorEquipped";
		}
		
		[Signal]
		public delegate void EquipmentChanged(Transport<Equipment> equipment);
		
		public Equipment Equipment { get; set; }
		
		private MetadataManager metadataManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			
			if(!(Equipment is Equipment))
				Equipment = new Equipment();
			else
				refreshSelected();
			
			var armor = GetNode<ArmorOptionsButton>(NodePathBuilder.SceneUnique(Names.ArmorEquipped));
			armor.Connect(Constants.Signal.ItemSelected, this, nameof(changed_Armor));
			armor.Connect(nameof(ArmorOptionsButton.ItemsChanged), this, nameof(refreshSelected));
		}
		
		private void changed_Armor(int index)
		{
			var optionsButton = GetNode<ArmorOptionsButton>(NodePathBuilder.SceneUnique(Names.ArmorEquipped));
			var name = optionsButton.GetItemText(index);
			if(metadataManager.Container is DnDFifthContainer dfc)
			{
				if(dfc.Armor.Find(a => a.Name.Equals(name)) is ItemArmor armor)
					Equipment.Armor = armor;
				else
					Equipment.Armor = null;
			}
			
			EmitSignal(nameof(EquipmentChanged), new Transport<Equipment>(Equipment));
		}
		
		public void refreshSelected()
		{
			refreshArmor();
		}
		
		private void refreshArmor()
		{
			var armorOptions = GetNode<ArmorOptionsButton>(NodePathBuilder.SceneUnique(Names.ArmorEquipped));
			if(Equipment.Armor is ItemArmor armor && metadataManager.Container is DnDFifthContainer dfc)
			{
				var newIndex = dfc.Armor.FindIndex(a => a.Equals(armor)) + 1;
				armorOptions.Selected = newIndex;
				changed_Armor(newIndex);
			}
			else
			{
				armorOptions.Selected = 0;
				changed_Armor(0);
			}
		}
	}
}
