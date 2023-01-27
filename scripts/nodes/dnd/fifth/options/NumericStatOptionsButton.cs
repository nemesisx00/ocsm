using Godot;
using OCSM.DnD.Fifth;

namespace OCSM.Nodes.DnD.Fifth
{
	public class NumericStatOptionsButton : OptionButton
	{
		public override void _Ready()
		{
			refreshMetadata();
		}
		
		private void refreshMetadata()
		{
			var index = Selected;
			
			Clear();
			NumericStatNames.asList().ForEach(label => AddItem(label));
			Selected = index;
		}
	}
}
