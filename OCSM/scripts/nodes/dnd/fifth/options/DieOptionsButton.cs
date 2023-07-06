using Godot;
using System;

namespace Ocsm.Nodes.Dnd.Fifth
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
				AddItem(Ocsm.Dnd.Fifth.Die.d4.ToString());
			
			AddItem(Ocsm.Dnd.Fifth.Die.d6.ToString());
			AddItem(Ocsm.Dnd.Fifth.Die.d8.ToString());
			AddItem(Ocsm.Dnd.Fifth.Die.d10.ToString());
			AddItem(Ocsm.Dnd.Fifth.Die.d12.ToString());
			
			if(!BardicInspiration && !DamageDie)
			{
				AddItem(Ocsm.Dnd.Fifth.Die.d20.ToString());
				AddItem(Ocsm.Dnd.Fifth.Die.d100.ToString());
			}
		}
	}
}
