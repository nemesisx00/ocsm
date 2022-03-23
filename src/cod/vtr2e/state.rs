#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::{
	cod::{
		tracks::{
			TrackerState,
		},
		vtr2e::{
			advantages::{
				TemplateAdvantages,
				TemplateAdvantagesType,
				bloodPotencyVitaeMax,
			},
			details::{
				Details,
				DetailsField,
			},
		},
	},
};

pub static KindredAdvantages: AtomRef<TemplateAdvantages> = |_| TemplateAdvantages::default();
pub static KindredDetails: AtomRef<Details> = |_| Details::default();

pub fn updateTemplateAdvantage<T>(scope: &Scope<T>, advantage: TemplateAdvantagesType, value: usize)
{
	let templateRef = use_atom_ref(&scope, KindredAdvantages);
	let mut template = templateRef.write();
	
	match advantage
	{
		TemplateAdvantagesType::BloodPotency =>
		{
			template.bloodPotency = value;
			template.vitae.updateMax(bloodPotencyVitaeMax(value));
		}
		
		TemplateAdvantagesType::Humanity => { template.humanity = value; }
		TemplateAdvantagesType::Vitae => { template.vitae.updateMax(value); }
	}
}

pub fn updateDetail<T>(scope: &Scope<T>, field: DetailsField, value: String)
{
	let detailsRef = use_atom_ref(&scope, KindredDetails);
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

pub fn updateVitae<T>(scope: &Scope<T>, damageType: TrackerState, index: Option<usize>)
{
	let templateRef = use_atom_ref(&scope, KindredAdvantages);
	let mut template = templateRef.write();
	
	match index
	{
		Some(_) =>
		{
			match damageType
			{
				TrackerState::Two => { template.vitae.remove(TrackerState::Two); }
				_ => { template.vitae.add(TrackerState::Two); }
			}
		}
		None => { template.vitae.add(TrackerState::Two); }
	}
}
