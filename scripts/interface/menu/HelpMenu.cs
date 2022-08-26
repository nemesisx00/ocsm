using Godot;
using OCSM;

public class HelpMenu : MenuButton
{
	private const string PopupName = "DarkPack";
	
	public enum MenuItem { About, Attributions, TheDarkPack }
	
	private SheetManager sheetManager;
	
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
			case MenuItem.Attributions:
				GD.Print("Show Attributions Popup");
				break;
			case MenuItem.TheDarkPack:
				showDarkPack();
				break;
		}
	}
	
	private void showDarkPack()
	{
		var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.DarkPack);
		var instance = resource.Instance<WindowDialog>();
		instance.Name = PopupName;
		GetTree().CurrentScene.AddChild(instance);
		NodeUtilities.centerControl(instance, GetViewportRect().GetCenter());
		instance.GetCloseButton().Connect(Constants.Signal.Pressed, this, nameof(hideDarkPack));
		instance.Show();
	}
	
	private void hideDarkPack()
	{
		GetTree().CurrentScene.GetNode<WindowDialog>(PopupName).QueueFree();
	}
}
