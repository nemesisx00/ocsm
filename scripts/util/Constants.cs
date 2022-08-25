
namespace OCSM
{
	public class Constants
	{
		public const int TextInputMinHeight = 23;
		
		public sealed class Action
		{
			public const string Cancel = "ui_cancel";
			public const string FileNew = "fileNew";
			public const string FileOpen = "fileOpen";
			public const string FileSave = "fileSave";
			public const string FileSaveAs = "fileSaveAs";
			public const string FileCloseSheet = "fileCloseSheet";
		}
		
		public sealed class NodePath
		{
			public const string AppManager = "/root/AppManager";
			public const string AppRoot = "/root/AppRoot";
			public const string NewSheet = "/root/AppRoot/NewSheet";
			public const string SheetManager = "/root/SheetManager";
			public const string SheetTabs = "/root/AppRoot/Column/SheetTabs";
		}
		
		public sealed class Scene
		{
			public const string ConfirmQuit = "res://scenes/ConfirmQuit.tscn";
			public const string DarkPack = "res://scenes/DarkPack.tscn";
			public const string NewSheet = "res://scenes/NewSheet.tscn";
			
			public sealed class CoD
			{
				public const string BoxComplex = "res://scenes/cod/nodes/BoxComplex.tscn";
				public const string ItemDots = "res://scenes/cod/nodes/ItemDots.tscn";
				public const string ToggleButton = "res://scenes/cod/nodes/ToggleButton.tscn";
				public const string Specialty = "res://scenes/cod/nodes/Specialty.tscn";
				
				public sealed class Mortal
				{
					public const string Sheet = "res://scenes/cod/sheets/Mortal.tscn";
				}
				
				public sealed class Changeling
				{
					public const string Sheet = "res://scenes/cod/sheets/ChangelingTheLost.tscn";
					public const string Contract = "res://scenes/cod/nodes/ctl/Contract.tscn";
				}
			}
		}
		
		public sealed class Signal
		{
			public const string Confirmed = "confirmed";
			public const string GuiInput = "gui_input";
			public const string IdPressed = "id_pressed";
			public const string ItemSelected = "item_selected";
			public const string NodeChanged = "NodeChanged";
			public const string Pressed = "pressed";
			public const string Released = "released";
			public const string TextChanged = "text_changed";
			public const string ValueChanged = "ValueChanged";
		}
		
		public sealed class Texture
		{
			public const string FullTransparent = "res://assets/textures/cod/FullTransparent.png";
			public const string TrackBoxBorder = "res://assets/textures/cod/TrackBoxBorder.png";
			public const string TrackBox1 = "res://assets/textures/cod/TrackBox1.png";
			public const string TrackBox2 = "res://assets/textures/cod/TrackBox2.png";
			public const string TrackBox3 = "res://assets/textures/cod/TrackBox3.png";
			public const string TrackCircle = "res://assets/textures/cod/TrackCircle.png";
			public const string TrackCircleFill = "res://assets/textures/cod/TrackCircleFill.png";
		}
	}
}
