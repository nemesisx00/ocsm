using Godot;

namespace OCSM.Nodes
{
	public partial class HelpMenu : MenuButton
	{
		private sealed class ItemNames
		{
			public const string About = "About";
			public const string GameSystemLicenses = "Game System Licences";
		}
		
		public enum MenuItem { About, GameSystemLicenses }
		
		private Window licensePopup;
		
		public override void _Ready()
		{
			var popup = GetPopup();
			popup.AddItem(ItemNames.About, (int)MenuItem.About);
			popup.AddItem(ItemNames.GameSystemLicenses, (int)MenuItem.GameSystemLicenses);
			popup.IdPressed += handleMenuItem;
			
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
			if(!(licensePopup is Window))
			{
				var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.GameSystemLicenses);
				licensePopup = resource.Instantiate<Window>();
				
				GetTree().CurrentScene.AddChild(licensePopup);
				licensePopup.CloseRequested += hideGameSystemLicenses;
				licensePopup.PopupCentered();
			}
		}
		
		private void hideGameSystemLicenses()
		{
			if(licensePopup is Window)
			{
				licensePopup.QueueFree();
				licensePopup = null;
			}
		}
	}
}
