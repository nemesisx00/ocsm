using Godot;
using System;

namespace Ocsm.Nodes;

public partial class OpenSheet : FileDialog
{
	[Signal]
	public delegate void JsonLoadedEventHandler(string json);
	
	public static OpenSheet GenerateInstance()
	{
		return new()
		{
			Access = AccessEnum.Filesystem,
			CurrentDir = FileSystemUtilities.DefaultSheetDirectory,
			FileMode = FileModeEnum.OpenFile,
			Filters = ["*.ocsd", "OCSM Character Sheet Data"],
			InitialPosition = WindowInitialPosition.CenterPrimaryScreen,
			OkButtonText = "Open",
			ShowHiddenFiles = true,
			Size = new(720, 480),
			Theme = GD.Load<Theme>("res://resources/Default.tres"),
			Title = "Open Sheet from File",
		};
	}
	
	public override void _Ready() => FileSelected += doOpen;
	
	private void doOpen(string filePath)
	{
		string json = null;
		try
		{
			json = FileSystemUtilities.ReadString(filePath);
		}
		catch(Exception ex)
		{
			GD.PrintErr("Error opening sheet: ", ex);
		}
		
		if(!string.IsNullOrEmpty(json))
			EmitSignal(SignalName.JsonLoaded, json);
	}
}
