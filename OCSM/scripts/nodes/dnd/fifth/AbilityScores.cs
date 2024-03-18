using Godot;
using System.Collections.Generic;
using Ocsm.Dnd.Fifth;

namespace Ocsm.Nodes.Dnd.Fifth;

public partial class AbilityScores : Container
{
	public void initialize<T>(List<AbilityInfo> abilities, AbilityColumn.AbilityChangedEventHandler handler)
		where T: AbilityColumn
	{
		abilities.ForEach(a => InitAbilityColumn(GetNode<AbilityColumn>("%" + a.Name), a, handler));
	}
	
	public void initialize<T>(List<AbilityInfo> abilities, AbilityRow.AbilityChangedEventHandler handler)
		where T: AbilityRow
	{
		abilities.ForEach(a => InitAbilityRow(GetNode<AbilityRow>("%" + a.Name), a, handler));
	}
	
	protected void InitAbilityColumn(AbilityColumn node, AbilityInfo initialValue, AbilityColumn.AbilityChangedEventHandler handler)
	{
		if(node is AbilityColumn)
		{
			if(initialValue is AbilityInfo)
			{
				node.Ability = initialValue;
				node.Refresh();
			}
			node.AbilityChanged += handler;
		}
	}
	
	protected void InitAbilityRow(AbilityRow node, AbilityInfo initialValue, AbilityRow.AbilityChangedEventHandler handler)
	{
		if(node is AbilityRow)
		{
			if(initialValue is AbilityInfo)
			{
				node.Ability = initialValue;
				node.Refresh();
			}
			node.AbilityChanged += handler;
		}
	}
}
