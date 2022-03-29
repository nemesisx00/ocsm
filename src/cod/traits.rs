#![allow(non_snake_case, non_upper_case_globals)]

use std::{
	collections::BTreeMap,
	iter::Iterator
};
use serde::{
	Serialize,
	Deserialize,
};
use strum::IntoEnumIterator;
use strum_macros::{
	AsRefStr,
	EnumCount,
	EnumIter
};

/// The possible types of a Chronicles of Darkness Trait.
#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, Serialize, PartialOrd, Ord)]
pub enum BaseTraitType
{
	Mental,
	Physical,
	Social,
}

/// The Attributes of a Chronicles of Darkness character.
#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, Serialize, PartialOrd, Ord)]
pub enum CoreAttributeType
{
	//Ordering according to how they're typically ordered on the sheet to make generating the UI easier
	Intelligence,
	Wits,
	Resolve,
	Strength,
	Dexterity,
	Stamina,
	Presence,
	Manipulation,
	Composure,
}

impl CoreAttributeType
{
	/// Generates a collection mapping `CoreAttributeType` to a default `usize`.
	/// 
	/// Used to represent a Chronicles of Darkness character's current Attribute values.
	pub fn asMap() -> BTreeMap<Self, usize>
	{
		let mut map = BTreeMap::<Self, usize>::new();
		for cat in Self::iter()
		{
			map.insert(cat, 1);
		}
		return map;
	}
	
	/// Generates the subset of Attributes defined as Mental Traits.
	pub fn mental() -> Vec<Self>
	{
		return vec![
			Self::Intelligence,
			Self::Wits,
			Self::Resolve,
		];
	}
	
	/// Generates the subset of Attributes defined as Physical Traits.
	pub fn physical() -> Vec<Self>
	{
		return vec![
			Self::Strength,
			Self::Dexterity,
			Self::Stamina,
		];
	}
	
	/// Generates the subset of Attributes defined as Social Traits.
	pub fn social() -> Vec<Self>
	{
		return vec![
			Self::Presence,
			Self::Manipulation,
			Self::Composure,
		];
	}
}

/// The Skills of a Chronicles of Darkness character.
#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, Serialize, PartialOrd, Ord)]
pub enum CoreSkillType
{
	Academics,
	AnimalKen,
	Athletics,
	Brawl,
	Computer,
	Crafts,
	Drive,
	Empathy,
	Expression,
	Firearms,
	Investigation,
	Intimidation,
	Larceny,
	Medicine,
	Occult,
	Persuasion,
	Politics,
	Science,
	Socialize,
	Stealth,
	Streetwise,
	Subterfuge,
	Survival,
	Weaponry,
}

impl CoreSkillType
{
	/// Generates a collection mapping `CoreSkillType` to a default `usize`.
	/// 
	/// Used to represent a Chronicles of Darkness character's current Skill values.
	pub fn asMap() -> BTreeMap<Self, usize>
	{
		let mut map = BTreeMap::<Self, usize>::new();
		for cst in Self::iter()
		{
			map.insert(cst, 0);
		}
		return map;
	}
	
	/// Generates the subset of Skills defined as Mental Traits.
	pub fn mental() -> Vec<CoreSkillType>
	{
		return vec![
			CoreSkillType::Academics,
			CoreSkillType::Computer,
			CoreSkillType::Crafts,
			CoreSkillType::Investigation,
			CoreSkillType::Medicine,
			CoreSkillType::Occult,
			CoreSkillType::Politics,
			CoreSkillType::Science,
		];
	}
	
	/// Generates the subset of Skills defined as Physical Traits.
	pub fn physical() -> Vec<CoreSkillType>
	{
		return vec![
			CoreSkillType::Athletics,
			CoreSkillType::Brawl,
			CoreSkillType::Drive,
			CoreSkillType::Firearms,
			CoreSkillType::Larceny,
			CoreSkillType::Stealth,
			CoreSkillType::Survival,
			CoreSkillType::Weaponry,
		];
	}
	
	/// Generates the subset of Skills defined as Social Traits.
	pub fn social() -> Vec<CoreSkillType>
	{
		return vec![
			CoreSkillType::AnimalKen,
			CoreSkillType::Empathy,
			CoreSkillType::Expression,
			CoreSkillType::Intimidation,
			CoreSkillType::Persuasion,
			CoreSkillType::Socialize,
			CoreSkillType::Streetwise,
			CoreSkillType::Subterfuge,
		];
	}
	
	/// Generates a human-readable name for the given `CoreSkillType`.
	pub fn getSkillName(coreSkillType: Self) -> String
	{
		return match coreSkillType
		{
			CoreSkillType::AnimalKen => { "Animal Ken".to_string() }
			_ => { coreSkillType.as_ref().to_string() }
		};
	}
}

#[cfg(test)]
mod tests
{
	use super::*;
	
	#[test]
	fn test_CoreAttributeType_asMap()
	{
		let mut expected = BTreeMap::new();
		expected.insert(CoreAttributeType::Intelligence, 1);
		expected.insert(CoreAttributeType::Wits, 1);
		expected.insert(CoreAttributeType::Resolve, 1);
		expected.insert(CoreAttributeType::Strength, 1);
		expected.insert(CoreAttributeType::Dexterity, 1);
		expected.insert(CoreAttributeType::Stamina, 1);
		expected.insert(CoreAttributeType::Composure, 1);
		expected.insert(CoreAttributeType::Manipulation, 1);
		expected.insert(CoreAttributeType::Presence, 1);
		
		let result = CoreAttributeType::asMap();
		
		assert_eq!(expected, result);
	}
	
	#[test]
	fn test_CoreSkillType_getSkillName()
	{
		let pairs = BTreeMap::from([
			("Socialize".to_string(), CoreSkillType::Socialize),
			("Animal Ken".to_string(), CoreSkillType::AnimalKen),
		]);
		
		pairs.iter().for_each(|(s, t)|
		{
			let result = CoreSkillType::getSkillName(*t);
			assert_eq!(*s, result);
		});
	}
	
	#[test]
	fn test_CoreSkillType_asMap()
	{
		let mut expected = BTreeMap::new();
		expected.insert(CoreSkillType::Academics, 0);
		expected.insert(CoreSkillType::AnimalKen, 0);
		expected.insert(CoreSkillType::Athletics, 0);
		expected.insert(CoreSkillType::Brawl, 0);
		expected.insert(CoreSkillType::Computer, 0);
		expected.insert(CoreSkillType::Crafts, 0);
		expected.insert(CoreSkillType::Drive, 0);
		expected.insert(CoreSkillType::Empathy, 0);
		expected.insert(CoreSkillType::Expression, 0);
		expected.insert(CoreSkillType::Firearms, 0);
		expected.insert(CoreSkillType::Investigation, 0);
		expected.insert(CoreSkillType::Intimidation, 0);
		expected.insert(CoreSkillType::Larceny, 0);
		expected.insert(CoreSkillType::Medicine, 0);
		expected.insert(CoreSkillType::Occult, 0);
		expected.insert(CoreSkillType::Persuasion, 0);
		expected.insert(CoreSkillType::Politics, 0);
		expected.insert(CoreSkillType::Science, 0);
		expected.insert(CoreSkillType::Socialize, 0);
		expected.insert(CoreSkillType::Stealth, 0);
		expected.insert(CoreSkillType::Streetwise, 0);
		expected.insert(CoreSkillType::Subterfuge, 0);
		expected.insert(CoreSkillType::Survival, 0);
		expected.insert(CoreSkillType::Weaponry, 0);
		
		let result = CoreSkillType::asMap();
		
		assert_eq!(expected, result);
	}
}
