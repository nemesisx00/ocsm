using System;

namespace Ocsm;

/**
<summary>
Class providing convenience methods for evaluating logic.
</summary>
*/
public static class Logic
{
	/**
	<summary>
	Determine if <c>o1</c> and <c>o2</c> are equal, accounting for null values.
	</summary>
	<remarks>
	Null values are only considered equal if both <c>o1</c> and <c>o2</c> are null.
	</remarks>
	<typeparam name="T">Any type that extends <c>System.Object</c>.</typeparam>
	<param name="o1">The first object being compared.</param>
	<param name="o2">The second object being compared.</param>
	<returns>The boolean value resulting from the logical evaluation.</returns>
	*/
	public static bool AreEqualOrNull<T>(T o1, T o2)
		=> (o1 is not null && o1.Equals(o2))
			|| (o1 is null && o2 is null);
	
	/**
	<summary>
	Determine if <c>o1</c> and <c>o2</c> are equal, accounting for null values.
	</summary>
	<remarks>
	Null values are only considered equal if both <c>o1</c> and <c>o2</c> are null.
	</remarks>
	<typeparam name="T">Any type that extends <c>System.Object</c>.</typeparam>
	<param name="o1">The first nullable object being compared.</param>
	<param name="o2">The second nullable object being compared.</param>
	<returns>The boolean value resulting from the logical evaluation.</returns>
	*/
	public static bool AreEqualOrNull<T>(T? o1, T? o2)
			where T : struct
		=> (o1 is not null && o1.Equals(o2))
			|| (o1 is null && o2 is null);
	
	/**
	<summary>
	Compare <c>o1</c> and <c>o2</c>, accounting for null values.
	</summary>
	<remarks>
	Null values are considered to be equal to other null values and greater than
	non-null values for the purposes of sorting null values to the end of a list.
	</remarks>
	<typeparam name="T">Any type that implements <c>IComparable</c>.</typeparam>
	<param name="instance">The first nullable object being compared</param>
	<param name="other">The second nullable object being compared</param>
	<returns>
	A value that indicates the relative order of the objects being compared.
	The return value has these meanings:
	<para>Less than zero – instance precedes other in the sort order.</para>
	<para>Zero – instance occurs in the same position in the sort order as other.</para>
	<para>Greater than zero – instance follows other in the sort order.</para>
	</returns>
	*/
	public static int CompareNullables<T>(T instance, T other)
			where T : IComparable<T>
		=> instance is not null
			? instance.CompareTo(other)
			: other is not null ? 1 : 0;
	
	/**
	<summary>
	Compare <c>o1</c> and <c>o2</c>, accounting for null values.
	</summary>
	<remarks>
	Null values are considered to be equal to other null values and greater than
	non-null values for the purposes of sorting null values to the end of a list.
	</remarks>
	<typeparam name="T">
	Any type that is a struct and implements <c>IComparable</c>.
	</typeparam>
	<param name="instance">The first nullable object being compared</param>
	<param name="other">The second nullable object being compared</param>
	<returns>
	A value that indicates the relative order of the objects being compared.
	The return value has these meanings:
	<para>Less than zero – instance precedes other in the sort order.</para>
	<para>Zero – instance occurs in the same position in the sort order as other.</para>
	<para>Greater than zero – instance follows other in the sort order.</para>
	</returns>
	*/
	public static int CompareNullables<T>(T? instance, T? other)
			where T : struct, IComparable<T>
		=> instance is not null
			? ((T)instance).CompareTo(other ?? default)
			: other is not null ? 1 : 0;
}
