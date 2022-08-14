using Godot;
using System;

public class BoxTrack : GridContainer
{
	[Export]
	public int Max { get; set; } = 5;
	
	public TrackThreeState State
	{
		get
		{
			var state = new TrackThreeState();
			foreach(Node c in GetChildren())
			{
				var box = c.GetChild<ThreeStateBox>(0);
				switch(box.CurrentState)
				{
					case ThreeStateBox.State.One:
						state.One++;
						break;
					case ThreeStateBox.State.Two:
						state.Two++;
						break;
					case ThreeStateBox.State.Three:
						state.Three++;
						break;
					case ThreeStateBox.State.None:
						break;
				}
			}
			
			return state;
		}
	}
	
	public override void _Ready()
	{
		if(Max > 0)
		{
			var resource = GD.Load<PackedScene>(Constants.Scene.CoD.ThreeStateBox);
			for(var i = 0; i < Max; i++)
			{
				var instance = resource.Instance<TextureRect>();
				AddChild(instance);
			}
		}
	}
}
