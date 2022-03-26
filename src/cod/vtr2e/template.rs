#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::HashMap;
use dioxus::prelude::{
	Scope,
	use_atom_ref,
};
use serde::{
	Serialize,
	Deserialize,
};
use crate::{
	core::template::StatefulTemplate,
	cod::{
		advantages::BaseAdvantages,
		state::{
			CharacterAdvantages,
			CharacterAttributes,
			CharacterMerits,
			CharacterSkills,
		},
		merits::Merit,
		traits::{
			BaseAttribute,
			BaseAttributeType,
			BaseSkill,
			BaseSkillType,
		},
		vtr2e::{
			advantages::TemplateAdvantages,
			details::Details,
			disciplines::{
				Devotion,
				Discipline,
			},
			state::{
				KindredAdvantages,
				KindredDetails,
				KindredDevotions,
				KindredDisciplines,
			}
		}
	}
};

#[derive(Clone, Default, Deserialize, Serialize)]
pub struct Kindred
{
	pub advantages: BaseAdvantages,
	pub attributes: HashMap<BaseAttributeType, BaseAttribute>,
	pub details: Details,
	pub disciplines: Vec<Discipline>,
	pub devotions: Vec<Devotion>,
	pub merits: Vec<Merit>,
	pub skills: HashMap<BaseSkillType, BaseSkill>,
	pub templateAdvantages: TemplateAdvantages,
}

impl StatefulTemplate for Kindred
{
	fn pull<T>(&mut self, cx: &Scope<T>)
	{
		let advantages = use_atom_ref(cx, CharacterAdvantages);
		let attributes = use_atom_ref(cx, CharacterAttributes);
		let details = use_atom_ref(cx, KindredDetails);
		let devotions = use_atom_ref(cx, KindredDevotions);
		let disciplines = use_atom_ref(cx, KindredDisciplines);
		let merits = use_atom_ref(cx, CharacterMerits);
		let skills = use_atom_ref(cx, CharacterSkills);
		let templateAdvantages = use_atom_ref(cx, KindredAdvantages);
		
		self.advantages = advantages.read().clone();
		self.attributes = attributes.read().clone();
		self.details = details.read().clone();
		self.devotions = devotions.read().clone();
		self.disciplines = disciplines.read().clone();
		self.merits = merits.read().clone();
		self.skills = skills.read().clone();
		self.templateAdvantages = templateAdvantages.read().clone();
	}
	
	fn push<T>(&self, cx: &Scope<T>)
	{
		let advantages = use_atom_ref(cx, CharacterAdvantages);
		let attributes = use_atom_ref(cx, CharacterAttributes);
		let details = use_atom_ref(cx, KindredDetails);
		let devotions = use_atom_ref(cx, KindredDevotions);
		let disciplines = use_atom_ref(cx, KindredDisciplines);
		let merits = use_atom_ref(cx, CharacterMerits);
		let skills = use_atom_ref(cx, CharacterSkills);
		let templateAdvantages = use_atom_ref(cx, KindredAdvantages);
		
		(*advantages.write()) = self.advantages.clone();
		(*attributes.write()) = self.attributes.clone();
		(*details.write()) = self.details.clone();
		(*devotions.write()) = self.devotions.clone();
		(*disciplines.write()) = self.disciplines.clone();
		(*merits.write()) = self.merits.clone();
		(*skills.write()) = self.skills.clone();
		(*templateAdvantages.write()) = self.templateAdvantages.clone();
	}
}
