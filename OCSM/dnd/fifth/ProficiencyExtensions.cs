using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth;

public static class ProficiencyExtensions
{
	public static Proficiency ToProficiency(this StatefulButton.States state) => state switch
	{
		StatefulButton.States.One => Proficiency.HalfProficiency,
		StatefulButton.States.Two => Proficiency.Proficiency,
		StatefulButton.States.Three => Proficiency.DoubleProficiency,
		_ => Proficiency.NoProficiency,
	};
	
	public static StatefulButton.States ToStatefulButtonState(this Proficiency value) => value switch
	{
		Proficiency.HalfProficiency => StatefulButton.States.One,
		Proficiency.Proficiency => StatefulButton.States.Two,
		Proficiency.DoubleProficiency => StatefulButton.States.Three,
		_ => StatefulButton.States.None,
	};
}
