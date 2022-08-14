using System;

public class Constants
{
	public sealed class NodePath
	{
		public const string AppManager = "/root/AppManager";
	}
	
	public sealed class Scene
	{
		public const string ConfirmQuit = "res://scenes/ConfirmQuit.tscn";
		
		public sealed class CoD
		{
			public const string ThreeStateBox = "res://scenes/cod/nodes/ThreeStateBox.tscn";
			public const string MortalSheet = "res://scenes/cod/sheets/Mortal.tscn";
		}
	}
	
	public sealed class Signal
	{
		public const string Confirmed = "confirmed";
		public const string GuiInput = "gui_input";
		public const string IdPressed = "id_pressed";
		public const string Pressed = "pressed";
		public const string Released = "released";
	}
	
	public sealed class Texture
	{
		public const string TrackState0 = "res://assets/textures/cod/TrackState0.png";
		public const string TrackState1 = "res://assets/textures/cod/TrackState1.png";
		public const string TrackState2 = "res://assets/textures/cod/TrackState2.png";
		public const string TrackState3 = "res://assets/textures/cod/TrackState3.png";
	}
}
