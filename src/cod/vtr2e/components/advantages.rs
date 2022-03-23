#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::{
	cod::{
		components::{
			dots::{
				Dots,
				DotsProps,
			},
			tracks::{
				Track,
				TrackProps,
			},
		},
		tracks::{
			TrackerState,
		},
		vtr2e::{
			kindred::{
				AdvantageType,
			},
			state::{
				KindredAdvantages,
				updateNumericAdvantage,
				updateHealth,
				updateWillpower,
			}
		}
	},
};

pub fn Advantages(scope: Scope) -> Element
{
	let advantages = use_atom_ref(&scope, KindredAdvantages);
	
	let defense = advantages.read().defense;
	let initiative = advantages.read().initiative;
	let size = advantages.read().size;
	let speed = advantages.read().speed;
	
	return scope.render(rsx!
	{		
		div
		{
			class: "advantages row",
			
			div
			{
				class: "column",
				
				div { class: "row", div { "Defense:" } div { "{defense}" } }
				div { class: "row", div { "Initiative:" } div { "{initiative}" } }
				div { class: "row", div { "Size:" } div { "{size}" } }
				div { class: "row", div { "Speed:" } div { "{speed}" } }
			}
			
			div
			{
				class: "trackers column",
				
				Track { label: "Health".to_string(), tracker: advantages.read().health.clone(), handler: healthHandler }
				Track { label: "Willpower".to_string(), tracker: advantages.read().willpower.clone(), handler: willpowerHandler }
				Dots { label: "Humanity".to_string(), max: 10, value: advantages.read().humanity, handler: humanityHandler }
				Dots { label: "Blood Potency".to_string(), max: 10, value: advantages.read().bloodPotency, handler: bloodPotencyHandler }
			}
		}
	});
}

fn bloodPotencyHandler(scope: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	updateNumericAdvantage(scope, AdvantageType::BloodPotency, clickedValue);
}

fn healthHandler(scope: &Scope<TrackProps>, index: usize)
{
	match scope.props.tracker.values.get(index)
	{
		Some(ts) =>
		{
			match ts
			{
				TrackerState::One => { updateHealth(&scope, TrackerState::Two, false, Some(index)); }
				TrackerState::Two => { updateHealth(&scope, TrackerState::Three, false, Some(index)); }
				TrackerState::Three => { updateHealth(&scope, TrackerState::Three, true, Some(index)); }
			}
		}
		None => { updateHealth(&scope, TrackerState::One, false, None); }
	}
}

fn humanityHandler(scope: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	updateNumericAdvantage(scope, AdvantageType::Humanity, clickedValue);
}

fn willpowerHandler(scope: &Scope<TrackProps>, index: usize)
{
	match scope.props.tracker.values.get(index)
	{
		Some(ts) => { updateWillpower(scope, *ts, Some(index)); }
		None => { updateWillpower(scope, TrackerState::Two, None); }
	}
}
