namespace Ocsm.Dnd.Fifth;

public class CurrencyConverter
{
	public const int CopperToSilver = 10;
	public const int CopperToElectrum = CopperToSilver * SilverToElectrum;
	public const int CopperToGold = CopperToSilver * SilverToGold;
	public const int CopperToPlatinum = CopperToGold * GoldToPlatinum;
	public const int SilverToElectrum = 5;
	public const int SilverToGold = 10;
	public const int SilverToPlatinum = SilverToGold * GoldToPlatinum;
	public const int ElectrumToGold = 2;
	public const int ElectrumToPlatinum = ElectrumToGold * GoldToPlatinum;
	public const int GoldToPlatinum = 10;
	
	public static CurrencyConversion Convert(int value, Currency from, Currency to) => new(from switch
	{
		Currency.Copper => to switch
		{
			Currency.Silver => (value / CopperToSilver, value % CopperToSilver),
			Currency.Electrum => (value / CopperToElectrum, value % CopperToElectrum),
			Currency.Gold => (value / CopperToGold, value % CopperToGold),
			Currency.Platinum => (value / CopperToPlatinum, value % CopperToPlatinum),
			_ => (value, 0),
		},

		Currency.Silver => to switch
		{
			Currency.Copper => (value * CopperToSilver, 0),
			Currency.Electrum => (value / SilverToElectrum, value % SilverToElectrum),
			Currency.Gold => (value / SilverToGold, value % SilverToGold),
			Currency.Platinum => (value / SilverToPlatinum, value % SilverToPlatinum),
			_ => (value, 0),
		},

		Currency.Electrum => to switch
		{
			Currency.Copper => (value * CopperToElectrum, 0),
			Currency.Silver => (value * SilverToElectrum, 0),
			Currency.Gold => (value / ElectrumToGold, value % ElectrumToGold),
			Currency.Platinum => (value / ElectrumToPlatinum, value % ElectrumToPlatinum),
			_ => (value, 0),
		},

		Currency.Gold => to switch
		{
			Currency.Copper => (value * CopperToGold, 0),
			Currency.Silver => (value * SilverToGold, 0),
			Currency.Electrum => (value * ElectrumToGold, 0),
			Currency.Platinum => (value / GoldToPlatinum, value % GoldToPlatinum),
			_ => (value, 0),
		},

		Currency.Platinum => to switch
		{
			Currency.Copper => (value * CopperToPlatinum, 0),
			Currency.Silver => (value * SilverToPlatinum, 0),
			Currency.Electrum => (value * ElectrumToPlatinum, 0),
			Currency.Gold => (value * GoldToPlatinum, 0),
			_ => (value, 0),
		},

		_ => (value, 0),
	});
}
