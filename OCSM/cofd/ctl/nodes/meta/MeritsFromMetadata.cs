using Godot;
using System.Linq;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Nodes.Autoload;
using Ocsm.Meta;

namespace Ocsm.Cofd.Ctl.Nodes.Meta;

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
	
	public override void _ExitTree()
	{
		metadataManager.MetadataLoaded -= refreshMerits;
		metadataManager.MetadataSaved -= refreshMerits;
		
		base._ExitTree();
	}
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
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
			
			foreach(var merit in container.Merits)
				optionButton.AddItem(merit.Name);
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
