using System;

namespace Ocsm.Dnd.Fifth;

public class CoinPurse() : IEquatable<CoinPurse>
{
	public bool AutoConvert { get; set; }
	public Currency PreferredCurrency { get; set; } = Currency.Gold;
	
	public int Copper
	{
		get => copper;
		set => SetCurrency(Currency.Copper, value);
	}
	
	public int Electrum
	{
		get => electrum;
		set => SetCurrency(Currency.Electrum, value);
	}
	
	public int Gold
	{
		get => gold;
		set => SetCurrency(Currency.Gold, value);
	}
	
	public int Platinum
	{
		get => platinum;
		set => SetCurrency(Currency.Platinum, value);
	}
	
	public int Silver
	{
		get => silver;
		set => SetCurrency(Currency.Silver, value);
	}
	
	private int copper;
	private int electrum;
	private int gold;
	private int platinum;
	private int silver;
	
	/**
	<summary>
	Convert all currency in the coin purse into the specified currency, leaving
	the remainders as their original currencies.
	</summary>
	*/
	public void Convert(Currency? currency = null)
	{
		Currency to = currency ?? PreferredCurrency;
		
		var cop = CurrencyConverter.Convert(Copper, Currency.Copper, to);
		var sil = CurrencyConverter.Convert(Silver, Currency.Silver, to);
		var ele = CurrencyConverter.Convert(Electrum, Currency.Electrum, to);
		var gol = CurrencyConverter.Convert(Gold, Currency.Gold, to);
		var pla = CurrencyConverter.Convert(Platinum, Currency.Platinum, to);
		
		Copper = cop.Remainder;
		Silver = sil.Remainder;
		Electrum = ele.Remainder;
		Gold = gol.Remainder;
		Platinum = pla.Remainder;
		
		switch(to)
		{
			case Currency.Copper:
				Copper = cop.Result + sil.Result + ele.Result + gol.Result + pla.Result;
				break;
			
			case Currency.Silver:
				Silver = cop.Result + sil.Result + ele.Result + gol.Result + pla.Result;
				break;
			
			case Currency.Electrum:
				Electrum = cop.Result + sil.Result + ele.Result + gol.Result + pla.Result;
				break;
			
			case Currency.Gold:
				Gold = cop.Result + sil.Result + ele.Result + gol.Result + pla.Result;
				break;
			
			case Currency.Platinum:
				Platinum = cop.Result + sil.Result + ele.Result + gol.Result + pla.Result;
				break;
		}
	}
	
	public bool Equals(CoinPurse other) => Copper == other?.Copper
		&& Electrum == other?.Electrum
		&& Gold == other?.Gold
		&& Platinum == other?.Platinum
		&& Silver == other?.Silver;
	
	public override bool Equals(object obj) => Equals(obj as CoinPurse);
	public override int GetHashCode() => HashCode.Combine(Copper, Electrum, Gold, Platinum, Silver);
	
	public void SetCurrency(Currency currency, int value)
	{
		var amount = value;
		if(amount < 0)
			amount = 0;
		
		switch(currency)
		{
			case Currency.Copper:
				copper = amount;
				break;
			
			case Currency.Silver:
				silver = amount;
				break;
			
			case Currency.Electrum:
				electrum = amount;
				break;
			
			case Currency.Gold:
				gold = amount;
				break;
			
			case Currency.Platinum:
				platinum = amount;
				break;
		}
	}
}
