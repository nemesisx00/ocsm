using Godot;
using System;

public class TrackCircle : GridContainer
{
	[Export]
	public int Max { get; set; } = 5;
	[Export]
	public int Value { get; set; } = 0;
	
	public override void _Ready()
	{
		if(Max > 0)
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.CoD.CircleToggle);
			for(var i = 0; i < Max; i++)
			{
				var instance = resource.Instance<TextureRect>();
				AddChild(instance);
				instance.GetChild(0).Connect(nameof(CircleToggle.StateToggled), this, nameof(handleCircleToggle));
			}
			
			updateCircles(Value);
		}
	}
	
	public void updateCircles(int value = -1)
	{
		var target = value;
		if(target == Value)
			target--;
		if(target < 0)
			target = 0;
		Value = target;
		
		var children = GetChildren();
		foreach(Node c in children)
		{
			var circle = c.GetChild<CircleToggle>(0);
			if(children.IndexOf(c) < target)
				circle.CurrentState = true;
			else
				circle.CurrentState = false;
			circle.updateTexture();
		}
	}
	
	private void handleCircleToggle(CircleToggle circle)
	{
		var value = GetChildren().IndexOf(circle.GetParent());
		if(value > -1)
			value++;
		updateCircles(value);
	}
}
