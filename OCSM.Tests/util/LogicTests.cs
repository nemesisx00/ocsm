
namespace OCSM.Tests.util
{
	public class LogicTests
	{
		public static IEnumerable<object[]> GetTrueData()
			=> new object[][]
			{
				new object[] { "Hi", "Hi" },
				new object[] { 5, 5 },
				new object[] { null!, null! },
				new object[] { new Pair() { Key = "fun", Value = "times" } , new Pair() { Key = "fun", Value = "times" } },
			};
		
		public static IEnumerable<object[]> GetFalseData()
			=> new object[][]
			{
				new object[] { "Hi", "Bye" },
				new object[] { 5, 10 },
				new object[] { null!, true },
				new object[] { new Pair() { Key = "fun", Value = "times" } , new Pair() { Key = "fail", Value = "times" } },
				new object[] { new Pair() { Key = "fun", Value = "times" } , new Pair() { Key = "fun", Value = "again" } },
			};
		
		[Theory]
		[MemberData(nameof(GetTrueData))]
		public void TestEqual_True<T>(T one, T two)
		{
			Assert.True(Logic.AreEqualOrNull(one, two));
		}
		
		[Theory]
		[MemberData(nameof(GetFalseData))]
		public void TestEqual_False<T>(T one, T two)
		{
			Assert.False(Logic.AreEqualOrNull(one, two));
		}
	}
}
