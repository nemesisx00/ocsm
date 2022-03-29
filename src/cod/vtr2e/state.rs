#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::BTreeMap;
use dioxus::prelude::*;
use crate::{
	cod::{
		state::updateTrackerState_SingleState,
		tracks::TrackerState,
		vtr2e::{
			advantages::{
				TemplateAdvantages,
				TemplateAdvantageType,
				bloodPotencyVitaeMax,
			},
			details::DetailType,
			disciplines::{
				Devotion,
				DisciplineType,
			},
		},
	},
};

pub static KindredAdvantages: AtomRef<TemplateAdvantages> = |_| TemplateAdvantages::default();
pub static KindredDetails: AtomRef<BTreeMap<DetailType, String>> = |_| DetailType::asMap();
pub static KindredDevotions: AtomRef<Vec<Devotion>> = |_| Vec::<Devotion>::new();
pub static KindredDisciplines: AtomRef<BTreeMap<DisciplineType, usize>> = |_| BTreeMap::<DisciplineType, usize>::new();
pub static KindredTouchstones: AtomRef<Vec<String>> = |_| Vec::<String>::new();

pub fn updateTemplateAdvantage<T>(cx: &Scope<T>, advantage: TemplateAdvantageType, value: usize)
{
	let templateRef = use_atom_ref(&cx, KindredAdvantages);
	let mut template = templateRef.write();
	
	match advantage
	{
		TemplateAdvantageType::BloodPotency =>
		{
			template.bloodPotency = value;
			template.vitae.updateMax(bloodPotencyVitaeMax(value));
		}
		
		TemplateAdvantageType::Humanity => { template.humanity = value; }
		TemplateAdvantageType::Vitae => { template.vitae.updateMax(value); }
	}
}

pub fn updateDetail<T>(cx: &Scope<T>, detailType: DetailType, value: String)
{
	let detailsRef = use_atom_ref(&cx, KindredDetails);
	let mut details = detailsRef.write();
	
	match details.get_mut(&detailType)
	{
		Some(detail) => { *detail = value; }
		None => {}
	}
}

pub fn updateVitae<T>(cx: &Scope<T>, index: usize)
{
	let templateRef = use_atom_ref(&cx, KindredAdvantages);
	let mut template = templateRef.write();
	
	updateTrackerState_SingleState(&mut template.vitae, index, TrackerState::Two, false);
}

/// Reset every stateful value in the application, regardless of game system.
pub fn resetGlobalStateVtr2e<T>(cx: &Scope<T>)
{
	let kindredAdvantages = use_atom_ref(cx, KindredAdvantages);
	let kindredDetails = use_atom_ref(cx, KindredDetails);
	let kindredDevotions = use_atom_ref(cx, KindredDevotions);
	let kindredDisciplines = use_atom_ref(cx, KindredDisciplines);
	let kindredTouchstones = use_atom_ref(cx, KindredTouchstones);
	
	(*kindredAdvantages.write()) = TemplateAdvantages::default();
	(*kindredDetails.write()) = DetailType::asMap();
	(*kindredDevotions.write()) = Vec::<Devotion>::new();
	(*kindredDisciplines.write()) = BTreeMap::<DisciplineType, usize>::new();
	(*kindredTouchstones.write()) = Vec::<String>::new();
}
