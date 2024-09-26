
namespace Ocsm;

public static class StringUtilities
{
	public static string FormatModifier(double value) => value < 0
		? value.ToString()
		: $"+{value}";
	
	public static string FormatModifier(int value) => FormatModifier((double)value);
}
