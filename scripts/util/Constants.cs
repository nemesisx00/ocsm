
namespace OCSM
{
	public class Constants
	{
		public const int TextInputMinHeight = 23;
		public const string NewSheetFileName = "New Sheet";
		public const string SheetFileExtension = ".ocsd";
		
		public sealed class Action
		{
			public const string Cancel = "ui_cancel";
			public const string FileNew = "fileNew";
			public const string FileOpen = "fileOpen";
			public const string FileSave = "fileSave";
			public const string FileSaveAs = "fileSaveAs";
			public const string FileCloseSheet = "fileCloseSheet";
		}
		
		public sealed class Json
		{
			public const string SheetType = "type";
			public const string SheetData = "data";
			
			public sealed class DataType
			{
				public const string CodMortal = "OCSM.Mortal";
				public const string CodChangeling = "OCSM.Changeling";
			}
		}
		
		public sealed class NodePath
		{
			public const string AppManager = "/root/AppManager";
			public const string AppRoot = "/root/AppRoot";
			public const string MetadataManager = "/root/MetadataManager";
			public const string NewSheet = "/root/AppRoot/NewSheet";
			public const string SheetManager = "/root/SheetManager";
			public const string SheetTabs = "/root/AppRoot/Column/SheetTabs";
		}
		
		public sealed class Scene
		{
			public const string ConfirmQuit = "res://scenes/ConfirmQuit.tscn";
			public const string DarkPack = "res://scenes/DarkPack.tscn";
			public const string GameSystemLicenses = "res://scenes/GameSystemLicenses.tscn";
			public const string NewSheet = "res://scenes/NewSheet.tscn";
			public const string OpenSheet = "res://scenes/OpenSheet.tscn";
			public const string SaveSheet = "res://scenes/SaveSheet.tscn";
			
			public sealed class Meta
			{
				public const string ConfirmDeleteEntry = "res://scenes/meta/ConfirmDeleteEntry.tscn";
			}
			
			public sealed class CoD
			{
				public const string BoxComplex = "res://scenes/cod/nodes/BoxComplex.tscn";
				public const string ItemDots = "res://scenes/cod/nodes/ItemDots.tscn";
				public const string ToggleButton = "res://scenes/cod/nodes/ToggleButton.tscn";
				public const string Specialty = "res://scenes/cod/nodes/Specialty.tscn";
				
				public sealed class Mortal
				{
					public const string NewSheetName = "New Mortal";
					public const string Sheet = "res://scenes/cod/sheets/Mortal.tscn";
				}
				
				public sealed class Changeling
				{
					public const string Contract = "res://scenes/cod/nodes/ctl/Contract.tscn";
					public const string NewSheetName = "New Changeling";
					public const string SeemingBenefit = "res://scenes/cod/nodes/ctl/SeemingBenefit.tscn";
					public const string Sheet = "res://scenes/cod/sheets/ChangelingTheLost.tscn";
					
					public sealed class Meta
					{
						public const string AddEditMetadata = "res://scenes/cod/nodes/ctl/meta/AddEditMetadata.tscn";
					}
				}
			}
			
			public sealed class DnD
			{
				public sealed class Fifth
				{
					public const string Sheet = "res://scenes/dnd/sheets/Fifth.tscn";
					public const string NewSheetName = "New Adventurer";
					public const string NewFeature = "res://scenes/dnd/fifth/NewFeature.tscn";
					public const string FeatureSection = "res://scenes/dnd/fifth/FeatureSection.tscn";
				}
			}
		}
		
		public sealed class Signal
		{
			public const string Confirmed = "confirmed";
			public const string FileSelected = "file_selected";
			public const string GuiInput = "gui_input";
			public const string IdPressed = "id_pressed";
			public const string ItemSelected = "item_selected";
			public const string NodeChanged = "NodeChanged";
			public const string Pressed = "pressed";
			public const string Released = "released";
			public const string TabChanged = "tab_changed";
			public const string TabSelected = "tab_selected";
			public const string TextChanged = "text_changed";
			public const string ValueChanged = "value_changed";
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
