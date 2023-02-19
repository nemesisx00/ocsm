using Godot;

namespace OCSM.Nodes
{
	public partial class StatefulButton : TextureButton
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
		public bool UseCircles { get; set; } = false;
		
		[Signal]
		public delegate void StateChangedEventHandler(StatefulButton box);
		
		public override void _Ready()
		{
			updateTexture();
			GuiInput += handleClick;
		}
		
		public void nextState(bool reverse = false)
		{
			nextState(CurrentState, reverse);
		}
		
		public void updateTexture()
		{
			if(UseCircles)
				GetChild<TextureRect>(0).Texture = ResourceLoader.Load<CompressedTexture2D>(Constants.Texture.TrackCircle);
			else
				GetChild<TextureRect>(0).Texture = ResourceLoader.Load<CompressedTexture2D>(Constants.Texture.TrackBoxBorder);
			
			var tex = Constants.Texture.FullTransparent;
			switch(CurrentState)
			{
				case State.One:
					if(UseCircles)
						tex = Constants.Texture.TrackCircleHalf;
					else
						tex = Constants.Texture.TrackBox1;
					break;
				case State.Two:
					if(UseCircles)
						tex = Constants.Texture.TrackCircleFill;
					else
						tex = Constants.Texture.TrackBox2;
					break;
				case State.Three:
					if(UseCircles)
						tex = Constants.Texture.TrackCircleRed;
					else
						tex = Constants.Texture.TrackBox3;
					break;
				case State.None:
				default:
					break;
			}
			
			TextureNormal = ResourceLoader.Load<CompressedTexture2D>(tex);
		}
		
		private void handleClick(InputEvent e)
		{
			if(e is InputEventMouseButton buttonEvent && buttonEvent.Pressed)
			{
				switch(buttonEvent.ButtonIndex)
				{
					case MouseButton.Left:
						CurrentState = nextState(CurrentState);
						updateTexture();
						EmitSignal(nameof(StateChanged), this);
						break;
					case MouseButton.Right:
						CurrentState = nextState(CurrentState, true);
						updateTexture();
						EmitSignal(nameof(StateChanged), this);
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
}
