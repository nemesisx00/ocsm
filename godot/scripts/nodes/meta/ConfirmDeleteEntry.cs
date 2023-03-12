using Godot;
using System;

namespace OCSM.Nodes.Meta
{
	public partial class ConfirmDeleteEntry : ConfirmationDialog
	{
		private const string TextFormat = "Are you sure you want to delete this {0}?";
		
		public string EntryTypeName { get; set; } = "Metadata Entry";
		
		public override void _Ready()
		{
			DialogText = String.Format(TextFormat, EntryTypeName);
			
			Confirmed += close;
			Canceled += close;
		}
		
		private void close()
		{
			QueueFree();
		}
	}
}
