using Godot;

namespace Ocsm.Nodes.Autoload;

public partial class AppManager : Node
{
	public static readonly NodePath NodePath = new("/root/AppManager");
	public bool IsQuitting { get; set; }
	
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
	
	//Prevent the game from simply ending so that we have a chance to free memory in QuitGame() if necessary
	public override void _Ready() => GetTree().AutoAcceptQuit = false;
	
	public void ShowQuitConfirm()
	{
		if(confirmQuit is not null && IsInstanceValid(confirmQuit))
			confirmQuit.PopupCentered();
		else if(!IsQuitting)
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.ConfirmQuit);
			confirmQuit = resource.Instantiate<ConfirmQuit>();
			confirmQuit.CloseRequested += () => NodeUtilities.QueueFree(ref confirmQuit);
			
			GetTree().CurrentScene.AddChild(confirmQuit);
			confirmQuit.PopupCentered();
			IsQuitting = true;
		}
	}
}
