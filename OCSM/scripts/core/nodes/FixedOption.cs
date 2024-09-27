using Godot;
using System.Collections.Generic;

namespace Ocsm.Nodes;

public abstract partial class FixedOption : OptionButton
{
	[Signal]
	public delegate void ItemsChangedEventHandler();
	
	[Export]
	public bool EmptyOption { get; set; }
	
	public override void _Ready() => refreshMetadata();
	
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
	
	protected void replaceItems(IEnumerable<string> items)
	{
		var index = GetSelectedId();
		Clear();
		
		if(EmptyOption)
			AddItem(string.Empty);
		
		foreach(var i in items)
			AddItem(i);
		
		Selected = index;
	}
	
	protected virtual void refreshMetadata() { }
}
