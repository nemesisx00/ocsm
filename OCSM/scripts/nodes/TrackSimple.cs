using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Ocsm.Nodes;

[GlobalClass]
public partial class TrackSimple : GridContainer
{
	public const int DefaultMax = 5;
	public const int DefaultMin = 0;
	
	[Signal]
	public delegate void ValueChangedEventHandler(int value, string name);
	
	[ExportGroup("Values")]
	[Export]
	public bool EnableToggling { get; set; } = true;
	[Export]
	public int Max { get; set; } = DefaultMax;
	[Export]
	public int Min { get; set; } = DefaultMin;
	[Export]
	public int Value { get; set; } = DefaultMin;
	[Export]
	private int allowedMax = DefaultMax;
	
	[ExportGroup("Textures")]
	[Export]
	public bool UseCircles { get; set; } = true;
	[Export]
	public Texture2D circleActive;
	[Export]
	public Texture2D circleInactive;
	[Export]
	public Texture2D boxActive;
	[Export]
	public Texture2D boxInactive;
	
	private readonly List<bool> allowedValues = [];
	
	public override void _Ready()
	{
		if(Max > 0)
		{
			for(var i = 0; i < Max; i++)
				allowedValues.Add(i < allowedMax);
			
			refresh();
			toggleChildren(Value);
		}
	}
	
	public void SetMax(int max)
	{
		Max = max;
		refresh();
		toggleChildren(Value);
	}
	
	public void SetValue(int value) => toggleChildren(sanitizeValue(value, true));
	
	private void handleToggle(ToggleButton button)
	{
		var value = (int)GetChildren().IndexOf(button);
		value = sanitizeValue(++value);
		toggleChildren(value);
		
		EmitSignal(SignalName.ValueChanged, value, Name);
	}
	
	private void refresh()
	{
		var children = GetChildren();
		if(children.Count < Max)
		{
			for(var i = children.Count; i < Max; i++)
			{
				ToggleButton instance = new()
				{
					Active = UseCircles ? circleActive : boxActive,
					Inactive = UseCircles ? circleInactive : boxInactive,
				};
				
				AddChild(instance);
				instance.Toggle += handleToggle;
			}
		}
		else
		{
			children.Select((child, index) => (child, index))
				.Where(pair => pair.index >= Max)
				.ToList()
				.ForEach(pair => pair.child.QueueFree());
		}
		
		children = GetChildren();
		for(var i = 0; i < children.Count; i++)
		{
			if(allowedValues[i])
				(children[i] as ToggleButton).Enable();
			else
				(children[i] as ToggleButton).Disable();
		}
	}
	
	private int sanitizeValue(int value, bool force = false)
	{
		var safe = value;
		if(safe < 0)
			safe = 0;
		if(safe > Max)
			safe = Max;
		
		if(!force && EnableToggling && safe > 0 && safe == Value)
		{
			safe--;
			
			//Keep reducing the value until we find a valid value
			while(safe > Min && !allowedValues[safe - 1])
			{
				safe--;
			}
			
			Value = safe;
		}
		else
			Value = safe;
		
		if(Value < Min)
			Value = Min;
		
		_ = EmitSignal(SignalName.ValueChanged, Value);
		return Value;
	}
	
	private void toggleChildren(int value = -1)
	{
		GetChildren().Select((child, index) => (child as ToggleButton, index))
			.ToList()
			.ForEach(pair => pair.Item1.State = pair.index < value);
	}
}
