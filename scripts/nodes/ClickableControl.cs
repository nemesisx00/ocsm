using Godot;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes
{
	public class ClickableControl : Control
	{
		[Signal]
		public delegate void Clicked();
		
		private AppManager appManager;
		
		private bool pressed;
		
		public override void _Input(InputEvent e)
		{
			if(!appManager.IsQuitting)
			{
				if(e is InputEventMouseButton iemb && iemb.ButtonIndex.Equals((int)ButtonList.Left))
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
