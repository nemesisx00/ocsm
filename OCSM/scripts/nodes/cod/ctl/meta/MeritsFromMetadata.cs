using Godot;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes.Cofd.Ctl.Meta;

public partial class MeritsFromMetadata : Container
{
	private static class NodePaths
	{
		public static readonly NodePath MeritsName = new("%ExistingMerits");
	}
	
	[Signal]
	public delegate void AddMeritEventHandler(string name);
	
	private MetadataManager metadataManager;
	private OptionButton optionButton;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		metadataManager.MetadataLoaded += refreshMerits;
		metadataManager.MetadataSaved += refreshMerits;
		
		optionButton = GetNode<OptionButton>(NodePaths.MeritsName);
		optionButton.ItemSelected += meritSelected;
		
		refreshMerits();
	}
	
	private void refreshMerits()
	{
		if(metadataManager.Container is CofdChangelingContainer container)
		{
			optionButton.Clear();
			optionButton.AddItem(string.Empty);
			container.Merits.ForEach(m => optionButton.AddItem(m.Name));
		}
	}
	
	private void meritSelected(long index)
	{
		if(index > 0)
		{
			EmitSignal(SignalName.AddMerit, optionButton.GetItemText((int)index));
			optionButton.Deselect();
		}
	}
}
