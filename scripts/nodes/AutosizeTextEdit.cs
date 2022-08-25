using Godot;
using System;
using OCSM;

public class AutosizeTextEdit : TextEdit
{
	public override void _Ready()
	{
		Connect(Constants.Signal.TextChanged, this, nameof(autosize));
	}
	
	private void autosize()
	{
		TextEditUtilities.autoSize(this, Constants.TextInputMinHeight);
	}
}
