using Godot;
using System.Linq;
using System.Collections.Generic;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class NumericBonusEditList : Container
{
	[Signal]
	public delegate void ValuesChangedEventHandler(Transport<List<NumericBonus>> values);
	
	public List<NumericBonus> Values { get; set; } = [];
	
	public override void _Ready()
	{
		Refresh();
	}
	
	public void Refresh()
	{
		foreach(Node c in GetChildren())
		{
			c.QueueFree();
		}
		
		Values.ForEach(v => addInput(v));
		
		addInput();
	}
	
	private void numericBonusChanged(Transport<NumericBonus> transport) => updateValues();
	
	private void updateValues()
	{
		var values = new List<NumericBonus>();
		var children = GetChildren();
		foreach(NumericBonusEdit c in children.Cast<NumericBonusEdit>())
		{
			var value = c.Value;
			
			if(!string.IsNullOrEmpty(value.Name) || !value.Type.Equals(NumericBonus.StatTypes.None))
				values.Add(value);
			else if(children.IndexOf(c) != children.Count - 1)
				c.QueueFree();
		}
		
		Values = values;
		_ = EmitSignal(SignalName.ValuesChanged, new Transport<List<NumericBonus>>(Values));
		
		if(children.Count <= values.Count)
			addInput();
	}
	
	private void addInput(NumericBonus bonus = null)
	{
		var resource = GD.Load<PackedScene>(Constants.Scene.Dnd.Fifth.Meta.NumericBonusEdit);
		var instance = resource.Instantiate<NumericBonusEdit>();
		
		AddChild(instance);
		
		if(bonus is not null)
			instance.SetValue(bonus);
		
		instance.ValueChanged += numericBonusChanged;
	}
}
