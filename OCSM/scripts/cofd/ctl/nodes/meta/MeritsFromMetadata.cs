using Godot;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Cofd.Ctl.Nodes;

public partial class MeritsFromMetadata : Container
{
	private sealed class NodePaths
	{
		public static readonly NodePath MeritsName = new("%ExistingMerits");
	}
	
	[Signal]
	public delegate void AddMeritEventHandler(string name);
	
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		metadataManager.MetadataLoaded += refreshMerits;
		metadataManager.MetadataSaved += refreshMerits;
		
		GetNode<OptionButton>(NodePaths.MeritsName).ItemSelected += meritSelected;
		
		refreshMerits();
	}
	
	private void refreshMerits()
	{
		if(metadataManager.Container is CofdChangelingContainer ccc)
		{
			var optionButton = GetNode<OptionButton>(NodePaths.MeritsName);
			optionButton.Clear();
			optionButton.AddItem(string.Empty);
			ccc.Merits.ForEach(m => optionButton.AddItem(m.Name));
		}
	}
	
	private void meritSelected(long index)
	{
		if(index > 0)
		{
			var node = GetNode<OptionButton>(NodePaths.MeritsName);
			_ = EmitSignal(SignalName.AddMerit, node.GetItemText((int)index));
			node.Deselect();
		}
	}
}
