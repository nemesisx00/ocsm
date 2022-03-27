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
		vtr2e::{
			advantages::TemplateAdvantages,
			details::DetailType,
			disciplines::{
				Devotion,
				DisciplineType,
			},
			state::{
				KindredAdvantages,
				KindredDetails,
				KindredDevotions,
				KindredDisciplines,
				KindredTouchstones,
			}
		}
	}
};

#[derive(Clone, Debug, Default, Deserialize, Serialize)]
pub struct Vampire
{
	#[serde(default)]
	pub advantages: TemplateAdvantages,
	#[serde(default)]
	pub baseCharacter: BaseCharacter,
	#[serde(default)]
	pub details: BTreeMap<DetailType, String>,
	#[serde(default)]
	pub disciplines: BTreeMap<DisciplineType, usize>,
	#[serde(default)]
	pub devotions: Vec<Devotion>,
	#[serde(default)]
	pub touchstones: Vec<String>,
}

impl StatefulTemplate for Vampire
{
	fn pull<T>(&mut self, cx: &Scope<T>)
	{
		self.baseCharacter.pull(cx);
		
		let advantages = use_atom_ref(cx, KindredAdvantages);
		let details = use_atom_ref(cx, KindredDetails);
		let devotions = use_atom_ref(cx, KindredDevotions);
		let disciplines = use_atom_ref(cx, KindredDisciplines);
		let touchstones = use_atom_ref(cx, KindredTouchstones);
		
		self.advantages = advantages.read().clone();
		self.details = details.read().clone();
		self.devotions = devotions.read().clone();
		self.disciplines = disciplines.read().clone();
		self.touchstones = touchstones.read().clone();
		
		self.validate();
	}
	
	fn push<T>(&mut self, cx: &Scope<T>)
	{
		self.baseCharacter.push(cx);
		
		self.validate();
		
		let advantages = use_atom_ref(cx, KindredAdvantages);
		let details = use_atom_ref(cx, KindredDetails);
		let devotions = use_atom_ref(cx, KindredDevotions);
		let disciplines = use_atom_ref(cx, KindredDisciplines);
		let touchstones = use_atom_ref(cx, KindredTouchstones);
		
		(*advantages.write()) = self.advantages.clone();
		(*details.write()) = self.details.clone();
		(*devotions.write()) = self.devotions.clone();
		(*disciplines.write()) = self.disciplines.clone();
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

#[cfg(test)]
mod tests
{
	use super::*;
	
	#[test]
	fn test_Vampire_validate()
	{
		let expected = DetailType::asMap();
		
		let mut vampire = Vampire::default();
		vampire.details = BTreeMap::new();
		
		vampire.details.iter().for_each(|(dt, value)| assert_ne!(expected[dt], *value));
		
		vampire.validate();
		
		vampire.details.iter().for_each(|(dt, value)| assert_eq!(expected[dt], *value));
	}
}
