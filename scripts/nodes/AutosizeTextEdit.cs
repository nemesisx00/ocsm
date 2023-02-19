using Godot;

namespace OCSM.Nodes
{
	public partial class AutosizeTextEdit : TextEdit
	{
		public override void _Ready()
		{
			TextChanged += autosize;
		}
		
		public void autosize()
		{
			NodeUtilities.autoSize(this, Constants.TextInputMinHeight);
		}
	}
}