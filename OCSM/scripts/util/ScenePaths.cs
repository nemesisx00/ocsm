namespace Ocsm;

public static class ScenePaths
{
	public const string AboutGodot = $"{Prefix}/AboutGodot.tscn";
	public const string AboutOcsm = $"{Prefix}/AboutOCSM.tscn";
	public const string ConfirmQuit = $"{Prefix}/ConfirmQuit.tscn";
	public const string DarkPack = $"{Prefix}/DarkPack.tscn";
	public const string GameSystemLicenses = $"{Prefix}/GameSystemLicenses.tscn";
	public const string AddSheet = $"{Prefix}/AddSheet.tscn";
	public const string NewSheet = $"{Prefix}/NewSheet.tscn";
	public const string NewSheetSection = $"{Prefix}/NewSheetSection.tscn";
	public const string OpenSheet = $"{Prefix}/OpenSheet.tscn";
	public const string SaveSheet = $"{Prefix}/SaveSheet.tscn";
	public const string DynamicTextLabel = $"{Prefix}/DynamicTextLabel.tscn";
	public const string StatefulButton = $"{Prefix}/StatefulButton.tscn";
	public const string ToggleButton = $"{Prefix}/ToggleButton.tscn";
	
	private const string Prefix = "res://nodes";
	
	public static class Meta
	{
		public const string ConfirmDeleteEntry = $"{PrefixMeta}/ConfirmDeleteEntry.tscn";
		
		private const string PrefixMeta = $"{Prefix}/meta";
	}
}
