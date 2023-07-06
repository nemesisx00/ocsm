using Godot;

namespace Ocsm
{
	/// <summary>
	/// Generic container for transferring an object which does not extend
	/// Godot.Object via Godot's Signal system.
	/// </summary>
	/// <typeparam name="T">Any type that extends <c>System.Object</c>.</typeparam>
	public partial class Transport<T> : GodotObject
	{
		public T Value { get; set; }
		
		/// <summary>Constructor</summary>
		/// <param name="value">The object to be transported.</param>
		public Transport(T value)
		{
			Value = value;
		}
	}
}
