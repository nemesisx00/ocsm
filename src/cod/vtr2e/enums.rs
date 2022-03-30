#![allow(non_snake_case, non_upper_case_globals)]

use serde::{
	Serialize,
	Deserialize,
};
use std::{
	collections::BTreeMap,
	iter::Iterator,
};
use strum::IntoEnumIterator;
use strum_macros::{
	AsRefStr,
	EnumCount,
	EnumIter
};

/// The possible Clans of a Vampire: The Requiem 2e Kindred.
#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, Serialize, PartialOrd, Ord)]
pub enum Clan
{
	Daeva,
	Gangrel,
	Mehket,
	Nosferatu,
	Ventrue,
}

/// The possible fields of a `Devotion`.
#[derive(AsRefStr, Clone, Copy, Debug, EnumCount, EnumIter, Eq, PartialEq, Deserialize, Serialize, PartialOrd, Ord)]
pub enum DevotionField
{
	Action,
	Cost,
	DicePool,
	Disciplines,
	Duration,
	Name,
}

/// The Disciplines of a Vampire: The Requiem 2e Kindred.
#[derive(AsRefStr, Clone, Copy, Debug, EnumCount, EnumIter, Eq, Hash, PartialEq, Deserialize, Serialize, PartialOrd, Ord)]
pub enum Discipline
{
	Animalism,
	Auspex,
	Celerity,
	Dominate,
	Majesty,
	Nightmare,
	Obfuscate,
	Protean,
	Resilience,
	Vigor,
}

impl Discipline
{
	/// Generates a collection mapping each `Discipline` to its corresponding name.
	pub fn asMap() -> BTreeMap<Discipline, String>
	{
		let mut map = BTreeMap::<Discipline, String>::new();
		for dt in Discipline::iter()
		{
			map.insert(dt, dt.as_ref().to_string());
		}
		return map;
	}
}
