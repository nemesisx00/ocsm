using System.Collections.Generic;

namespace OCSM.DnD.Fifth.Meta
{
	public sealed class SpellSlots
	{
		public static Dictionary<int, int> forFullCaster(int casterLevel = 1)
		{
			var slots = new Dictionary<int, int>();
			slots.Add(1, 2);
			if(casterLevel == 1)
				return slots;
			
			slots[1]++;
			if(casterLevel == 2)
				return slots;
			
			slots[1]++;
			slots.Add(2, 2);
			if(casterLevel == 3)
				return slots;
			
			slots[2]++;
			if(casterLevel == 4)
				return slots;
			
			slots.Add(3, 2);
			if(casterLevel == 5)
				return slots;
			
			slots[3]++;
			if(casterLevel == 6)
				return slots;
			
			slots.Add(4, 1);
			if(casterLevel == 7)
				return slots;
			
			slots[4]++;
			if(casterLevel == 8)
				return slots;
			
			slots[4]++;
			slots.Add(5, 1);
			if(casterLevel == 9)
				return slots;
			
			slots[5]++;
			if(casterLevel == 10)
				return slots;
			
			slots.Add(6, 1);
			if(casterLevel == 11 || casterLevel == 12)
				return slots;
			
			slots.Add(7, 1);
			if(casterLevel == 13 || casterLevel == 14)
				return slots;
			
			slots.Add(8, 1);
			if(casterLevel == 15 || casterLevel == 16)
				return slots;
			
			slots.Add(9, 1);
			if(casterLevel == 17)
				return slots;
			
			slots[5]++;
			if(casterLevel == 18)
				return slots;
			
			slots[6]++;
			if(casterLevel == 19)
				return slots;
			
			slots[7]++;
			return slots;
		}
		
		public static Dictionary<int, int> forHalfCaster(int casterLevel = 1)
		{
			var slots = new Dictionary<int, int>();
			if(casterLevel == 1)
				return slots;
			
			slots.Add(1, 2);
			if(casterLevel == 2)
				return slots;
			
			slots[1]++;
			if(casterLevel == 3 || casterLevel == 4)
				return slots;
			
			slots[1]++;
			slots.Add(2, 2);
			if(casterLevel == 5 || casterLevel == 6)
				return slots;
			
			slots[2]++;
			if(casterLevel == 7 || casterLevel == 8)
				return slots;
			
			slots.Add(3, 2);
			if(casterLevel == 9 || casterLevel == 10)
				return slots;
			
			slots[3]++;
			if(casterLevel == 11 || casterLevel == 12)
				return slots;
			
			slots.Add(4, 1);
			if(casterLevel == 13 || casterLevel == 14)
				return slots;
			
			slots[4]++;
			if(casterLevel == 15 || casterLevel == 16)
				return slots;
			
			slots[4]++;
			slots.Add(5, 1);
			if(casterLevel == 17 || casterLevel == 18)
				return slots;
			
			slots[5]++;
			return slots;
		}
		
		public static Dictionary<int, int> forThirdCaster(int casterLevel = 1)
		{
			var slots = new Dictionary<int, int>();
			if(casterLevel <= 2)
				return slots;
			
			slots.Add(1, 2);
			if(casterLevel == 3)
				return slots;
			
			slots[1]++;
			if(casterLevel >= 4 && casterLevel <= 6)
				return slots;
			
			slots[1]++;
			slots.Add(2, 2);
			if(casterLevel >= 7 && casterLevel <= 9)
				return slots;
			
			slots[2]++;
			if(casterLevel >= 10 && casterLevel <= 12)
				return slots;
			
			slots.Add(3, 2);
			if(casterLevel >= 13 && casterLevel <= 15)
				return slots;
			
			slots[3]++;
			if(casterLevel >= 16 && casterLevel <= 18)
				return slots;
			
			slots.Add(4, 1);
			return slots;
		}
	}
}
