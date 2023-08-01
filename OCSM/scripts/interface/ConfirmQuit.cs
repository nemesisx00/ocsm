using Godot;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes;

public partial class ConfirmQuit : ConfirmationDialog
{
	public override void _Ready()
	{
		Confirmed += quitGame;
		Canceled += hideConfirmQuit;
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
		GetNode<AppManager>(AppManager.NodePath).IsQuitting = false;
		QueueFree();
	}
}
