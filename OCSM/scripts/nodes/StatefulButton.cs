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
	
	[Export]
	public bool TwoState { get; set; }
	
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
		GetChild<TextureRect>(0).Texture = GD.Load<CompressedTexture2D>(UseCircles ? TexturePaths.CircleEmpty : TexturePaths.BoxBorder);
		
		var tex = State switch
		{
			States.One => UseCircles ? TexturePaths.CircleFillHalf : TexturePaths.SlashOne,
			States.Two => UseCircles ? TexturePaths.CircleFill : TexturePaths.SlashTwo,
			States.Three => UseCircles ? TexturePaths.CircleFillRed : TexturePaths.SlashThree,
			_ => TexturePaths.BoxTransparent,
		};
		
		TextureNormal = GD.Load<CompressedTexture2D>(tex);
	}
	
	private void handleClick(InputEvent e)
	{
		if(e is InputEventMouseButton buttonEvent && buttonEvent.Pressed)
		{
			var changed = false;
			switch(buttonEvent.ButtonIndex)
			{
				case MouseButton.Left:
					State = nextState(State);
					changed = true;
					break;
				
				case MouseButton.Right:
					State = nextState(State, true);
					changed = true;
					break;
			}
			
			if(changed)
			{
				UpdateTexture();
				_ = EmitSignal(SignalName.StateChanged, this);
			}
		}
	}
	
	private States nextState(States state, bool reverse = false)
	{
		if(reverse)
		{
			return state switch
			{
				States.Three => States.Two,
				States.Two => States.One,
				States.None => TwoState ? States.Two : States.Three,
				_ => States.None,
			};
		}
		else
		{
			return state switch
			{
				States.None => States.One,
				States.One => States.Two,
				States.Two => TwoState ? States.None : States.Three,
				_ => States.None,
			};
		}
	}
}
