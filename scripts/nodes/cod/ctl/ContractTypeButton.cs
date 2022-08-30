using Godot;
using OCSM;
using OCSM.CoD.CtL.Meta;

public class ContractTypeButton : OptionButton
{
	public override void _Ready()
	{
		var container = GetNode<MetadataManager>(Constants.NodePath.MetadataManager).Container;
		if(container is CoDChangelingContainer ccc)
		{
			foreach(var contractType in ccc.ContractTypes)
			{
				AddItem(contractType.Name);
			}
		}
	}
}
