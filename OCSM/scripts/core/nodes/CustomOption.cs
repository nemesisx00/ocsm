using Godot;
using System.Collections.Generic;
using Ocsm.Nodes.Autoload;

namespace Ocsm.Nodes;

public abstract partial class CustomOption : OptionButton
{
	[Signal]
	public delegate void ItemsChangedEventHandler();
	
	[Export]
	public bool EmptyOption { get; protected set; }
	
	protected MetadataManager metadataManager;
	
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
			if(GetItemText(i).Equals(text))
			{
				Selected = i;
				break;
			}
		}
	}
	
	protected void replaceItems(List<string> items)
	{
		var index = GetSelectedId();
		Clear();
		if(EmptyOption)
			AddItem(string.Empty);
		items.ForEach(i => AddItem(i));
		Selected = index;
	}
	
	protected virtual void refreshMetadata() { }
}
