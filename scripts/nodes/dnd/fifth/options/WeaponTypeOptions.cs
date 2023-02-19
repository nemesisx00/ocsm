using System;
using EnumsNET;
using OCSM.DnD.Fifth.Inventory;

namespace OCSM.Nodes.DnD.Fifth
{
	public partial class WeaponTypeOptions : CustomOption
	{
		public override void _Ready()
		{
			refreshMetadata();
		}
		
		protected override void refreshMetadata()
		{
			var index = Selected;
			
			Clear();
			AddItem(String.Empty);
			
			foreach(var member in Enums.GetMembers<ItemWeapon.WeaponType>())
			{
				if(member.Attributes.Get<ItemWeapon.LabelAttribute>() is ItemWeapon.LabelAttribute attr)
					AddItem(attr.Label);
			}
			
			Selected = index;
		}
	}
}
