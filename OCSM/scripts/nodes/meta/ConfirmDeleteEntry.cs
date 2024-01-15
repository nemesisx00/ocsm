using Godot;

namespace Ocsm.Nodes;

public partial class ConfirmDeleteEntry : ConfirmationDialog
{
	private const string TextFormat = "Are you sure you want to delete this {0}?";
	
	public string EntryTypeName { get; set; } = "Metadata Entry";
	
	public override void _Ready()
	{
		DialogText = string.Format(TextFormat, EntryTypeName);
		
		Confirmed += QueueFree;
		Canceled += QueueFree;
	}
}
