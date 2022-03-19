#![allow(non_snake_case, non_upper_case_globals)]

const DefaultMax: i8 = 5;

#[derive(Clone, Copy)]
pub enum DamageType
{
	Bashing,
	Bludgeoning,
	Superficial,
	Lethal,
	Aggravated
}

#[derive(Clone, Copy, PartialEq)]
pub struct Tracker
{
	pub max: i8,
	pub superficial: i8,
	pub aggravated: i8
}

impl Tracker
{
	pub fn new(max: i8) -> Tracker
	{
		let finalMax = match max > 0
		{
			true => max,
			false => DefaultMax
		};
		
		return Tracker
		{
			max: finalMax,
			superficial: 0,
			aggravated: 0
		};
	}
	
	pub fn getDamageTotal(self) -> i8 { return self.superficial + self.aggravated; }
	
	pub fn decreaseDamage(mut self, damageType: DamageType)
	{
		match damageType
		{
			DamageType::Superficial => if self.superficial > 0 { self.superficial -= 1; },
			DamageType::Aggravated => if self.aggravated > 0 { self.aggravated -= 1; },
			_ => {}
		}
	}
	
	pub fn increaseDamage(mut self, damageType: DamageType)
	{
		println!("superficial: {}", self.superficial);
		if self.getDamageTotal() < self.max
		{
			match damageType
			{
				DamageType::Superficial => self.superficial += 1,
				DamageType::Aggravated => self.aggravated += 1,
				_ => {}
			}
		}
		else
		{
			if self.superficial > 0
			{
				self.superficial -= 1;
				self.aggravated += 1;
			}
		}
		println!("superficial: {}", self.superficial);
	}
}

impl Default for Tracker
{
	fn default() -> Self { return Tracker::new(DefaultMax); }
}
