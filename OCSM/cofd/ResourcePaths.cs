namespace Ocsm.Cofd;

public static class ResourcePaths
{
	public const string ItemDots = $"{PrefixCofdNodes}/ItemDots.tscn";
	public const string PrefixAssets = "res://cofd/assets";
	public const string Specialty = $"{PrefixCofdNodes}/Specialty.tscn";
	
	private const string PrefixCofd = "res://cofd";
	private const string PrefixCofdNodes = $"{PrefixCofd}/nodes";
	
	public static class Mortal
	{
		public const string NewSheetName = "New Mortal";
		public const string Sheet = $"{PrefixMortal}/Mortal.tscn";
		
		private const string PrefixMortal = $"{PrefixCofd}/mortal/nodes";
	}
	
	public static class Changeling
	{
		public const string ContractNode = $"{PrefixCtl}/ContractNode.tscn";
		public const string NewSheetName = "New Changeling";
		public const string SeemingBenefit = $"{PrefixCtl}/SeemingBenefit.tscn";
		public const string Sheet = $"{PrefixCtl}/ChangelingTheLost.tscn";
		
		private const string PrefixCtl = $"{PrefixCofd}/ctl/nodes";
		
		public static class Meta
		{
			public const string AddEditMetadata = $"{PrefixCtl}/meta/CofdChangelingAddEditMetadata.tscn";
		}
	}
	
	public static class Mage
	{
		public const string NewSheetName = "New Mage";
		//public const string Sheet = $"{PrefixMta}/MageTheAwakening.tscn";
		public const string Sheet = "";
		
		private const string PrefixMta = $"{PrefixCofd}/mta/nodes";
	}
	
	public static class Vampire
	{
		public const string NewSheetName = "New Kindred";
		//public const string Sheet = $"{PrefixVtr}/VampireTheRequiem.tscn";
		public const string Sheet = "";
		
		private const string PrefixVtr = $"{PrefixCofd}/vtr/nodes";
	}
}
