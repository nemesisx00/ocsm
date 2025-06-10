using Godot;
using Godot.Collections;
using Ocsm.Meta;

namespace Ocsm.Nodes;

public partial class DynamicMetadataLabel : DynamicLabel
{
	private static class NodePaths
	{
		public static readonly NodePath Label = new("%Label");
		public static readonly NodePath Option = new("%MetadataOption");
	}
	
	[Signal]
	public delegate void ItemSelectedEventHandler(long index);
	
	[Export]
	public HorizontalAlignment Alignment
	{
		get => alignment;
		
		set
		{
			alignment = value;
			
			if(option is not null)
				option.Alignment = alignment;
		}
	}
	
	[Export]
	public bool EmptyOption { get; set; }
	
	[Export]
	public Array<string> MetadataTypes { get; set; }
	
	public Metadata Value
	{
		get => option.SelectedMetadata;
		set => option.SelectedMetadata = value;
	}
	
	private HorizontalAlignment alignment;
	private MetadataOption option;
	
	public override void _Ready()
	{
		base._Ready();
		
		option = GetNode<MetadataOption>(NodePaths.Option);
		option.Alignment = Alignment;
		option.EmptyOption = EmptyOption;
		option.FocusNext = $"../{FocusNext}";
		option.FocusPrevious = $"../{FocusPrevious}";
		option.MetadataTypes = MetadataTypes;
		option.SizeFlagsHorizontal = SizeFlagsHorizontal;
		
		option.RefreshMetadata();
		option.FocusExited += ToggleEditMode;
		option.ItemSelected += handleItemSelected;
	}
	
	public new void GrabFocus()
	{
		if(EditMode)
			option.GrabFocus();
		else
			ToggleEditMode();
	}
	
	public override void ToggleEditMode()
	{
		base.ToggleEditMode();
		
		if(EditMode)
		{
			label.Text = string.Empty;
			option.Show();
			option.GrabFocus();
		}
		else
		{
			if(Value is not null)
				label.Text = Value.Name;
			
			option.Hide();
		}
	}
	
	private void handleItemSelected(long index)
	{
		EmitSignal(SignalName.ItemSelected, index);
		ToggleEditMode();
	}
}
