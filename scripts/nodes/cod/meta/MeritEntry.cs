using Godot;
using OCSM;
using OCSM.CoD;

public class MeritEntry : BasicMetadataEntry
{
	[Signal]
	public new delegate void SaveClicked(string name, string description, int value);
	
	private const string DotsName = "Dots";
	
	public override void _Ready()
	{
		base._Ready();
	}
	
	public void loadMerit(Merit merit)
	{
		base.loadEntry(merit);
		
		GetNode<TrackSimple>(NodePathBuilder.SceneUnique(DotsName)).updateValue(merit.Value);
	}
	
	protected override void clearInputs()
	{
		base.clearInputs();
		
		GetNode<TrackSimple>(NodePathBuilder.SceneUnique(DotsName)).updateValue(0);
	}

	protected override void doSave()
	{
		var name = GetNode<LineEdit>(NodePathBuilder.SceneUnique(NameInput)).Text;
		var description = GetNode<TextEdit>(NodePathBuilder.SceneUnique(DescriptionInput)).Text;
		var value = GetNode<TrackSimple>(NodePathBuilder.SceneUnique(DotsName)).Value;
		
		EmitSignal(nameof(SaveClicked), name, description, value);
		clearInputs();
	}
}
