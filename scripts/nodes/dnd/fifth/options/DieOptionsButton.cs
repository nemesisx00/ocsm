using Godot;
using System;

namespace OCSM.Nodes.DnD.Fifth
{
	public class DieOptionsButton : OptionButton
	{
		private const string DFormat = "d{0}";
		
		[Export]
		public bool BardicInspiration { get; set; } = false;
		
		public override void _Ready()
		{
			AddItem(String.Empty);
			
			if(!BardicInspiration)
				AddItem(String.Format(DFormat, OCSM.DnD.Fifth.Die.d4.Sides));
			
			AddItem(String.Format(DFormat, OCSM.DnD.Fifth.Die.d6.Sides));
			AddItem(String.Format(DFormat, OCSM.DnD.Fifth.Die.d8.Sides));
			AddItem(String.Format(DFormat, OCSM.DnD.Fifth.Die.d10.Sides));
			AddItem(String.Format(DFormat, OCSM.DnD.Fifth.Die.d12.Sides));
			
			if(!BardicInspiration)
			{
				AddItem(String.Format(DFormat, OCSM.DnD.Fifth.Die.d20.Sides));
				AddItem(String.Format(DFormat, OCSM.DnD.Fifth.Die.d100.Sides));
			}
		}
	}
}
