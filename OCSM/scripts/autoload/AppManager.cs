using Godot;

namespace Ocsm.Nodes.Autoload;

public partial class AppManager : Node
{
	public const string NodePath = "/root/AppManager";
	
	public bool IsQuitting { get; set; } = false;
	
	private ConfirmationDialog confirmQuit;
	
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
	}
	
	public void ShowQuitConfirm()
	{
		if(confirmQuit is not null && IsInstanceValid(confirmQuit))
			confirmQuit.PopupCentered();
		else if(!IsQuitting)
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.ConfirmQuit);
			confirmQuit = resource.Instantiate<ConfirmQuit>();
			confirmQuit.CloseRequested += () => NodeUtilities.queueFree(ref confirmQuit);
			
			GetTree().CurrentScene.AddChild(confirmQuit);
			confirmQuit.PopupCentered();
			IsQuitting = true;
		}
	}
}
