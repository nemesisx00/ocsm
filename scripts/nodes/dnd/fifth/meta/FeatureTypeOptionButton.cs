using Godot;
using OCSM.DnD.Fifth.Meta;

public class FeatureTypeOptionButton : OptionButton
{
	[Export]
	public bool emptyOption = true;
	
	public override void _Ready()
	{
		if(emptyOption)
			AddItem("");
		
		foreach(var type in FeatureType.asList())
		{
			AddItem(type);
		}
	}
}
