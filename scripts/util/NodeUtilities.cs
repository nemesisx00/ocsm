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
			var resource = GD.Load<PackedScene>(Constants.Scene.Meta.ConfirmDeleteEntry);
			var instance = resource.Instantiate<ConfirmDeleteEntry>();
			instance.EntryTypeName = label;
			parent.AddChild(instance);
			instance.Confirmed += handler.doDelete;
			instance.PopupCentered();
		}
	}
}
