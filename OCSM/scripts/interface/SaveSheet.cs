using Godot;
using System;

namespace Ocsm.Nodes;

public partial class SaveSheet : FileDialog
{
	public string SheetData { get; set; }
	
	public override void _Ready()
	{
		var path = FileSystemUtilities.DefaultSheetDirectory;
		CurrentDir = path;
		FileSelected += doSave;
	}
	
	private void doSave(string filePath)
	{
		var path = filePath;
		if(string.IsNullOrEmpty(CurrentFile) || CurrentFile.Equals(Constants.SheetFileExtension))
		{
			var extensionIndex = path.LastIndexOf(Constants.SheetFileExtension);
			path = path.Insert(extensionIndex, Constants.NewSheetFileName);
		}
		else if(!path.EndsWith(Constants.SheetFileExtension))
			path += Constants.SheetFileExtension;
		
		if(!string.IsNullOrEmpty(SheetData))
			FileSystemUtilities.WriteString(path, SheetData);
	}
}
