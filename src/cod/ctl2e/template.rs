#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::BTreeMap;
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
use strum::IntoEnumIterator;
use crate::{
	core::template::StatefulTemplate,
	cod::{
		character::CoreCharacter,
		ctl2e::{
			advantages::TemplateAdvantages,
			details::DetailType,
			regalia::{
				Contract,
				Regalia,
			},
			state::{
				ChangelingAdvantages,
				ChangelingContracts,
				ChangelingDetails,
				ChangelingFavoredRegalia,
				ChangelingFrailties,
				ChangelingTouchstones,
			},
		},
	},
};


/// Data structure defining a Changeling: The Lost 2e Changeling.
#[derive(Clone, Debug, Default, Deserialize, Serialize)]
pub struct Changeling
{
	#[serde(default)]
	pub advantages: TemplateAdvantages,
	#[serde(default)]
	pub coreCharacter: CoreCharacter,
	#[serde(default)]
	pub contracts: Vec<Contract>,
	#[serde(default)]
	pub details: BTreeMap<DetailType, String>,
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
		
		let advantages = use_atom_ref(cx, ChangelingAdvantages);
		let contracts = use_atom_ref(cx, ChangelingContracts);
		let details = use_atom_ref(cx, ChangelingDetails);
		let favoredRegalia = use_read(cx, ChangelingFavoredRegalia);
		let frailties = use_atom_ref(cx, ChangelingFrailties);
		let touchstones = use_atom_ref(cx, ChangelingTouchstones);
		
		self.advantages = advantages.read().clone();
		self.contracts = contracts.read().clone();
		self.details = details.read().clone();
		self.favoredRegalia = favoredRegalia.clone();
		self.frailties = frailties.read().clone();
		self.touchstones = touchstones.read().clone();
		
		self.validate();
	}
	
	fn push<T>(&mut self, cx: &Scope<T>)
	{
		self.coreCharacter.push(cx);
		
		self.validate();
		
		let advantages = use_atom_ref(cx, ChangelingAdvantages);
		let contracts = use_atom_ref(cx, ChangelingContracts);
		let details = use_atom_ref(cx, ChangelingDetails);
		let favoredRegalia = use_set(cx, ChangelingFavoredRegalia);
		let frailties = use_atom_ref(cx, ChangelingFrailties);
		let touchstones = use_atom_ref(cx, ChangelingTouchstones);
		
		(*advantages.write()) = self.advantages.clone();
		(*contracts.write()) = self.contracts.clone();
		(*details.write()) = self.details.clone();
		favoredRegalia(self.favoredRegalia.clone());
		(*frailties.write()) = self.frailties.clone();
		(*touchstones.write()) = self.touchstones.clone();
	}
	
	fn validate(&mut self)
	{
		for dt in DetailType::iter()
		{
			match self.details.get(&dt)
			{
				Some(_) => {}
				None => { self.details.insert(dt, "".to_string()); }
			}
		}
	}
}
