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

pub const BaseSpeed: usize = 5;

/// The Advantages of a Chronicles of Darkness character.
#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub enum CoreAdvantageType
{
	Defense,
	Health,
	Initiative,
	//Integrity(String),
	Size,
	Speed,
	//Vice(String),
	//Virtue(String),
	Willpower,
}

/// Data structure defining the Advantages of a Chronicles of Darkness character.
#[derive(Clone, Debug, Deserialize, Serialize)]
pub struct CoreAdvantages
{
	#[serde(default)]
	pub defense: usize,
	#[serde(default)]
	pub health: Tracker,
	#[serde(default)]
	pub initiative: usize,
	#[serde(default)]
	pub size: usize,
	#[serde(default)]
	pub speed: usize,
	#[serde(default)]
	pub willpower: Tracker,
}

impl Default for CoreAdvantages
{
	fn default() -> Self
	{
		Self
		{
			defense: 1,
			health: Tracker::new(6),
			initiative: 2,
			size: 5,
			speed: 7,
			willpower: Tracker::new(2),
		}
	}
}
