#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::BTreeMap;
use dioxus::prelude::*;
use crate::{
	cod::{
		state::updateTrackerState_SingleState,
		tracks::TrackerState,
		ctl2e::{
			advantages::{
				TemplateAdvantages,
				TemplateAdvantageType,
				wyrdGlamourMax,
			},
			details::DetailType,
			regalia::{
				Contract,
				Regalia,
			},
		},
	},
};

pub static ChangelingAdvantages: AtomRef<TemplateAdvantages> = |_| TemplateAdvantages::default();
pub static ChangelingContracts: AtomRef<Vec<Contract>> = |_| Vec::<Contract>::new();
pub static ChangelingDetails: AtomRef<BTreeMap<DetailType, String>> = |_| DetailType::asMap();
pub static ChangelingFavoredRegalia: Atom<Option<Regalia>> = |_| None;
pub static ChangelingFrailties: AtomRef<Vec<String>> = |_| Vec::<String>::new();
pub static ChangelingTouchstones: AtomRef<Vec<String>> = |_| Vec::<String>::new();

pub fn updateTemplateAdvantage<T>(cx: &Scope<T>, advantage: TemplateAdvantageType, value: usize)
{
	let templateRef = use_atom_ref(&cx, ChangelingAdvantages);
	let mut template = templateRef.write();
	
	match advantage
	{
		TemplateAdvantageType::Wyrd =>
		{
			template.wyrd = value;
			template.glamour.updateMax(wyrdGlamourMax(value));
		}
		
		TemplateAdvantageType::Clarity => { template.clarity = value; }
		TemplateAdvantageType::Glamour => { template.glamour.updateMax(value); }
	}
}

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

pub fn updateGlamour<T>(cx: &Scope<T>, index: usize)
{
	let templateRef = use_atom_ref(&cx, ChangelingAdvantages);
	let mut template = templateRef.write();
	
	updateTrackerState_SingleState(&mut template.glamour, index, TrackerState::Two, false);
}
