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
			public const string TwoStateCircle = "res://scenes/cod/nodes/TwoStateCircle.tscn";
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
		public const string FullTransparent = "res://assets/textures/cod/FullTransparent.png";
		public const string TrackBox1 = "res://assets/textures/cod/TrackBox1.png";
		public const string TrackBox2 = "res://assets/textures/cod/TrackBox2.png";
		public const string TrackBox3 = "res://assets/textures/cod/TrackBox3.png";
		public const string TrackCircle1 = "res://assets/textures/cod/TrackCircleFill.png";
	}
}
