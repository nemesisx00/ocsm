namespace Ocsm.Tests.Util;

public class LogicTests
{
	public static IEnumerable<object[]> GetTrueData()
		=> new object[][]
		{
			["Hi", "Hi"],
			[5, 5],
			[null!, null!],
			[new Pair<string, string>() { Key = "fun", Value = "times" } , new Pair<string, string>() { Key = "fun", Value = "times" }],
		};
	
	public static IEnumerable<object[]> GetFalseData()
		=> new object[][]
		{
			["Hi", "Bye"],
			[5, 10],
			[null!, true],
			[new Pair<string, string>() { Key = "fun", Value = "times" } , new Pair<string, string>() { Key = "fail", Value = "times" }],
			[new Pair<string, string>() { Key = "fun", Value = "times" } , new Pair<string, string>() { Key = "fun", Value = "again" }],
		};
	
	[Theory]
	[MemberData(nameof(GetTrueData))]
	public void TestEqual_True<T>(T one, T two) => Assert.True(Logic.AreEqualOrNull(one, two));
	
	[Theory]
	[MemberData(nameof(GetFalseData))]
	public void TestEqual_False<T>(T one, T two) => Assert.False(Logic.AreEqualOrNull(one, two));
}
