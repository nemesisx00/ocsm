using Godot;
using System;

public class TrackBox : GridContainer
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
				var box = c.GetChild<BoxToggle>(0);
				switch(box.CurrentState)
				{
					case BoxToggle.State.One:
						state.One++;
						break;
					case BoxToggle.State.Two:
						state.Two++;
						break;
					case BoxToggle.State.Three:
						state.Three++;
						break;
					case BoxToggle.State.None:
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
			var resource = GD.Load<PackedScene>(Constants.Scene.CoD.BoxToggle);
			for(var i = 0; i < Max; i++)
			{
				var instance = resource.Instance<TextureRect>();
				AddChild(instance);
			}
		}
	}
}
