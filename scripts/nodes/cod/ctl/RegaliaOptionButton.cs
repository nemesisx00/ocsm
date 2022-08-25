using Godot;
using OCSM;

public class RegaliaOptionButton : OptionButton
{
	[Export]
	public bool emptyOption = true;
	[Export]
	private bool includeNonRegalia = false;
	
	public override void _Ready()
	{
		if(emptyOption)
			AddItem("");
		
		foreach(var regalia in Contracts.Regalia.asList())
		{
			AddItem(regalia);
		}
		
		if(includeNonRegalia)
		{
			foreach(var court in Court.asList())
			{
				AddItem(court);
			}
		}
	}
}
