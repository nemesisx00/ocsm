using Godot;

namespace Ocsm.Nodes;

public partial class DynamicTextLabel : DynamicLabel
{
	private static class NodePaths
	{
		public static readonly NodePath LineEdit = new("%LineEdit");
		public static readonly NodePath TextEdit = new("%TextEdit");
		public static readonly NodePath Panel = new("%Panel");
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
			Text = EditMode ? string.Empty : value;
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
		
		textEdit = GetNode<TextEdit>(NodePaths.TextEdit);
		textEdit.FocusExited += ToggleEditMode;
		textEdit.TextChanged += handleTextChanged;
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
			
			Text = string.Empty;
		}
		else
		{
			Text = Multiline
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
