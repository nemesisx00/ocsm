
namespace OCSM
{
	public class Transport<T> : Godot.Object
	{
		public T Value { get; set; }
		
		public Transport(T value)
		{
			Value = value;
		}
	}
}
