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
	}
	
	public sealed class Signal
	{
		public const string Confirmed = "confirmed";
		public const string IdPressed = "id_pressed";
		public const string Pressed = "pressed";
		public const string Released = "released";
	}
}
