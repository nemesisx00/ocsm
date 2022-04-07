#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::{
	Scope,
	use_atom_ref,
	use_read,
	use_set,
};
use serde::{
	Serialize,
	Deserialize,
};
use std::collections::BTreeMap;
use strum::IntoEnumIterator;
use crate::{
	cod::{
		enums::{
			CoreAttribute,
			CoreDetail,
			CoreSkill,
		},
		state::{
			CharacterAdvantages,
			CharacterAspirations,
			CharacterAttributes,
			CharacterBeats,
			CharacterConditions,
			CharacterDetails,
			CharacterExperience,
			CharacterMerits,
			CharacterSkills,
			CharacterSpecialties,
		},
	},
	core::{
		enums::TrackerState,
		state::StatefulTemplate,
		structs::Tracker,
	},
};

/// Data structure defining a single Active Ability.
/// 
/// This is a generic data structure which should cover most, if not all, active
/// abilities of the various Chronicles of Darkness archetypes. Abilities like
/// Changeling Contracts, Kindred Disciplines, and Mage Spells.
#[derive(Clone, Debug, Default, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub struct ActiveAbility
{
	#[serde(default)]
	pub action: String,
	
	#[serde(default)]
	pub cost: String,
	
	#[serde(default)]
	pub description: String,
	
	#[serde(default)]
	pub dicePool: String,
	
	#[serde(default)]
	pub duration: String,
	
	#[serde(default)]
	pub effects: String,
	
	#[serde(default)]
	pub name: String,
	
	#[serde(default)]
	pub requirements: String,
}

/// Data structure defining the Advantages of a Chronicles of Darkness character.
/// 
/// While the specific names may vary, every Chronicles of Darkness
/// game system has stats corresponding to Integrity, Power,
/// and Resource. The only exceptions are Chronicles of Darkness
/// Mortals who do not possess a Power or Resource stat.
#[derive(Clone, Debug, Deserialize, Eq, PartialEq, PartialOrd, Serialize, Ord)]
pub struct CoreAdvantages
{
	#[serde(default)]
	pub armor: usize,
	
	#[serde(default)]
	pub defense: usize,
	
	#[serde(default)]
	pub health: Tracker,
	
	#[serde(default)]
	pub initiative: usize,
	
	#[serde(default)]
	pub integrity: usize,
	
	#[serde(default)]
	pub power: Option<usize>,
	
	#[serde(default)]
	pub resource: Option<Tracker>,
	
	#[serde(default)]
	pub size: usize,
	
	#[serde(default)]
	pub speed: usize,
	
	#[serde(default)]
	pub willpower: Tracker,
}

impl CoreAdvantages
{
	pub fn mortal() -> Self
	{
		return Self
		{
			power: None,
			resource: None,
			..Default::default()
		};
	}
}

impl Default for CoreAdvantages
{
	fn default() -> Self
	{
		return Self
		{
			armor: 0,
			defense: 1,
			health: Tracker::new(6),
			initiative: 2,
			integrity: 7,
			power: Some(1),
			resource: Some(Tracker::new(10)),
			size: 5,
			speed: 7,
			willpower: Tracker::new(2),
		};
	}
}

// --------------------------------------------------

/// Data structure defining a Chronicles of Darkness core character.
#[derive(Clone, Debug, Deserialize, Eq, PartialEq, PartialOrd, Serialize, Ord)]
pub struct CoreCharacter
{
	#[serde(default)]
	pub advantages: CoreAdvantages,
	
	#[serde(default)]
	pub aspirations: Vec<String>,
	
	#[serde(default)]
	pub attributes: BTreeMap<CoreAttribute, usize>,
	
	#[serde(default)]
	pub details: BTreeMap<CoreDetail, String>,
	
	#[serde(default)]
	pub beats: Tracker,
	
	#[serde(default)]
	pub conditions: Vec<String>,
	
	#[serde(default)]
	pub experience: usize,
	
	#[serde(default)]
	pub merits: Vec<(String, usize)>,
	
	#[serde(default)]
	pub skills: BTreeMap<CoreSkill, usize>,
	
	#[serde(default)]
	pub specialties: Vec<String>,
}

impl CoreCharacter
{
	pub fn mortal() -> Self
	{
		return Self
		{
			advantages: CoreAdvantages::mortal(),
			..Default::default()
		};
	}
}

