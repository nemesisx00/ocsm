using Godot;
using OCSM;

public class SkillOptionButton : OptionButton
{
	[Export]
	public bool emptyOption = true;
	
	public override void _Ready()
	{
		if(emptyOption)
			AddItem("");
		
		foreach(var skill in Skill.asList())
		{
			AddItem(skill.Name);
		}
	}
}
