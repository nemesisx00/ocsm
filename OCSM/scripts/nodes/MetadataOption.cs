using System.Collections.Generic;
using System.Linq;
using Godot;
using Ocsm.Meta;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes;

public partial class MetadataOption : OptionButton
{
	[Signal]
	public delegate void ItemsChangedEventHandler();
	
	[Export]
	public bool EmptyOption { get; private set; } = false;
	
	[Export]
	public GameSystem GameSystem { get; set; }
	[Export]
	public MetadataType MetadataType { get; set; }
	
	private MetadataManager metadataManager;
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(Constants.NodePath.MetadataManager);
		metadataManager.MetadataLoaded += refreshMetadata;
		metadataManager.MetadataSaved += refreshMetadata;
		
		refreshMetadata();
	}
	
	public void Select(string text)
	{
		for(var i = 0; i < ItemCount; i++)
		{
			if(GetItemText(i) == text)
			{
				Selected = i;
				break;
			}
		}
	}
	
	private void replaceItems(List<string> items)
	{
		var index = Selected;
		Clear();
		if(EmptyOption)
			AddItem(string.Empty);
		items.ForEach(i => AddItem(i));
		Selected = index;
	}
	
	private void refreshMetadata()
	{
		if(metadataManager.Container is BaseContainer container)
		{
			replaceItems(container.Metadata
				.Where(m => m.Type == MetadataType)
				.Select(m => m.Name)
				.ToList());
		}
	}
}
