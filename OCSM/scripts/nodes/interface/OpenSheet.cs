using Godot;
using System;

namespace Ocsm.Nodes;

public partial class OpenSheet : FileDialog
{
	[Signal]
	public delegate void JsonLoadedEventHandler(string json);
	
	public override void _Ready()
	{
		var path = FileSystemUtilities.DefaultSheetDirectory;
		CurrentDir = path;
		FileSelected += doOpen;
	}
	
	private void doOpen(string filePath)
	{
		var path = filePath;
		if(string.IsNullOrEmpty(CurrentFile) || CurrentFile.Equals(AppConstants.SheetFileExtension))
		{
			var extensionIndex = path.LastIndexOf(AppConstants.SheetFileExtension);
			path = path.Insert(extensionIndex, AppConstants.NewSheetFileName);
		}
		else if(!path.EndsWith(AppConstants.SheetFileExtension))
			path += AppConstants.SheetFileExtension;
		
		string json = null;
		try
		{
			json = FileSystemUtilities.ReadString(path);
		}
		catch(Exception ex)
		{
			GD.PrintErr("Error opening sheet: ", ex);
		}
		
		if(!string.IsNullOrEmpty(json))
			_ = EmitSignal(SignalName.JsonLoaded, json);
	}
}