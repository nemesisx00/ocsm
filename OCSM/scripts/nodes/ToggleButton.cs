using Godot;
using System;

namespace Ocsm.Nodes;

public partial class ToggleButton : TextureButton
{
	[Signal]
	public delegate void StateToggledEventHandler(ToggleButton button);
	
	[Export]
	public bool CurrentState { get; set; } = false;
	[Export]
	public bool UseCircles { get; set; } = false;
	public CompressedTexture2D ToggledTexture { get; set; }
	public CompressedTexture2D EmptyTexture { get; set; }
	
	public override void _Ready()
	{
		if(UseCircles)
		{
			GetChild<TextureRect>(0).Texture = GD.Load<CompressedTexture2D>(Constants.Texture.TrackCircle);
			ToggledTexture = GD.Load<CompressedTexture2D>(Constants.Texture.TrackCircleFill);
		}
		else
		{
			GetChild<TextureRect>(0).Texture = GD.Load<CompressedTexture2D>(Constants.Texture.TrackBoxBorder);
			ToggledTexture = GD.Load<CompressedTexture2D>(Constants.Texture.TrackBox2);
		}
		
		EmptyTexture = GD.Load<CompressedTexture2D>(Constants.Texture.FullTransparent);
		
		GuiInput += handleClick;
		MouseDefaultCursorShape = CursorShape.PointingHand;
		
		updateTexture();
	}
	
	public void toggleState()
	{
		CurrentState = !CurrentState;
		updateTexture();
		EmitSignal(nameof(StateToggled), this);
	}
	
	public void updateTexture()
	{
		if(CurrentState)
			TextureNormal = ToggledTexture;
		else
			TextureNormal = EmptyTexture;
	}
	
	private void handleClick(InputEvent e)
	{
		if(e is InputEventMouseButton buttonEvent && buttonEvent.Pressed && MouseButton.Left == buttonEvent.ButtonIndex)
			toggleState();
	}
}
