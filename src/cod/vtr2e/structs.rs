#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::{
	Scope,
	use_atom_ref
};
use serde::{
	Deserialize,
	Serialize,
};
use std::collections::BTreeMap;
use crate::{
	cod::{
		structs::CoreCharacter,
		vtr2e::{
			enums::Discipline,
			state::{
				KindredDevotions,
				KindredDisciplines,
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
	pub disciplines: BTreeMap<Discipline, usize>,
	
	#[serde(default)]
	pub devotions: Vec<Devotion>,
	
	#[serde(default)]
	pub touchstones: Vec<String>,
}

impl StatefulTemplate for Vampire
{
	fn pull<T>(&mut self, cx: &Scope<T>)
	{
		self.coreCharacter.pull(cx);
		
		let devotions = use_atom_ref(cx, KindredDevotions);
		let disciplines = use_atom_ref(cx, KindredDisciplines);
		let touchstones = use_atom_ref(cx, KindredTouchstones);
		
		self.devotions = devotions.read().clone();
		self.disciplines = disciplines.read().clone();
		self.touchstones = touchstones.read().clone();
		
		self.validate();
	}
	
	fn push<T>(&mut self, cx: &Scope<T>)
	{
		self.coreCharacter.push(cx);
		
		self.validate();
		
		let devotions = use_atom_ref(cx, KindredDevotions);
		let disciplines = use_atom_ref(cx, KindredDisciplines);
		let touchstones = use_atom_ref(cx, KindredTouchstones);
		
		(*devotions.write()) = self.devotions.clone();
		(*disciplines.write()) = self.disciplines.clone();
		(*touchstones.write()) = self.touchstones.clone();
	}
	
	fn validate(&mut self) { }
}

/// Data structure defining a single Vampire: The Requiem 2e Kindred Devotion or Discipline Power.
#[derive(Clone, Debug, Default, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub struct Devotion
{
	#[serde(default)]
	pub action: String,
	
	#[serde(default)]
	pub cost: String,
	
	#[serde(default)]
	pub dicePool: String,
	
	#[serde(default)]
	pub disciplines: String,
	
	#[serde(default)]
	pub duration: String,
	
	#[serde(default)]
	pub name: String,
}
