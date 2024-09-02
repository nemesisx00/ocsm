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
	
	public Die SelectedDie
	{
		get
		{
			Die die = null;
			
			if(Selected > -1)
			{
				var text = GetItemText(Selected);
				if(int.TryParse(text.Replace('d', ' ').Trim(), out int sides))
					die = sides switch
					{
						4 => Die.D4,
						8 => Die.D8,
						10 => Die.D10,
						12 => Die.D12,
						20 => Die.D20,
						100 => Die.D100,
						_ => Die.D6,
					};
			}
			
			return die;
		}
		
		set
		{
			for(int i = 0; i < ItemCount; i++)
			{
				if(GetItemText(i) == value?.ToString())
				{
					Selected = i;
					break;
				}
			}
		}
	}
	
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
