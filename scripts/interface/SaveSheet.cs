using Godot;
using System;
using OCSM;

public class SaveSheet : Godot.FileDialog
{
	public string SheetData { get; set; }
	
	public override void _Ready()
	{
		string path;
		switch(System.Environment.OSVersion.Platform)
		{
			case PlatformID.Unix:
				path = System.IO.Path.GetFullPath(System.Environment.ExpandEnvironmentVariables(Constants.FilePath.Linux) + Constants.FilePath.Sheets).ToLower();
				break;
			case PlatformID.Win32NT:
			default:
				path = System.IO.Path.GetFullPath(System.Environment.ExpandEnvironmentVariables(Constants.FilePath.Windows) + Constants.FilePath.Sheets);
				break;
		}
		System.IO.Directory.CreateDirectory(path);
		CurrentDir = path;
		Connect(Constants.Signal.FileSelected, this, nameof(doSave));
	}
	
	private void doSave(string filePath)
	{
		var path = filePath;
		if(String.IsNullOrEmpty(CurrentFile) || CurrentFile.Equals(Constants.SheetFileExtension))
		{
			var extensionIndex = path.FindLast(Constants.SheetFileExtension);
			path = path.Insert(extensionIndex, Constants.NewSheetFileName);
		}
		else if(!path.EndsWith(Constants.SheetFileExtension))
			path += Constants.SheetFileExtension;
		
		if(!String.IsNullOrEmpty(SheetData))
			System.IO.File.WriteAllText(path, SheetData);
	}
}
