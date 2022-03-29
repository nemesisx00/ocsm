#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::BTreeMap;
use dioxus::prelude::*;
use serde::{
	Serialize,
	Deserialize,
};
use strum::IntoEnumIterator;
use crate::{
	core::template::StatefulTemplate,
	cod::{
		advantages::{
			CoreAdvantages,
		},
		merits::Merit,
		tracks::{
			Tracker,
		},
		traits::{
			CoreAttributeType,
			CoreSkillType,
		},
		state::{
			CharacterAdvantages,
			CharacterAspirations,
			CharacterAttributes,
			CharacterBeats,
			CharacterExperience,
			CharacterMerits,
			CharacterSkills,
			CharacterSpecialties,
		}
	},
};

/// Data structure defining a Chronicles of Darkness core character.
#[derive(Clone, Debug, Deserialize, Serialize)]
pub struct CoreCharacter
{
	#[serde(default)]
	pub advantages: CoreAdvantages,
	#[serde(default)]
	pub aspirations: Vec<String>,
	#[serde(default)]
	pub attributes: BTreeMap<CoreAttributeType, usize>,
	#[serde(default)]
	pub beats: Tracker,
	#[serde(default)]
	pub experience: usize,
	#[serde(default)]
	pub merits: Vec<Merit>,
	#[serde(default)]
	pub skills: BTreeMap<CoreSkillType, usize>,
	#[serde(default)]
	pub specialties: Vec<String>,
}

impl Default for CoreCharacter
{
	fn default() -> Self
	{
		Self
		{
			advantages: CoreAdvantages::default(),
			aspirations: Vec::<String>::new(),
			attributes: CoreAttributeType::asMap(),
			beats: Tracker::new(5),
			experience: 0,
			merits: Vec::<Merit>::new(),
			skills: CoreSkillType::asMap(),
			specialties: Vec::<String>::new(),
		}
	}
}

impl StatefulTemplate for CoreCharacter
{
	fn pull<T>(&mut self, cx: &Scope<T>)
	{
		let advantages = use_atom_ref(cx, CharacterAdvantages);
		let aspirations = use_atom_ref(cx, CharacterAspirations);
		let attributes = use_atom_ref(cx, CharacterAttributes);
		let beats = use_atom_ref(cx, CharacterBeats);
		let experience = use_read(cx, CharacterExperience);
		let merits = use_atom_ref(cx, CharacterMerits);
		let skills = use_atom_ref(cx, CharacterSkills);
		let specialties = use_atom_ref(cx, CharacterSpecialties);
		
		self.advantages = advantages.read().clone();
		self.aspirations = aspirations.read().clone();
		self.attributes = attributes.read().clone();
		self.beats = beats.read().clone();
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
		let experience = use_set(cx, CharacterExperience);
		let merits = use_atom_ref(cx, CharacterMerits);
		let skills = use_atom_ref(cx, CharacterSkills);
		let specialties = use_atom_ref(cx, CharacterSpecialties);
		
		(*advantages.write()) = self.advantages.clone();
		(*aspirations.write()) = self.aspirations.clone();
		(*attributes.write()) = self.attributes.clone();
		(*beats.write()) = self.beats.clone();
		experience(self.experience);
		(*merits.write()) = self.merits.clone();
		(*skills.write()) = self.skills.clone();
		(*specialties.write()) = self.specialties.clone();
	}
	
	fn validate(&mut self)
	{
		for cat in CoreAttributeType::iter()
		{
			match self.attributes.get(&cat)
			{
				Some(_) => {}
				None => { self.attributes.insert(cat, 1); }
			}
		}
		
		for cst in CoreSkillType::iter()
		{
			match self.skills.get(&cst)
			{
				Some(_) => {}
				None => { self.skills.insert(cst, 0); }
			}
		}
	}
}

#[cfg(test)]
mod tests
{
	use super::*;
	
	#[test]
	fn test_CoreCharacter_validate()
	{
		let attributes = CoreAttributeType::asMap();
		let skills = CoreSkillType::asMap();
		
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
