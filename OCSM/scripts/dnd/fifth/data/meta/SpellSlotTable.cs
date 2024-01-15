using System.Collections.Generic;

namespace Ocsm.Dnd.Fifth.Meta;

public sealed class SpellSlots
{
	public static List<int> ForFullCaster(int level = 1)
	{
		return level switch
		{
			 1 => [2],
			 2 => [3],
			 3 => [4, 2],
			 4 => [4, 3],
			 5 => [4, 3, 2],
			 6 => [4, 3, 3],
			 7 => [4, 3, 3, 1],
			 8 => [4, 3, 3, 2],
			 9 => [4, 3, 3, 3, 1],
			10 => [4, 3, 3, 3, 2],
			11 => [4, 3, 3, 3, 2, 1],
			12 => [4, 3, 3, 3, 2, 1],
			13 => [4, 3, 3, 3, 2, 1, 1],
			14 => [4, 3, 3, 3, 2, 1, 1],
			15 => [4, 3, 3, 3, 2, 1, 1, 1],
			16 => [4, 3, 3, 3, 2, 1, 1, 1],
			17 => [4, 3, 3, 3, 2, 1, 1, 1, 1],
			18 => [4, 3, 3, 3, 3, 1, 1, 1, 1],
			19 => [4, 3, 3, 3, 3, 2, 1, 1, 1],
			20 => [4, 3, 3, 3, 3, 2, 2, 1, 1],
			_ => []
		};
	}
	
	public static List<int> ForHalfCaster(int level = 1)
	{
		var realLevel = level / 2;
		if(level > 1 && level % 2 != 0)
			realLevel++;
		
		return ForFullCaster(realLevel);
	}
	
	public static List<int> ForThirdCaster(int level = 1)
	{
		var realLevel = level / 3;
		if(level > 2 && level % 3 != 0)
			realLevel++;
		
		return ForFullCaster(realLevel);
	}
}
