#![allow(non_snake_case,non_upper_case_globals,unused_imports,unused_variables)]
#[cfg(test)]

use rand::prelude::*;
use rand::Rng;
use rand::thread_rng;

use serde::{
	Deserialize,
	Serialize
};

#[derive(Clone, Default, Deserialize, Serialize)]
pub struct Roll
{
	rolls: Vec<u64>,
	total: u64
}

#[derive(Clone, Copy, Default)]
pub struct DieRoller { }

impl DieRoller
{
	/// Generate a number as though rolling a die with an arbitrary number of sides.
	/// 
	/// ## Arguments
	/// * `sides` - The number of sides of the die being rolled.
	/// 
	/// ## Return
	/// Returns the rolled value.
	pub fn roll(self, sides: u64) -> u64
	{
		return thread_rng().gen_range(1..=sides);
	}
	
	/// Generate an arbitrary number of numbers as those rolling multiple dice with an arbitrary number of sides.
	/// 
	/// ## Arguments
	/// * `sides` - The number of sides of the die being rolled.
	/// * `quantity` - The amount of dice being rolled.
	/// 
	/// ## Return
	/// Returns the complete `Roll`.
	pub fn rollMultiple(self, sides: u64, quantity: u64) -> Roll
	{
		let mut values: Vec<u64> = Vec::new();
		let mut total: u64 = 0;
		for i in 0..quantity
		{
			let value = self.roll(sides);
			values.push(value);
			total += value;
		}
		
		return Roll { rolls: values, total: total };
	}
}

#[test]
fn dieRollerRolled_d4()
{
	let roller: DieRoller = DieRoller::default();
	let sides: u64 = 4;
	let mut result: u64;
	
	for i in 0..100
	{
		result = roller.roll(sides);
		assert!(0 < result && 5 > result, "Generated Number outside expected range: {}", result);
	}
}

#[test]
fn dieRollerRolledMultiple_d8()
{
	let roller: DieRoller = DieRoller::default();
	let sides: u64 = 8;
	let quantity: u64 = 3;
	
	let mut result: Roll;
	for i in 0..100
	{
		result = roller.rollMultiple(sides, quantity);
		assert!(2 < result.total && 25 > result.total, "Generated Number outside expected range: {}", result.total);
		assert_eq!(3, result.rolls.len());
	}
}
