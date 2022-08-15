using Godot;
using System.Collections.Generic;

public class TrackComplex : GridContainer
{
	[Export]
	public int Max { get; set; } = 5;
	
	public Dictionary<string, int> Values
	{
		get
		{
			var values = new Dictionary<string, int>();
			values.Add(BoxComplex.State.One, 0);
			values.Add(BoxComplex.State.Two, 0);
			values.Add(BoxComplex.State.Three, 0);
			
			foreach(Node c in GetChildren())
			{
				var box = c.GetChild<BoxComplex>(0);
				switch(box.CurrentState)
				{
					case BoxComplex.State.One:
						values[BoxComplex.State.One]++;
						break;
					case BoxComplex.State.Two:
						values[BoxComplex.State.Two]++;
						break;
					case BoxComplex.State.Three:
						values[BoxComplex.State.Three]++;
						break;
					case BoxComplex.State.None:
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
			var resource = GD.Load<PackedScene>(Constants.Scene.CoD.BoxComplex);
			for(var i = 0; i < Max; i++)
			{
				var instance = resource.Instance<TextureRect>();
				AddChild(instance);
				instance.GetChild(0).Connect(nameof(BoxComplex.StateChanged), this, nameof(handleBoxComplex));
			}
		}
	}
	
	private void handleBoxComplex(BoxComplex box)
	{
		updateBoxes();
	}
	
	public void updateBoxes()
	{
		var values = Values;
		var children = GetChildren();
		foreach(Node c in children)
		{
			var state = BoxComplex.State.None;
			if(children.IndexOf(c) < values[BoxComplex.State.Three])
				state = BoxComplex.State.Three;
			else if(children.IndexOf(c) < values[BoxComplex.State.Three] + values[BoxComplex.State.Two])
				state = BoxComplex.State.Two;
			else if(children.IndexOf(c) < values[BoxComplex.State.Three] + values[BoxComplex.State.Two] + values[BoxComplex.State.One])
				state = BoxComplex.State.One;
			
			var box = c.GetChild<BoxComplex>(0);
			box.CurrentState = state;
			box.updateTexture();
		}
	}
}
