
namespace OCSM
{
	public class Logic
	{
		public static bool AreEqualOrNull<T>(T o1, T o2)
		{
			return (
				(o1 is T && o1.Equals(o2))
				|| (!(o1 is T) && !(o2 is T))
			);
		}
	}
}
