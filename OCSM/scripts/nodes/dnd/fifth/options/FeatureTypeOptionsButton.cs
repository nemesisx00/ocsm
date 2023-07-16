using Godot;
using System;
using Ocsm.Dnd.Fifth;

namespace Ocsm.Nodes.Dnd.Fifth
{
	public partial class FeatureTypeOptionsButton : CustomOption
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
