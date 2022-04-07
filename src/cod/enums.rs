#![allow(non_snake_case, non_upper_case_globals)]

use serde::{
	Serialize,
	Deserialize,
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

/// The fields of a Chronicles of Darkness character's Active Ability.
#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, PartialEq, PartialOrd, Serialize, Ord)]
pub enum ActiveAbilityField
{
	Action,
	Cost,
	Description,
	DicePool,
	Duration,
	Effects,
	Name,
	Requirements,
}

/// The Advantages of a Chronicles of Darkness character.
#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, PartialEq, PartialOrd, Serialize, Ord)]
pub enum CoreAdvantage
{
	Defense,
	Health,
	Initiative,
	Integrity,
	Power,
	Resource,
	Size,
	Speed,
	Willpower,
}

/// The Attributes of a Chronicles of Darkness character.
#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum CoreAttribute
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

impl CoreAttribute
{
	/// Generates a collection mapping `CoreAttribute` to a default `usize`.
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
	
	pub fn getByName(name: String) -> Option<Self>
	{
		return match Self::asMap().iter().filter(|(ca, _)| ca.as_ref().to_string() == name.clone()).next()
		{
			Some((ca, _)) => Some(*ca),
			None => None
		};
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

/// The Details of a Chronicles of Darkness character.
#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum CoreDetail
{
	Chronicle,
	Concept,
	Faction,
	Name,
	Player,
	TypePrimary,
	TypeSecondary,
	Virtue,
	Vice,
}

impl CoreDetail
{
	pub fn asMap() -> BTreeMap<Self, String>
	{
		let mut map = BTreeMap::<Self, String>::new();
		for cst in Self::iter()
		{
			map.insert(cst, "".to_string());
		}
		return map;
	}
}

/// The Skills of a Chronicles of Darkness character.
#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum CoreSkill
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

impl CoreSkill
{
	/// Generates a collection mapping `CoreSkill` to a default `usize`.
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
	pub fn mental() -> Vec<CoreSkill>
	{
		return vec![
			CoreSkill::Academics,
			CoreSkill::Computer,
			CoreSkill::Crafts,
			CoreSkill::Investigation,
			CoreSkill::Medicine,
			CoreSkill::Occult,
			CoreSkill::Politics,
			CoreSkill::Science,
		];
	}
	
	/// Generates the subset of Skills defined as Physical Traits.
	pub fn physical() -> Vec<CoreSkill>
	{
		return vec![
			CoreSkill::Athletics,
			CoreSkill::Brawl,
			CoreSkill::Drive,
			CoreSkill::Firearms,
			CoreSkill::Larceny,
			CoreSkill::Stealth,
			CoreSkill::Survival,
			CoreSkill::Weaponry,
		];
	}
	
	/// Generates the subset of Skills defined as Social Traits.
	pub fn social() -> Vec<CoreSkill>
	{
		return vec![
			CoreSkill::AnimalKen,
			CoreSkill::Empathy,
			CoreSkill::Expression,
			CoreSkill::Intimidation,
			CoreSkill::Persuasion,
			CoreSkill::Socialize,
			CoreSkill::Streetwise,
			CoreSkill::Subterfuge,
		];
	}
	
	/// Generates a human-readable name for the given `CoreSkill`.
	pub fn getSkillName(coreSkillType: Self) -> String
	{
		return match coreSkillType
		{
			CoreSkill::AnimalKen => { "Animal Ken".to_string() }
			_ => { coreSkillType.as_ref().to_string() }
		};
	}
	
	pub fn getByName(name: String) -> Option<Self>
	{
		return match Self::asMap().iter().filter(|(cs, _)| Self::getSkillName(**cs) == name.clone()).next()
		{
			Some((cs, _)) => Some(*cs),
			None => None
		};
	}
}

/// The categories of Chronicles of Darkness Traits.
#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum CoreTrait
{
	Mental,
	Physical,
	Social,
}

#[cfg(test)]
mod tests
{
	use super::*;
	
	#[test]
	fn CoreAttribute_asMap()
	{
		let mut expected = BTreeMap::new();
		expected.insert(CoreAttribute::Intelligence, 1);
		expected.insert(CoreAttribute::Wits, 1);
		expected.insert(CoreAttribute::Resolve, 1);
		expected.insert(CoreAttribute::Strength, 1);
		expected.insert(CoreAttribute::Dexterity, 1);
		expected.insert(CoreAttribute::Stamina, 1);
		expected.insert(CoreAttribute::Composure, 1);
		expected.insert(CoreAttribute::Manipulation, 1);
		expected.insert(CoreAttribute::Presence, 1);
		
		let result = CoreAttribute::asMap();
		
		assert_eq!(expected, result);
	}
	
	#[test]
	fn CoreSkill_getSkillName()
	{
		let pairs = BTreeMap::from([
			("Socialize".to_string(), CoreSkill::Socialize),
			("Animal Ken".to_string(), CoreSkill::AnimalKen),
		]);
		
		pairs.iter().for_each(|(s, t)|
		{
			let result = CoreSkill::getSkillName(*t);
			assert_eq!(*s, result);
		});
	}
	
	#[test]
	fn CoreSkill_asMap()
	{
		let mut expected = BTreeMap::new();
		expected.insert(CoreSkill::Academics, 0);
		expected.insert(CoreSkill::AnimalKen, 0);
		expected.insert(CoreSkill::Athletics, 0);
		expected.insert(CoreSkill::Brawl, 0);
		expected.insert(CoreSkill::Computer, 0);
		expected.insert(CoreSkill::Crafts, 0);
		expected.insert(CoreSkill::Drive, 0);
		expected.insert(CoreSkill::Empathy, 0);
		expected.insert(CoreSkill::Expression, 0);
		expected.insert(CoreSkill::Firearms, 0);
		expected.insert(CoreSkill::Investigation, 0);
		expected.insert(CoreSkill::Intimidation, 0);
		expected.insert(CoreSkill::Larceny, 0);
		expected.insert(CoreSkill::Medicine, 0);
		expected.insert(CoreSkill::Occult, 0);
		expected.insert(CoreSkill::Persuasion, 0);
		expected.insert(CoreSkill::Politics, 0);
		expected.insert(CoreSkill::Science, 0);
		expected.insert(CoreSkill::Socialize, 0);
		expected.insert(CoreSkill::Stealth, 0);
		expected.insert(CoreSkill::Streetwise, 0);
		expected.insert(CoreSkill::Subterfuge, 0);
		expected.insert(CoreSkill::Survival, 0);
		expected.insert(CoreSkill::Weaponry, 0);
		
		let result = CoreSkill::asMap();
		
		assert_eq!(expected, result);
	}
}
