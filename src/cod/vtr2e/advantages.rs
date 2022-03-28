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

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub enum TemplateAdvantageType
{
	BloodPotency,
	Humanity,
	Vitae,
}

#[derive(Clone, Debug, Deserialize, Serialize)]
pub struct TemplateAdvantages
{
	#[serde(default)]
	pub bloodPotency: usize,
	#[serde(default)]
	pub humanity: usize,
	#[serde(default)]
	pub vitae: Tracker,
}

impl Default for TemplateAdvantages
{
	fn default() -> Self
	{
		Self
		{
			bloodPotency: 1,
			humanity: 7,
			vitae: Tracker::new(10),
		}
	}
}

pub fn bloodPotencyTraitMax(bloodPotency: usize) -> usize
{
	return match bloodPotency
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

pub fn bloodPotencyVitaeMax(bloodPotency: usize) -> usize
{
	return match bloodPotency
	{
		0 => { 5 }
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
pub fn bloodPotencyVitaePerTurn(bloodPotency: usize) -> usize
{
	return match bloodPotency
	{
		0 => { 1 }
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
