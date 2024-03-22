namespace Ocsm;

public static class ScenePaths
{
	private const string Prefix = "res://scenes";
	
	public const string AboutGodot = $"{Prefix}/AboutGodot.tscn";
	public const string AboutOcsm = $"{Prefix}/AboutOcsm.tscn";
	public const string ConfirmQuit = $"{Prefix}/ConfirmQuit.tscn";
	public const string DarkPack = $"{Prefix}/DarkPack.tscn";
	public const string GameSystemLicenses = $"{Prefix}/GameSystemLicenses.tscn";
	public const string NewSheet = $"{Prefix}/NewSheet.tscn";
	public const string OpenSheet = $"{Prefix}/OpenSheet.tscn";
	public const string SaveSheet = $"{Prefix}/SaveSheet.tscn";
	public const string StatefulButton = $"{Prefix}/StatefulButton.tscn";
	public const string ToggleButton = $"{Prefix}/ToggleButton.tscn";
	
	public static class Meta
	{
		private const string PrefixMeta = $"{Prefix}/meta";
		
		public const string ConfirmDeleteEntry = $"{PrefixMeta}/ConfirmDeleteEntry.tscn";
	}
	
	public static class Cofd
	{
		private const string PrefixCofd = $"{Prefix}/cofd";
		
		public const string ItemDots = $"{PrefixCofd}/ItemDots.tscn";
		public const string Specialty = $"{PrefixCofd}/Specialty.tscn";
		
		public static class Mortal
		{
			private const string PrefixMortal = $"{PrefixCofd}/mortal";
			
			public const string NewSheetName = "New Mortal";
			public const string Sheet = $"{PrefixMortal}/Mortal.tscn";
		}
		
		public static class Changeling
		{
			private const string PrefixCtl = $"{PrefixCofd}/ctl";
			
			public const string ContractNode = $"{PrefixCtl}/ContractNode.tscn";
			public const string NewSheetName = "New Changeling";
			public const string SeemingBenefit = $"{PrefixCtl}/SeemingBenefit.tscn";
			public const string Sheet = $"{PrefixCtl}/ChangelingTheLost.tscn";
			
			public static class Meta
			{
				public const string AddEditMetadata = $"{PrefixCtl}/meta/CofdChangelingAddEditMetadata.tscn";
			}
		}
	}
	
	public static class Dnd
	{
		private const string PrefixDnd = $"{Prefix}/dnd";
		
		public static class Fifth
		{
			private const string PrefixDnd5e = $"{PrefixDnd}/fifth";
			
			public const string Skill = $"{PrefixDnd5e}/Skill.tscn";
			public const string Sheet = $"{PrefixDnd5e}/Fifth.tscn";
			public const string NewSheetName = "New Adventurer";
			public const string Feature = $"{PrefixDnd5e}/Feature.tscn";
			public const string FeatureSection = $"{PrefixDnd5e}/FeatureSection.tscn";
			public const string InventoryItem = $"{PrefixDnd5e}/InventoryItem.tscn";
			
			public static class Meta
			{
				private const string PrefixDnd5eMeta = $"{PrefixDnd5e}/meta";
				
				public const string AddEditMetadata = $"{PrefixDnd5eMeta}/DndFifthAddEditMetadata.tscn";
				public const string FeatureSectionEntry = $"{PrefixDnd5eMeta}/FeatureSectionEntry.tscn";
				public const string FeatureEntry = $"{PrefixDnd5eMeta}/FeatureEntry.tscn";
				public const string NumericBonusEdit = $"{PrefixDnd5eMeta}/NumericBonusEdit.tscn";
			}
		}
	}
}
