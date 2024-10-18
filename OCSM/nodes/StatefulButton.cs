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
	
	private States nextState(States state, bool reverse = false) => state switch
	{
		States.One => reverse ? States.None : States.Two,
		States.Two => reverse ? States.One : TwoState ? States.None : States.Three,
		States.Three => reverse ? States.Two : States.None,
		States.None => reverse ? TwoState ? States.Two : States.Three : States.One,
		_ => States.None,
	};
	
	[Export]
	public States CurrentState { get; set; }
	[Export]
	public bool UseCircles { get; set; }
	[Export]
	public bool TwoState { get; set; }
	
	[Signal]
	public delegate void StateChangedEventHandler(StatefulButton box);
	
	public override void _Ready()
	{
		UpdateTexture();
		GuiInput += handleClick;
	}
	
	public void NextState(bool reverse = false) => nextState(CurrentState, reverse);
	
	public void UpdateTexture()
	{
		if(UseCircles)
			GetChild<TextureRect>(0).Texture = GD.Load<CompressedTexture2D>(TexturePaths.TrackCircle);
		else
			GetChild<TextureRect>(0).Texture = GD.Load<CompressedTexture2D>(TexturePaths.TrackBoxBorder);
		
		var tex = TexturePaths.FullTransparent;
		switch(CurrentState)
		{
			case States.One:
				if(UseCircles)
					tex = TexturePaths.TrackCircleHalf;
				else
					tex = TexturePaths.TrackBox1;
				break;
			
			case States.Two:
				if(UseCircles)
					tex = TexturePaths.TrackCircleFill;
				else
					tex = TexturePaths.TrackBox2;
				break;
			
			case States.Three:
				if(UseCircles)
					tex = TexturePaths.TrackCircleRed;
				else
					tex = TexturePaths.TrackBox3;
				break;
		}
		
		TextureNormal = GD.Load<CompressedTexture2D>(tex);
	}
	
	private void handleClick(InputEvent e)
	{
		if(e is InputEventMouseButton buttonEvent && buttonEvent.Pressed)
		{
			CurrentState = nextState(CurrentState, buttonEvent.ButtonIndex == MouseButton.Right);
			UpdateTexture();
			EmitSignal(SignalName.StateChanged, this);
		}
	}
}
