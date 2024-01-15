
using Ocsm.Dnd.Fifth.Meta;

namespace Ocsm.Tests.util;

public class SpellSlotsTests
{
	public static IEnumerable<object[]> GetFullData()
		=> new object[][]
		{
			[1, new int[1] {2}],
			[2, new int[1] {3}],
			[3, new int[2] {4, 2}],
			[4, new int[2] {4, 3}],
			[5, new int[3] {4, 3, 2}],
			[6, new int[3] {4, 3, 3}],
			[7, new int[4] {4, 3, 3, 1}],
			[8, new int[4] {4, 3, 3, 2}],
			[9, new int[5] {4, 3, 3, 3, 1}],
			[10, new int[5] {4, 3, 3, 3, 2}],
			[11, new int[6] {4, 3, 3, 3, 2, 1}],
			[12, new int[6] {4, 3, 3, 3, 2, 1}],
			[13, new int[7] {4, 3, 3, 3, 2, 1, 1}],
			[14, new int[7] {4, 3, 3, 3, 2, 1, 1}],
			[15, new int[8] {4, 3, 3, 3, 2, 1, 1, 1}],
			[16, new int[8] {4, 3, 3, 3, 2, 1, 1, 1}],
			[17, new int[9] {4, 3, 3, 3, 2, 1, 1, 1, 1}],
			[18, new int[9] {4, 3, 3, 3, 3, 1, 1, 1, 1}],
			[19, new int[9] {4, 3, 3, 3, 3, 2, 1, 1, 1}],
			[20, new int[9] {4, 3, 3, 3, 3, 2, 2, 1, 1}],
		};
	
	public static IEnumerable<object[]> GetHalfData()
		=> new object[][]
		{
			[1, Array.Empty<int>()],
			[2, new int[1] {2}],
			[3, new int[1] {3}],
			[4, new int[1] {3}],
			[5, new int[2] {4, 2}],
			[6, new int[2] {4, 2}],
			[7, new int[2] {4, 3}],
			[8, new int[2] {4, 3}],
			[9, new int[3] {4, 3, 2}],
			[10, new int[3] {4, 3, 2}],
			[11, new int[3] {4, 3, 3}],
			[12, new int[3] {4, 3, 3}],
			[13, new int[4] {4, 3, 3, 1}],
			[14, new int[4] {4, 3, 3, 1}],
			[15, new int[4] {4, 3, 3, 2}],
			[16, new int[4] {4, 3, 3, 2}],
			[17, new int[5] {4, 3, 3, 3, 1}],
			[18, new int[5] {4, 3, 3, 3, 1}],
			[19, new int[5] {4, 3, 3, 3, 2}],
			[20, new int[5] {4, 3, 3, 3, 2}],
		};
	
	public static IEnumerable<object[]> GetThirdData()
		=> new object[][]
		{
			[1, Array.Empty<int>()],
			[2, Array.Empty<int>()],
			[3, new int[1] {2}],
			[4, new int[1] {3}],
			[5, new int[1] {3}],
			[6, new int[1] {3}],
			[7, new int[2] {4, 2}],
			[8, new int[2] {4, 2}],
			[9, new int[2] {4, 2}],
			[10, new int[2] {4, 3}],
			[11, new int[2] {4, 3}],
			[12, new int[2] {4, 3}],
			[13, new int[3] {4, 3, 2}],
			[14, new int[3] {4, 3, 2}],
			[15, new int[3] {4, 3, 2}],
			[16, new int[3] {4, 3, 3}],
			[17, new int[3] {4, 3, 3}],
			[18, new int[3] {4, 3, 3}],
			[19, new int[4] {4, 3, 3, 1}],
			[20, new int[4] {4, 3, 3, 1}],
		};
	
	[Theory]
	[MemberData(nameof(GetFullData))]
	public void TestSlots_Full(int level, int[] expected)
	{
		var result = SpellSlots.ForFullCaster(level);
		Assert.Equal([.. expected], result);
	}
	
	[Theory]
	[MemberData(nameof(GetHalfData))]
	public void TestSlots_Half(int level, int[] expected)
	{
		var result = SpellSlots.ForHalfCaster(level);
		Assert.NotNull(result);
		Assert.Equal([.. expected], result);
	}
	
	[Theory]
	[MemberData(nameof(GetThirdData))]
	public void TestSlots_Third(int level, int[] expected)
	{
		var result = SpellSlots.ForThirdCaster(level);
		Assert.Equal([.. expected], result);
	}
}
