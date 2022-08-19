using Godot;
using System.Collections.Generic;
using OCSM;

public class TrackComplex : GridContainer
{
	[Signal]
	public delegate void ValueChanged(Dictionary<string, int> values);
	
	[Export]
	public int Max { get; set; } = 5;
	
	public Dictionary<string, int> Values
	{
		get
		{
			var values = new Dictionary<string, int>();
			values.Add(BoxComplex.State.One, 0);
			values.Add(BoxComplex.State.Two, 0);
			values.Add(BoxComplex.State.Three, 0);
			
			foreach(Node c in GetChildren())
			{
				var box = c.GetChild<BoxComplex>(0);
				switch(box.CurrentState)
				{
					case BoxComplex.State.One:
						values[BoxComplex.State.One]++;
						break;
					case BoxComplex.State.Two:
						values[BoxComplex.State.Two]++;
						break;
					case BoxComplex.State.Three:
						values[BoxComplex.State.Three]++;
						break;
					case BoxComplex.State.None:
						break;
				}
			}
			
			return values;
		}
		
		set
		{
			var children = GetChildren();
			foreach(Node c in children)
			{
				if(children.IndexOf(c) < value[BoxComplex.State.Three])
					c.GetChild<BoxComplex>(0).CurrentState = BoxComplex.State.Three;
				else if(children.IndexOf(c) < value[BoxComplex.State.Three] + value[BoxComplex.State.Two])
					c.GetChild<BoxComplex>(0).CurrentState = BoxComplex.State.Two;
				else if(children.IndexOf(c) < value[BoxComplex.State.Three] + value[BoxComplex.State.Two] + value[BoxComplex.State.One])
					c.GetChild<BoxComplex>(0).CurrentState = BoxComplex.State.One;
			}
			updateBoxes();
		}
	}
	
	public override void _Ready()
	{
		updateMax(Max);
	}
	
	private void handleBoxComplex(BoxComplex box)
	{
		updateBoxes();
		EmitSignal(nameof(ValueChanged), Values);
	}
	
	public void updateBoxes()
	{
		var values = Values;
		var children = GetChildren();
		foreach(Node c in children)
		{
			var state = BoxComplex.State.None;
			if(children.IndexOf(c) < values[BoxComplex.State.Three])
				state = BoxComplex.State.Three;
			else if(children.IndexOf(c) < values[BoxComplex.State.Three] + values[BoxComplex.State.Two])
				state = BoxComplex.State.Two;
			else if(children.IndexOf(c) < values[BoxComplex.State.Three] + values[BoxComplex.State.Two] + values[BoxComplex.State.One])
				state = BoxComplex.State.One;
			
			var box = c.GetChild<BoxComplex>(0);
			box.CurrentState = state;
			box.updateTexture();
		}
	}
	
	public void updateMax(int max = 1)
	{
		Max = max;
		if(Max < 1)
			Max = 1;
		
		var children = GetChildren();
		if(children.Count < Max)
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.CoD.BoxComplex);
			for(var i = children.Count; i < Max; i++)
			{
				var instance = resource.Instance<TextureRect>();
				AddChild(instance);
				instance.GetChild(0).Connect(nameof(BoxComplex.StateChanged), this, nameof(handleBoxComplex));
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
