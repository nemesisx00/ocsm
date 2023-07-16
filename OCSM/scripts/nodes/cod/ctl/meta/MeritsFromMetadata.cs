using Godot;
using System;
using Ocsm.Cofd.Ctl.Meta;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes.Cofd.Ctl.Meta
{
	public partial class MeritsFromMetadata : Container
	{
		private sealed class NodePath
		{
			public const string MeritsName = "%ExistingMerits";
		}
		
		[Signal]
		public delegate void AddMeritEventHandler(string name);
		
		
		private MetadataManager metadataManager;
		
		public override void _Ready()
		{
			metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
			metadataManager.MetadataLoaded += refreshMerits;
			metadataManager.MetadataSaved += refreshMerits;
			
			GetNode<OptionButton>(NodePath.MeritsName).ItemSelected += meritSelected;
			
			refreshMerits();
		}
		
		private void refreshMerits()
		{
			if(metadataManager.Container is CofdChangelingContainer ccc)
			{
				var optionButton = GetNode<OptionButton>(NodePath.MeritsName);
				optionButton.Clear();
				optionButton.AddItem(String.Empty);
				ccc.Merits.ForEach(m => optionButton.AddItem(m.Name));
			}
		}
		
		private void meritSelected(long index)
		{
			if(index > 0)
			{
				var node = GetNode<OptionButton>(NodePath.MeritsName);
				EmitSignal(nameof(AddMerit), node.GetItemText((int)index));
				node.Deselect();
			}
		}
	}
}
