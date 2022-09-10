using Godot;
using System;
using System.Collections.Generic;
using OCSM.DnD.Fifth;

namespace OCSM.Nodes.DnD.Fifth.Meta
{
	public class NumericBonusEditList : Container
	{
		[Signal]
		public delegate void ValuesChanged(List<Transport<NumericBonus>> values);
		
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
			
			foreach(var v in Values)
			{
				if(v is NumericBonus)
					addInput(v);
			}
			
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
			doEmitSignal();
			
			if(children.Count <= values.Count)
				addInput();
		}
		
		private void doEmitSignal()
		{
			var list = new List<Transport<NumericBonus>>();
			foreach(var fs in Values)
			{
				list.Add(new Transport<NumericBonus>(fs));
			}
			EmitSignal(nameof(ValuesChanged), list);
		}
		
		private void addInput(NumericBonus bonus = null)
		{
			var resource = ResourceLoader.Load<PackedScene>(Constants.Scene.DnD.Fifth.Meta.NumericBonusEdit);
			var instance = resource.Instance<NumericBonusEdit>();
			
			AddChild(instance);
			if(bonus is NumericBonus)
				instance.setValue(bonus);
			
			instance.Connect(nameof(NumericBonusEdit.ValueChanged), this, nameof(numericBonusChanged));
		}
	}
}
