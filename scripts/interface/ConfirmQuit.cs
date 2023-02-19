using Godot;
using OCSM.Nodes.Autoload;

namespace OCSM.Nodes
{
	public partial class ConfirmQuit : CenterContainer
	{
		private const string confirmQuitPath = "ConfirmQuit";
		
		public override void _Ready()
		{
			var confirmQuit = GetNode<ConfirmationDialog>(confirmQuitPath);
			confirmQuit.Confirmed += quitGame;
			confirmQuit.Canceled += hideConfirmQuit;
			confirmQuit.Position = new Vector2I((int)(GetViewportRect().GetCenter().X - (confirmQuit.Size.X / 2)), (int)(GetViewportRect().GetCenter().Y - (confirmQuit.Size.Y / 2)));
			confirmQuit.Show();
		}
		
		private void quitGame()
		{
			//The current node tree is freed automatically on quit.
			if(OS.IsDebugBuild())
			{
				GD.Print("DEBUG Stray Nodes ----- START");
				PrintOrphanNodes();
				GD.Print("DEBUG Stray Nodes ----- END");
			}
			GetTree().Quit();
		}
		
		private void hideConfirmQuit()
		{
			GetNode<AppManager>(Constants.NodePath.AppManager).IsQuitting = false;
			Hide();
			QueueFree();
		}
	}
}
