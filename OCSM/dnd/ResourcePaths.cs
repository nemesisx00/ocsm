namespace Ocsm.Dnd;

public static class ResourcePaths
{
	public const string PrefixAssets = "res://dnd/assets";
	
	private const string PrefixDnd = "res://dnd";
	
	public static class Fifth
	{
		public const string ClassRow = $"{PrefixDnd5e}/ClassRow.tscn";
		public const string Skill = $"{PrefixDnd5e}/Skill.tscn";
		public const string Sheet = $"{PrefixDnd5e}/Fifth.tscn";
		public const string NewSheetName = "New Adventurer";
		public const string Feature = $"{PrefixDnd5e}/Feature.tscn";
		public const string FeatureSection = $"{PrefixDnd5e}/FeatureSection.tscn";
		public const string InventoryItem = $"{PrefixDnd5e}/InventoryItem.tscn";
		
		private const string PrefixDnd5e = $"{PrefixDnd}/fifth/nodes";
		
		public static class Meta
		{
			public const string AddEditMetadata = $"{PrefixDnd5eMeta}/DndFifthAddEditMetadata.tscn";
			public const string FeatureSectionEntry = $"{PrefixDnd5eMeta}/FeatureSectionEntry.tscn";
			public const string FeatureEntry = $"{PrefixDnd5eMeta}/FeatureEntry.tscn";
			public const string NumericBonusEdit = $"{PrefixDnd5eMeta}/NumericBonusEdit.tscn";
			
			private const string PrefixDnd5eMeta = $"{PrefixDnd5e}/meta";
		}
	}
}
