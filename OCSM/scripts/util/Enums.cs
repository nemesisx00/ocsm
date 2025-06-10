using System;
using System.Linq;

namespace Ocsm;

/// <summary>
/// Class providing convenience methods for evaluating logic.
/// </summary>
public static class Enums
{
	/// <summary>
	/// Convenience method to get an enumeration value based on its name string.
	/// <summary>
	/// <param name="name">The name of the enumeration value to be retrieved.</param>
	/// <returns>
	/// If found, returns the corresponding enumeration value. Otherwise
	/// returns null.
	/// </returns>
	public static T? FromName<T>(string name)
			where T: struct, Enum
		=> Enum.GetValues<T>()
			.Where(e => Enum.GetName(e) == name)
			.FirstOrDefault();
	
	/// <summary>
	/// Convenience method to get an enumeration value based on its label attribute.
	/// <summary>
	/// <param name="label">The label of the enumeration value to be retrieved.</param>
	/// <returns>
	/// If found, returns the corresponding enumeration value. Otherwise
	/// returns null.
	/// </returns>
	public static T? FromLabel<T>(string label)
			where T: struct, Enum
		=> Enum.GetValues<T>()
			.Where(e => e.GetLabel() == label)
			.FirstOrDefault();
	
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
		=> (o1 is not null && o1.Equals(o2))
			|| (o1 is null && o2 is null);
}
