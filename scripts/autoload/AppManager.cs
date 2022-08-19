using Godot;
using OCSM;

public class AppManager : Node
{
	public bool IsQuitting { get; set; } = false;
	
	public override void _Notification(int notificationCode)
	{
		switch(notificationCode)
		{
			case MainLoop.NotificationWmQuitRequest:
				showQuitConfirm();
				break;
		}
	}
	
	public override void _Ready()
	{
		//Prevent the game from simply ending so that we have a chance to free memory in QuitGame()
		GetTree().AutoAcceptQuit = false;
	}
	
	private void showQuitConfirm()
	{
		if(!IsQuitting)
		{
			var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.ConfirmQuit);
			var confirmQuit = resource.Instance<ConfirmQuit>();
			GetTree().CurrentScene.AddChild(confirmQuit);
			confirmQuit.Show();
			IsQuitting = true;
		}
	}
}
