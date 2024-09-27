using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Ocsm.Nodes;

public partial class FlowList : FlowContainer
{
	private static class NodePaths
	{
		public static readonly NodePath AddButton = new("%Add");
	}
	
	[Signal]
	public delegate void ValueChangedEventHandler(Transport<List<string>> values);
	
	public List<string> Values { get; set; } = [];
	
	private Button addButton;
	private PackedScene inputNodeScene;
	
	public override void _ExitTree()
	{
		foreach(var node in GetChildren().Where(c => c is DynamicTextLabel).Cast<DynamicTextLabel>())
			node.TextChanged -= handleTextChanged;
		
		base._ExitTree();
	}
	
	public override void _Ready()
	{
		inputNodeScene = GD.Load<PackedScene>(ScenePaths.DynamicTextLabel);
		
		addButton = GetNode<Button>(NodePaths.AddButton);
		addButton.Pressed += handleAddPressed;
		
		Refresh();
	}
	
	public void Refresh()
	{
		foreach(var node in GetChildren().Where(c => c is DynamicTextLabel).Cast<DynamicTextLabel>())
			removeInput(node);
		
		foreach(var v in Values.Where(v => !string.IsNullOrEmpty(v)))
			addInput(v);
	}
	
	public void UpdateValues()
	{
		var values = new List<string>();
		foreach(var c in GetChildren()
			.Where(c => c is DynamicTextLabel)
			.Cast<DynamicTextLabel>())
		{
			if(!string.IsNullOrEmpty(c.Value))
				values.Add(c.Value);
			else
				removeInput(c);
		}
		
		Values = values;
	}
	
	private void addInput(string value = "")
	{
		if(inputNodeScene.CanInstantiate())
		{
			var node = inputNodeScene.Instantiate<DynamicTextLabel>();
			node.TextChanged += handleTextChanged;
			AddChild(node);
			
			if(string.IsNullOrEmpty(value))
				node.ToggleEditMode();
			else
				node.Value = value;
		}
		
		MoveChild(addButton, GetChildCount());
	}
	
	private void handleAddPressed() => addInput();
	private void handleTextChanged(string _) => UpdateValues();
	
	private void removeInput(DynamicTextLabel node)
	{
		node.TextChanged -= handleTextChanged;
		node.QueueFree();
	}
}
