using Godot;

namespace OCSM.Nodes
{
	public partial class HelpMenu : MenuButton
	{
		private sealed class ItemNames
		{
			public const string About = "About";
			public const string GameSystemLicenses = "Game System Licences";
			public const string Godot = "About Godot Engine";
		}
		
		public enum MenuItem : long { About, GameSystemLicenses, Godot }
		
		private Window licensePopup;
		private Window godotPopup;
		
		public override void _Ready()
		{
			var popup = GetPopup();
			popup.AddItem(ItemNames.About, (int)MenuItem.About);
			popup.AddItem(ItemNames.GameSystemLicenses, (int)MenuItem.GameSystemLicenses);
			popup.AddItem(ItemNames.Godot, (int)MenuItem.Godot);
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
				case MenuItem.Godot:
					showGodot();
					break;
			}
		}
		
		private void showGameSystemLicenses()
		{
			if(!(licensePopup is Window))
			{
				var resource = GD.Load<PackedScene>(Constants.Scene.GameSystemLicenses);
				licensePopup = resource.Instantiate<Window>();
				licensePopup.CloseRequested += () => NodeUtilities.queueFree(ref licensePopup);
				
				GetTree().CurrentScene.AddChild(licensePopup);
				licensePopup.PopupCentered();
			}
		}
		
		private void showGodot()
		{
			if(!(godotPopup is Window))
			{
				var resource = GD.Load<PackedScene>(Constants.Scene.AboutGodot);
				GD.Print("resource");
				godotPopup = resource.Instantiate<Window>();
				godotPopup.GetNode<TextEdit>("%LicenseText").Text = Engine.GetLicenseText();
				godotPopup.CloseRequested += () => NodeUtilities.queueFree(ref godotPopup);
				
				GetTree().CurrentScene.AddChild(godotPopup);
				godotPopup.PopupCentered();
			}
		}
	}
}
