using Godot;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes
{
	public partial class ClickableControl : Control
	{
		[Signal]
		public delegate void ClickedEventHandler();
		
		private AppManager appManager;
		
		private bool pressed;
		
		public override void _Input(InputEvent e)
		{
			if(!appManager.IsQuitting)
			{
				if(e is InputEventMouseButton iemb && iemb.ButtonIndex.Equals((int)MouseButton.Left))
				{
					if(!pressed)
					{
						//First press, send the signal
						EmitSignal(nameof(Clicked));
					}
					pressed = iemb.Pressed;
				}
			}
		}
		
		public override void _Ready()
		{
			appManager = GetNode<AppManager>(Constants.NodePath.AppManager);
			pressed = false;
		}
	}
}
