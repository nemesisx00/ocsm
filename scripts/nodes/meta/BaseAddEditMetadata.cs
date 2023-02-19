using Godot;

namespace OCSM.Nodes.Meta
{
	public abstract partial class BaseAddEditMetadata : Window
	{
		[Signal]
		public delegate void MetadataChangedEventHandler();
	}
}
