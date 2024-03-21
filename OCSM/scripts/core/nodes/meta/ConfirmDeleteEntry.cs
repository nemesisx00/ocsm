using Godot;

namespace Ocsm.Nodes.Meta;

public partial class ConfirmDeleteEntry : ConfirmationDialog
{
	public string EntryTypeName { get; set; } = "Metadata Entry";
	
	public override void _Ready()
	{
		DialogText = $"Are you sure you want to delete this {EntryTypeName}?";
		
		Confirmed += QueueFree;
		Canceled += QueueFree;
	}
}
