using Godot;
using Ocsm.Nodes;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class DieOptionsButton : CustomOption
{
	[Export]
	public bool BardicInspiration { get; set; }
	[Export]
	public bool DamageDie { get; set; }
	
	public override void _Ready()
	{
		AddItem(string.Empty);
		
		if(!BardicInspiration)
			AddItem(Die.Four.ToString());
		
		AddItem(Die.Six.ToString());
		AddItem(Die.Eight.ToString());
		AddItem(Die.Ten.ToString());
		AddItem(Die.Twelve.ToString());
		
		if(!BardicInspiration && !DamageDie)
		{
			AddItem(Die.Twenty.ToString());
			AddItem(Die.OneHundred.ToString());
		}
	}
}
