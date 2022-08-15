using Godot;
using System;

public class CircleTrack : GridContainer
{
	[Export]
	public int Max { get; set; } = 5;
	
	public int Value
	{
		get
		{
			var value = 0;
			foreach(Node c in GetChildren())
			{
				var circle = c.GetChild<CircleState>(0);
				if(circle.CurrentState)
					value++;
			}
			return value;
		}
	}
	
	public override void _Ready()
	{
		if(Max > 0)
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.CoD.TwoStateCircle);
			for(var i = 0; i < Max; i++)
			{
				var instance = resource.Instance<TextureRect>();
				AddChild(instance);
			}
		}
	}
}
