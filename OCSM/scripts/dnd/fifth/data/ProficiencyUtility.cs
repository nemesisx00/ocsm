using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth;

public sealed class ProficiencyUtility
{
	public const string NoProficiency = "Not Proficient";
	public const string HalfProficiency = "Half Proficiency";
	public const string Proficient = "Proficient";
	public const string DoubleProficiency = "Expertise";
	
	public static string ByEnum(Proficiency value)
	{
		return value switch
		{
			Proficiency.HalfProficiency => HalfProficiency,
			Proficiency.Proficiency => Proficient,
			Proficiency.DoubleProficiency => DoubleProficiency,
			_ => NoProficiency,
		};
	}
	
	public static Proficiency ByName(string name)
	{
		return name switch
		{
			HalfProficiency => Proficiency.HalfProficiency,
			Proficient => Proficiency.Proficiency,
			DoubleProficiency => Proficiency.DoubleProficiency,
			_ => Proficiency.NoProficiency,
		};
	}
	
	public static Proficiency FromStatefulButtonState(StatefulButton.States state)
	{
		return state switch
		{
			StatefulButton.States.One => Proficiency.HalfProficiency,
			StatefulButton.States.Two => Proficiency.Proficiency,
			StatefulButton.States.Three => Proficiency.DoubleProficiency,
			_ => Proficiency.NoProficiency,
		};
	}
	
	public static StatefulButton.States ToStatefulButtonState(Proficiency proficiency)
	{
		return proficiency switch
		{
			Proficiency.HalfProficiency => StatefulButton.States.One,
			Proficiency.Proficiency => StatefulButton.States.Two,
			Proficiency.DoubleProficiency => StatefulButton.States.Three,
			_ => StatefulButton.States.None,
		};
	}
}
