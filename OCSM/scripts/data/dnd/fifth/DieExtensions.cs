namespace Ocsm.Dnd.Fifth;

public static class DieExtensions
{
	/**
	<summary>
	Roll two dice and keep the higher result.
	</summary>
	*/
	public static int Advantage(this Die self)
	{
		var one = self.Roll();
		var two = self.Roll();
		return one > two ? one : two;	
	}
	
	/**
	<summary>
	Roll two dice and keep the lower result.
	</summary>
	*/
	public static int Disadvantage(this Die self)
	{
		var one = self.Roll();
		var two = self.Roll();
		return one < two ? one : two;
	}
}
