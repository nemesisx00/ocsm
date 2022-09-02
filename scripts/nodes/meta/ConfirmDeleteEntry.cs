using Godot;
using System;

namespace OCSM.Nodes.Meta
{
	public class ConfirmDeleteEntry : ConfirmationDialog
	{
		private const string TextFormat = "Are you sure you want to delete this {0}?";
		
		public string EntryTypeName { get; set; } = "Metadata Entry";
		
		public override void _Ready()
		{
			DialogText = String.Format(TextFormat, EntryTypeName);
			
			Connect(Constants.Signal.Confirmed, this, nameof(close));
			GetCancel().Connect(Constants.Signal.Pressed, this, nameof(close));
			GetCloseButton().Connect(Constants.Signal.Pressed, this, nameof(close));
		}
		
		private void close()
		{
			QueueFree();
		}
	}
}
