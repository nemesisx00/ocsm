using System.Linq;
using Godot;

namespace Ocsm.Nodes;

public partial class TrackSimple : GridContainer
{
	public const int DefaultMax = 5;
	
	[Signal]
	public delegate void NodeChangedEventHandler(TrackSimple node);
	[Signal]
	public delegate void ValueChangedEventHandler(int value);
	
	[Export]
	public int Max { get; set; } = DefaultMax;
	[Export]
	public int Value { get; set; }
	[Export]
	public bool UseCircles { get; set; } = true;
	[Export]
	public bool EnableToggling { get; set; } = true;
	
	public override void _Ready()
	{
		if(Max > 0)
		{
			UpdateMax(Max);
			
			//Work around for updateChildren setting the target -1 if it matches the current Value
			if(EnableToggling)
				Value++;
			Value = filterValue(Value);
			UpdateChildren(Value);
		}
	}
	
	public void UpdateChildren(int value = -1)
	{
		var children = GetChildren();
		foreach(var c in children.Cast<ToggleButton>())
		{
			if(children.IndexOf(c) < value)
				c.CurrentState = true;
			else
				c.CurrentState = false;
			c.UpdateTexture();
		}
	}
	
	public void UpdateMax(int max = 1)
	{
		Max = max;
		if(max < 1)
			Max = 1;
		
		var children = GetChildren();
		if(children.Count < Max)
		{
			var resource = GD.Load<PackedScene>(ScenePaths.ToggleButton);
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
			foreach(var c in children.Cast<ToggleButton>())
			{
				if(children.IndexOf(c) >= Max)
				{
					c.StateToggled -= handleToggle;
					c.QueueFree();
				}
			}
		}
	}
	
	public void UpdateValue(int value)
	{
		Value = filterValue(value);
		UpdateChildren(value);
	}
	
	private int filterValue(int value)
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
		var value = (int)GetChildren().IndexOf(button);
		
		if(value > -1)
			value++;
		
		value = filterValue(value);
		UpdateChildren(value);
		
		EmitSignal(SignalName.NodeChanged, this);
		EmitSignal(SignalName.ValueChanged, value);
	}
}
