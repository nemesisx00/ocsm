#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::{
	cod::{
		tracks::TrackerState,
		vtr2e::{
			advantages::{
				TemplateAdvantages,
				TemplateAdvantageType,
				bloodPotencyVitaeMax,
			},
			details::{
				Details,
				DetailsField,
			},
			disciplines::{
				Devotion,
				Discipline,
			},
		},
	},
};

pub static KindredAdvantages: AtomRef<TemplateAdvantages> = |_| TemplateAdvantages::default();
pub static KindredDetails: AtomRef<Details> = |_| Details::default();
pub static KindredDevotions: AtomRef<Vec<Devotion>> = |_| Vec::<Devotion>::new();
pub static KindredDisciplines: AtomRef<Vec<Discipline>> = |_| Vec::<Discipline>::new();
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

pub fn updateDetail<T>(cx: &Scope<T>, field: DetailsField, value: String)
{
	let detailsRef = use_atom_ref(&cx, KindredDetails);
	let mut details = detailsRef.write();
	
	match field
	{
		DetailsField::Bloodline => { details.bloodline = value; }
		DetailsField::Chronicle => { details.chronicle = value; }
		DetailsField::Clan => { details.clan = value; }
		DetailsField::Concept => { details.concept = value; }
		DetailsField::Covenant => { details.covenant = value; }
		DetailsField::Dirge => { details.dirge = value; }
		DetailsField::Mask => { details.mask = value; }
		DetailsField::Name => { details.name = value; }
		DetailsField::Player => { details.player = value; }
	}
}

pub fn updateVitae<T>(cx: &Scope<T>, index: usize)
{
	let templateRef = use_atom_ref(&cx, KindredAdvantages);
	let mut template = templateRef.write();
	
	let len = template.vitae.values.len();
	
	if index >= len
	{
		for _ in len..index+1 { template.vitae.add(TrackerState::Two) }
	}
	else if index < len
	{
		for _ in index..len { template.vitae.remove(TrackerState::Two) }
		
		// If we're trying to "disable" more than one box, add one back in.
		// People naturally click where they want the checks to stop
		// not one above where they want them to stop.
		// However, this makes clicking the top most checked box act weird
		// thus... the if.
		if len - index > 1
		{
			template.vitae.add(TrackerState::Two);
		}
	}
}
