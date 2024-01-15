using Godot;

namespace Ocsm.Nodes;

public partial class ToggleButton : TextureButton
{
	[Signal]
	public delegate void ToggleEventHandler(ToggleButton button);
	
	public Texture2D Inactive { get; set; }
	public Texture2D Active { get; set; }
	
	public bool State
	{
		get { return currentState; }
		
		set
		{
			currentState = value;
			TextureNormal = currentState ? Active : Inactive;
		}
	}
	
	private bool currentState;
	
	public override void _Ready()
	{
		MouseDefaultCursorShape = CursorShape.PointingHand;
		TextureNormal = Inactive;
		GuiInput += handleClick;
	}
	
	public void Disable()
	{
		if(!Disabled)
		{
			Disabled = true;
			GuiInput -= handleClick;
		}
	}
	
	public void Enable()
	{
		if(Disabled)
		{
			Disabled = false;
			GuiInput += handleClick;
		}
	}
	
	private void handleClick(InputEvent e)
	{
		if(!Disabled && e is InputEventMouseButton iemb && iemb.Pressed && MouseButton.Left == iemb.ButtonIndex)
		{
			State = !currentState;
			_ = EmitSignal(SignalName.Toggle, this);
		}
	}
}
