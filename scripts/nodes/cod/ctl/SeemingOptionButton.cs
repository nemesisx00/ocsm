using Godot;
using System;
using OCSM;

public class SeemingOptionButton : OptionButton
{
	[Export]
	public bool emptyOption = true;
	
	public override void _Ready()
	{
		if(emptyOption)
			AddItem("");
		
		foreach(var seeming in Seeming.asList())
		{
			AddItem(seeming);
		}
	}
}
