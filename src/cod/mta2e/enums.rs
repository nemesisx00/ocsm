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

impl Arcana
{
	pub fn getByName(name: String) -> Option<Self>
	{
		return match Self::asMap().iter().filter(|(_, n)| *n.clone() == name.clone()).next()
		{
			Some((a, _)) => Some(*a),
			None => None
		};
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

#[derive(AsRefStr, Clone, Copy, Debug, EnumCount, EnumIter, Eq, Hash, PartialEq, Deserialize, Serialize, PartialOrd, Ord)]
pub enum PraxisField
{
	Arcanum,
	Level,
	Name,
}

#[derive(AsRefStr, Clone, Copy, Debug, EnumCount, EnumIter, Eq, Hash, PartialEq, Deserialize, Serialize, PartialOrd, Ord)]
pub enum RoteField
{
	Arcanum,
	Creator,
	Level,
	Name,
	Skill,
}

#[derive(AsRefStr, Clone, Copy, Debug, EnumCount, EnumIter, Eq, Hash, PartialEq, Deserialize, Serialize, PartialOrd, Ord)]
pub enum SpellCastingMethod
{
	Improvised,
	Praxis,
	Rote,
}

#[derive(AsRefStr, Clone, Copy, Debug, EnumCount, EnumIter, Eq, Hash, PartialEq, Deserialize, Serialize, PartialOrd, Ord)]
pub enum SpellFactorType
{
	Standard,
	Advanced,
}

impl SpellFactorType
{
	pub fn getByName(name: String) -> Option<Self>
	{
		let mut out = None;
		for sft in Self::iter()
		{
			if sft.as_ref().to_string() == name.to_string()
			{
				out = Some(sft);
			}
		}
		return out;
	}
}

#[derive(AsRefStr, Clone, Copy, Debug, EnumCount, EnumIter, Eq, Hash, PartialEq, Deserialize, Serialize, PartialOrd, Ord)]
pub enum SpellYantras
{
	Concentration,
	Demesne,
	Environment,
	Mudra,
	OrderTool,
	PathTool,
	Runes,
	Sacrament,
	Sympathy,
}

impl SpellYantras
{
	pub fn asStringVec() -> Vec<String>
	{
		let mut out = vec![];
		for sy in Self::iter()
		{
			out.push(Self::getName(sy));
		}
		return out.clone();
	}
	
	pub fn asMap() -> BTreeMap<Self, String>
	{
		let mut out = BTreeMap::new();
		for sy in Self::iter()
		{
			out.insert(sy, Self::getName(sy));
		}
		return out.clone();
	}
	
	pub fn getName(yantra: Self) -> String
	{
		return match yantra
		{
			SpellYantras::OrderTool => "Order Tool".to_string(),
			SpellYantras::PathTool => "Path Tool".to_string(),
			_ => yantra.as_ref().to_string(),
		};
	}
}
