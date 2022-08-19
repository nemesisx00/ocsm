using Godot;
using OCSM;

public class TrackSimple : GridContainer
{
	[Signal]
	public delegate void ValueChanged(int value);
	
	[Export]
	public int Max { get; set; } = 5;
	[Export]
	public int Value { get; set; } = 0;
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
	
	public void updateChildren(int value = -1)
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
	
	public void updateMax(int max = 1)
	{
		Max = max;
		if(max < 1)
			Max = 1;
		
		var children = GetChildren();
		if(children.Count < Max)
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.CoD.ToggleButton);
			for(var i = children.Count; i < Max; i++)
			{
				var instance = resource.Instance<ToggleButton>();
				if(UseCircles)
				{
					instance.Texture = GD.Load<StreamTexture>(Constants.Texture.TrackCircle);
					instance.ToggledTexturePath = Constants.Texture.TrackCircleFill;
				}
				else
				{
					instance.Texture = GD.Load<StreamTexture>(Constants.Texture.TrackBoxBorder);
					instance.ToggledTexturePath = Constants.Texture.TrackBox2;
				}
				
				AddChild(instance);
				instance.Connect(nameof(ToggleButton.StateToggled), this, nameof(handleToggle));
			}
		}
		else
		{
			foreach(ToggleButton c in children)
			{
				if(children.IndexOf(c) >= Max)
				{
					c.QueueFree();
				}
			}
		}
	}
	
	public void updateValue(int value)
	{
		updateChildren(filterValue(value));
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
		var value = GetChildren().IndexOf(button);
		if(value > -1)
			value++;
		value = filterValue(value);
		updateChildren(value);
		EmitSignal(nameof(ValueChanged), value);
	}
}
