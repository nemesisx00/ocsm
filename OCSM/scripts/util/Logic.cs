
namespace OCSM
{
	/// <summary>
	/// Class providing convenience methods for evaluating logic.
	/// </summary>
	public class Logic
	{
		/// <summary>
		/// Determine if <c>o1</c> and <c>o2</c> are equal, accounting for null values.
		/// </summary>
		/// <remarks>
		/// Null values are only considered equal if both <c>o1</c> and <c>o2</c> are null.
		/// </remarks>
		/// <typeparam name="T">Any type that extends <c>System.Object</c>.</typeparam>
		/// <param name="o1">The first object being compared.</param>
		/// <param name="o2">The second object being compared.</param>
		/// <returns>The boolean value resulting from the logical evaluation.</returns>
		public static bool AreEqualOrNull<T>(T o1, T o2)
		{
			return (
				(o1 is T && o1.Equals(o2))
				|| (!(o1 is T) && !(o2 is T))
			);
		}
		
		/// <summary>
		/// Determine if <c>o1</c> and <c>o2</c> are equal, accounting for null values.
		/// </summary>
		/// <remarks>
		/// Null values are only considered equal if both <c>o1</c> and <c>o2</c> are null.
		/// </remarks>
		/// <typeparam name="T">Any type that extends <c>System.Object</c>.</typeparam>
		/// <param name="o1">The first nullable object being compared.</param>
		/// <param name="o2">The second nullable object being compared.</param>
		/// <returns>The boolean value resulting from the logical evaluation.</returns>
		public static bool AreEqualOrNull<T>(T? o1, T? o2)
			where T: struct
		{
			return (
				(o1 is T && o1.Equals(o2))
				|| (!(o1 is T) && !(o2 is T))
			);
		}
	}
}
