#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::HashMap;
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
			BaseAdvantages,
		},
		merits::Merit,
		tracks::{
			Tracker,
		},
		traits::{
			BaseAttribute,
			BaseAttributeType,
			BaseSkill,
			BaseSkillType,
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

#[derive(Clone, Debug, Deserialize, Serialize)]
pub struct BaseCharacter
{
	#[serde(default)]
	pub advantages: BaseAdvantages,
	#[serde(default)]
	pub aspirations: Vec<String>,
	#[serde(default)]
	pub attributes: HashMap<BaseAttributeType, BaseAttribute>,
	#[serde(default)]
	pub beats: Tracker,
	#[serde(default)]
	pub experience: usize,
	#[serde(default)]
	pub merits: Vec<Merit>,
	#[serde(default)]
	pub skills: HashMap<BaseSkillType, BaseSkill>,
	#[serde(default)]
	pub specialties: Vec<String>,
}

impl Default for BaseCharacter
{
	fn default() -> Self
	{
		Self
		{
			advantages: BaseAdvantages::default(),
			aspirations: Vec::<String>::new(),
			attributes: BaseAttribute::newAllAttributes(),
			beats: Tracker::new(5),
			experience: 0,
			merits: Vec::<Merit>::new(),
			skills: BaseSkill::newAllSkills(),
			specialties: Vec::<String>::new(),
		}
	}
}

impl StatefulTemplate for BaseCharacter
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
		for bat in BaseAttributeType::iter()
		{
			match self.attributes.get(&bat)
			{
				Some(_) => {}
				None => { self.attributes.insert(bat, BaseAttribute::new(bat)); }
			}
		}
		
		for bst in BaseSkillType::iter()
		{
			match self.skills.get(&bst)
			{
				Some(_) => {}
				None => { self.skills.insert(bst, BaseSkill::new(bst)); }
			}
		}
	}
}

#[cfg(test)]
mod tests
{
	use super::*;
	
	#[test]
	fn test_BaseCharacter_validate()
	{
		let attributes = BaseAttribute::newAllAttributes();
		let skills = BaseSkill::newAllSkills();
		
		let mut character = BaseCharacter::default();
		character.attributes = HashMap::new();
		character.skills = HashMap::new();
		
		assert_ne!(attributes, character.attributes);
		assert_ne!(skills, character.skills);
		
		character.validate();
		
		assert_eq!(attributes, character.attributes);
		assert_eq!(skills, character.skills);
	}
}
