#![allow(non_snake_case, non_upper_case_globals)]

use serde::{
	Serialize,
	Deserialize,
};
use std::{
	collections::HashMap,
	iter::Iterator
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

#[derive(Clone, Debug, Deserialize, PartialEq, Serialize)]
pub struct BaseAttribute
{
	pub name: String,
	pub value: usize,
}

impl Default for BaseAttribute
{
	fn default() -> Self
	{
		return BaseAttribute { name: "".to_string(), value: 1 };
	}
}

impl BaseAttribute
{
	fn new(baseAttributeType: BaseAttributeType) -> Self
	{
		return Self
		{
			name: baseAttributeType.as_ref().to_string(),
			..Default::default()
		};
	}
	
	pub fn newAllAttributes() -> HashMap<BaseAttributeType, Self>
	{
		let mut attributes = HashMap::<BaseAttributeType, Self>::new();
		for bat in BaseAttributeType::iter()
		{
			attributes.insert(bat, Self::new(bat));
		}
		return attributes.clone();
	}
	
	pub fn newMentalAttributes() -> HashMap<BaseAttributeType, Self>
	{
		let mut attributes = HashMap::<BaseAttributeType, Self>::new();
		BaseAttributeType::mental().iter().for_each(|bat| { attributes.insert(*bat, Self::new(*bat)); });
		return attributes.clone();
	}
	
	pub fn newPhysicalAttributes() -> HashMap<BaseAttributeType, Self>
	{
		let mut attributes = HashMap::<BaseAttributeType, Self>::new();
		BaseAttributeType::physical().iter().for_each(|bat| { attributes.insert(*bat, Self::new(*bat)); });
		return attributes.clone();
	}
	
	pub fn newSocialAttributes() -> HashMap<BaseAttributeType, Self>
	{
		let mut attributes = HashMap::<BaseAttributeType, Self>::new();
		BaseAttributeType::social().iter().for_each(|bat| { attributes.insert(*bat, Self::new(*bat)); });
		return attributes.clone();
	}
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

#[derive(Clone, Debug, Deserialize, PartialEq, Serialize)]
pub struct BaseAttributes
{
	pub composure: BaseAttribute,
	pub dexterity: BaseAttribute,
	pub intelligence: BaseAttribute,
	pub manipulation: BaseAttribute,
	pub presence: BaseAttribute,
	pub resolve: BaseAttribute,
	pub stamina: BaseAttribute,
	pub strength: BaseAttribute,
	pub wits: BaseAttribute,
}

impl Default for BaseAttributes
{
	fn default() -> Self
	{
		return Self
		{
			composure: BaseAttribute { name: "Composure".to_string(), ..Default::default() },
			dexterity: BaseAttribute { name: "Dexterity".to_string(), ..Default::default() },
			intelligence: BaseAttribute { name: "Intelligence".to_string(), ..Default::default() },
			manipulation: BaseAttribute { name: "Manipulation".to_string(), ..Default::default() },
			presence: BaseAttribute { name: "Presence".to_string(), ..Default::default() },
			resolve: BaseAttribute { name: "Resolve".to_string(), ..Default::default() },
			stamina: BaseAttribute { name: "Stamina".to_string(), ..Default::default() },
			strength: BaseAttribute { name: "Strength".to_string(), ..Default::default() },
			wits: BaseAttribute { name: "Wits".to_string(), ..Default::default() }
		};
	}
}

#[derive(Clone, Debug, Default, Eq, Deserialize, PartialEq, PartialOrd, Serialize, Ord)]
pub struct BaseSkill
{
	pub name: String,
	pub value: usize,
}

impl BaseSkill
{
	pub fn new(baseSkillType: BaseSkillType) -> Self
	{
		return Self
		{
			name: BaseSkillType::getSkillName(baseSkillType),
			..Default::default()
		};
	}
	
	pub fn newAllSkills() -> HashMap<BaseSkillType, Self>
	{
		let mut skills = HashMap::<BaseSkillType, Self>::new();
		for s in BaseSkillType::iter() { skills.insert(s, Self::new(s)); }
		return skills.clone();
	}
	
	pub fn newMentalSkills() -> HashMap<BaseSkillType, Self>
	{
		let mut skills = HashMap::<BaseSkillType, Self>::new();
		BaseSkillType::mental().iter().for_each(|bst| { skills.insert(*bst, Self::new(*bst)); });
		return skills.clone();
	}
	
	pub fn newPhysicalSkills() -> HashMap<BaseSkillType, Self>
	{
		let mut skills = HashMap::<BaseSkillType, Self>::new();
		BaseSkillType::physical().iter().for_each(|bst| { skills.insert(*bst, Self::new(*bst)); });
		return skills.clone();
	}
	
	pub fn newSocialSkills() -> HashMap<BaseSkillType, Self>
	{
		let mut skills = HashMap::<BaseSkillType, Self>::new();
		BaseSkillType::social().iter().for_each(|bst| { skills.insert(*bst, Self::new(*bst)); });
		return skills.clone();
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
	fn test_BaseAttribute_new()
	{
		let expected = BaseAttribute { name: "Stamina".to_string(), ..Default::default() };
		let result = BaseAttribute::new(BaseAttributeType::Stamina);
		
		assert_eq!(expected, result);
	}
	
	#[test]
	fn test_BaseAttribute_newAllAttributes()
	{
		let mut expected = HashMap::new();
		expected.insert(BaseAttributeType::Intelligence, BaseAttribute::new(BaseAttributeType::Intelligence));
		expected.insert(BaseAttributeType::Wits, BaseAttribute::new(BaseAttributeType::Wits));
		expected.insert(BaseAttributeType::Resolve, BaseAttribute::new(BaseAttributeType::Resolve));
		expected.insert(BaseAttributeType::Strength, BaseAttribute::new(BaseAttributeType::Strength));
		expected.insert(BaseAttributeType::Dexterity, BaseAttribute::new(BaseAttributeType::Dexterity));
		expected.insert(BaseAttributeType::Stamina, BaseAttribute::new(BaseAttributeType::Stamina));
		expected.insert(BaseAttributeType::Composure, BaseAttribute::new(BaseAttributeType::Composure));
		expected.insert(BaseAttributeType::Manipulation, BaseAttribute::new(BaseAttributeType::Manipulation));
		expected.insert(BaseAttributeType::Presence, BaseAttribute::new(BaseAttributeType::Presence));
		
		let result = BaseAttribute::newAllAttributes();
		
		assert_eq!(expected, result);
	}
	
	#[test]
	fn test_BaseAttribute_newMentalAttributes()
	{
		let mut expected = HashMap::new();
		expected.insert(BaseAttributeType::Intelligence, BaseAttribute::new(BaseAttributeType::Intelligence));
		expected.insert(BaseAttributeType::Wits, BaseAttribute::new(BaseAttributeType::Wits));
		expected.insert(BaseAttributeType::Resolve, BaseAttribute::new(BaseAttributeType::Resolve));
		
		let result = BaseAttribute::newMentalAttributes();
		
		assert_eq!(expected, result);
	}
	
	#[test]
	fn test_BaseAttribute_newPhysicalAttributes()
	{
		let mut expected = HashMap::new();
		expected.insert(BaseAttributeType::Strength, BaseAttribute::new(BaseAttributeType::Strength));
		expected.insert(BaseAttributeType::Dexterity, BaseAttribute::new(BaseAttributeType::Dexterity));
		expected.insert(BaseAttributeType::Stamina, BaseAttribute::new(BaseAttributeType::Stamina));
		
		let result = BaseAttribute::newPhysicalAttributes();
		
		assert_eq!(expected, result);
	}
	
	#[test]
	fn test_BaseAttribute_newSocialAttributes()
	{
		let mut expected = HashMap::new();
		expected.insert(BaseAttributeType::Composure, BaseAttribute::new(BaseAttributeType::Composure));
		expected.insert(BaseAttributeType::Manipulation, BaseAttribute::new(BaseAttributeType::Manipulation));
		expected.insert(BaseAttributeType::Presence, BaseAttribute::new(BaseAttributeType::Presence));
		
		let result = BaseAttribute::newSocialAttributes();
		
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
	fn test_BaseSkill_new()
	{
		let expected = BaseSkill { name: "Academics".to_string(), ..Default::default() };
		let result = BaseSkill::new(BaseSkillType::Academics);
		
		assert_eq!(expected, result);
	}
	
	#[test]
	fn test_BaseSkill_newAllSkills()
	{
		let mut expected = HashMap::new();
		expected.insert(BaseSkillType::Academics, BaseSkill::new(BaseSkillType::Academics));
		expected.insert(BaseSkillType::AnimalKen, BaseSkill::new(BaseSkillType::AnimalKen));
		expected.insert(BaseSkillType::Athletics, BaseSkill::new(BaseSkillType::Athletics));
		expected.insert(BaseSkillType::Brawl, BaseSkill::new(BaseSkillType::Brawl));
		expected.insert(BaseSkillType::Computer, BaseSkill::new(BaseSkillType::Computer));
		expected.insert(BaseSkillType::Crafts, BaseSkill::new(BaseSkillType::Crafts));
		expected.insert(BaseSkillType::Drive, BaseSkill::new(BaseSkillType::Drive));
		expected.insert(BaseSkillType::Empathy, BaseSkill::new(BaseSkillType::Empathy));
		expected.insert(BaseSkillType::Expression, BaseSkill::new(BaseSkillType::Expression));
		expected.insert(BaseSkillType::Firearms, BaseSkill::new(BaseSkillType::Firearms));
		expected.insert(BaseSkillType::Investigation, BaseSkill::new(BaseSkillType::Investigation));
		expected.insert(BaseSkillType::Intimidation, BaseSkill::new(BaseSkillType::Intimidation));
		expected.insert(BaseSkillType::Larceny, BaseSkill::new(BaseSkillType::Larceny));
		expected.insert(BaseSkillType::Medicine, BaseSkill::new(BaseSkillType::Medicine));
		expected.insert(BaseSkillType::Occult, BaseSkill::new(BaseSkillType::Occult));
		expected.insert(BaseSkillType::Persuasion, BaseSkill::new(BaseSkillType::Persuasion));
		expected.insert(BaseSkillType::Politics, BaseSkill::new(BaseSkillType::Politics));
		expected.insert(BaseSkillType::Science, BaseSkill::new(BaseSkillType::Science));
		expected.insert(BaseSkillType::Socialize, BaseSkill::new(BaseSkillType::Socialize));
		expected.insert(BaseSkillType::Stealth, BaseSkill::new(BaseSkillType::Stealth));
		expected.insert(BaseSkillType::Streetwise, BaseSkill::new(BaseSkillType::Streetwise));
		expected.insert(BaseSkillType::Subterfuge, BaseSkill::new(BaseSkillType::Subterfuge));
		expected.insert(BaseSkillType::Survival, BaseSkill::new(BaseSkillType::Survival));
		expected.insert(BaseSkillType::Weaponry, BaseSkill::new(BaseSkillType::Weaponry));
		
		let result = BaseSkill::newAllSkills();
		
		assert_eq!(expected, result);
	}
	
	#[test]
	fn test_BaseSkill_newMentalSkills()
	{
		let mut expected = HashMap::new();
		expected.insert(BaseSkillType::Academics, BaseSkill { name: "Academics".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Computer, BaseSkill { name: "Computer".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Crafts, BaseSkill { name: "Crafts".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Investigation, BaseSkill { name: "Investigation".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Medicine, BaseSkill { name: "Medicine".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Occult, BaseSkill { name: "Occult".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Politics, BaseSkill { name: "Politics".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Science, BaseSkill { name: "Science".to_string(), ..Default::default() });
		
		let result = BaseSkill::newMentalSkills();
		
		assert_eq!(expected, result);
	}
	
	#[test]
	fn test_BaseSkill_newPhysicalSkills()
	{
		let mut expected = HashMap::new();
		expected.insert(BaseSkillType::Athletics, BaseSkill { name: "Athletics".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Brawl, BaseSkill { name: "Brawl".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Drive, BaseSkill { name: "Drive".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Firearms, BaseSkill { name: "Firearms".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Larceny, BaseSkill { name: "Larceny".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Stealth, BaseSkill { name: "Stealth".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Survival, BaseSkill { name: "Survival".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Weaponry, BaseSkill { name: "Weaponry".to_string(), ..Default::default() });
		
		let result = BaseSkill::newPhysicalSkills();
		
		assert_eq!(expected, result);
	}
	
	#[test]
	fn test_BaseSkill_newSocialSkills()
	{
		let mut expected = HashMap::new();
		expected.insert(BaseSkillType::AnimalKen, BaseSkill { name: "Animal Ken".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Empathy, BaseSkill { name: "Empathy".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Expression, BaseSkill { name: "Expression".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Intimidation, BaseSkill { name: "Intimidation".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Persuasion, BaseSkill { name: "Persuasion".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Socialize, BaseSkill { name: "Socialize".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Streetwise, BaseSkill { name: "Streetwise".to_string(), ..Default::default() });
		expected.insert(BaseSkillType::Subterfuge, BaseSkill { name: "Subterfuge".to_string(), ..Default::default() });
		
		let result = BaseSkill::newSocialSkills();
		
		assert_eq!(expected, result);
	}
}
