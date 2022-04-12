#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::Scope;
use serde::{
	Deserialize,
	Serialize,
};
use crate::{
	core::{
		state::StatefulTemplate,
		util::spaceOutCapitals,
	},
	dnd::{
		fifth::{
			enums::{
				Ability,
				Advantage,
				DamageType,
				Die,
				ItemType,
				Proficiency,
				Skill,
				WeaponProperty,
			},
		},
	},
};

#[derive(Clone, Debug, Default, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub struct Dnd5eCharacter
{
	#[serde(default)]
	pub name: String,
	
	#[serde(default)]
	pub abilityScores: Vec<AbilityScore>,
	
	#[serde(default)]
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

#[derive(Clone, Debug, Deserialize, Eq, Hash, PartialEq, Serialize, PartialOrd, Ord)]
pub struct AbilityScore
{
	pub ability: Ability,
	
	#[serde(default)]
	pub score: isize,
}

impl AbilityScore
{
	pub fn new(ability: Ability) -> Self
	{
		return Self
		{
			ability: ability,
			score: 10,
		};
	}
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
	pub itemType: ItemType,
	
	#[serde(default)]
	pub cost: isize,
	
	#[serde(default)]
	pub armor: Option<ItemArmor>,
	
	#[serde(default)]
	pub damage: Option<ItemDamage>,
	
	#[serde(default)]
	pub weight: Option<isize>,
}

// --------------------------------------------------

#[derive(Clone, Debug, Default, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub struct ItemArmor
{
	#[serde(default)]
	pub armorClass: isize,
	
	#[serde(default)]
	pub dexterityModMax: Option<isize>,
}

// --------------------------------------------------

#[derive(Clone, Debug, Default, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub struct ItemDamage
{
	#[serde(default)]
	pub damageDie: Die,
	
	#[serde(default)]
	pub damageRolls: isize,
	
	pub damageType: DamageType,
	
	#[serde(default)]
	pub properties: Option<Vec<WeaponProperty>>,
}

// --------------------------------------------------

#[derive(Clone, Debug, Deserialize, Eq, Hash, PartialEq, Serialize, PartialOrd, Ord)]
pub struct SkillScore
{
	pub ability: Ability,
	
	#[serde(default)]
	pub advantage: Advantage,
	
	#[serde(default)]
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
			advantage: Advantage::None,
			proficiency: Proficiency::None,
			skill: skill,
		};
	}
	
	pub fn getFullName(self) -> String
	{
		return format!("{} ({})", self.ability.as_ref(), spaceOutCapitals(self.skill.as_ref()));
	}
}
