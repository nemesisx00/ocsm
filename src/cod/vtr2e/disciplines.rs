#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::BTreeMap;
use serde::{
	Deserialize,
	Serialize,
};
use std::iter::Iterator;
use strum::IntoEnumIterator;
use strum_macros::{
	AsRefStr,
	EnumCount,
	EnumIter
};

#[derive(AsRefStr, Clone, Copy, Debug, EnumCount, EnumIter, Eq, Hash, PartialEq, Deserialize, Serialize, PartialOrd, Ord)]
pub enum DisciplineType
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

impl DisciplineType
{
	pub fn asMap() -> BTreeMap<DisciplineType, String>
	{
		let mut map = BTreeMap::<DisciplineType, String>::new();
		for dt in DisciplineType::iter()
		{
			map.insert(dt, dt.as_ref().to_string());
		}
		return map;
	}
}

#[derive(Clone, Debug, Default, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub struct Devotion
{
	#[serde(default)]
	pub action: String,
	#[serde(default)]
	pub cost: String,
	#[serde(default)]
	pub dicePool: String,
	#[serde(default)]
	pub disciplines: String,
	#[serde(default)]
	pub duration: String,
	#[serde(default)]
	pub name: String,
}

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
