using Godot;
using OCSM.CoD;

namespace OCSM.Nodes.CoD
{
	public partial class SkillOptionButton : OptionButton
	{
		[Export]
		public bool emptyOption = true;
		
		public override void _Ready()
		{
			if(emptyOption)
				AddItem("");
			
			Skill.asList()
				.ForEach(s => AddItem(s.Name));
		}
	}
}
