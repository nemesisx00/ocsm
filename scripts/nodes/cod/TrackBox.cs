using Godot;
using System.Collections.Generic;

public class TrackBox : GridContainer
{
	[Export]
	public int Max { get; set; } = 5;
	
	public Dictionary<string, int> Values
	{
		get
		{
			var values = new Dictionary<string, int>();
			values.Add(BoxToggle.State.One, 0);
			values.Add(BoxToggle.State.Two, 0);
			values.Add(BoxToggle.State.Three, 0);
			
			foreach(Node c in GetChildren())
			{
				var box = c.GetChild<BoxToggle>(0);
				switch(box.CurrentState)
				{
					case BoxToggle.State.One:
						values[BoxToggle.State.One]++;
						break;
					case BoxToggle.State.Two:
						values[BoxToggle.State.Two]++;
						break;
					case BoxToggle.State.Three:
						values[BoxToggle.State.Three]++;
						break;
					case BoxToggle.State.None:
						break;
				}
			}
			
			return values;
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
				instance.GetChild(0).Connect(nameof(BoxToggle.StateChanged), this, nameof(handleBoxToggle));
			}
		}
	}
	
	private void handleBoxToggle(BoxToggle box)
	{
		updateBoxes();
	}
	
	public void updateBoxes()
	{
		var values = Values;
		var children = GetChildren();
		foreach(Node c in children)
		{
			var state = BoxToggle.State.None;
			if(children.IndexOf(c) < values[BoxToggle.State.Three])
				state = BoxToggle.State.Three;
			else if(children.IndexOf(c) < values[BoxToggle.State.Three] + values[BoxToggle.State.Two])
				state = BoxToggle.State.Two;
			else if(children.IndexOf(c) < values[BoxToggle.State.Three] + values[BoxToggle.State.Two] + values[BoxToggle.State.One])
				state = BoxToggle.State.One;
			
			var box = c.GetChild<BoxToggle>(0);
			box.CurrentState = state;
			box.updateTexture();
		}
	}
}
