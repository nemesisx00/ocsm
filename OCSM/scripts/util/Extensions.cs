using System;
using System.Reflection;
using Godot;

namespace OCSM
{
	public static class Extensions
	{
		// Enum extensions
		
		public static string GetLabel(this Enum value)
		{
			return value.GetType()
				.GetField(value.ToString())
				.GetCustomAttribute<LabelAttribute>(false)?
				.Label ?? String.Empty;
		}
		
		public static string GetLabelOrName(this Enum value)
		{
			var ret = value.GetLabel();
			if(String.IsNullOrEmpty(ret))
				ret = value.ToString();
			return ret;
		}
		
		// --------------------------------------------------
		
		// OptionButton extensions
		
		public static void Deselect(this OptionButton node)
		{
			node.Selected = -1;
		}
		
		public static void SetDisabledAll(this OptionButton node, bool disabled = false)
		{
			var count = node.ItemCount;
			for(var i = 0; i < count; i++)
			{
				node.SetItemDisabled(i, false);
			}
		}
		
		public static void SetDisabledByText(this OptionButton node, string text, bool disabled = false)
		{
			node.SetItemDisabled(node.GetFirstItemIndexByText(text), disabled);
		}
		
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
			var ret = String.Empty;
			var index = node.Selected;
			if(index >= 0 && index < node.ItemCount)
				ret = node.GetItemText(index);
			return ret;
		}
		
		public static void SelectItemByText(this OptionButton node, string text)
		{
			node.Selected = node.GetFirstItemIndexByText(text);
		}
	}
}
