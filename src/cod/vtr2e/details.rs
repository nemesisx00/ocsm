#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::BTreeMap;
use serde::{
	Serialize,
	Deserialize,
};
use std::iter::Iterator;
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

/// The possible Detail fields of a Vampire: The Requiem 2e Kindred.
#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, Serialize, PartialOrd, Ord)]
pub enum DetailType
{
	Bloodline,
	Chronicle,
	Clan,
	Concept,
	Covenant,
	Dirge,
	Mask,
	Name,
	Player,
}

impl DetailType
{
	/// Generates a collection mapping each `DetailType` to a default value.
	pub fn asMap() -> BTreeMap<Self, String>
	{
		let mut map = BTreeMap::<Self, String>::new();
		for dt in Self::iter()
		{
			map.insert(dt, "".to_string());
		}
		return map;
	}
}

#[cfg(test)]
mod tests
{
	use super::*;
	
	#[test]
	fn test_DetailType_asMap()
	{
		let mut expected = BTreeMap::new();
		expected.insert(DetailType::Bloodline, "".to_string());
		expected.insert(DetailType::Chronicle, "".to_string());
		expected.insert(DetailType::Clan, "".to_string());
		expected.insert(DetailType::Concept, "".to_string());
		expected.insert(DetailType::Covenant, "".to_string());
		expected.insert(DetailType::Dirge, "".to_string());
		expected.insert(DetailType::Mask, "".to_string());
		expected.insert(DetailType::Name, "".to_string());
		expected.insert(DetailType::Player, "".to_string());
		let result = DetailType::asMap();
		
		assert_eq!(expected, result);
	}
}
