using Godot;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class DieOptionsButton : OptionButton
{
	[Export]
	public bool BardicInspiration { get; set; }
	[Export]
	public bool DamageDie { get; set; }
	[Export]
	public bool EmptyOption { get; set; }
	
	public override void _Ready()
	{
		if(EmptyOption)
			AddItem(string.Empty);
		
		if(!BardicInspiration)
			AddItem(Die.D4.ToString());
		
		AddItem(Die.D6.ToString());
		AddItem(Die.D8.ToString());
		AddItem(Die.D10.ToString());
		AddItem(Die.D12.ToString());
		
		if(!BardicInspiration && !DamageDie)
		{
			AddItem(Die.D20.ToString());
			AddItem(Die.D100.ToString());
		}
	}
}
