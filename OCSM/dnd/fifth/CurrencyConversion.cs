namespace Ocsm.Dnd.Fifth;

public class CurrencyConversion((int, int) pair)
{
	public int Result { get; set; } = pair.Item1;
	public int Remainder { get; set; } = pair.Item2;
}
