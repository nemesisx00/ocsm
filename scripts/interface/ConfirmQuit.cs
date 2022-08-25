using Godot;
using OCSM;

public class ConfirmQuit : CenterContainer
{
	private const string confirmQuitPath = "ConfirmQuit";
	
	public override void _Ready()
	{
		var confirmQuit = GetNode<ConfirmationDialog>(confirmQuitPath);
		confirmQuit.Connect(Constants.Signal.Confirmed, this, nameof(quitGame));
		confirmQuit.GetCancel().Connect(Constants.Signal.Pressed, this, nameof(hideConfirmQuit));
		confirmQuit.GetCloseButton().Connect(Constants.Signal.Pressed, this, nameof(hideConfirmQuit));
		confirmQuit.RectPosition = new Vector2(GetViewportRect().GetCenter().x - (confirmQuit.RectSize.x / 2), GetViewportRect().GetCenter().y - (confirmQuit.RectSize.y / 2));
		confirmQuit.Show();
	}
	
	private void quitGame()
	{
		//The current node tree is freed automatically on quit.
		if(OS.IsDebugBuild())
		{
			GD.Print("DEBUG Stray Nodes ----- START");
			PrintStrayNodes();
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
