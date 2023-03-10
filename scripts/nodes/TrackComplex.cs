using Godot;
using System.Collections.Generic;

namespace OCSM.Nodes
{
	public partial class TrackComplex : GridContainer
	{
		[Signal]
		public delegate void ValueChangedEventHandler(Transport<Dictionary<string, long>> values);
		
		[Export]
		public long Max { get; set; } = 5;
		
		public Dictionary<string, long> Values
		{
			get
			{
				var values = new Dictionary<string, long>();
				values.Add(StatefulButton.State.One, 0);
				values.Add(StatefulButton.State.Two, 0);
				values.Add(StatefulButton.State.Three, 0);
				
				foreach(StatefulButton c in GetChildren())
				{
					switch(c.CurrentState)
					{
						case StatefulButton.State.One:
							values[StatefulButton.State.One]++;
							break;
						case StatefulButton.State.Two:
							values[StatefulButton.State.Two]++;
							break;
						case StatefulButton.State.Three:
							values[StatefulButton.State.Three]++;
							break;
						case StatefulButton.State.None:
							break;
					}
				}
				
				return values;
			}
			
			set
			{
				var children = GetChildren();
				foreach(StatefulButton c in children)
				{
					if(children.IndexOf(c) < value[StatefulButton.State.Three])
						c.CurrentState = StatefulButton.State.Three;
					else if(children.IndexOf(c) < value[StatefulButton.State.Three] + value[StatefulButton.State.Two])
						c.CurrentState = StatefulButton.State.Two;
					else if(children.IndexOf(c) < value[StatefulButton.State.Three] + value[StatefulButton.State.Two] + value[StatefulButton.State.One])
						c.CurrentState = StatefulButton.State.One;
				}
				updateBoxes();
			}
		}
		
		public override void _Ready()
		{
			updateMax(Max);
		}
		
		private void handleStatefulButton(StatefulButton box)
		{
			updateBoxes();
			EmitSignal(nameof(ValueChanged), new Transport<Dictionary<string, long>>(Values));
		}
		
		public void updateBoxes()
		{
			var values = Values;
			var children = GetChildren();
			foreach(StatefulButton c in children)
			{
				var state = StatefulButton.State.None;
				if(children.IndexOf(c) < values[StatefulButton.State.Three])
					state = StatefulButton.State.Three;
				else if(children.IndexOf(c) < values[StatefulButton.State.Three] + values[StatefulButton.State.Two])
					state = StatefulButton.State.Two;
				else if(children.IndexOf(c) < values[StatefulButton.State.Three] + values[StatefulButton.State.Two] + values[StatefulButton.State.One])
					state = StatefulButton.State.One;
				
				c.CurrentState = state;
				c.updateTexture();
			}
		}
		
		public void updateMax(long max = 1)
		{
			Max = max;
			if(Max < 1)
				Max = 1;
			
			var children = GetChildren();
			if(children.Count < Max)
			{
				var resource = GD.Load<PackedScene>(Constants.Scene.StatefulButton);
				for(var i = children.Count; i < Max; i++)
				{
					var instance = resource.Instantiate<StatefulButton>();
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
	}
}
