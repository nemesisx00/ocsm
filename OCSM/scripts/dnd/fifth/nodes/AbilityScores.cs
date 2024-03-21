using Godot;
using System.Collections.Generic;

namespace Ocsm.Dnd.Fifth.Nodes;

public partial class AbilityScores : Container
{
	public void Initialize<T>(List<AbilityInfo> abilities, AbilityColumn.AbilityChangedEventHandler handler)
			where T : AbilityColumn
		=> abilities.ForEach(a => InitAbilityColumn(GetNode<AbilityColumn>($"%{a.AbilityType}"), a, handler));
	
	public void Initialize<T>(List<AbilityInfo> abilities, AbilityRow.AbilityChangedEventHandler handler)
			where T : AbilityRow
		=> abilities.ForEach(a => InitAbilityRow(GetNode<AbilityRow>($"%{a.AbilityType}"), a, handler));
	
	protected static void InitAbilityColumn(AbilityColumn node, AbilityInfo initialValue, AbilityColumn.AbilityChangedEventHandler handler)
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
	
	protected static void InitAbilityRow(AbilityRow node, AbilityInfo initialValue, AbilityRow.AbilityChangedEventHandler handler)
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
