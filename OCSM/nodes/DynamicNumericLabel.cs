using Godot;

namespace Ocsm.Nodes;

public partial class DynamicNumericLabel : DynamicLabel
{
	private static class NodePaths
	{
		public static readonly NodePath Label = new("%Label");
		public static readonly NodePath SpinBox = new("%SpinBox");
	}
	
	[Signal]
	public delegate void ValueChangedEventHandler(double value);
	
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
	public HorizontalAlignment SpinBoxAlignment
	{
		get => spinBoxAlignment;
		
		set
		{
			spinBoxAlignment = value;
			
			if(spinBox is not null)
				spinBox.Alignment = spinBoxAlignment;
		}
	}
	
	[Export]
	public string Suffix { get; set; } = string.Empty;
	
	public double Value
	{
		get => spinBox.Value;
		set
		{
			spinBox.Value = value;
			
			if(!EditMode)
				updateText();
		}
	}
	
	private bool allowGreater;
	private bool allowLesser;
	private double currentValue;
	private double maxValue = 100;
	private double minValue = 0;
	private SpinBox spinBox;
	private HorizontalAlignment spinBoxAlignment;
	
	public override void _Ready()
	{
		base._Ready();
		
		spinBox = GetNode<SpinBox>(NodePaths.SpinBox);
		spinBox.Alignment = SpinBoxAlignment;
		spinBox.AllowGreater = AllowGreater;
		spinBox.AllowLesser = AllowLesser;
		spinBox.FocusNext = $"../{FocusNext}";
		spinBox.FocusPrevious = $"../{FocusPrevious}";
		spinBox.MaxValue = MaxValue;
		spinBox.MinValue = MinValue;
		spinBox.GetLineEdit().FocusExited += ToggleEditMode;
		spinBox.ValueChanged += handleValueChanged;
		spinBox.SizeFlagsHorizontal = SizeFlagsHorizontal;
	}
	
	private void handleValueChanged(double value)
	{
		Value = value;
		EmitSignal(SignalName.ValueChanged, Value);
	}
	
	public new void GrabFocus()
	{
		if(EditMode)
			spinBox.GrabFocus();
		else
			ToggleEditMode();
	}
	
	public override void ToggleEditMode()
	{
		base.ToggleEditMode();
		
		if(EditMode)
		{
			label.Text = string.Empty;
			spinBox.Show();
			spinBox.GetLineEdit().GrabFocus();
		}
		else
		{
			updateText();
			spinBox.Hide();
		}
	}
	
	private void updateText()
	{
		var value = ShowSign
			? StringUtilities.FormatModifier(spinBox.Value)
			: spinBox.Value.ToString();
		
		label.Text = $"{Prefix}{value}{Suffix}";
	}
}
