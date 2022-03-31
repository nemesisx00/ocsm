#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::{
	Scope,
	use_atom_ref,
};
use serde::{
	Deserialize,
	Serialize,
};
use crate::{
	cod::{
		mta2e::{
			enums::Arcana,
			state::{
				MageArcana,
				MageObsessions,
			},
		},
		structs::CoreCharacter,
	},
	core::state::StatefulTemplate,
};

/// Data structure defining a Mage: The Awakening 2e Mage.
#[derive(Clone, Debug, Default, Deserialize, Serialize)]
pub struct Mage
{
	#[serde(default)]
	pub coreCharacter: CoreCharacter,
	
	#[serde(default)]
	pub arcana: Vec<(Arcana, usize)>,
	
	#[serde(default)]
	pub obsessions: Vec<String>,
}

impl StatefulTemplate for Mage
{
	fn pull<T>(&mut self, cx: &Scope<T>)
	{
		self.coreCharacter.pull(cx);
		
		let arcana = use_atom_ref(cx, MageArcana);
		let obsessions = use_atom_ref(cx, MageObsessions);
		
		self.arcana = arcana.read().clone();
		self.obsessions = obsessions.read().clone();
		
		self.validate();
	}
	
	fn push<T>(&mut self, cx: &Scope<T>)
	{
		self.coreCharacter.push(cx);
		
		self.validate();
		
		let arcana = use_atom_ref(cx, MageArcana);
		let obsessions = use_atom_ref(cx, MageObsessions);
		
		(*arcana.write()) = self.arcana.clone();
		(*obsessions.write()) = self.obsessions.clone();
	}
	
	fn validate(&mut self)
	{
		
	}
}
