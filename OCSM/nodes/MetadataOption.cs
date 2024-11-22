using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using Ocsm.Meta;

namespace Ocsm.Nodes;

public partial class MetadataOption : DynamicOption
{
	[Export]
	public Array<string> MetadataTypes { get; set; } = [];
	
	public Metadata SelectedMetadata
	{
		get => metadataManager.Container is BaseContainer container
			? container.Metadata.Find(
				m => MetadataTypes.Where(mt => m.Types.Contains(mt)).Any()
					&& Selected > -1
					&& m.Name == GetItemText(Selected)
			)
			: null;
		
		set
		{
			if(value is not null)
			{
				for(var i = 0; i < ItemCount; i++)
				{
					if(GetItemText(i) == value.Name)
					{
						Selected = i;
						break;
					}
				}
			}
			else
				Selected = 0;
		}
	}
	
	public void RefreshMetadata() => refreshMetadata();
	
	protected override void refreshMetadata()
	{
		if(metadataManager.Container is BaseContainer container)
		{
			replaceItems(container.Metadata
				.Where(m => MetadataTypes.Where(mt => m.Types.Contains(mt)).Any())
				.Select(m => m.Name)
				.ToList());
		}
	}
	
	private void replaceItems(List<string> items)
	{
		var index = Selected;
		Clear();
		
		if(EmptyOption)
			AddItem(string.Empty);
		
		foreach(var i in items)
			AddItem(i);
		
		Selected = index;
	}
}
