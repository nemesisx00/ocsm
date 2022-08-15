using Godot;
using System;

public class TrackSimple : GridContainer
{
	[Export]
	public int Max { get; set; } = 5;
	[Export]
	public int Value { get; set; } = 0;
	[Export]
	public bool UseCircles { get; set; } = true;
	
	public override void _Ready()
	{
		if(Max > 0)
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.CoD.ToggleButton);
			for(var i = 0; i < Max; i++)
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
			
			//Work around for updateChildren setting the target -1 if it matches the current Value
			Value++;
			updateChildren(Value);
		}
	}
	
	public void updateChildren(int value = -1)
	{
		var target = value;
		if(target == Value)
			target--;
		if(target < 0)
			target = 0;
		Value = target;
		
		var children = GetChildren();
		foreach(ToggleButton c in children)
		{
			if(children.IndexOf(c) < target)
				c.CurrentState = true;
			else
				c.CurrentState = false;
			c.updateTexture();
		}
	}
	
	private void handleToggle(ToggleButton button)
	{
		var value = GetChildren().IndexOf(button);
		if(value > -1)
			value++;
		updateChildren(value);
	}
}
