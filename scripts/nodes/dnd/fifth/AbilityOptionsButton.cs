using Godot;
using System;
using OCSM.DnD.Fifth;

namespace OCSM.Nodes.DnD.Fifth
{
	public class AbilityOptionsButton : OptionButton
	{
		public override void _Ready()
		{
			refreshMetadata();
		}
		
		private void refreshMetadata()
		{
			var index = Selected;
			
			Clear();
			AddItem(String.Empty);
			foreach(var label in Ability.Names.asList())
			{
				AddItem(label);
			}
			
			Selected = index;
		}
	}
}
