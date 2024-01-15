using Godot;
using System.Collections.Generic;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class AbilityScores : Container
{
	public void Initialize<T>(List<Ability> abilities, AbilityColumn.AbilityChangedEventHandler handler)
		where T: AbilityColumn
	{
		abilities.ForEach(a => InitAbilityColumn(GetNode<AbilityColumn>($"%{a.Name}"), a, handler));
	}
	
	public void Initialize<T>(List<Ability> abilities, AbilityRow.AbilityChangedEventHandler handler)
		where T: AbilityRow
	{
		abilities.ForEach(a => InitAbilityRow(GetNode<AbilityRow>($"%{a.Name}"), a, handler));
	}
	
	protected static void InitAbilityColumn(AbilityColumn node, Ability initialValue, AbilityColumn.AbilityChangedEventHandler handler)
	{
		if(node is not null)
		{
			if(initialValue is not null)
			{
				node.Ability = initialValue;
				node.Refresh();
			}
			
			node.AbilityChanged += handler;
		}
	}
	
	protected static void InitAbilityRow(AbilityRow node, Ability initialValue, AbilityRow.AbilityChangedEventHandler handler)
	{
		if(node is not null)
		{
			if(initialValue is not null)
			{
				node.Ability = initialValue;
				node.Refresh();
			}
			
			node.AbilityChanged += handler;
		}
	}
}
