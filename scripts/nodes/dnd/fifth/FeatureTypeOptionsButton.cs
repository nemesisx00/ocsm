using Godot;
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
				AddItem("");
			
			foreach(var type in FeatureType.asList())
			{
				AddItem(type);
			}
		}
	}
}
