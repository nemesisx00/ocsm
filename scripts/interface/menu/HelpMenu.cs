using Godot;

namespace OCSM.Nodes
{
	public class HelpMenu : MenuButton
	{
		private const string PopupName = "GameSystemLicenses";
		
		public enum MenuItem { About, GameSystemLicenses }
		
		public override void _Ready()
		{
			GetPopup().Connect(Constants.Signal.IdPressed, this, nameof(handleMenuItem));
			GetNode<AppRoot>(Constants.NodePath.AppRoot).Connect(nameof(AppRoot.HelpMenuTriggered), this, nameof(handleMenuItem));
		}
		
		private void handleMenuItem(int id)
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
			var instance = resource.Instance<WindowDialog>();
			instance.Name = PopupName;
			GetTree().CurrentScene.AddChild(instance);
			NodeUtilities.centerControl(instance, GetViewportRect().GetCenter());
			instance.GetCloseButton().Connect(Constants.Signal.Pressed, this, nameof(hideGameSystemLicenses));
			instance.Popup_();
		}
		
		private void hideGameSystemLicenses()
		{
			GetTree().CurrentScene.GetNode<WindowDialog>(PopupName).QueueFree();
		}
	}
}
