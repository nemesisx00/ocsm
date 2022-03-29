#![allow(non_snake_case, non_upper_case_globals)]

use serde::{
	Serialize,
	Deserialize,
};
use std::iter::Iterator;
use strum_macros::{
	AsRefStr,
	EnumCount,
	EnumIter
};
use crate::cod::tracks::Tracker;

/// The Advantages specific to a Changeling: The Lost 2e Changeling.
#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub enum TemplateAdvantageType
{
	Wyrd,
	Clarity,
	Glamour,
}

/// Data structure defining the Advantages of a Changeling: The Lost 2e Changeling.
#[derive(Clone, Debug, Deserialize, Serialize)]
pub struct TemplateAdvantages
{
	#[serde(default)]
	pub wyrd: usize,
	#[serde(default)]
	pub clarity: usize,
	#[serde(default)]
	pub glamour: Tracker,
}

impl Default for TemplateAdvantages
{
	fn default() -> Self
	{
		Self
		{
			wyrd: 1,
			clarity: 7,
			glamour: Tracker::new(10),
		}
	}
}

/// Get the maximum allowable value for Traits based on Wyrd.
pub fn wyrdTraitMax(wyrd: usize) -> usize
{
	return match wyrd
	{
		0 => { 5 }
		1 => { 5 }
		2 => { 5 }
		3 => { 5 }
		4 => { 5 }
		5 => { 5 }
		6 => { 6 }
		7 => { 7 }
		8 => { 8 }
		9 => { 9 }
		10 => { 10 }
		_ => { 10 }
	};
}

/// Get the maximum Glamour capacity based on Wyrd.
pub fn wyrdGlamourMax(wyrd: usize) -> usize
{
	return match wyrd
	{
		0 => { 0 }
		1 => { 10 }
		2 => { 11 }
		3 => { 12 }
		4 => { 13 }
		5 => { 15 }
		6 => { 20 }
		7 => { 25 }
		8 => { 30 }
		9 => { 50 }
		10 => { 75 }
		_ => { 75 }
	};
}

#[allow(dead_code)]
/// Get the maximum Glamour per turn based on Wyrd.
pub fn wyrdGlamourPerTurn(wyrd: usize) -> usize
{
	return match wyrd
	{
		0 => { 0 }
		1 => { 1 }
		2 => { 2 }
		3 => { 3 }
		4 => { 4 }
		5 => { 5 }
		6 => { 6 }
		7 => { 7 }
		8 => { 8 }
		9 => { 10 }
		10 => { 15 }
		_ => { 15 }
	};
}
