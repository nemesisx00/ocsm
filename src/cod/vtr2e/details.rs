#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::HashMap;
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

#[allow(dead_code)]
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
	pub fn asMap() -> HashMap<Self, String>
	{
		let mut map = HashMap::<Self, String>::new();
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
		let mut expected = HashMap::new();
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
