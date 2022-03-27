#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::BTreeMap;
use dioxus::prelude::*;
use crate::{
	cod::{
		state::updateTrackerState_SingleState,
		tracks::TrackerState,
		ctl2e::{
			details::DetailType,
		},
	},
};

//pub static ChangelingAdvantages: AtomRef<TemplateAdvantages> = |_| TemplateAdvantages::default();
pub static ChangelingDetails: AtomRef<BTreeMap<DetailType, String>> = |_| DetailType::asMap();
//pub static ChangelingTouchstones: AtomRef<Vec<String>> = |_| Vec::<String>::new();

/*
pub fn updateTemplateAdvantage<T>(cx: &Scope<T>, advantage: TemplateAdvantageType, value: usize)
{
	let templateRef = use_atom_ref(&cx, ChangelingAdvantages);
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
*/

pub fn updateDetail<T>(cx: &Scope<T>, detailType: DetailType, value: String)
{
	let detailsRef = use_atom_ref(&cx, ChangelingDetails);
	let mut details = detailsRef.write();
	
	match details.get_mut(&detailType)
	{
		Some(detail) => { *detail = value; }
		None => {}
	}
}

/*
pub fn updateVitae<T>(cx: &Scope<T>, index: usize)
{
	let templateRef = use_atom_ref(&cx, ChangelingAdvantages);
	let mut template = templateRef.write();
	
	updateTrackerState_SingleState(&mut template.vitae, index, TrackerState::Two, false);
}
*/
