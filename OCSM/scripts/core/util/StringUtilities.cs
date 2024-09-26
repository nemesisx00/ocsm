
namespace Ocsm;

public static class StringUtilities
{
	public static string FormatModifier(int value) => value < 0
		? value.ToString()
		: $"+{value}";
}
