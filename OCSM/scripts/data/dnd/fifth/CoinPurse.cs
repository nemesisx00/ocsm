using System;

namespace Ocsm.Dnd.Fifth;

public class CoinPurse : IEquatable<CoinPurse>
{
	public bool AutoConvert { get; set; }
	public Currency PreferredCurrency { get; set; }
	
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
	
	
	private int copper = 0;
	private int silver = 0;
	private int electrum = 0;
	private int gold = 0;
	private int platinum = 0;
	
	public CoinPurse()
	{
		AutoConvert = false;
		PreferredCurrency = Currency.Gold;
		
		Copper = 0;
		Silver = 0;
		Electrum = 0;
		Gold = 0;
		Platinum = 0;
	}
	
	public void convert(Currency? currency = null)
	{
		Currency to = currency ?? PreferredCurrency;
		
		var cop = CurrencyConverter.convert(Copper, Currency.Copper, to);
		var sil = CurrencyConverter.convert(Silver, Currency.Silver, to);
		var ele = CurrencyConverter.convert(Electrum, Currency.Electrum, to);
		var gol = CurrencyConverter.convert(Gold, Currency.Gold, to);
		var pla = CurrencyConverter.convert(Platinum, Currency.Platinum, to);
		
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
	
	public bool Equals(CoinPurse coinPurse)
	{
		return coinPurse.Copper.Equals(Copper)
			&& coinPurse.Silver.Equals(Silver)
			&& coinPurse.Electrum.Equals(Electrum)
			&& coinPurse.Gold.Equals(Gold)
			&& coinPurse.Platinum.Equals(Platinum);
	}
}
