using Godot;

namespace Ocsm;

/// <summary>
/// Class containing all constant values which need to be universally
/// accessible within Ocsm.
/// </summary>
public class AppConstants
{
	public const string AppVersion = "0.5.0";
	public const string MetadataFileExtension = ".ocmd";
	public const string NewSheetFileName = "New Sheet";
	public const string SheetFileExtension = ".ocsd";
}

public sealed class Actions
{
	public const string Cancel = "ui_cancel";
	public const string FileNew = "fileNew";
	public const string FileOpen = "fileOpen";
	public const string FileSave = "fileSave";
	public const string FileSaveAs = "fileSaveAs";
	public const string FileCloseSheet = "fileCloseSheet";
}

public sealed class ScenePaths
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
		public const string ItemDots = "res://scenes/cofd/nodes/ItemDots.tscn";
		public const string Specialty = "res://scenes/cofd/nodes/Specialty.tscn";
		
		public sealed class Mortal
		{
			public const string NewSheetName = "New Mortal";
			public const string Sheet = "res://scenes/cofd/sheets/Mortal.tscn";
		}
		
		public sealed class Changeling
		{
			public const string ContractNode = "res://scenes/cofd/nodes/ctl/ContractNode.tscn";
			public const string NewSheetName = "New Changeling";
			public const string SeemingBenefit = "res://scenes/cofd/nodes/ctl/SeemingBenefit.tscn";
			public const string Sheet = "res://scenes/cofd/sheets/ChangelingTheLost.tscn";
			
			public sealed class Meta
			{
				public const string AddEditMetadata = "res://scenes/cofd/nodes/ctl/meta/CofdChangelingAddEditMetadata.tscn";
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
	
	public sealed class Wod
	{
		public sealed class V5
		{
			public static readonly StringName Sheet = "res://scenes/wod/sheets/V5Kindred.tscn";
			public static readonly StringName NewSheetName = "New Kindred";
		}
	}
}

public sealed class TexturePaths
{
	public static readonly StringName BoxTransparent = new("res://assets/textures/box-transparent-16.png");
	public static readonly StringName BoxBorder = new("res://assets/textures/box-border-16.png");
	public static readonly StringName SlashOne = new("res://assets/textures/slash-one.png");
	public static readonly StringName SlashOneBox = new("res://assets/textures/slash-one-box.png");
	public static readonly StringName SlashTwo = new("res://assets/textures/slash-two.png");
	public static readonly StringName SlashTwoBox = new("res://assets/textures/slash-two-box.png");
	public static readonly StringName SlashThree = new("res://assets/textures/slash-three.png");
	public static readonly StringName SlashThreeBox = new("res://assets/textures/slash-three-box.png");
	public static readonly StringName CircleEmpty = new("res://assets/textures/circle-empty.png");
	public static readonly StringName CircleFill = new("res://assets/textures/circle-fill.png");
	public static readonly StringName CircleFillHalf = new("res://assets/textures/circle-fill-half.png");
	public static readonly StringName CircleFillRed = new("res://assets/textures/circle-fill-red.png");
}
