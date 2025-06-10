using Godot;

namespace Ocsm.Nodes;

public partial class DynamicTextLabel : DynamicLabel
{
	private static class NodePaths
	{
		public static readonly NodePath Label = new("%Label");
		public static readonly NodePath LineEdit = new("%LineEdit");
		public static readonly NodePath TextEdit = new("%TextEdit");
	}
	
	[Signal]
	public delegate void TextChangedEventHandler(string text);
	
	[Export]
	public bool Multiline { get; set; }
	
	private LineEdit lineEdit;
	private TextEdit textEdit;
	
	public string Value
	{
		get => Multiline
			? textEdit.Text
			: lineEdit.Text;
		
		set
		{
			label.Text = EditMode ? string.Empty : value;
			lineEdit.Text = value;
			textEdit.Text = value;
		}
	}
	
	public override void _Ready()
	{
		base._Ready();
		
		lineEdit = GetNode<LineEdit>(NodePaths.LineEdit);
		lineEdit.FocusExited += ToggleEditMode;
		lineEdit.TextChanged += handleTextChanged;
		lineEdit.FocusNext = $"../{FocusNext}";
		lineEdit.FocusPrevious = $"../{FocusPrevious}";
		lineEdit.SizeFlagsHorizontal = SizeFlagsHorizontal;
		
		textEdit = GetNode<TextEdit>(NodePaths.TextEdit);
		textEdit.FocusExited += ToggleEditMode;
		textEdit.TextChanged += handleTextChanged;
		textEdit.FocusNext = $"../{FocusNext}";
		textEdit.FocusPrevious = $"../{FocusPrevious}";
		textEdit.SizeFlagsHorizontal = SizeFlagsHorizontal;
	}
	
	public new void GrabFocus()
	{
		if(EditMode)
		{
			if(Multiline)
				textEdit.GrabFocus();
			else
				lineEdit.GrabFocus();
		}
		else
			ToggleEditMode();
	}
	
	public override void ToggleEditMode()
	{
		base.ToggleEditMode();
		
		if(EditMode)
		{
			if(Multiline)
			{
				CustomMinimumSize = textEdit.GetMinimumSize();
				textEdit.Show();
				textEdit.GrabFocus();
			}
			else
			{
				CustomMinimumSize = lineEdit.GetMinimumSize();
				lineEdit.Show();
				lineEdit.GrabFocus();
			}
			
			label.Text = string.Empty;
		}
		else
		{
			label.Text = Multiline
				? textEdit.Text
				: lineEdit.Text;
			
			lineEdit.Hide();
			textEdit.Hide();
			
			CustomMinimumSize = Vector2.Zero;
		}
	}
	
	private void handleTextChanged() => handleTextChanged(null);
	private void handleTextChanged(string _)
	{
		if(EditMode)
		{
			CustomMinimumSize = Multiline
				? textEdit.GetMinimumSize()
				: lineEdit.GetMinimumSize();
		}
		
		EmitSignal(SignalName.TextChanged, Value);
	}
}
