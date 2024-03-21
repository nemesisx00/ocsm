
namespace Ocsm;

/**
<summary>
Class containing all constant values which need to be universally
accessible within Ocsm.
</summary>
*/
public class Constants
{
	public const string AppVersion = "0.5.0";
	public const string MetadataFileExtension = ".ocmd";
	public const string NewSheetFileName = "New Sheet";
	public const string SheetFileExtension = ".ocsd";
	
	public const string The = "The";
	
	public static class Action
	{
		public const string Cancel = "ui_cancel";
		public const string FileNew = "fileNew";
		public const string FileOpen = "fileOpen";
		public const string FileSave = "fileSave";
		public const string FileSaveAs = "fileSaveAs";
		public const string FileCloseSheet = "fileCloseSheet";
	}
	
	public static class Json
	{
		public const string SheetType = "type";
		public const string SheetData = "data";
		
		public static class DataType
		{
			public const string CofdMortal = "Ocsm.Mortal";
			public const string CofdChangeling = "Ocsm.Changeling";
		}
	}
	
	public static class NodePath
	{
		public const string AppManager = "/root/AppManager";
		public const string AppRoot = "/root/AppRoot";
		public const string MetadataManager = "/root/MetadataManager";
		public const string NewSheet = "/root/AppRoot/NewSheet";
		public const string SheetManager = "/root/SheetManager";
		public const string SheetTabs = "/root/AppRoot/Column/SheetTabs";
	}
	
	public static class Scene
	{
		public const string AboutGodot = "res://scenes/AboutGodot.tscn";
		public const string AboutOcsm = "res://scenes/AboutOcsm.tscn";
		public const string ConfirmQuit = "res://scenes/ConfirmQuit.tscn";
		public const string DarkPack = "res://scenes/DarkPack.tscn";
		public const string GameSystemLicenses = "res://scenes/GameSystemLicenses.tscn";
		public const string NewSheet = "res://scenes/NewSheet.tscn";
		public const string OpenSheet = "res://scenes/OpenSheet.tscn";
		public const string SaveSheet = "res://scenes/SaveSheet.tscn";
		public const string StatefulButton = "res://scenes/nodes/StatefulButton.tscn";
		public const string ToggleButton = "res://scenes/nodes/ToggleButton.tscn";
		
		public static class Meta
		{
			public const string ConfirmDeleteEntry = "res://scenes/meta/ConfirmDeleteEntry.tscn";
		}
		
		public static class Cofd
		{
			public const string ItemDots = "res://scenes/cofd/nodes/ItemDots.tscn";
			public const string Specialty = "res://scenes/cofd/nodes/Specialty.tscn";
			
			public static class Mortal
			{
				public const string NewSheetName = "New Mortal";
				public const string Sheet = "res://scenes/cofd/sheets/Mortal.tscn";
			}
			
			public static class Changeling
			{
				public const string ContractNode = "res://scenes/cofd/nodes/ctl/ContractNode.tscn";
				public const string NewSheetName = "New Changeling";
				public const string SeemingBenefit = "res://scenes/cofd/nodes/ctl/SeemingBenefit.tscn";
				public const string Sheet = "res://scenes/cofd/sheets/ChangelingTheLost.tscn";
				
				public static class Meta
				{
					public const string AddEditMetadata = "res://scenes/cofd/nodes/ctl/meta/CofdChangelingAddEditMetadata.tscn";
				}
			}
		}
		
		public static class Dnd
		{
			public static class Fifth
			{
				public const string Skill = "res://scenes/dnd/fifth/Skill.tscn";
				public const string Sheet = "res://scenes/dnd/sheets/Fifth.tscn";
				public const string NewSheetName = "New Adventurer";
				public const string Feature = "res://scenes/dnd/fifth/Feature.tscn";
				public const string FeatureSection = "res://scenes/dnd/fifth/FeatureSection.tscn";
				public const string InventoryItem = "res://scenes/dnd/fifth/InventoryItem.tscn";
				
				public static class Meta
				{
					public const string AddEditMetadata = "res://scenes/dnd/fifth/meta/DndFifthAddEditMetadata.tscn";
					public const string FeatureSectionEntry = "res://scenes/dnd/fifth/meta/FeatureSectionEntry.tscn";
					public const string FeatureEntry = "res://scenes/dnd/fifth/meta/FeatureEntry.tscn";
					public const string NumericBonusEdit = "res://scenes/dnd/fifth/meta/NumericBonusEdit.tscn";
				}
			}
		}
	}
	
	public static class Texture
	{
		public const string FullTransparent = "res://assets/textures/box-transparent-16.png";
		public const string TrackBoxBorder = "res://assets/textures/box-border-16.png";
		public const string TrackBox1 = "res://assets/textures/slash-one.png";
		public const string TrackBox2 = "res://assets/textures/slash-two.png";
		public const string TrackBox3 = "res://assets/textures/slash-three.png";
		public const string TrackCircle = "res://assets/textures/circle-empty.png";
		public const string TrackCircleFill = "res://assets/textures/circle-fill.png";
		public const string TrackCircleHalf = "res://assets/textures/circle-fill-half.png";
		public const string TrackCircleRed = "res://assets/textures/circle-fill-red.png";
	}
}
