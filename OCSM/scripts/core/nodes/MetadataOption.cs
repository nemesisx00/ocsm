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
	
	public override void _ExitTree()
	{
		metadataManager.MetadataLoaded -= RefreshMetadata;
		metadataManager.MetadataSaved -= RefreshMetadata;
		
		base._ExitTree();
	}
	
	public override void _Ready()
	{
		metadataManager = GetNode<MetadataManager>(MetadataManager.NodePath);
		metadataManager.MetadataLoaded += RefreshMetadata;
		metadataManager.MetadataSaved += RefreshMetadata;
		
		RefreshMetadata();
	}
	
	public void RefreshMetadata()
	{
		if(metadataManager.Container is BaseContainer container)
		{
			replaceItems(container.Metadata
				.Where(m => m.Type == MetadataType)
				.Select(m => m.Name)
				.ToList());
		}
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
}
