using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth
{
	public enum Proficiency { NoProficiency, HalfProficiency, Proficiency, DoubleProficiency }
	
	public sealed class ProficiencyUtility
	{
		public const string NoProficiency = "Not Proficient";
		public const string HalfProficiency = "Half Proficiency";
		public const string Proficient = "Proficient";
		public const string DoubleProficiency = "Expertise";
		
		public static string byEnum(Proficiency value)
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
		
		public static Proficiency byName(string name)
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
		
		public static Proficiency fromStatefulButtonState(string state)
		{
			switch(state)
			{
				case StatefulButton.State.One:
					return Proficiency.HalfProficiency;
				case StatefulButton.State.Two:
					return Proficiency.Proficiency;
				case StatefulButton.State.Three:
					return Proficiency.DoubleProficiency;
				case StatefulButton.State.None:
				default:
					return Proficiency.NoProficiency;
			}
		}
		
		public static string toStatefulButtonState(Proficiency proficiency)
		{
			switch(proficiency)
			{
				case Proficiency.HalfProficiency:
					return StatefulButton.State.One;
				case Proficiency.Proficiency:
					return StatefulButton.State.Two;
				case Proficiency.DoubleProficiency:
					return StatefulButton.State.Three;
				case Proficiency.NoProficiency:
				default:
					return StatefulButton.State.None;
			}
		}
	}
}
