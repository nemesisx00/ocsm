using Godot;
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
	public GameSystem GameSystem { get; set; }
	
	[Export]
	public MetadataType MetadataType { get; set; }
	
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
		option.GameSystem = GameSystem;
		option.MetadataType = MetadataType;
		
		option.RefreshMetadata();
		option.ItemSelected += handleItemSelected;
	}
	
	private void handleItemSelected(long index)
	{
		EmitSignal(SignalName.ItemSelected, index);
		ToggleEditMode();
	}
	
	public override void ToggleEditMode()
	{
		base.ToggleEditMode();
		
		if(EditMode)
		{
			Text = string.Empty;
			option.Show();
		}
		else
		{
			Text = Value.Name;
			option.Hide();
		}
	}
}
