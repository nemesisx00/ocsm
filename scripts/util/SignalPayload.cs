using Godot;

namespace OCSM
{
	public class SignalPayload<T> : Godot.Object
	{
		public T Payload { get; set; }
	}
}
