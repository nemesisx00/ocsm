using Godot;
using System.Linq;
using System.Collections.Generic;

namespace Ocsm.Dnd.Fifth.Nodes.Meta;

public partial class NumericBonusEditList : Container
{
	[Signal]
	public delegate void ValuesChangedEventHandler(Transport<List<NumericBonus>> values);
	
	public List<NumericBonus> Values { get; set; } = [];

	public override void _Ready() => Refresh();

	public void Refresh()
	{
		foreach(Node c in GetChildren())
		{
			if(c is NumericBonusEdit nbe)
				nbe.ValueChanged -= numericBonusChanged;
			
			c.QueueFree();
		}
		
		foreach(var value in Values.Where(v => v is not null))
			addInput(value);
		
		addInput();
	}
	
	private void numericBonusChanged(Transport<NumericBonus> transport) => updateValues();
	
	private void updateValues()
	{
		var values = new List<NumericBonus>();
		var children = GetChildren();
		foreach(var c in children.Cast<NumericBonusEdit>())
		{
			var value = c.Value;
			
			if(!string.IsNullOrEmpty(value.Name) || value.Type != NumericStats.None)
				values.Add(value);
			else if(children.IndexOf(c) != children.Count - 1)
				c.QueueFree();
		}
		
		Values = values;
		EmitSignal(SignalName.ValuesChanged, new Transport<List<NumericBonus>>(Values));
		
		if(children.Count <= values.Count)
			addInput();
	}
	
	private void addInput(NumericBonus bonus = null)
	{
		var resource = GD.Load<PackedScene>(ScenePaths.Dnd.Fifth.Meta.NumericBonusEdit);
		var instance = resource.Instantiate<NumericBonusEdit>();
		
		AddChild(instance);
		
		if(bonus is not null)
			instance.SetValue(bonus);
		
		instance.ValueChanged += numericBonusChanged;
	}
}
