
namespace OCSM.DnD.Fifth
{
	public enum Proficiency { NoProficiency, HalfProficiency, Proficiency, DoubleProficiency }
	
	public sealed class ProficiencyUtility
	{
		public const string NoProficiency = "Not Proficient";
		public const string HalfProficiency = "Half Proficiency";
		public const string Proficient = "Proficient";
		public const string DoubleProficiency = "Expertise";
		
		public string byEnum(Proficiency value)
		{
			switch(value)
			{
				case Proficiency.HalfProficiency:
					return HalfProficiency;
				case Proficiency.Proficiency:
					return Proficient;
				case Proficiency.DoubleProficiency:
					return DoubleProficiency;
				case Proficiency.NoProficiency:
				default:
					return NoProficiency;
			}
		}
		
		public Proficiency byName(string name)
		{
			switch(name)
			{
				case HalfProficiency:
					return Proficiency.HalfProficiency;
				case Proficient:
					return Proficiency.Proficiency;
				case DoubleProficiency:
					return Proficiency.DoubleProficiency;
				case NoProficiency:
				default:
					return Proficiency.NoProficiency;
			}
		}
	}
}
