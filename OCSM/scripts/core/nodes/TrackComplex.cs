using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Ocsm.Nodes;

public partial class TrackComplex : GridContainer
{
	public const int DefaultMax = 5;
	
	[Signal]
	public delegate void ValueChangedEventHandler(Transport<Dictionary<StatefulButton.States, int>> values);
	
	[Export]
	public int Max { get; set; } = DefaultMax;
	
	public Dictionary<StatefulButton.States, int> Values
	{
		get
		{
			Dictionary<StatefulButton.States, int> values = new()
			{
				{ StatefulButton.States.One, 0 },
				{ StatefulButton.States.Two, 0 },
				{ StatefulButton.States.Three, 0 }
			};
			
			foreach(var c in GetChildren().Cast<StatefulButton>())
			{
				switch(c.CurrentState)
				{
					case StatefulButton.States.One:
						values[StatefulButton.States.One]++;
						break;
					
					case StatefulButton.States.Two:
						values[StatefulButton.States.Two]++;
						break;
					
					case StatefulButton.States.Three:
						values[StatefulButton.States.Three]++;
						break;
				}
			}
			
			return values;
		}
		
		set
		{
			var children = GetChildren();
			foreach(var c in children.Cast<StatefulButton>())
			{
				if(children.IndexOf(c) < value[StatefulButton.States.Three])
					c.CurrentState = StatefulButton.States.Three;
				else if(children.IndexOf(c) < value[StatefulButton.States.Three] + value[StatefulButton.States.Two])
					c.CurrentState = StatefulButton.States.Two;
				else if(children.IndexOf(c) < value[StatefulButton.States.Three] + value[StatefulButton.States.Two] + value[StatefulButton.States.One])
					c.CurrentState = StatefulButton.States.One;
			}
			UpdateBoxes();
		}
	}
	
	public override void _Ready() => UpdateMax(Max);
	
	private void handleStatefulButton(StatefulButton box)
	{
		UpdateBoxes();
		EmitSignal(SignalName.ValueChanged, new Transport<Dictionary<StatefulButton.States, int>>(Values));
	}
	
	public void UpdateBoxes()
	{
		var values = Values;
		var children = GetChildren();
		foreach(var c in children.Cast<StatefulButton>())
		{
			var state = StatefulButton.States.None;
			if(children.IndexOf(c) < values[StatefulButton.States.Three])
				state = StatefulButton.States.Three;
			else if(children.IndexOf(c) < values[StatefulButton.States.Three] + values[StatefulButton.States.Two])
				state = StatefulButton.States.Two;
			else if(children.IndexOf(c) < values[StatefulButton.States.Three] + values[StatefulButton.States.Two] + values[StatefulButton.States.One])
				state = StatefulButton.States.One;
			
			c.CurrentState = state;
			c.UpdateTexture();
		}
	}
	
	public void UpdateMax(int max = 1)
	{
		Max = max;
		if(Max < 1)
			Max = 1;
		
		var children = GetChildren();
		if(children.Count < Max)
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.StatefulButton);
			for(var i = children.Count; i < Max; i++)
			{
				var instance = resource.Instantiate<StatefulButton>();
				AddChild(instance);
				instance.StateChanged += handleStatefulButton;
			}
		}
		else
		{
			foreach(Node c in children)
			{
				if(children.IndexOf(c) >= Max)
					c.QueueFree();
			}
		}
	}
}
