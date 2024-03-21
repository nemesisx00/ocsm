using Godot;
using System.Collections.Generic;
using Ocsm.Nodes;
using Ocsm.Nodes.Meta;

namespace Ocsm;

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
	/// Instantiate and display the <c>Ocsm.Nodes.Meta.ConfirmDeleteEntry</c> node.
	/// </summary>
	/// <param name="label">The text denoting what is being deleted.</param>
	/// <param name="parent">The <c>Godot.Node</c> to which to add the instance.</param>
	/// <param name="handler">
	/// The <c>Godot.Node</c> which will be handling the Confirmed signal
	/// emitted by the instance.
	/// </param>
	public static void DisplayDeleteConfirmation(string label, Node parent, ICanDelete handler)
	{
		var resource = GD.Load<PackedScene>(Constants.Scene.Meta.ConfirmDeleteEntry);
		var instance = resource.Instantiate<ConfirmDeleteEntry>();
		instance.EntryTypeName = label;
		parent.AddChild(instance);
		instance.Confirmed += handler.DoDelete;
		instance.PopupCentered();
	}
	
	/// <summary>
	/// Queue a <c>Godot.Node</c> to be freed and then null out the reference to it.
	/// </summary>
	/// <param name="node">The node to be freed.</param>
	public static void queueFree<T>(ref T node)
		where T: Node
	{
		if(node is T)
		{
			node.QueueFree();
			node = null;
		}
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
	/// Move the child nodes of a given parent <c>Godot.Node</c> to match the
	/// order as represented by the list <c>orderedChildren</c>.
	/// </summary>
	/// <param name="parent">The node containing the child nodes to be rearranged.</param>
	/// <param name="orderedChildren">The list of nodes to be rearranged.</param>
	public static void rearrangeNodes(Node parent, List<Node> orderedChildren)
	{
		if(parent is Node && orderedChildren is List<Node>)
		{
			var i = 0;
			orderedChildren.ForEach(c => parent.MoveChild(c, i++));
		}
	}
}
