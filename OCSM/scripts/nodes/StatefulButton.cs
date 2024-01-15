using Godot;

namespace Ocsm.Nodes;

public partial class StatefulButton : TextureButton
{
	public enum States
	{
		None,
		One,
		Two,
		Three,
	}
	
	[Export(PropertyHint.Enum, "None,One,Two,Three")]
	public States State { get; set; }
	[Export]
	public bool UseCircles { get; set; }
	
	[Signal]
	public delegate void StateChangedEventHandler(StatefulButton box);
	
	public override void _Ready()
	{
		UpdateTexture();
		GuiInput += handleClick;
	}
	
	public void NextState(bool reverse = false) => nextState(State, reverse);
	
	public void UpdateTexture()
	{
		GetChild<TextureRect>(0).Texture = GD.Load<CompressedTexture2D>(UseCircles ? Constants.Texture.TrackCircle : Constants.Texture.TrackBoxBorder);
		
		var tex = State switch
		{
			States.One => UseCircles ? Constants.Texture.TrackCircleHalf : Constants.Texture.TrackBox1,
			States.Two => UseCircles ? Constants.Texture.TrackCircleFill : Constants.Texture.TrackBox2,
			States.Three => UseCircles ? Constants.Texture.TrackCircleRed : Constants.Texture.TrackBox3,
			_ => Constants.Texture.FullTransparent,
		};
		
		TextureNormal = GD.Load<CompressedTexture2D>(tex);
	}
	
	private void handleClick(InputEvent e)
	{
		if(e is InputEventMouseButton buttonEvent && buttonEvent.Pressed)
		{
			switch(buttonEvent.ButtonIndex)
			{
				case MouseButton.Left:
					State = nextState(State);
					UpdateTexture();
					_ = EmitSignal(SignalName.StateChanged, this);
					break;
				
				case MouseButton.Right:
					State = nextState(State, true);
					UpdateTexture();
					_ = EmitSignal(SignalName.StateChanged, this);
					break;
			}
		}
	}
	
	private static States nextState(States state, bool reverse = false)
	{
		if(reverse)
		{
			return state switch
			{
				States.Two => States.One,
				States.Three => States.Two,
				States.None => States.Three,
				_ => States.None,
			};
		}
		else
		{
			return state switch
			{
				States.None => States.One,
				States.One => States.Two,
				States.Two => States.Three,
				_ => States.None,
			};
		}
	}
}
