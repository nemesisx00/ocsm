#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::Scope;
use serde::{
	Deserialize,
	Serialize,
};
use std::collections::BTreeMap;
use crate::{
	core::{
		state::StatefulTemplate,
		util::spaceOutCapitals,
	},
	dnd::fifth::enums::{
		Ability,
		DamageType,
		Die,
		ItemType,
		Proficiency,
		Skill,
		WeaponProperty,
	},
};

#[derive(Clone, Debug, Default, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub struct Dnd5eCharacter
{
	pub name: String,
	
	pub savingThrows: Vec<SavingThrow>,
	
	pub skills: Vec<SkillScore>,
}

impl StatefulTemplate for Dnd5eCharacter
{
	fn pull<T>(&mut self, cx: &Scope<T>)
	{
		self.validate();
	}
	
	fn push<T>(&mut self, cx: &Scope<T>)
	{
		self.validate();
	}
	
	fn validate(&mut self)
	{
		
	}
}

// --------------------------------------------------

#[derive(Clone, Debug, Default, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub struct Aesthetics
{
	
}

// --------------------------------------------------

#[derive(Clone, Debug, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub struct AbilityScore
{
	pub ability: Ability,
	
	#[serde(default)]
	pub value: isize,
}

impl AbilityScore
{
	pub fn new(ability: Ability) -> Self
	{
		return Self
		{
			ability: ability,
			value: 10,
		};
	}
}

// --------------------------------------------------

#[derive(Clone, Debug, Default, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub struct Feature
{
	#[serde(default)]
	pub name: String,
	
	#[serde(default)]
	pub description: String,
}

// --------------------------------------------------

#[derive(Clone, Debug, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub struct Item
{
	#[serde(default)]
	pub name: String,
	
	#[serde(default)]
	pub description: String,
	
	#[serde(default)]
	pub cost: isize,
	
	#[serde(default)]
	pub itemType: ItemType,
	
	#[serde(default)]
	pub armorClass: Option<isize>,
	
	#[serde(default)]
	pub dexterityModMax: Option<isize>,
	
	#[serde(default)]
	pub damageDie: Option<Die>,
	
	#[serde(default)]
	pub damageRolls: Option<isize>,
	
	#[serde(default)]
	pub damageType: Option<DamageType>,
	
	#[serde(default)]
	pub properties: Option<Vec<WeaponProperty>>,
	
	#[serde(default)]
	pub weight: Option<isize>,
}

// --------------------------------------------------

#[derive(Clone, Debug, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub struct LevelFeatures
{
	pub class: String,
	
	#[serde(default)]
	pub level: usize,
	
	#[serde(default)]
	pub features: Vec<Feature>
}

impl LevelFeatures
{
	pub fn new(class: String, level: usize) -> Self
	{
		return Self
		{
			class: class,
			level: level,
			features: Vec::new(),
		};
	}
}

// --------------------------------------------------

#[derive(Clone, Debug, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub struct SavingThrow
{
	pub ability: Ability,
	
	pub proficiency: Proficiency,
}

impl SavingThrow
{
	pub fn new(ability: Ability) -> Self
	{
		return Self
		{
			ability: ability,
			proficiency: Proficiency::None,
		};
	}
}

// --------------------------------------------------

#[derive(Clone, Debug, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub struct SkillScore
{
	pub ability: Ability,
	
	pub proficiency: Proficiency,
	
	pub skill: Skill,
}

impl SkillScore
{
	pub fn new(skill: Skill, ability: Ability) -> Self
	{
		return Self
		{
			ability: ability,
			proficiency: Proficiency::None,
			skill: skill,
		};
	}
	
	pub fn getFullName(self) -> String
	{
		return format!("{} ({})", self.ability.as_ref(), spaceOutCapitals(self.skill.as_ref()));
	}
}
