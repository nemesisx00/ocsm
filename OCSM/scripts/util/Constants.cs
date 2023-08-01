
namespace Ocsm;

/// <summary>
/// Class containing all constant values which need to be universally
/// accessible within Ocsm.
/// </summary>
public class Constants
{
	public const string AppVersion = "0.5.0";
	public const string MetadataFileExtension = ".ocmd";
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
	
	public sealed class GameSystem
	{
		public sealed class Cofd
		{
			public const string Mortal = "CodMortal";
			public const string Changeling = "CodChangeling";
		}
		
		public sealed class Dnd
		{
			public const string Fifth = "Dnd5e";
		}
	}
	
	public sealed class Json
	{
		public const string SheetType = "type";
		public const string SheetData = "data";
		
		public sealed class DataType
		{
			public const string CodMortal = "Ocsm.Mortal";
			public const string CodChangeling = "Ocsm.Changeling";
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
		
		public sealed class Meta
		{
			public const string ConfirmDeleteEntry = "res://scenes/meta/ConfirmDeleteEntry.tscn";
		}
		
		public sealed class Cofd
		{
			public const string ItemDots = "res://scenes/cod/nodes/ItemDots.tscn";
			public const string Specialty = "res://scenes/cod/nodes/Specialty.tscn";
			
			public sealed class Mortal
			{
				public const string NewSheetName = "New Mortal";
				public const string Sheet = "res://scenes/cod/sheets/Mortal.tscn";
			}
			
			public sealed class Changeling
			{
				public const string ContractNode = "res://scenes/cod/nodes/ctl/ContractNode.tscn";
				public const string NewSheetName = "New Changeling";
				public const string SeemingBenefit = "res://scenes/cod/nodes/ctl/SeemingBenefit.tscn";
				public const string Sheet = "res://scenes/cod/sheets/ChangelingTheLost.tscn";
				
				public sealed class Meta
				{
					public const string AddEditMetadata = "res://scenes/cod/nodes/ctl/meta/CodChangelingAddEditMetadata.tscn";
				}
			}
		}
		
		public sealed class Dnd
		{
			public sealed class Fifth
			{
				public const string Skill = "res://scenes/Dnd/Fifth/Skill.tscn";
				public const string Sheet = "res://scenes/Dnd/sheets/Fifth.tscn";
				public const string NewSheetName = "New Adventurer";
				public const string Feature = "res://scenes/Dnd/Fifth/Feature.tscn";
				public const string FeatureSection = "res://scenes/Dnd/Fifth/FeatureSection.tscn";
				public const string InventoryItem = "res://scenes/Dnd/Fifth/InventoryItem.tscn";
				
				public sealed class Meta
				{
					public const string AddEditMetadata = "res://scenes/Dnd/Fifth/meta/DndFifthAddEditMetadata.tscn";
					public const string FeatureSectionEntry = "res://scenes/Dnd/Fifth/meta/FeatureSectionEntry.tscn";
					public const string FeatureEntry = "res://scenes/Dnd/Fifth/meta/FeatureEntry.tscn";
					public const string NumericBonusEdit = "res://scenes/Dnd/Fifth/meta/NumericBonusEdit.tscn";
				}
			}
		}
	}
	
	public sealed class Texture
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
