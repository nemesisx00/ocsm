using Godot;

namespace Ocsm.Nodes;

public abstract partial class DynamicLabel : Container
{
	private static class NodePaths
	{
		public static readonly NodePath Label = new ("%Label");
		public static readonly NodePath Panel = new("%Panel");
	}
	
	public bool EditMode { get; set; }
	
	protected Label label;
	protected Panel panel;
	
	public override void _GuiInput(InputEvent evt)
	{
		if(!EditMode)
		{
			if(evt.IsActionReleased(Actions.Click))
				ToggleEditMode();
		}
	}
	
	public override void _Ready()
	{
		FocusEntered += ToggleEditMode;
		MouseEntered += handleMouseEntered;
		MouseExited += handleMouseExited;
		
		label = GetNode<Label>(NodePaths.Label);
		panel = GetNode<Panel>(NodePaths.Panel);
		
		label.HorizontalAlignment = SizeFlagsHorizontal switch
		{
			SizeFlags.ShrinkCenter => HorizontalAlignment.Center,
			SizeFlags.ShrinkEnd => HorizontalAlignment.Right,
			SizeFlags.ExpandFill => HorizontalAlignment.Fill,
			_ => HorizontalAlignment.Left,
		};
	}
	
	public virtual void ToggleEditMode()
	{
		EditMode = !EditMode;
		
		if(EditMode)
		{
			label.Hide();
			panel.Hide();
		}
		else
			label.Show();
	}
	
	protected void handleMouseEntered()
	{
		if(!EditMode)
			panel.Show();
	}
	
	protected void handleMouseExited() => panel.Hide();
}
