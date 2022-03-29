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
use crate::cod::ctl2e::details::Seeming;

/// The Regalia available to a Changeling: The Lost 2e Changeling.
#[derive(AsRefStr, Clone, Copy, Debug, EnumCount, EnumIter, Eq, Hash, PartialEq, Deserialize, Serialize, PartialOrd, Ord)]
pub enum Regalia
{
	Crown,
	Jewels,
	Mirror,
	Shield,
	Steed,
	Sword,
}

impl Regalia
{
	/// Generate a collection mapping each `Regalia` to its human-readable label.
	pub fn asMap() -> BTreeMap<Regalia, String>
	{
		let mut map = BTreeMap::<Regalia, String>::new();
		for dt in Regalia::iter()
		{
			map.insert(dt, dt.as_ref().to_string());
		}
		return map;
	}
	
	/// Get the default Favored Regalia for the designated `Seeming`.
	pub fn getBySeeming(seeming: Seeming) -> Self
	{
		return match seeming
		{
			Seeming::Beast => { Regalia::Steed }
			Seeming::Darkling => { Regalia::Mirror }
			Seeming::Elemental => { Regalia::Sword }
			Seeming::Fairest => { Regalia::Crown }
			Seeming::Ogre => { Regalia::Shield }
			Seeming::Wizened => { Regalia::Jewels }
		};
	}
}

/// Data structure defining a single Changeling: The Lost 2e Contract.
#[derive(Clone, Debug, Default, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub struct Contract
{
	#[serde(default)]
	pub action: String,
	#[serde(default)]
	pub cost: String,
	#[serde(default)]
	pub dicePool: String,
	#[serde(default)]
	pub regalia: String,
	#[serde(default)]
	pub duration: String,
	#[serde(default)]
	pub name: String,
}

/// The fields of a Changeling: The Lost 2e Contract.
#[derive(AsRefStr, Clone, Copy, Debug, EnumCount, EnumIter, Eq, PartialEq, Deserialize, Serialize, PartialOrd, Ord)]
pub enum ContractField
{
	Action,
	Cost,
	DicePool,
	Regalia,
	Duration,
	Name,
}
