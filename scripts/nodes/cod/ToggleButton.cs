using Godot;

public class ToggleButton : TextureRect
{
	[Signal]
	public delegate void StateToggled(ToggleButton circle);
	
	public bool CurrentState { get; set; } = false;
	public string ToggledTexturePath { get; set; }
	
	public override void _Ready()
	{
		updateTexture();
		
		var button = GetChild<TextureButton>(0);
		button.Connect(Constants.Signal.GuiInput, this, nameof(handleClick));
		button.MouseDefaultCursorShape = CursorShape.PointingHand;
	}
	
	public void toggleState()
	{
		CurrentState = !CurrentState;
		updateTexture();
		EmitSignal(nameof(StateToggled), this);
	}
	
	public void updateTexture()
	{
		var tex = Constants.Texture.FullTransparent;
		if(CurrentState)
			tex = ToggledTexturePath;
		GetChild<TextureButton>(0).TextureNormal = GD.Load<StreamTexture>(tex);
	}
	
	private void handleClick(InputEvent e)
	{
		if(e is InputEventMouseButton buttonEvent && buttonEvent.Pressed && ButtonList.Left == (ButtonList)buttonEvent.ButtonIndex)
			toggleState();
	}
}
