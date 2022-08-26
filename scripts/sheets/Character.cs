
namespace OCSM
{
	public sealed class GameSystem
	{
		public sealed class Cod
		{
			public const string Mortal = "CodMortal";
			public const string Changeling = "CodChangeling";
		}
	}
	
	public class Character
	{
		public string GameSystem { get; set; }
		public string Name { get; set; }
	}
}
