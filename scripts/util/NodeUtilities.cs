using Godot;
using OCSM.Nodes;
using OCSM.Nodes.Meta;

namespace OCSM
{
	/// <summary>
	/// A collection of utility methods for creating or modifying one or more
	/// <c>Godot.Node</c>s.
	/// </summary>
	public class NodeUtilities
	{
		/// <summary>
		/// Automatically resize a <c>Godot.TextEdit</c> node according to its
		/// current content.
		/// </summary>
		/// <param name="node">The node to be resized.</param>
		/// <param name="absoluteMinimumHeight">The minimum allowed height for the node.</param>
		public static void autoSize(TextEdit node, int absoluteMinimumHeight = 0)
		{
			var lineHeight = node.GetLineHeight();
			var offset = absoluteMinimumHeight - lineHeight;
			if(offset < 0)
				offset = 0;
			
			var minY = (lineHeight * NodeUtilities.getLineCount(node)) + offset;
			node.CustomMinimumSize = new Vector2(node.CustomMinimumSize.X, minY);
		}
		
		/// <summary>
		/// Automatically resize all <c>Godot.TextEdit</c> nodes which are children
		/// of the given <c>node</c>.
		/// </summary>
		/// <remarks>
		/// Recurses through the children of every <c>Godot.Control</c> node.
		/// </remarks>
		/// <param name="node">The node whose child nodes will be resized.</param>
		/// <param name="absoluteMinimumHeight">The minimum allowed height for the nodes.</param>
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
		
		/// <summary>
		/// Reposition a <c>Godot.Control</c> to the center of the viewport,
		/// based on the given <c>center</c> coordinates and the current <c>Size</c>
		///  of the given <c>control</c>.
		/// </summary>
		/// <param name="control">The control being repositioned.</param>
		/// <param name="center">
		/// The <c>Godot.Vector2</c> defining the coordinates of the center to which
		/// <c>control</c> is being repositioned.
		/// </param>
		public static void centerControl(Control control, Vector2 center)
		{
			control.Position = new Vector2(center.X - (control.Size.X / 2), center.Y - (control.Size.Y / 2));
		}
		
		/// <summary>
		/// Create a new <c>Godot.Label</c> instance with its <c>Align</c> and
		/// <c>VAlign</c> properties set to <c>Center</c>.
		/// </summary>
		/// <param name="text">The <c>string</c> to set as the instance's <c>Text</c> property.</param>
		/// <returns>An instance of <c>Godot.Label</c>.</returns>
		public static Label createCenteredLabel(string text)
		{
			return new Label() { Text = text, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, };
		}
		
		/// <summary>
		/// Calculate the total number actual lines, compensating for line wrapping,
		/// contained in the <c>Godot.TextEdit</c>'s content.
		/// </summary>
		/// <param name="node">The node whose content is being measured.</param>
		/// <returns>The total line count as an integer.</returns>
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
		
		/// <summary>
		/// Instantiate and display the <c>OCSM.Nodes.Meta.ConfirmDeleteEntry</c> node.
		/// </summary>
		/// <param name="label">The text denoting what is being deleted.</param>
		/// <param name="parent">The <c>Godot.Node</c> to which to add the instance.</param>
		/// <param name="center">The <c>Godot.Vector2</c> used to center the instance.</param>
		/// <param name="handler">
		/// The <c>Godot.Node</c> which will be handling the Confirmed signal
		/// emitted by the instance.
		/// </param>
		/// <param name="doDelete">
		/// The name of the method to call when handling the Confirmed signal
		/// emitted by the instance.
		/// </param>
		public static void displayDeleteConfirmation(string label, Node parent, Vector2 center, ICanDelete handler, string doDelete)
		{
			var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.Meta.ConfirmDeleteEntry);
			var instance = resource.Instantiate<ConfirmDeleteEntry>();
			instance.EntryTypeName = label;
			parent.AddChild(instance);
			instance.Confirmed += handler.doDelete;
			instance.PopupCentered();
		}
	}
}
