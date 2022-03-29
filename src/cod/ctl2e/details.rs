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

/// The value of the bonus to initiative and speed provided by the Beast Seeming Blessing.
pub const BeastBonus: usize = 3;

/// The possible Seemings of a Changeling: The Lost 2e Changeling.
#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, Serialize, PartialOrd, Ord)]
pub enum Seeming
{
	Beast,
	Darkling,
	Elemental,
	Fairest,
	Ogre,
	Wizened,
}

/// The Detail fields of a Changeling: The Lost 2e Changeling.
#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, Serialize, PartialOrd, Ord)]
pub enum DetailType
{
	Chronicle,
	Concept,
	Court,
	Kith,
	Needle,
	Seeming,
	Thread,
	Name,
	Player,
}

impl DetailType
{
	/// Generate a collection mapping each `DetailField` to a default value.
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
		expected.insert(DetailType::Chronicle, "".to_string());
		expected.insert(DetailType::Concept, "".to_string());
		expected.insert(DetailType::Court, "".to_string());
		expected.insert(DetailType::Kith, "".to_string());
		expected.insert(DetailType::Name, "".to_string());
		expected.insert(DetailType::Needle, "".to_string());
		expected.insert(DetailType::Player, "".to_string());
		expected.insert(DetailType::Seeming, "".to_string());
		expected.insert(DetailType::Thread, "".to_string());
		let result = DetailType::asMap();
		
		assert_eq!(expected, result);
	}
}
