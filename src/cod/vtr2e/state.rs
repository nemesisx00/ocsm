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

/// A Vampire: The Requiem 2e Kindred's Advantages.
pub static KindredAdvantages: AtomRef<TemplateAdvantages> = |_| TemplateAdvantages::default();
/// A Vampire: The Requiem 2e Kindred's details, such as Clan.
pub static KindredDetails: AtomRef<BTreeMap<DetailType, String>> = |_| DetailType::asMap();
/// A Vampire: The Requiem 2e Kindred's list of Devotions and Discipline Powers.
pub static KindredDevotions: AtomRef<Vec<Devotion>> = |_| Vec::<Devotion>::new();
/// A Vampire: The Requiem 2e Kindred's list of Disciplines.
pub static KindredDisciplines: AtomRef<BTreeMap<DisciplineType, usize>> = |_| BTreeMap::<DisciplineType, usize>::new();
/// A Vampire: The Requiem 2e Kindred's list of Touchstones.
pub static KindredTouchstones: AtomRef<Vec<String>> = |_| Vec::<String>::new();

/// Update the designated Kindred Advantage.
/// 
/// Automaticaly updates any affected Traits.
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

/// Update the designated Kindred Detail.
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

/// Update the value of the Vitae Track.
pub fn updateVitae<T>(cx: &Scope<T>, index: usize)
{
	let templateRef = use_atom_ref(&cx, KindredAdvantages);
	let mut template = templateRef.write();
	
	updateTrackerState_SingleState(&mut template.vitae, index, TrackerState::Two, false);
}

/// Reset all `cod::vtr2e::state` global state values.
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
