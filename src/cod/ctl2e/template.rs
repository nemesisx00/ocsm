#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::BTreeMap;
use dioxus::prelude::{
	Scope,
	use_atom_ref,
};
use serde::{
	Serialize,
	Deserialize,
};
use strum::IntoEnumIterator;
use crate::{
	core::template::StatefulTemplate,
	cod::{
		character::BaseCharacter,
		ctl2e::{
			details::DetailType,
			state::{
				ChangelingDetails,
			},
		},
	},
};

#[derive(Clone, Debug, Default, Deserialize, Serialize)]
pub struct Changeling
{
	/*
	#[serde(default)]
	pub advantages: TemplateAdvantages,
	*/
	#[serde(default)]
	pub baseCharacter: BaseCharacter,
	#[serde(default)]
	pub details: BTreeMap<DetailType, String>,
	/*
	#[serde(default)]
	pub touchstones: Vec<String>,
	*/
}

impl StatefulTemplate for Changeling
{
	fn pull<T>(&mut self, cx: &Scope<T>)
	{
		self.baseCharacter.pull(cx);
		
		//let advantages = use_atom_ref(cx, ChangelingAdvantages);
		let details = use_atom_ref(cx, ChangelingDetails);
		//let touchstones = use_atom_ref(cx, ChangelingTouchstones);
		
		//self.advantages = advantages.read().clone();
		self.details = details.read().clone();
		//self.touchstones = touchstones.read().clone();
		
		self.validate();
	}
	
	fn push<T>(&mut self, cx: &Scope<T>)
	{
		self.baseCharacter.push(cx);
		
		self.validate();
		
		//let advantages = use_atom_ref(cx, ChangelingAdvantages);
		let details = use_atom_ref(cx, ChangelingDetails);
		//let touchstones = use_atom_ref(cx, ChangelingTouchstones);
		
		//(*advantages.write()) = self.advantages.clone();
		(*details.write()) = self.details.clone();
		//(*touchstones.write()) = self.touchstones.clone();
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
