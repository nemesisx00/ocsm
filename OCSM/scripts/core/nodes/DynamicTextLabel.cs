using Godot;

namespace Ocsm.Nodes;

public partial class DynamicTextLabel : Container
{
	private static class NodePaths
	{
		public static readonly NodePath Label = new("%Label");
		public static readonly NodePath LineEdit = new("%LineEdit");
		public static readonly NodePath TextEdit = new("%TextEdit");
	}
	
	[Signal]
	public delegate void TextChangedEventHandler(string text);
	
	public bool EditMode { get; set; }
	
	[Export]
	public bool Multiline { get; set; }
	
	private Label label;
	private LineEdit lineEdit;
	private TextEdit textEdit;
	
	public string Value
	{
		get => Multiline
			? textEdit.Text
			: lineEdit.Text;
		
		set
		{
			lineEdit.Text = value;
			textEdit.Text = value;
			
			if(Multiline)
				label.Text = lineEdit.Text;
			else
				label.Text = textEdit.Text;
		}
	}
	
	public override void _ExitTree()
	{
		lineEdit.TextChanged -= handleTextChanged;
		textEdit.TextChanged -= handleTextChanged;
	}
	
	public override void _GuiInput(InputEvent evt)
	{
		if(!EditMode && evt.IsActionReleased(Actions.Click))
			toggleEditMode();
	}
	
	public override void _Ready()
	{
		label = GetNode<Label>(NodePaths.Label);
		
		lineEdit = GetNode<LineEdit>(NodePaths.LineEdit);
		lineEdit.FocusExited += toggleEditMode;
		lineEdit.TextChanged += handleTextChanged;
		
		textEdit = GetNode<TextEdit>(NodePaths.TextEdit);
		textEdit.FocusExited += toggleEditMode;
		textEdit.TextChanged += handleTextChanged;
	}
	
	private void handleTextChanged() => handleTextChanged(Value);
	private void handleTextChanged(string text) => EmitSignal(SignalName.TextChanged, text);
	
	private void toggleEditMode()
	{
		EditMode = !EditMode;
		
		if(EditMode)
		{
			label.Hide();
			
			if(Multiline)
			{
				textEdit.Show();
				textEdit.GrabFocus();
			}
			else
			{
				lineEdit.Show();
				lineEdit.GrabFocus();
			}
		}
		else
		{
			label.Text = Multiline
				? textEdit.Text
				: lineEdit.Text;
			
			lineEdit.Hide();
			textEdit.Hide();
			label.Show();
		}
	}
}
