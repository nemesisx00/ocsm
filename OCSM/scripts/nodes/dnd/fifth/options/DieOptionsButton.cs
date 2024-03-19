using Godot;

namespace Ocsm.Nodes.Dnd.Fifth;

public partial class DieOptionsButton : CustomOption
{
	[Export]
	public bool BardicInspiration { get; set; } = false;
	[Export]
	public bool DamageDie { get; set; } = false;
	
	public override void _Ready()
	{
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
