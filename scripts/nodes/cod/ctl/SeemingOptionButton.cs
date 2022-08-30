using Godot;
using OCSM;
using OCSM.CoD.CtL.Meta;

public class SeemingOptionButton : OptionButton
{
	[Export]
	public bool emptyOption = true;
	
	public override void _Ready()
	{
		if(emptyOption)
			AddItem("");
		
		var container = GetNode<MetadataManager>(Constants.NodePath.MetadataManager).Container;
		if(container is CoDChangelingContainer ccc)
		{
			foreach(var seeming in ccc.Seemings)
			{
				AddItem(seeming.Name);
			}
		}
	}
}
