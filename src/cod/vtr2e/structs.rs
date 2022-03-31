#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::{
	Scope,
	use_atom_ref
};
use serde::{
	Deserialize,
	Serialize,
};
use crate::{
	cod::{
		structs::{
			ActiveAbility,
			CoreCharacter,
		},
		vtr2e::{
			enums::Discipline,
			state::{
				KindredDisciplines,
				KindredPowers,
				KindredTouchstones,
			},
		},
	},
	core::state::StatefulTemplate,
};

/// Data structure defining a Vampire: The Requiem 2e Kindred.
#[derive(Clone, Debug, Default, Deserialize, Serialize)]
pub struct Vampire
{
	#[serde(default)]
	pub coreCharacter: CoreCharacter,
	
	#[serde(default)]
	pub disciplines: Vec<(Discipline, usize)>,
	
	#[serde(default)]
	pub powers: Vec<ActiveAbility>,
	
	#[serde(default)]
	pub touchstones: Vec<String>,
}

impl StatefulTemplate for Vampire
{
	fn pull<T>(&mut self, cx: &Scope<T>)
	{
		self.coreCharacter.pull(cx);
		
		let powers = use_atom_ref(cx, KindredPowers);
		let disciplines = use_atom_ref(cx, KindredDisciplines);
		let touchstones = use_atom_ref(cx, KindredTouchstones);
		
		self.powers = powers.read().clone();
		self.disciplines = disciplines.read().clone();
		self.touchstones = touchstones.read().clone();
		
		self.validate();
	}
	
	fn push<T>(&mut self, cx: &Scope<T>)
	{
		self.coreCharacter.push(cx);
		
		self.validate();
		
		let powers = use_atom_ref(cx, KindredPowers);
		let disciplines = use_atom_ref(cx, KindredDisciplines);
		let touchstones = use_atom_ref(cx, KindredTouchstones);
		
		(*powers.write()) = self.powers.clone();
		(*disciplines.write()) = self.disciplines.clone();
		(*touchstones.write()) = self.touchstones.clone();
	}
	
	fn validate(&mut self) { }
}
