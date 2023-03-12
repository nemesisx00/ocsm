using Godot;
using System;

namespace OCSM.Nodes.DnD.Fifth
{
	public partial class DieOptionsButton : CustomOption
	{
		[Export]
		public bool BardicInspiration { get; set; } = false;
		[Export]
		public bool DamageDie { get; set; } = false;
		
		public override void _Ready()
		{
			AddItem(String.Empty);
			
			if(!BardicInspiration)
				AddItem(OCSM.DnD.Fifth.Die.d4.ToString());
			
			AddItem(OCSM.DnD.Fifth.Die.d6.ToString());
			AddItem(OCSM.DnD.Fifth.Die.d8.ToString());
			AddItem(OCSM.DnD.Fifth.Die.d10.ToString());
			AddItem(OCSM.DnD.Fifth.Die.d12.ToString());
			
			if(!BardicInspiration && !DamageDie)
			{
				AddItem(OCSM.DnD.Fifth.Die.d20.ToString());
				AddItem(OCSM.DnD.Fifth.Die.d100.ToString());
			}
		}
	}
}
