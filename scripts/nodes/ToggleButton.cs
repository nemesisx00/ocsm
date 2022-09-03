using Godot;
using System;

namespace OCSM.Nodes
{
	public class ToggleButton : TextureButton
	{
		[Signal]
		public delegate void StateToggled(ToggleButton button);
		
		[Export]
		public bool CurrentState { get; set; } = false;
		[Export]
		public bool UseCircles { get; set; } = false;
		public StreamTexture ToggledTexture { get; set; }
		public StreamTexture EmptyTexture { get; set; }
		
		public override void _Ready()
		{
			if(UseCircles)
			{
				GetChild<TextureRect>(0).Texture = ResourceLoader.Load<StreamTexture>(Constants.Texture.TrackCircle);
				ToggledTexture = ResourceLoader.Load<StreamTexture>(Constants.Texture.TrackCircleFill);
			}
			else
			{
				GetChild<TextureRect>(0).Texture = ResourceLoader.Load<StreamTexture>(Constants.Texture.TrackBoxBorder);
				ToggledTexture = ResourceLoader.Load<StreamTexture>(Constants.Texture.TrackBox2);
			}
			
			EmptyTexture = ResourceLoader.Load<StreamTexture>(Constants.Texture.FullTransparent);
			
			Connect(Constants.Signal.GuiInput, this, nameof(handleClick));
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
			if(e is InputEventMouseButton buttonEvent && buttonEvent.Pressed && ButtonList.Left == (ButtonList)buttonEvent.ButtonIndex)
				toggleState();
		}
	}
}
