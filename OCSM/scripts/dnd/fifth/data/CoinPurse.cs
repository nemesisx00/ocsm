using System;

namespace Ocsm.Dnd.Fifth;

public class CoinPurse() : IEquatable<CoinPurse>
{
	public bool AutoConvert { get; set; }
	public Currency PreferredCurrency { get; set; } = Currency.Gold;
	
	public int Copper
	{
		get { return copper; }
		set
		{
			copper = value;
			if(copper < 0)
				copper = 0;
		}
	}
	
	public int Silver
	{
		get { return silver; }
		set
		{
			silver = value;
			if(silver < 0)
				silver = 0;
		}
	}
	
	public int Electrum
	{
		get { return electrum; }
		set
		{
			electrum = value;
			if(electrum < 0)
				electrum = 0;
		}
	}
	
	public int Gold
	{
		get { return gold; }
		set
		{
			gold = value;
			if(gold < 0)
				gold = 0;
		}
	}
	
	public int Platinum
	{
		get { return platinum; }
		set
		{
			platinum = value;
			if(platinum < 0)
				platinum = 0;
		}
	}
	
	private int copper;
	private int silver;
	private int electrum;
	private int gold;
	private int platinum;
	
	public void Convert(Currency? currency = null)
	{
		Currency to = currency ?? PreferredCurrency;
		
		var cop = CurrencyConverter.Convert(Copper, Currency.Copper, to);
		var sil = CurrencyConverter.Convert(Silver, Currency.Silver, to);
		var ele = CurrencyConverter.Convert(Electrum, Currency.Electrum, to);
		var gol = CurrencyConverter.Convert(Gold, Currency.Gold, to);
		var pla = CurrencyConverter.Convert(Platinum, Currency.Platinum, to);
		
		switch(to)
		{
			case Currency.Copper:
				Copper = cop.Result + sil.Result + ele.Result + gol.Result + pla.Result;
				Silver = sil.Remainder;
				Electrum = ele.Remainder;
				Gold = gol.Remainder;
				Platinum = pla.Remainder;
				break;
			
			case Currency.Silver:
				Copper = cop.Remainder;
				Silver = cop.Result + sil.Result + ele.Result + gol.Result + pla.Result;
				Electrum = ele.Remainder;
				Gold = gol.Remainder;
				Platinum = pla.Remainder;
				break;
			
			case Currency.Electrum:
				Copper = cop.Remainder;
				Silver = sil.Remainder;
				Electrum = cop.Result + sil.Result + ele.Result + gol.Result + pla.Result;
				Gold = gol.Remainder;
				Platinum = pla.Remainder;
				break;
			
			case Currency.Gold:
				Copper = cop.Remainder;
				Silver = sil.Remainder;
				Electrum = ele.Remainder;
				Gold = cop.Result + sil.Result + ele.Result + gol.Result + pla.Result;
				Platinum = pla.Remainder;
				break;
			
			case Currency.Platinum:
				Copper = cop.Remainder;
				Silver = sil.Remainder;
				Electrum = ele.Remainder;
				Gold = gol.Remainder;
				Platinum = cop.Result + sil.Result + ele.Result + gol.Result + pla.Result;
				break;
		}
	}
	
	public override bool Equals(object other) => Equals(other as CoinPurse);
	
	public bool Equals(CoinPurse other) => Copper.Equals(other?.Copper)
		&& Silver.Equals(other?.Silver)
		&& Electrum.Equals(other?.Electrum)
		&& Gold.Equals(other?.Gold)
		&& Platinum.Equals(other?.Platinum);

	public override int GetHashCode() => HashCode.Combine(
		Copper,
		Silver,
		Electrum,
		Gold,
		Platinum
	);
}
