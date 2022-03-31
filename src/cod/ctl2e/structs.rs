#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::{
	Scope,
	use_atom_ref,
	use_read,
	use_set,
};
use serde::{
	Deserialize,
	Serialize,
};
use crate::{
	cod::{
		ctl2e::{
			enums::Regalia,
			state::{
				ChangelingContracts,
				ChangelingFavoredRegalia,
				ChangelingFrailties,
				ChangelingTouchstones,
			},
		},
		structs::{
			ActiveAbility,
			CoreCharacter,
		},
	},
	core::state::StatefulTemplate,
};

/// Data structure defining a Changeling: The Lost 2e Changeling.
#[derive(Clone, Debug, Default, Deserialize, Serialize)]
pub struct Changeling
{
	#[serde(default)]
	pub coreCharacter: CoreCharacter,
	
	#[serde(default)]
	pub contracts: Vec<ActiveAbility>,
	
	#[serde(default)]
	pub favoredRegalia: Option<Regalia>,
	
	#[serde(default)]
	pub frailties: Vec<String>,
	
	#[serde(default)]
	pub touchstones: Vec<String>,
}

impl StatefulTemplate for Changeling
{
	fn pull<T>(&mut self, cx: &Scope<T>)
	{
		self.coreCharacter.pull(cx);
		
		let contracts = use_atom_ref(cx, ChangelingContracts);
		let favoredRegalia = use_read(cx, ChangelingFavoredRegalia);
		let frailties = use_atom_ref(cx, ChangelingFrailties);
		let touchstones = use_atom_ref(cx, ChangelingTouchstones);
		
		self.contracts = contracts.read().clone();
		self.favoredRegalia = favoredRegalia.clone();
		self.frailties = frailties.read().clone();
		self.touchstones = touchstones.read().clone();
		
		self.validate();
	}
	
	fn push<T>(&mut self, cx: &Scope<T>)
	{
		self.coreCharacter.push(cx);
		
		self.validate();
		
		let contracts = use_atom_ref(cx, ChangelingContracts);
		let favoredRegalia = use_set(cx, ChangelingFavoredRegalia);
		let frailties = use_atom_ref(cx, ChangelingFrailties);
		let touchstones = use_atom_ref(cx, ChangelingTouchstones);
		
		(*contracts.write()) = self.contracts.clone();
		favoredRegalia(self.favoredRegalia.clone());
		(*frailties.write()) = self.frailties.clone();
		(*touchstones.write()) = self.touchstones.clone();
	}
	
	fn validate(&mut self)
	{
		
	}
}
