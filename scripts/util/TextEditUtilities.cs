using Godot;

namespace OCSM
{
	public class TextEditUtilities
	{
		public static void autoSize(TextEdit node, int absoluteMinimumHeight = 0)
		{
			var lineHeight = node.GetLineHeight();
			var offset = absoluteMinimumHeight - lineHeight;
			if(offset < 0)
				offset = 0;
			
			var minY = (lineHeight * TextEditUtilities.getLineCount(node)) + offset;
			node.RectMinSize = new Vector2(node.RectMinSize.x, minY);
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
	}
}