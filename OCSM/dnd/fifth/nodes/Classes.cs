using Godot;
using System.Collections.Generic;
using Ocsm.Meta;
using Ocsm.Nodes;
using System.Linq;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class Classes : MarginContainer
{
	[Signal]
	public delegate void ClassAddedEventHandler(string name);
	[Signal]
	public delegate void ClassHitDieEventHandler(string name, int sides, int current);
	[Signal]
	public delegate void ClassLevelEventHandler(string name, int level);
	
	private static class NodePaths
	{
		public static readonly NodePath ClassList = new("%ClassList");
		public static readonly NodePath NewClass = new("%NewClass");
	}
	
	private VBoxContainer classList;
	private MetadataOption newClass;
	
	public override void _Ready()
	{
		classList = GetNode<VBoxContainer>(NodePaths.ClassList);
		newClass = GetNode<MetadataOption>(NodePaths.NewClass);
		newClass.ItemSelected += addNewClass;
	}
	
	public void RefreshClasses(List<ClassData> classes)
	{
		foreach(var child in classList.GetChildren().Cast<ClassRow>())
		{
			child.HitDiceChanged -= handleHitDice;
			child.LevelChanged -= handleLevel;
			child.QueueFree();
		}
		
		var resource = GD.Load<PackedScene>(ResourcePaths.Fifth.ClassRow);
		if(resource.CanInstantiate())
		{
			foreach(var data in classes)
			{
				var row = resource.Instantiate<ClassRow>();
				classList.AddChild(row);
				
				row.ClassName = data.Class.Name;
				row.Level = data.Level;
				row.HitDie = data.HitDie;
				row.HitDiceCurrent = data.HitDieCurrent;
				
				row.HitDiceChanged += handleHitDice;
				row.LevelChanged += handleLevel;
			}
		}
	}
	
	private void addNewClass(long index)
	{
		EmitSignal(SignalName.ClassAdded, newClass.GetItemText((int)index));
		newClass.SelectedMetadata = null;
	}
	
	private void handleHitDice(ClassRow node) => EmitSignal(SignalName.ClassHitDie, node.ClassName, node.HitDie?.Sides ?? Die.DefaultHitDieSides, node.HitDiceCurrent);
	private void handleLevel(ClassRow node) => EmitSignal(SignalName.ClassLevel, node.ClassName, node.Level);
}
