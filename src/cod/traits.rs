#![allow(non_snake_case, non_upper_case_globals)]

use std::{
	collections::HashMap,
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

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, Serialize, PartialOrd, Ord)]
pub enum BaseTraitType
{
	Mental,
	Physical,
	Social,
}

//Ordering according to how they're typically ordered on the sheet to make generating the UI easier
#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, Serialize, PartialOrd, Ord)]
pub enum BaseAttributeType
{
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

impl BaseAttributeType
{
	pub fn asMap() -> HashMap<Self, usize>
	{
		let mut map = HashMap::<Self, usize>::new();
		for bat in Self::iter()
		{
			map.insert(bat, 1);
		}
		return map;
	}
	
	pub fn mental() -> Vec<Self>
	{
		return vec![
			Self::Intelligence,
			Self::Wits,
			Self::Resolve,
		];
	}
	
	pub fn physical() -> Vec<Self>
	{
		return vec![
			Self::Strength,
			Self::Dexterity,
			Self::Stamina,
		];
	}
	
	pub fn social() -> Vec<Self>
	{
		return vec![
			Self::Presence,
			Self::Manipulation,
			Self::Composure,
		];
	}
}

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, Serialize, PartialOrd, Ord)]
pub enum BaseSkillType
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

impl BaseSkillType
{
	pub fn asMap() -> HashMap<Self, usize>
	{
		let mut map = HashMap::<Self, usize>::new();
		for bst in Self::iter()
		{
			map.insert(bst, 0);
		}
		return map;
	}
	
	pub fn mental() -> Vec<BaseSkillType>
	{
		return vec![
			BaseSkillType::Academics,
			BaseSkillType::Computer,
			BaseSkillType::Crafts,
			BaseSkillType::Investigation,
			BaseSkillType::Medicine,
			BaseSkillType::Occult,
			BaseSkillType::Politics,
			BaseSkillType::Science,
		];
	}
	
	pub fn physical() -> Vec<BaseSkillType>
	{
		return vec![
			BaseSkillType::Athletics,
			BaseSkillType::Brawl,
			BaseSkillType::Drive,
			BaseSkillType::Firearms,
			BaseSkillType::Larceny,
			BaseSkillType::Stealth,
			BaseSkillType::Survival,
			BaseSkillType::Weaponry,
		];
	}
	
	pub fn social() -> Vec<BaseSkillType>
	{
		return vec![
			BaseSkillType::AnimalKen,
			BaseSkillType::Empathy,
			BaseSkillType::Expression,
			BaseSkillType::Intimidation,
			BaseSkillType::Persuasion,
			BaseSkillType::Socialize,
			BaseSkillType::Streetwise,
			BaseSkillType::Subterfuge,
		];
	}
	
	pub fn getSkillName(baseSkillType: Self) -> String
	{
		return match baseSkillType
		{
			BaseSkillType::AnimalKen => { "Animal Ken".to_string() }
			_ => { baseSkillType.as_ref().to_string() }
		};
	}
}

#[cfg(test)]
mod tests
{
	use super::*;
	
	#[test]
	fn test_BaseAttributeType_asMap()
	{
		let mut expected = HashMap::new();
		expected.insert(BaseAttributeType::Intelligence, 1);
		expected.insert(BaseAttributeType::Wits, 1);
		expected.insert(BaseAttributeType::Resolve, 1);
		expected.insert(BaseAttributeType::Strength, 1);
		expected.insert(BaseAttributeType::Dexterity, 1);
		expected.insert(BaseAttributeType::Stamina, 1);
		expected.insert(BaseAttributeType::Composure, 1);
		expected.insert(BaseAttributeType::Manipulation, 1);
		expected.insert(BaseAttributeType::Presence, 1);
		
		let result = BaseAttributeType::asMap();
		
		assert_eq!(expected, result);
	}
	
	#[test]
	fn test_BaseSkillType_getSkillName()
	{
		let pairs = HashMap::from([
			("Socialize".to_string(), BaseSkillType::Socialize),
			("Animal Ken".to_string(), BaseSkillType::AnimalKen),
		]);
		
		pairs.iter().for_each(|(s, t)|
		{
			let result = BaseSkillType::getSkillName(*t);
			assert_eq!(*s, result);
		});
	}
	
	#[test]
	fn test_BaseSkillType_asMap()
	{
		let mut expected = HashMap::new();
		expected.insert(BaseSkillType::Academics, 0);
		expected.insert(BaseSkillType::AnimalKen, 0);
		expected.insert(BaseSkillType::Athletics, 0);
		expected.insert(BaseSkillType::Brawl, 0);
		expected.insert(BaseSkillType::Computer, 0);
		expected.insert(BaseSkillType::Crafts, 0);
		expected.insert(BaseSkillType::Drive, 0);
		expected.insert(BaseSkillType::Empathy, 0);
		expected.insert(BaseSkillType::Expression, 0);
		expected.insert(BaseSkillType::Firearms, 0);
		expected.insert(BaseSkillType::Investigation, 0);
		expected.insert(BaseSkillType::Intimidation, 0);
		expected.insert(BaseSkillType::Larceny, 0);
		expected.insert(BaseSkillType::Medicine, 0);
		expected.insert(BaseSkillType::Occult, 0);
		expected.insert(BaseSkillType::Persuasion, 0);
		expected.insert(BaseSkillType::Politics, 0);
		expected.insert(BaseSkillType::Science, 0);
		expected.insert(BaseSkillType::Socialize, 0);
		expected.insert(BaseSkillType::Stealth, 0);
		expected.insert(BaseSkillType::Streetwise, 0);
		expected.insert(BaseSkillType::Subterfuge, 0);
		expected.insert(BaseSkillType::Survival, 0);
		expected.insert(BaseSkillType::Weaponry, 0);
		
		let result = BaseSkillType::asMap();
		
		assert_eq!(expected, result);
	}
}
