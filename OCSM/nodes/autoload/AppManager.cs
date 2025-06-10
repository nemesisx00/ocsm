using Godot;

namespace Ocsm.Nodes.Autoload;

public partial class AppManager : Node
{
	public static readonly NodePath NodePath = new("/root/AppManager");
	
	public bool IsQuitting { get; set; }
	
	private ConfirmationDialog confirmQuit;
	private PackedScene confirmQuitScene;
	
	public override void _Notification(int notificationCode)
	{
		switch((long)notificationCode)
		{
			case NotificationWMCloseRequest:
				ShowQuitConfirm();
				break;
		}
	}
	
	public override void _Ready()
	{
		//Prevent the game from simply ending so that we have a chance to free memory in QuitGame() if necessary
		GetTree().AutoAcceptQuit = false;
		
		confirmQuitScene = GD.Load<PackedScene>(ScenePaths.ConfirmQuit);
	}
	
	public void ShowQuitConfirm()
	{
		if(confirmQuit is not null && IsInstanceValid(confirmQuit))
			confirmQuit.PopupCentered();
		else if(!IsQuitting)
		{
			confirmQuit = confirmQuitScene.Instantiate<ConfirmQuit>();
			confirmQuit.CloseRequested += () => NodeUtilities.QueueFree(ref confirmQuit);
			
			GetTree().CurrentScene.AddChild(confirmQuit);
			confirmQuit.PopupCentered();
			IsQuitting = true;
		}
	}
}
