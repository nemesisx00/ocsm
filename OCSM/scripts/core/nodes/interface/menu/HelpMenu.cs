using Godot;

namespace Ocsm.Nodes;

public partial class HelpMenu : MenuButton
{
	private static class ItemNames
	{
		public static readonly StringName About = new("About Ocsm");
		public static readonly StringName GameSystemLicenses = new("Game System Licences");
		public static readonly StringName Godot = new("About Godot Engine");
	}
	
	public enum MenuItem
	{
		About,
		GameSystemLicenses,
		Godot,
	}
	
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
		
		GetNode<AppRoot>(AppRoot.NodePaths.Self).HelpMenuTriggered += handleMenuItem;
	}
	
	private void handleMenuItem(long id) => handleMenuItem((int)id);
	private void handleMenuItem(int id)
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
		if(aboutOcsm is not null && IsInstanceValid(aboutOcsm))
			aboutOcsm.PopupCentered();
		else
		{
			var resource = GD.Load<PackedScene>(ScenePaths.AboutOcsm);
			aboutOcsm = resource.Instantiate<Window>();
			aboutOcsm.CloseRequested += () => NodeUtilities.QueueFree(ref aboutOcsm);
			
			GetTree().CurrentScene.AddChild(aboutOcsm);
			aboutOcsm.PopupCentered();
		}
	}
	
	private void showGameSystemLicenses()
	{
		if(gameLicenses is not null && IsInstanceValid(gameLicenses))
			gameLicenses.PopupCentered();
		else
		{
			var resource = GD.Load<PackedScene>(ScenePaths.GameSystemLicenses);
			gameLicenses = resource.Instantiate<Window>();
			gameLicenses.CloseRequested += () => NodeUtilities.QueueFree(ref gameLicenses);
			
			GetTree().CurrentScene.AddChild(gameLicenses);
			gameLicenses.PopupCentered();
		}
	}
	
	private void showGodot()
	{
		if(aboutGodot is not null && IsInstanceValid(aboutGodot))
			aboutGodot.PopupCentered();
		else
		{
			var resource = GD.Load<PackedScene>(ScenePaths.AboutGodot);
			aboutGodot = resource.Instantiate<Window>();
			aboutGodot.GetNode<TextEdit>("%LicenseText").Text = Engine.GetLicenseText();
			aboutGodot.CloseRequested += () => NodeUtilities.QueueFree(ref aboutGodot);
			
			GetTree().CurrentScene.AddChild(aboutGodot);
			aboutGodot.PopupCentered();
		}
	}
}
