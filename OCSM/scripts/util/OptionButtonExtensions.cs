using System.Collections.Generic;
using Godot;

namespace Ocsm;

public static class OptionButtonExtensions
{
	public static void Deselect(this OptionButton node) => node.Selected = -1;
	
	public static void SetDisabled(this OptionButton node, List<string> strings, bool disabled = false, List<string> exclusions = null)
	{
		var skip = new List<string>();
		
		if(exclusions is not null)
			skip.AddRange(exclusions);
		
		node.SetDisabledAll(false);
		
		foreach(var s in strings)
			node.SetDisabledByText(s, disabled);
		
		foreach(var s in skip)
			node.SetDisabledByText(s, !disabled);
	}
	
	public static void SetDisabledAll(this OptionButton node, bool disabled = false)
	{
		var count = node.ItemCount;
		for(var i = 0; i < count; i++)
			node.SetItemDisabled(i, disabled);
	}
	
	public static void SetDisabledByText(this OptionButton node, string text, bool disabled = false)
		=> node.SetItemDisabled(node.GetFirstItemIndexByText(text), disabled);
	
	public static int GetFirstItemIndexByText(this OptionButton node, string text)
	{
		var index = -1;
		var count = node.ItemCount;
		
		for(var i = 0; i < count; i++)
		{
			if(node.GetItemText(i).Equals(text))
			{
				index = i;
				break;
			}
		}
		
		return index;
	}
	
	public static string GetSelectedItemText(this OptionButton node)
	{
		var ret = string.Empty;
		var index = node.Selected;
		
		if(index >= 0 && index < node.ItemCount)
			ret = node.GetItemText(index);
		
		return ret;
	}
	
	public static void SelectItemByText(this OptionButton node, string text)
		=> node.Selected = node.GetFirstItemIndexByText(text);
}
