using Godot;
using System;

namespace OCSM.Nodes.CoD.CtL
{
	public partial class ActionOptionButton : OptionButton
	{
		[Export]
		public bool EmptyOption { get; private set; } = true;
		
		public enum Action
		{
			Reflexive = 1,
			Instant,
			Extended,
			Simple,
			Contested,
			Resisted
		}
		
		public static int GetActionIndex(string action)
		{
			var index = 0;
			Action value;
			if(Enum.TryParse<Action>(action, true, out value))
				index = (int)value;
			return index;
		}
		
		public override void _Ready()
		{
			if(EmptyOption)
				AddItem("");
			
			AddItem("Reflexive");
			AddItem("Instant");
			AddItem("Extended");
			AddItem("Simple");
			AddItem("Contested");
			AddItem("Resisted");
		}
	}
}
