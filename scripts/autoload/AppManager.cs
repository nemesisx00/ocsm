using Godot;

namespace OCSM.Nodes.Autoload
{
	public partial class AppManager : Node
	{
		public bool IsQuitting { get; set; } = false;
		
		public override void _Notification(int notificationCode)
		{
			switch((long)notificationCode)
			{
				case NotificationWMCloseRequest:
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
				var resource = GD.Load<PackedScene>(Constants.Scene.ConfirmQuit);
				var confirmQuit = resource.Instantiate<ConfirmQuit>();
				GetTree().CurrentScene.AddChild(confirmQuit);
				confirmQuit.PopupCentered();
				IsQuitting = true;
			}
		}
	}
}
