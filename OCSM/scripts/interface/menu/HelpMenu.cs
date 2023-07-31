using Godot;

namespace Ocsm.Nodes;

public partial class HelpMenu : MenuButton
{
	private sealed class ItemNames
	{
		public const string About = "About Ocsm";
		public const string GameSystemLicenses = "Game System Licences";
		public const string Godot = "About Godot Engine";
	}
	
	public enum MenuItem : long { About, GameSystemLicenses, Godot }
	
	private Window aboutGodot;
	private Window aboutOcsm;
	private Window gameLicenses;
	
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
				showAbout();
				break;
			case MenuItem.GameSystemLicenses:
				showGameSystemLicenses();
				break;
			case MenuItem.Godot:
				showGodot();
				break;
		}
	}
	
	private void showAbout()
	{
		if(aboutOcsm is Window && Node.IsInstanceValid(aboutOcsm))
			aboutOcsm.PopupCentered();
		else
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.AboutOcsm);
			aboutOcsm = resource.Instantiate<Window>();
			aboutOcsm.CloseRequested += () => NodeUtilities.queueFree(ref aboutOcsm);
			
			GetTree().CurrentScene.AddChild(aboutOcsm);
			aboutOcsm.PopupCentered();
		}
	}
	
	private void showGameSystemLicenses()
	{
		if(gameLicenses is Window && Node.IsInstanceValid(gameLicenses))
			gameLicenses.PopupCentered();
		else
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.GameSystemLicenses);
			gameLicenses = resource.Instantiate<Window>();
			gameLicenses.CloseRequested += () => NodeUtilities.queueFree(ref gameLicenses);
			
			GetTree().CurrentScene.AddChild(gameLicenses);
			gameLicenses.PopupCentered();
		}
	}
	
	private void showGodot()
	{
		if(aboutGodot is Window && Node.IsInstanceValid(aboutGodot))
			aboutGodot.PopupCentered();
		else
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.AboutGodot);
			aboutGodot = resource.Instantiate<Window>();
			aboutGodot.GetNode<TextEdit>("%LicenseText").Text = Engine.GetLicenseText();
			aboutGodot.CloseRequested += () => NodeUtilities.queueFree(ref aboutGodot);
			
			GetTree().CurrentScene.AddChild(aboutGodot);
			aboutGodot.PopupCentered();
		}
	}
}
