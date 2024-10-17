using Godot;

namespace Ocsm.Nodes;

public partial class SaveSheet : FileDialog
{
	public static SaveSheet GenerateInstance()
	{
		return new()
		{
			Access = AccessEnum.Filesystem,
			CurrentDir = FileSystemUtilities.DefaultSheetDirectory,
			FileMode = FileModeEnum.SaveFile,
			Filters = ["*.ocsd", "OCSM Character Sheet Data"],
			InitialPosition = WindowInitialPosition.CenterPrimaryScreen,
			OkButtonText = "Save",
			ShowHiddenFiles = true,
			Size = new(720, 480),
			Theme = GD.Load<Theme>("res://resources/Default.tres"),
			Title = "Save Sheet to File",
		};
	}
	
	public string SheetData { get; set; }
	
	public override void _Ready() => FileSelected += doSave;
	
	private void doSave(string filePath)
	{
		if(!string.IsNullOrEmpty(SheetData))
			FileSystemUtilities.WriteString(filePath, SheetData);
	}
}
