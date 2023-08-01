using Godot;

namespace Ocsm.Nodes;

public partial class TrackSimple : GridContainer
{
	public const long DefaultMax = 5;
	
	[Signal]
	public delegate void NodeChangedEventHandler(TrackSimple node);
	[Signal]
	public delegate void ValueChangedEventHandler(long value);
	
	[Export]
	public long Max { get; set; } = DefaultMax;
	[Export]
	public long Value { get; set; } = 0;
	[Export]
	public bool UseCircles { get; set; } = true;
	[Export]
	public bool EnableToggling { get; set; } = true;
	
	public override void _Ready()
	{
		if(Max > 0)
		{
			updateMax(Max);
			
			//Work around for updateChildren setting the target -1 if it matches the current Value
			if(EnableToggling)
				Value++;
			Value = filterValue(Value);
			updateChildren(Value);
		}
	}
	
	public void updateChildren(long value = -1)
	{
		var children = GetChildren();
		foreach(ToggleButton c in children)
		{
			if(children.IndexOf(c) < value)
				c.CurrentState = true;
			else
				c.CurrentState = false;
			c.updateTexture();
		}
	}
	
	public void updateMax(long max = 1)
	{
		Max = max;
		if(max < 1)
			Max = 1;
		
		var children = GetChildren();
		if(children.Count < Max)
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.ToggleButton);
			for(var i = children.Count; i < Max; i++)
			{
				var instance = resource.Instantiate<ToggleButton>();
				instance.UseCircles = UseCircles;
				AddChild(instance);
				instance.StateToggled += handleToggle;
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
	
	public void updateValue(long value)
	{
		Value = filterValue(value);
		updateChildren(value);
	}
	
	private long filterValue(long value)
	{
		var target = value;
		if(EnableToggling && target == Value)
			target--;
		if(target < 0)
			target = 0;
		if(target > Max)
			target = Max;
		Value = target;
		return target;
	}
	
	private void handleToggle(ToggleButton button)
	{
		var value = (long)GetChildren().IndexOf(button);
		if(value > -1)
			value++;
		value = filterValue(value);
		updateChildren(value);
		
		EmitSignal(nameof(NodeChanged), this);
		EmitSignal(nameof(ValueChanged), value);
	}
}
