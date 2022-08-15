using Godot;
using System;

public class CircleTrack : GridContainer
{
	[Export]
	public int Max { get; set; } = 5;
	[Export]
	public int Value { get; set; } = 0;
	
	public override void _Ready()
	{
		if(Max > 0)
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.CoD.TwoStateCircle);
			for(var i = 0; i < Max; i++)
			{
				var instance = resource.Instance<TextureRect>();
				AddChild(instance);
				instance.GetChild(0).Connect(nameof(CircleState.StateToggled), this, nameof(handleCircleToggle));
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
		
		foreach(Node c in GetChildren())
		{
			var circle = c.GetChild<CircleState>(0);
			if(GetChildren().IndexOf(c) < target)
				circle.CurrentState = true;
			else
				circle.CurrentState = false;
			circle.updateTexture();
		}
	}
	
	private void handleCircleToggle(CircleState circle)
	{
		var value = GetChildren().IndexOf(circle.GetParent());
		if(value > -1)
			value++;
		updateCircles(value);
	}
}
