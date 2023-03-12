using Godot;
using System;
using System.Linq;

namespace OCSM.Nodes.CoD.CtL
{
	public partial class ActionOptionButton : CustomOption
	{
		public enum Action
		{
			Reflexive = 1,
			Instant,
			Extended,
			Simple,
			Contested,
			Resisted
		}
		
		public override void _Ready()
		{
			refreshMetadata();
		}
		
		protected override void refreshMetadata()
		{
			replaceItems(Enum.GetValues<Action>()
				.ToList()
				.Select(a => Enum.GetName<Action>(a))
				.ToList());
		}
	}
}
