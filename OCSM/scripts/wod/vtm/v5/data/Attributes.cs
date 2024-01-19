
namespace Ocsm.Wod.Vtm.V5.Data;

public class Attributes()
{
	public const int MinimumValue = 1;
	
	public int Charisma { get; set; } = MinimumValue;
	public int Composure { get; set; } = MinimumValue;
	public int Dexterity { get; set; } = MinimumValue;
	public int Intelligence { get; set; } = MinimumValue;
	public int Manipulation { get; set; } = MinimumValue;
	public int Resolve { get; set; } = MinimumValue;
	public int Stamina { get; set; } = MinimumValue;
	public int Strength { get; set; } = MinimumValue;
	public int Wits { get; set; } = MinimumValue;
	
	public void Normalize()
	{
		if(Charisma < MinimumValue)
			Charisma = MinimumValue;
		
		if(Composure < MinimumValue)
			Composure = MinimumValue;
		
		if(Dexterity < MinimumValue)
			Dexterity = MinimumValue;
		
		if(Intelligence < MinimumValue)
			Intelligence = MinimumValue;
		
		if(Manipulation < MinimumValue)
			Manipulation = MinimumValue;
		
		if(Resolve < MinimumValue)
			Resolve = MinimumValue;
		
		if(Stamina < MinimumValue)
			Stamina = MinimumValue;
		
		if(Strength < MinimumValue)
			Strength = MinimumValue;
		
		if(Wits < MinimumValue)
			Wits = MinimumValue;
	}
}
