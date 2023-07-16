using System;

namespace Ocsm.Dnd.Fifth
{
	public enum Currency { Copper, Silver, Electrum, Gold, Platinum }
	
	public class CurrencyConversion
	{
		public int Result { get; set; }
		public int Remainder { get; set; }
		
		public CurrencyConversion()
		{
			Result = 0;
			Remainder = 0;
		}
	}
	
	public class CurrencyConverter
	{
		public const int CopperSilverRate = 10;
		public const int SilverElectrumRate = 5;
		public const int SilverGoldRate = 10;
		public const int ElectrumGoldRate = 2;
		public const int GoldPlatinumRate = 10;
		
		public static CurrencyConversion convert(int value, Currency from, Currency to)
		{
			int newValue = 0;
			int remainder = 0;
			switch(from)
			{
				case Currency.Copper:
					switch(to)
					{
						case Currency.Silver:
							newValue = value / CopperSilverRate;
							remainder = value % CopperSilverRate;
							break;
						case Currency.Electrum:
							newValue = value / (CopperSilverRate * SilverElectrumRate);
							remainder = value % (CopperSilverRate * SilverElectrumRate);
							break;
						case Currency.Gold:
							newValue = value / (CopperSilverRate * SilverGoldRate);
							remainder = value % (CopperSilverRate * SilverGoldRate);
							break;
						case Currency.Platinum:
							newValue = value / (CopperSilverRate * SilverGoldRate * GoldPlatinumRate);
							remainder = value % (CopperSilverRate * SilverGoldRate * GoldPlatinumRate);
							break;
						case Currency.Copper:
							newValue = value;
							remainder = 0;
							break;
					}
					break;
				
				case Currency.Silver:
					switch(to)
					{
						case Currency.Copper:
							newValue = value * CopperSilverRate;
							remainder = 0;
							break;
						case Currency.Electrum:
							newValue = value / SilverElectrumRate;
							remainder = value % SilverElectrumRate;
							break;
						case Currency.Gold:
							newValue = value / SilverGoldRate;
							remainder = value % SilverGoldRate;
							break;
						case Currency.Platinum:
							newValue = value / (SilverGoldRate * GoldPlatinumRate);
							remainder = value % (SilverGoldRate * GoldPlatinumRate);
							break;
						case Currency.Silver:
							newValue = value;
							remainder = 0;
							break;
					}
					break;
				
				case Currency.Electrum:
					switch(to)
					{
						case Currency.Copper:
							newValue = value * (CopperSilverRate * SilverElectrumRate);
							remainder = 0;
							break;
						case Currency.Silver:
							newValue = value * SilverElectrumRate;
							remainder = 0;
							break;
						case Currency.Gold:
							newValue = value / ElectrumGoldRate;
							remainder = value % ElectrumGoldRate;
							break;
						case Currency.Platinum:
							newValue = value / (ElectrumGoldRate * GoldPlatinumRate);
							remainder = value % (ElectrumGoldRate * GoldPlatinumRate);
							break;
						case Currency.Electrum:
							newValue = value;
							remainder = 0;
							break;
					}
					break;
				
				case Currency.Gold:
					switch(to)
					{
						case Currency.Copper:
							newValue = value * (CopperSilverRate * SilverGoldRate);
							remainder = 0;
							break;
						case Currency.Silver:
							newValue = value * SilverGoldRate;
							remainder = 0;
							break;
						case Currency.Electrum:
							newValue = value * ElectrumGoldRate;
							remainder = 0;
							break;
						case Currency.Platinum:
							newValue = value / GoldPlatinumRate;
							remainder = value % GoldPlatinumRate;
							break;
						case Currency.Gold:
							newValue = value;
							remainder = 0;
							break;
					}
					break;
				
				case Currency.Platinum:
					switch(to)
					{
						case Currency.Copper:
							newValue = value * (CopperSilverRate * SilverGoldRate * GoldPlatinumRate);
							remainder = 0;
							break;
						case Currency.Silver:
							newValue = value * (SilverGoldRate * GoldPlatinumRate);
							remainder = 0;
							break;
						case Currency.Electrum:
							newValue = value * (ElectrumGoldRate * GoldPlatinumRate);
							remainder = 0;
							break;
						case Currency.Gold:
							newValue = value * GoldPlatinumRate;
							remainder = 0;
							break;
						case Currency.Platinum:
							newValue = value;
							remainder = 0;
							break;
					}
					break;
			}
			
			return new CurrencyConversion() { Result = newValue, Remainder = remainder };
		}
	}
}