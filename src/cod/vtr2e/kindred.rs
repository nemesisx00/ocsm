#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use serde::{Serialize, Deserialize};
use crate::{
	cod::{
		state::{
			CharacterAttributes,
			CharacterSkills,
		},
		tracks::{
			Tracker,
		},
		traits::{
			Attributes,
			Skills
		},
		vtr2e::{
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
	pub advantages: Advantages,
	pub attributes: Attributes,
	pub details: Details,
	pub disciplines: Vec<usize>,
	pub skills: Skills,
}

impl Kindred
{
	/// Refresh this character's data from the global state.
	pub fn refresh<T>(&mut self, scope: &Scope<T>)
	{
		let advantages = use_atom_ref(scope, KindredAdvantages);
		let attributes = use_atom_ref(scope, CharacterAttributes);
		let details = use_atom_ref(scope, KindredDetails);
		let skills = use_atom_ref(scope, CharacterSkills);
		
		self.advantages = advantages.read().clone();
		self.attributes = attributes.read().clone();
		self.details = details.read().clone();
		self.skills = skills.read().clone();
	}
	
	/// Reload this character's data into the global state.
	pub fn reload<T>(&self, scope: &Scope<T>)
	{
		let advantages = use_atom_ref(scope, KindredAdvantages);
		let attributes = use_atom_ref(scope, CharacterAttributes);
		let details = use_atom_ref(scope, KindredDetails);
		let skills = use_atom_ref(scope, CharacterSkills);
		
		(*advantages.write()) = self.advantages.clone();
		(*attributes.write()) = self.attributes.clone();
		(*details.write()) = self.details.clone();
		(*skills.write()) = self.skills.clone();
	}
}

#[derive(Clone, Copy, Deserialize, PartialEq, Serialize)]
pub enum AdvantageType
{
	BloodPotency,
	Defense,
	Health,
	Humanity,
	Initiative,
	Size,
	Speed,
	Willpower,
}

#[derive(Clone, Deserialize, Serialize)]
pub struct Advantages
{
	pub bloodPotency: usize,
	pub defense: usize,
	pub health: Tracker,
	pub humanity: usize,
	pub initiative: usize,
	pub size: usize,
	pub speed: usize,
	pub willpower: Tracker,
}

impl Default for Advantages
{
	fn default() -> Self
	{
		Self
		{
			bloodPotency: 1,
			defense: 1,
			health: Tracker::new(6),
			humanity: 7,
			initiative: 2,
			size: 5,
			speed: 7,
			willpower: Tracker::new(2),
		}
	}
}
