using Godot;

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
		if(string.IsNullOrEmpty(CurrentFile) || CurrentFile.Equals(AppConstants.SheetFileExtension))
		{
			var extensionIndex = path.LastIndexOf(AppConstants.SheetFileExtension);
			path = path.Insert(extensionIndex, AppConstants.NewSheetFileName);
		}
		else if(!path.EndsWith(AppConstants.SheetFileExtension))
			path += AppConstants.SheetFileExtension;
		
		if(!string.IsNullOrEmpty(SheetData))
			FileSystemUtilities.WriteString(path, SheetData);
	}
}
