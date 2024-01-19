using Godot;
using System.Linq;

namespace Ocsm.Nodes;

[GlobalClass]
public partial class TrackSimple : GridContainer
{
	public const int DefaultMax = 5;
	public const int DefaultMin = 0;
	
	[Signal]
	public delegate void ValueChangedEventHandler(int value, string name);
	
	[ExportGroup("Values")]
	[Export]
	public bool EnableToggling { get; set; }
	[Export]
	public int Max
	{
		get => max;
		set
		{
			max = value;
			refreshChildren();
			toggleChildren(sanitizeValue(Value, true));
		}
	}
	
	[Export]
	public int Min { get; set; } = DefaultMin;
	
	[Export]
	public int Value
	{
		get => value;
		set => toggleChildren(sanitizeValue(value, true));
	}
	
	[ExportGroup("Textures")]
	[Export]
	public bool UseCircles { get; set; }
	[Export]
	public Texture2D circleActive = GD.Load<Texture2D>(TexturePaths.CircleFill);
	[Export]
	public Texture2D circleInactive = GD.Load<Texture2D>(TexturePaths.CircleEmpty);
	[Export]
	public Texture2D boxActive = GD.Load<Texture2D>(TexturePaths.SlashTwoBox);
	[Export]
	public Texture2D boxInactive = GD.Load<Texture2D>(TexturePaths.BoxBorder);
	
	private int max = DefaultMax;
	private int min = DefaultMin;
	private int value = DefaultMin;
	
	public override void _Ready()
	{
		Max = max;
		
		//Force update the textures
		//TODO: Dig in and find out whether this is really necessary or if it can be avoided
		foreach(var b in GetChildren().Cast<ToggleButton>())
		{
			b.Active = UseCircles ? circleActive : boxActive;
			b.Inactive = UseCircles ? circleInactive : boxInactive;
			b.State = b.State;
		}
	}
	
	private void createNecessaryButtons()
	{
		for(var i = GetChildCount(); i < Max; i++)
		{
			ToggleButton instance = new()
			{
				Active = UseCircles ? circleActive : boxActive,
				Inactive = UseCircles ? circleInactive : boxInactive,
			};
			
			AddChild(instance);
			instance.Toggle += handleToggle;
		}
	}
	
	private void handleToggle(ToggleButton button)
	{
		var value = (int)GetChildren().IndexOf(button);
		toggleChildren(sanitizeValue(++value));
		EmitSignal(SignalName.ValueChanged, value, Name);
	}
	
	private void refreshChildren()
	{
		createNecessaryButtons();
		
		if(GetChildCount() >= Max)
		{
			GetChildren()
				.Select((child, index) => (child, index))
				.Where(pair => pair.index >= Max)
				.ToList()
				.ForEach(pair => pair.child.QueueFree());
		}
	}
	
	private int sanitizeValue(int newValue, bool force = false)
	{
		var safe = newValue;
		
		if(safe < 0)
			safe = 0;
		
		if(safe > Max)
			safe = Max;
		
		if(!force && EnableToggling && safe > 0 && safe == Value)
			safe--;
		
		value = safe;
		
		if(Value < Min)
			value = Min;
		
		_ = EmitSignal(SignalName.ValueChanged, Value);
		return Value;
	}
	
	private void toggleChildren(int value = -1)
	{
		GetChildren().Select((child, index) => (child as ToggleButton, index))
			.ToList()
			.ForEach(pair => pair.Item1.State = pair.index < value);
	}
}
