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
				AddItem("");
			
			foreach(var attr in Attribute.asList())
			{
				AddItem(attr.Name);
			}
		}
	}
}
