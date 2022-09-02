using Godot;
using System;

namespace OCSM.Nodes
{
	public class OpenSheet : FileDialog
	{
		[Signal]
		public delegate void JsonLoaded(string json);
		
		public override void _Ready()
		{
			var path = FileSystemUtilities.DefaultSheetDirectory;
			CurrentDir = path;
			Connect(Constants.Signal.FileSelected, this, nameof(doOpen));
		}
		
		private void doOpen(string filePath)
		{
			var path = filePath;
			if(String.IsNullOrEmpty(CurrentFile) || CurrentFile.Equals(Constants.SheetFileExtension))
			{
				var extensionIndex = path.FindLast(Constants.SheetFileExtension);
				path = path.Insert(extensionIndex, Constants.NewSheetFileName);
			}
			else if(!path.EndsWith(Constants.SheetFileExtension))
				path += Constants.SheetFileExtension;
			
			string json = null;
			try
			{
				json = FileSystemUtilities.ReadString(path);
			}
			catch(Exception ex)
			{
				GD.PrintErr("Error opening sheet: ", ex);
			}
			
			if(!String.IsNullOrEmpty(json))
				EmitSignal(nameof(JsonLoaded), json);
		}
	}
}
