using Godot;
using System;
using OCSM.DnD.Fifth;

namespace OCSM.Nodes.DnD.Fifth
{
	public class FeatureTypeOptionsButton : OptionButton
	{
		[Export]
		public bool emptyOption = true;
		
		public override void _Ready()
		{
			if(emptyOption)
				AddItem(String.Empty);
			
			FeatureType.asList().ForEach(type => AddItem(type));
		}
	}
}
