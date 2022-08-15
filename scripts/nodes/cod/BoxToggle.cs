using Godot;
using System;

public class BoxToggle : TextureButton
{
	public sealed class State
	{
		public const string None = "None";
		public const string One = "One";
		public const string Two = "Two";
		public const string Three = "Three";
		public const string EnumHint = None + "," + One + "," + Two + "," + Three;
	}
	
	[Export(PropertyHint.Enum, State.EnumHint)]
	public string CurrentState { get; set; } = State.None;
	[Export]
	private bool HandleMouseEvents { get; set; } = true;
	
	public override void _Ready()
	{
		updateTexture();
		
		if(HandleMouseEvents)
			Connect(Constants.Signal.GuiInput, this, nameof(handleClick));
	}
	
	public void nextState(bool reverse = false)
	{
		nextState(CurrentState, reverse);
	}
	
	public void updateTexture()
	{
		var tex = Constants.Texture.FullTransparent;
		switch(CurrentState)
		{
			case State.One:
				tex = Constants.Texture.TrackBox1;
				break;
			case State.Two:
				tex = Constants.Texture.TrackBox2;
				break;
			case State.Three:
				tex = Constants.Texture.TrackBox3;
				break;
			case State.None:
			default:
				break;
		}
		
		TextureNormal = GD.Load<StreamTexture>(tex);
	}
	
	private void handleClick(InputEvent e)
	{
		if(e is InputEventMouseButton buttonEvent && buttonEvent.Pressed)
		{
			switch((ButtonList)buttonEvent.ButtonIndex)
			{
				case ButtonList.Left:
					CurrentState = nextState(CurrentState);
					updateTexture();
					break;
				case ButtonList.Right:
					CurrentState = nextState(CurrentState, true);
					updateTexture();
					break;
				default:
					break;
			}
		}
	}
	
	private string nextState(string state, bool reverse = false)
	{
		if(reverse)
		{
			switch(state)
			{
				case State.Two:
					return State.One;
				case State.Three:
					return State.Two;
				case State.None:
					return State.Three;
				case State.One:
				default:
					return State.None;
			}
		}
		else
		{
			switch(state)
			{
				case State.None:
					return State.One;
				case State.One:
					return State.Two;
				case State.Two:
					return State.Three;
				case State.Three:
				default:
					return State.None;
			}
		}
	}
}
