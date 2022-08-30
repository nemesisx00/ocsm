using Godot;
using OCSM;
using OCSM.CoD.CtL.Meta;

public class CourtOptionButton : OptionButton
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
			foreach(var court in ccc.Courts)
			{
				AddItem(court.Name);
			}
		}
	}
}