impl Default for CoreCharacter
{
	fn default() -> Self
	{
		return Self
		{
			advantages: CoreAdvantages::default(),
			aspirations: Vec::<String>::new(),
			attributes: CoreAttribute::asMap(),
			details: CoreDetail::asMap(),
			beats: Tracker::new(5),
			conditions: Vec::<String>::new(),
			experience: 0,
			merits: Vec::<(String, usize)>::new(),
			skills: CoreSkill::asMap(),
			specialties: Vec::<String>::new(),
		};
	}
}

impl StatefulTemplate for CoreCharacter
{
	fn pull<T>(&mut self, cx: &Scope<T>)
	{
		let advantages = use_atom_ref(cx, CharacterAdvantages);
		let aspirations = use_atom_ref(cx, CharacterAspirations);
		let attributes = use_atom_ref(cx, CharacterAttributes);
		let details = use_atom_ref(cx, CharacterDetails);
		let beats = use_atom_ref(cx, CharacterBeats);
		let conditions = use_atom_ref(cx, CharacterConditions);
		let experience = use_read(cx, CharacterExperience);
		let merits = use_atom_ref(cx, CharacterMerits);
		let skills = use_atom_ref(cx, CharacterSkills);
		let specialties = use_atom_ref(cx, CharacterSpecialties);
		
		self.advantages = advantages.read().clone();
		self.aspirations = aspirations.read().clone();
		self.attributes = attributes.read().clone();
		self.beats = beats.read().clone();
		self.conditions = conditions.read().clone();
		self.details = details.read().clone();
		self.experience = *experience;
		self.merits = merits.read().clone();
		self.skills = skills.read().clone();
		self.specialties = specialties.read().clone();
		
		self.validate();
	}
	
	fn push<T>(&mut self, cx: &Scope<T>)
	{
		self.validate();
		
		let advantages = use_atom_ref(cx, CharacterAdvantages);
		let aspirations = use_atom_ref(cx, CharacterAspirations);
		let attributes = use_atom_ref(cx, CharacterAttributes);
		let beats = use_atom_ref(cx, CharacterBeats);
		let conditions = use_atom_ref(cx, CharacterConditions);
		let details = use_atom_ref(cx, CharacterDetails);
		let experience = use_set(cx, CharacterExperience);
		let merits = use_atom_ref(cx, CharacterMerits);
		let skills = use_atom_ref(cx, CharacterSkills);
		let specialties = use_atom_ref(cx, CharacterSpecialties);
		
		(*advantages.write()) = self.advantages.clone();
		(*aspirations.write()) = self.aspirations.clone();
		(*attributes.write()) = self.attributes.clone();
		(*beats.write()) = self.beats.clone();
		(*conditions.write()) = self.conditions.clone();
		(*details.write()) = self.details.clone();
		experience(self.experience);
		(*merits.write()) = self.merits.clone();
		(*skills.write()) = self.skills.clone();
		(*specialties.write()) = self.specialties.clone();
	}
	
	fn validate(&mut self)
	{
		for ca in CoreAttribute::iter()
		{
			if self.attributes.get(&ca) == None { self.attributes.insert(ca, 1); }
		}
		
		for cd in CoreDetail::iter()
		{
			if self.details.get(&cd) == None { self.details.insert(cd, "".to_string()); }
		}
		
		for cs in CoreSkill::iter()
		{
			if self.skills.get(&cs) == None { self.skills.insert(cs, 0); }
		}
	}
}

// --------------------------------------------------

#[cfg(test)]
mod tests
{
	use super::*;
	
	#[test]
	fn CoreCharacter_validate()
	{
		let attributes = CoreAttribute::asMap();
		let skills = CoreSkill::asMap();
		
		let mut character = CoreCharacter::default();
		character.attributes = BTreeMap::new();
		character.skills = BTreeMap::new();
		
		character.attributes.iter().for_each(|(at, value)| assert_ne!(attributes[at], *value));
		character.skills.iter().for_each(|(st, value)| assert_ne!(skills[st], *value));
		
		character.validate();
		
		character.attributes.iter().for_each(|(at, value)| assert_eq!(attributes[at], *value));
		character.skills.iter().for_each(|(st, value)| assert_eq!(skills[st], *value));
	}
}
