using OCSM.DnD.Fifth;

namespace OCSM.Nodes.DnD.Fifth
{
	public partial class NumericStatOptionsButton : CustomOption
	{
		public override void _Ready()
		{
			refreshMetadata();
		}
		
		protected override void refreshMetadata()
		{
			var index = Selected;
			
			Clear();
			NumericStatNames.asList().ForEach(label => AddItem(label));
			
			Selected = index;
		}
	}
}
