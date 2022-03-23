#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::{
	cod::{
		tracks::{
			TrackerState,
		},
		vtr2e::{
			kindred::{
				Advantages,
				AdvantageType,
			},
			details::{
				Details,
				DetailsField,
			},
		},
	},
};

pub static KindredAdvantages: AtomRef<Advantages> = |_| Advantages::default();
pub static KindredDetails: AtomRef<Details> = |_| Details::default();

pub fn updateNumericAdvantage<T>(scope: &Scope<T>, advantage: AdvantageType, value: usize)
{
	let advantages = use_atom_ref(&scope, KindredAdvantages);
	
	match advantage
	{
		AdvantageType::BloodPotency => { advantages.write().bloodPotency = value; }
		AdvantageType::Defense => { advantages.write().defense = value; }
		AdvantageType::Health => { advantages.write().health.max = value; }
		AdvantageType::Humanity => { advantages.write().humanity = value; }
		AdvantageType::Initiative => { advantages.write().initiative = value; }
		AdvantageType::Size => { advantages.write().size = value; }
		AdvantageType::Speed => { advantages.write().speed = value; }
		AdvantageType::Willpower => { advantages.write().willpower.max = value; }
	}
}

pub fn updateDetail<T>(scope: &Scope<T>, field: DetailsField, value: String)
{
	let details = use_atom_ref(&scope, KindredDetails);
	
	match field
	{
		DetailsField::Bloodline => { details.write().bloodline = value; }
		DetailsField::Chronicle => { details.write().chronicle = value; }
		DetailsField::Clan => { details.write().clan = value; }
		DetailsField::Concept => { details.write().concept = value; }
		DetailsField::Covenant => { details.write().covenant = value; }
		DetailsField::Dirge => { details.write().dirge = value; }
		DetailsField::Mask => { details.write().mask = value; }
		DetailsField::Name => { details.write().name = value; }
		DetailsField::Player => { details.write().player = value; }
	}
}

pub fn updateHealth<T>(scope: &Scope<T>, damageType: TrackerState, remove: bool, index: Option<usize>)
{
	let advantages = use_atom_ref(&scope, KindredAdvantages);
	
	if remove
	{
		advantages.write().health.remove(damageType);
	}
	else
	{
		match index
		{
			Some(i) => { advantages.write().health.update(damageType, i); }
			None => { advantages.write().health.add(damageType); }
		}
	}
}

pub fn updateWillpower<T>(scope: &Scope<T>, damageType: TrackerState, index: Option<usize>)
{
	let advantages = use_atom_ref(&scope, KindredAdvantages);
	
	match index
	{
		Some(_) =>
		{
			match damageType
			{
				TrackerState::Two => { advantages.write().willpower.remove(TrackerState::Two); }
				_ => { advantages.write().willpower.add(TrackerState::Two); }
			}
		}
		None => { advantages.write().willpower.add(TrackerState::Two); }
	}
}
