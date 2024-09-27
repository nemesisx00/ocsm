using Godot;
using Ocsm.Meta;

namespace Ocsm.Nodes;

public partial class DynamicMetadataLabel : MarginContainer
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
			
			if(label is not null)
				label.HorizontalAlignment = alignment;
			
			if(option is not null)
				option.Alignment = alignment;
		}
	}
	
	public bool EditMode { get; set; }
	
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
	private Label label;
	private MetadataOption option;
	
	public override void _GuiInput(InputEvent evt)
	{
		if(!EditMode && evt.IsActionReleased(Actions.Click))
			toggleEditMode();
	}
	
	public override void _Ready()
	{
		label = GetNode<Label>(NodePaths.Label);
		label.HorizontalAlignment = Alignment;
		
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
		toggleEditMode();
	}
	
	private void toggleEditMode()
	{
		EditMode = !EditMode;
		
		if(EditMode)
		{
			label.Hide();
			option.Show();
			option.GrabFocus();
		}
		else
		{
			label.Text = Value.Name;
			
			option.Hide();
			label.Show();
		}
	}
}
