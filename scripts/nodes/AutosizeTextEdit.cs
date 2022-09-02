using Godot;

namespace OCSM.Nodes
{
	public class AutosizeTextEdit : TextEdit
	{
		public override void _Ready()
		{
			Connect(Constants.Signal.TextChanged, this, nameof(autosize));
		}
		
		public void autosize()
		{
			NodeUtilities.autoSize(this, Constants.TextInputMinHeight);
		}
	}
}