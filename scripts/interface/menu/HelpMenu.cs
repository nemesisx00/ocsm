using Godot;

namespace OCSM.Nodes
{
	public partial class HelpMenu : MenuButton
	{
		private const string PopupName = "GameSystemLicenses";
		
		public enum MenuItem { About, GameSystemLicenses }
		
		public override void _Ready()
		{
			GetPopup().IdPressed += handleMenuItem;
			GetNode<AppRoot>(Constants.NodePath.AppRoot).HelpMenuTriggered += handleMenuItem;
		}
		
		private void handleMenuItem(long id)
		{
			switch((MenuItem)id)
			{
				case MenuItem.About:
					GD.Print("Show About Popup");
					break;
				case MenuItem.GameSystemLicenses:
					showGameSystemLicenses();
					break;
			}
		}
		
		private void showGameSystemLicenses()
		{
			var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.GameSystemLicenses);
			var instance = resource.Instantiate<Window>();
			instance.Name = PopupName;
			GetTree().CurrentScene.AddChild(instance);
			instance.CloseRequested += hideGameSystemLicenses;
			instance.PopupCentered();
		}
		
		private void hideGameSystemLicenses()
		{
			GetTree().CurrentScene.GetNode<Window>(PopupName).QueueFree();
		}
	}
}
