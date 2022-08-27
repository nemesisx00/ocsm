using Godot;
using OCSM.CoD.CtL;

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
		
		foreach(var regalia in Regalia.asList())
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
