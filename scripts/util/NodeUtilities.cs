using Godot;
using OCSM.Nodes.Meta;

namespace OCSM
{
	public class NodeUtilities
	{
		public static void autoSize(TextEdit node, int absoluteMinimumHeight = 0)
		{
			var lineHeight = node.GetLineHeight();
			var offset = absoluteMinimumHeight - lineHeight;
			if(offset < 0)
				offset = 0;
			
			var minY = (lineHeight * NodeUtilities.getLineCount(node)) + offset;
			node.RectMinSize = new Vector2(node.RectMinSize.x, minY);
		}
		
		public static void autoSizeChildren(Control node, int absoluteMinimumHeight = 0)
		{
			foreach(var c in node.GetChildren())
			{
				if(c is TextEdit te)
					NodeUtilities.autoSize(te, absoluteMinimumHeight);
				
				if(c is Control cc)
					NodeUtilities.autoSizeChildren(cc, absoluteMinimumHeight);
			}
		}
		
		public static void centerControl(Control control, Vector2 center)
		{
			control.RectPosition = new Vector2(center.x - (control.RectSize.x / 2), center.y - (control.RectSize.y / 2));
		}
		
		public static int getLineCount(TextEdit node)
		{
			var lines = node.GetLineCount();
			for(var i = 0; i < node.GetLineCount(); i++)
			{
				lines += node.GetLineWrapCount(i);
			}
			
			if(lines < 1)
				lines = 1;
			
			return lines;
		}
		
		public static void displayDeleteConfirmation(string label, Node parent, Vector2 center, Node handler, string doDelete)
		{
			
			var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.Meta.ConfirmDeleteEntry);
			var instance = resource.Instance<ConfirmDeleteEntry>();
			instance.EntryTypeName = label;
			parent.AddChild(instance);
			NodeUtilities.centerControl(instance, center);
			instance.Connect(Constants.Signal.Confirmed, handler, doDelete);
			instance.Popup_();
		}
	}
}