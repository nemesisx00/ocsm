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
	/// Generates a collection mapping each `Arcanum` to its corresponding name.
	pub fn asMap() -> BTreeMap<Self, String>
	{
		let mut map = BTreeMap::<Self, String>::new();
		for a in Self::iter()
		{
			map.insert(a, a.as_ref().to_string());
		}
		return map;
	}
	
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
pub enum SpellFactor
{
	Potency,
	Duration,
	//Scale,
	//Range,
}

impl SpellFactor
{
	/// Generates a collection mapping each `Arcanum` to its corresponding name.
	pub fn asMap() -> BTreeMap<Self, String>
	{
		let mut map = BTreeMap::<Self, String>::new();
		for sf in Self::iter()
		{
			map.insert(sf, sf.as_ref().to_string());
		}
		return map;
	}
	
	pub fn getByName(name: String) -> Option<Self>
	{
		let mut out = None;
		for sf in Self::iter()
		{
			if sf.as_ref().to_string() == name.to_string()
			{
				out = Some(sf);
			}
		}
		return out;
	}
}


#[derive(AsRefStr, Clone, Copy, Debug, EnumCount, EnumIter, Eq, Hash, PartialEq, Deserialize, Serialize, PartialOrd, Ord)]
pub enum SpellFactorType
{
	Standard,
	Advanced,
	AdvancedFree,
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
pub enum SpellField
{
	Arcanum,
	Intent,
	Effects,
	Name,
	Practice,
	PrimaryFactor,
	Withstand,
}

#[derive(AsRefStr, Clone, Copy, Debug, EnumCount, EnumIter, Eq, Hash, PartialEq, Deserialize, Serialize, PartialOrd, Ord)]
pub enum SpellPractice
{
	Compelling,
	Knowing,
	Unveiling,
	Ruling,
	Shielding,
	Veiling,
	Fraying,
	Perfecting,
	Weaving,
	Patterning,
	Unraveling,
	Making,
	Unmaking,
}

impl SpellPractice
{
	/// Generates a collection mapping each `Arcanum` to its corresponding name.
	pub fn asMap() -> BTreeMap<Self, String>
	{
		let mut map = BTreeMap::<Self, String>::new();
		for sp in Self::iter()
		{
			map.insert(sp, sp.as_ref().to_string());
		}
		return map;
	}
	
	pub fn getByName(name: String) -> Option<Self>
	{
		let mut out = None;
		for sp in Self::iter()
		{
			if sp.as_ref().to_string() == name.to_string()
			{
				out = Some(sp);
			}
		}
		return out;
	}
	
	pub fn getValue(practice: Option<Self>) -> usize
	{
		if let Some(p) = practice
		{
			return match p
			{
				Self::Compelling => 1,
				Self::Knowing => 1,
				Self::Unveiling => 1,
				Self::Ruling => 2,
				Self::Shielding => 2,
				Self::Veiling => 2,
				Self::Fraying => 3,
				Self::Perfecting => 3,
				Self::Weaving => 3,
				Self::Patterning => 4,
				Self::Unraveling => 4,
				Self::Making => 5,
				Self::Unmaking => 5,
			};
		}
		return 0;
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
