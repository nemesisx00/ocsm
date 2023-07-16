using Godot;
using System.Collections.Generic;
using Ocsm.Dnd.Fifth;

namespace Ocsm.Nodes.Dnd.Fifth
{
	public partial class AbilityScores : Container
	{
		public void initialize<T>(List<Ability> abilities, AbilityColumn.AbilityChangedEventHandler handler)
			where T: AbilityColumn
		{
			abilities.ForEach(a => InitAbilityColumn(GetNode<AbilityColumn>("%" + a.Name), a, handler));
		}
		
		public void initialize<T>(List<Ability> abilities, AbilityRow.AbilityChangedEventHandler handler)
			where T: AbilityRow
		{
			abilities.ForEach(a => InitAbilityRow(GetNode<AbilityRow>("%" + a.Name), a, handler));
		}
		
		protected void InitAbilityColumn(AbilityColumn node, Ability initialValue, AbilityColumn.AbilityChangedEventHandler handler)
		{
			if(node is AbilityColumn)
			{
				if(initialValue is Ability)
				{
					node.Ability = initialValue;
					node.refresh();
				}
				node.AbilityChanged += handler;
			}
		}
		
		protected void InitAbilityRow(AbilityRow node, Ability initialValue, AbilityRow.AbilityChangedEventHandler handler)
		{
			if(node is AbilityRow)
			{
				if(initialValue is Ability)
				{
					node.Ability = initialValue;
					node.refresh();
				}
				node.AbilityChanged += handler;
			}
		}
	}
}
