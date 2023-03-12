using Godot;
using OCSM.CoD;

namespace OCSM.Nodes.CoD
{
	public partial class AttributeOptionButton : OptionButton
	{
		[Export]
		public bool emptyOption = true;
		
		public override void _Ready()
		{
			if(emptyOption)
				AddItem(System.String.Empty);
			
			Attribute.asList()
				.ForEach(a => AddItem(a.Name));
		}
	}
}
