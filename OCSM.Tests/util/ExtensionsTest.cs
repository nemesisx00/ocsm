
namespace OCSM.Tests.util
{
	public class ExtensionsTest
	{
		enum MyEnum
		{
			[Label("Hi")]
			Hi,
			[Label("There")]
			There,
		}
		
		[Fact]
		public void LabelTest()
		{
			Assert.Equal("Hi", MyEnum.Hi.GetLabel());
			Assert.Equal("There", MyEnum.There.GetLabel());
		}
	}
}
