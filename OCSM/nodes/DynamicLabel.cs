using Godot;

namespace Ocsm.Nodes;

public abstract partial class DynamicLabel : Label
{
	private static class NodePaths
	{
		public static readonly NodePath Panel = new("%Panel");
	}
	
	public bool EditMode { get; set; }
	
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
		MouseEntered += handleMouseEntered;
		MouseExited += handleMouseExited;
		
		panel = GetNode<Panel>(NodePaths.Panel);
	}
	
	public virtual void ToggleEditMode()
	{
		EditMode = !EditMode;
		
		if(EditMode)
			panel.Hide();
	}
	
	protected void handleMouseEntered()
	{
		if(!EditMode)
			panel.Show();
	}
	
	protected void handleMouseExited() => panel.Hide();
}
