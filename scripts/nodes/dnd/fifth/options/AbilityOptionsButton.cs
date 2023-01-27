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
			Ability.Names.asList().ForEach(label => AddItem(label));
			
			Selected = index;
		}
	}
}
