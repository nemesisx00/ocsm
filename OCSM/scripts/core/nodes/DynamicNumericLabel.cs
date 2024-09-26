using Godot;

namespace Ocsm.Nodes;

public partial class DynamicNumericLabel : Container
{
	private static class NodePaths
	{
		public static readonly NodePath Label = new("%Label");
		public static readonly NodePath SpinBox = new("%SpinBox");
	}
	
	[Signal]
	public delegate void ValueChangedEventHandler(double value);
	
	[Export]
	public HorizontalAlignment Alignment
	{
		get => alignment;
		
		set
		{
			alignment = value;
			
			if(label is not null)
				label.HorizontalAlignment = alignment;
			
			if(spinBox is not null)
				spinBox.Alignment = alignment;
		}
	}
	
	[Export]
	public bool AllowGreater
	{
		get => allowGreater;
		set
		{
			allowGreater = value;
			
			if(spinBox is not null)
				spinBox.AllowGreater = allowGreater;
		}
	}
	
	[Export]
	public bool AllowLesser
	{
		get => allowLesser;
		set
		{
			allowLesser = value;
			
			if(spinBox is not null)
				spinBox.AllowLesser = allowLesser;
		}
	}
	
	public bool EditMode { get; set; }
	
	[Export]
	public double MaxValue
	{
		get => maxValue;
		
		set
		{
			maxValue = value;
			
			if(spinBox is not null)
				spinBox.MaxValue = maxValue;
		}
	}
	
	[Export]
	public double MinValue
	{
		get => minValue;
		
		set
		{
			minValue = value;
			
			if(spinBox is not null)
				spinBox.MinValue = minValue;
		}
	}
	
	[Export]
	public string Prefix { get; set; } = string.Empty;
	
	[Export]
	public bool ShowSign { get; set; }
	
	[Export]
	public string Suffix { get; set; } = string.Empty;
	
	public double Value
	{
		get => currentValue;
		
		set
		{
			currentValue = value;
			
			if(spinBox is not null)
			{
				spinBox.Value = currentValue;
				updateLabel();
			}
		}
	}
	
	private HorizontalAlignment alignment;
	private bool allowGreater;
	private bool allowLesser;
	private double currentValue;
	private Label label;
	private double maxValue = 100;
	private double minValue = 0;
	private SpinBox spinBox;
	
	public override void _ExitTree() => spinBox.ValueChanged -= handleValueChanged;
	
	public override void _GuiInput(InputEvent evt)
	{
		if(!EditMode && evt.IsActionReleased(Actions.Click))
			toggleEditMode();
	}
	
	public override void _Ready()
	{
		label = GetNode<Label>(NodePaths.Label);
		label.HorizontalAlignment = Alignment;
		
		spinBox = GetNode<SpinBox>(NodePaths.SpinBox);
		spinBox.Alignment = Alignment;
		spinBox.AllowGreater = AllowGreater;
		spinBox.AllowLesser = AllowLesser;
		spinBox.MaxValue = MaxValue;
		spinBox.MinValue = MinValue;
		spinBox.GetLineEdit().FocusExited += toggleEditMode;
		spinBox.ValueChanged += handleValueChanged;
	}
	
	private void handleValueChanged(double value)
	{
		Value = value;
		EmitSignal(SignalName.ValueChanged, Value);
	}
	
	private void toggleEditMode()
	{
		EditMode = !EditMode;
		
		if(EditMode)
		{
			label.Hide();
			spinBox.Show();
			spinBox.GetLineEdit().GrabFocus();
		}
		else
		{
			updateLabel();
			spinBox.Hide();
			label.Show();
		}
	}
	
	private void updateLabel()
	{
		var value = ShowSign
			? StringUtilities.FormatModifier(spinBox.Value)
			: spinBox.Value.ToString();
		
		label.Text = $"{Prefix}{value}{Suffix}";
	}
}
