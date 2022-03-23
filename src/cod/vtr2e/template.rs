#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use serde::{Serialize, Deserialize};
use crate::{
	cod::{
		advantages::BaseAdvantages,
		state::{
			CharacterAdvantages,
			CharacterAttributes,
			CharacterSkills,
		},
		traits::{
			BaseAttributes,
			BaseSkills,
		},
		vtr2e::{
			advantages::{
				TemplateAdvantages,
			},
			details::{
				Details,
			},
			state::{
				KindredAdvantages,
				KindredDetails,
			}
		}
	}
};

#[derive(Clone, Default, Deserialize, Serialize)]
pub struct Kindred
{
	pub advantages: BaseAdvantages,
	pub attributes: BaseAttributes,
	pub details: Details,
	pub disciplines: Vec<usize>,
	pub skills: BaseSkills,
	pub templateAdvantages: TemplateAdvantages,
}

impl Kindred
{
	/// Refresh this character's data from the global state.
	pub fn refresh<T>(&mut self, scope: &Scope<T>)
	{
		let advantages = use_atom_ref(scope, CharacterAdvantages);
		let attributes = use_atom_ref(scope, CharacterAttributes);
		let details = use_atom_ref(scope, KindredDetails);
		let skills = use_atom_ref(scope, CharacterSkills);
		let templateAdvantages = use_atom_ref(scope, KindredAdvantages);
		
		self.advantages = advantages.read().clone();
		self.attributes = attributes.read().clone();
		self.details = details.read().clone();
		self.skills = skills.read().clone();
		self.templateAdvantages = templateAdvantages.read().clone();
	}
	
	/// Reload this character's data into the global state.
	pub fn reload<T>(&self, scope: &Scope<T>)
	{
		let advantages = use_atom_ref(scope, CharacterAdvantages);
		let attributes = use_atom_ref(scope, CharacterAttributes);
		let details = use_atom_ref(scope, KindredDetails);
		let skills = use_atom_ref(scope, CharacterSkills);
		let templateAdvantages = use_atom_ref(scope, KindredAdvantages);
		
		(*advantages.write()) = self.advantages.clone();
		(*attributes.write()) = self.attributes.clone();
		(*details.write()) = self.details.clone();
		(*skills.write()) = self.skills.clone();
		(*templateAdvantages.write()) = self.templateAdvantages.clone();
	}
}
