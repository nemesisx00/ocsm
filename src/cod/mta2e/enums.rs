#![allow(non_snake_case, non_upper_case_globals)]

use serde::{
	Deserialize,
	Serialize,
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

/// The Arcana a Mage: The Awakening 2e Mage can learn.
#[derive(AsRefStr, Clone, Copy, Debug, EnumCount, EnumIter, Eq, Hash, PartialEq, Deserialize, Serialize, PartialOrd, Ord)]
pub enum Arcana
{
	Death,
	Fate,
	Forces,
	Life,
	Matter,
	Mind,
	Prime,
	Space,
	Spirit,
	Time,
}

impl Arcana
{
	/// Generates a collection mapping each `Discipline` to its corresponding name.
	pub fn asMap() -> BTreeMap<Arcana, String>
	{
		let mut map = BTreeMap::<Arcana, String>::new();
		for a in Arcana::iter()
		{
			map.insert(a, a.as_ref().to_string());
		}
		return map;
	}
}


/// The Paths available to a Mage: The Awakening 2e Mage.
#[derive(AsRefStr, Clone, Copy, Debug, EnumCount, EnumIter, Eq, Hash, PartialEq, Deserialize, Serialize, PartialOrd, Ord)]
pub enum Path
{
	Acanthus,
	Mastigos,
	Moros,
	Obrimos,
	Thyrsus,
}
