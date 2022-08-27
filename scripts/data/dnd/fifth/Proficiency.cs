
namespace OCSM.DnD.Fifth
{
	public sealed class Proficiency
	{
		public enum Enum { NoProficiency, HalfProficiency, Proficiency, DoubleProficiency }
		
		public const string NoProficiency = "Not Proficient";
		public const string HalfProficiency = "Half Proficiency";
		public const string Proficient = "Proficient";
		public const string DoubleProficiency = "Expertise";
		
		public string byEnum(Enum value)
		{
			switch(value)
			{
				case Enum.HalfProficiency:
					return HalfProficiency;
				case Enum.Proficiency:
					return Proficient;
				case Enum.DoubleProficiency:
					return DoubleProficiency;
				case Enum.NoProficiency:
				default:
					return NoProficiency;
			}
		}
		
		public Enum byName(string name)
		{
			switch(name)
			{
				case HalfProficiency:
					return Enum.HalfProficiency;
				case Proficient:
					return Enum.Proficiency;
				case DoubleProficiency:
					return Enum.DoubleProficiency;
				case NoProficiency:
				default:
					return Enum.NoProficiency;
			}
		}
	}
}
