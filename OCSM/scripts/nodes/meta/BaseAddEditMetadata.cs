using Godot;

namespace Ocsm.Nodes.Meta;

public abstract partial class BaseAddEditMetadata : Window
{
	[Signal]
	public delegate void MetadataChangedEventHandler();
}
