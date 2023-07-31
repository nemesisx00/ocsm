using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using Ocsm.Dnd.Fifth;

namespace Ocsm.Nodes.Dnd.Fifth.Meta;

public partial class NumericBonusEditList : Container
{
	[Signal]
	public delegate void ValuesChangedEventHandler(Transport<List<NumericBonus>> values);
	
	public List<NumericBonus> Values { get; set; } = new List<NumericBonus>();
	
	public override void _Ready()
	{
		refresh();
	}
	
	public void refresh()
	{
		foreach(Node c in GetChildren())
		{
			c.QueueFree();
		}
		
		Values.Where(v => v is NumericBonus)
			.ToList()
			.ForEach(v => addInput(v));
		
		addInput();
	}
	
	private void numericBonusChanged(Transport<NumericBonus> transport) { updateValues(); }
	
	private void updateValues()
	{
		var values = new List<NumericBonus>();
		var children = GetChildren();
		foreach(NumericBonusEdit c in children)
		{
			var value = c.Value;
			
			if(!String.IsNullOrEmpty(value.Name) || !value.Type.Equals(NumericStat.None))
				values.Add(value);
			else if(children.IndexOf(c) != children.Count - 1)
				c.QueueFree();
		}
		
		Values = values;
		EmitSignal(nameof(ValuesChanged), new Transport<List<NumericBonus>>(Values));
		
		if(children.Count <= values.Count)
			addInput();
	}
	
	private void addInput(NumericBonus bonus = null)
	{
		var resource = GD.Load<PackedScene>(Constants.Scene.Dnd.Fifth.Meta.NumericBonusEdit);
		var instance = resource.Instantiate<NumericBonusEdit>();
		
		AddChild(instance);
		if(bonus is NumericBonus)
			instance.setValue(bonus);
		
		instance.ValueChanged += numericBonusChanged;
	}
}
