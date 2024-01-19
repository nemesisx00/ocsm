using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Ocsm.Nodes;

[GlobalClass]
public partial class TrackComplex : GridContainer
{
	public const int DefaultMax = 5;
	public const int MinimumMax = 1;
	
	[Signal]
	public delegate void ValueChangedEventHandler(Transport<Dictionary<StatefulButton.States, int>> values);
	
	[Export]
	public int Max { get; set; } = DefaultMax;
	
	[Export]
	public bool TwoState { get; set; } = false;
	
	public Dictionary<StatefulButton.States, int> Values
	{
		get
		{
			Dictionary<StatefulButton.States, int> values = new()
			{
				{ StatefulButton.States.One, 0 },
				{ StatefulButton.States.Two, 0 },
				{ StatefulButton.States.Three, 0 }
			};
			
			foreach (StatefulButton c in GetChildren().Cast<StatefulButton>())
			{
				switch (c.State)
				{
					case StatefulButton.States.One:
						values[StatefulButton.States.One]++;
						break;
					case StatefulButton.States.Two:
						values[StatefulButton.States.Two]++;
						break;
					case StatefulButton.States.Three:
						values[StatefulButton.States.Three]++;
						break;
				}
			}

			return values;
		}
		
		set => refreshButtons(value);
	}
	
	public override void _Ready() => UpdateMax(Max);
	
	public void UpdateMax(int max = 1)
	{
		Max = max;
		if(Max < MinimumMax)
			Max = MinimumMax;
		
		var children = GetChildren();
		if(children.Count < Max)
		{
			var resource = GD.Load<PackedScene>(ScenePaths.StatefulButton);
			for(var i = children.Count; i < Max; i++)
			{
				var instance = resource.Instantiate<StatefulButton>();
				instance.TwoState = TwoState;
				AddChild(instance);
				instance.StateChanged += handleStatefulButton;
			}
		}
		else
		{
			foreach(Node c in children)
			{
				if(children.IndexOf(c) >= Max)
					c.QueueFree();
			}
		}
	}
	
	private void handleStatefulButton(StatefulButton box)
	{
		refreshButtons(Values, true);
		_ = EmitSignal(SignalName.ValueChanged, new Transport<Dictionary<StatefulButton.States, int>>(Values));
	}
	
	private void refreshButtons(Dictionary<StatefulButton.States, int> values, bool refreshTextures = false)
	{
		var children = GetChildren();
		if(TwoState)
		{
			for(var i = 0; i < children.Count; i++)
			{
				var c = children[i] as StatefulButton;
				if(i < values[StatefulButton.States.Two])
					c.State = StatefulButton.States.Two;
				else if(i < values[StatefulButton.States.Two] + values[StatefulButton.States.One])
					c.State = StatefulButton.States.One;
				else
					c.State = StatefulButton.States.None;
				
				if(refreshTextures)
					c.UpdateTexture();
			}
		}
		else
		{
			for(var i = 0; i < children.Count; i++)
			{
				var c = children[i] as StatefulButton;
				if(i < values[StatefulButton.States.Three])
					c.State = StatefulButton.States.Three;
				else if(i < values[StatefulButton.States.Three] + values[StatefulButton.States.Two])
					c.State = StatefulButton.States.Two;
				else if(i < values[StatefulButton.States.Three] + values[StatefulButton.States.Two] + values[StatefulButton.States.One])
					c.State = StatefulButton.States.One;
				else
					c.State = StatefulButton.States.None;
				
				if(refreshTextures)
					c.UpdateTexture();
			}
		}
	}
}
