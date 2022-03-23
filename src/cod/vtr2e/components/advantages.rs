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
		state::{
			CharacterAdvantages,
			updateHealth,
			updateWillpower,
		},
		vtr2e::{
			advantages::{
				TemplateAdvantagesType,
			},
			state::{
				KindredAdvantages,
				updateTemplateAdvantage,
				updateVitae,
			}
		}
	},
};

pub fn Advantages(scope: Scope) -> Element
{
	let advantagesRef = use_atom_ref(&scope, CharacterAdvantages);
	let templateRef = use_atom_ref(&scope, KindredAdvantages);
	
	let advantages = advantagesRef.read();
	let template = templateRef.read();
	
	return scope.render(rsx!
	{		
		div
		{
			class: "advantages row",
			
			div
			{
				class: "column",
				
				Track { label: "Health".to_string(), tracker: advantages.health.clone(), handler: healthHandler }
				Track { label: "Willpower".to_string(), tracker: advantages.willpower.clone(), handler: willpowerHandler }
				Dots { label: "Humanity".to_string(), max: 10, value: template.humanity, handler: humanityHandler }
				Dots { label: "Blood Potency".to_string(), max: 10, value: template.bloodPotency, handler: bloodPotencyHandler }
				Track { label: "Vitae".to_string(), tracker: template.vitae.clone(), handler: vitaeHandler }
			}
		}
	});
}

fn bloodPotencyHandler(scope: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	updateTemplateAdvantage(scope, TemplateAdvantagesType::BloodPotency, clickedValue);
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
	updateTemplateAdvantage(scope, TemplateAdvantagesType::Humanity, clickedValue);
}

fn vitaeHandler(scope: &Scope<TrackProps>, index: usize)
{
	match scope.props.tracker.values.get(index)
	{
		Some(ts) => { updateVitae(scope, *ts, Some(index)); }
		None => { updateVitae(scope, TrackerState::Two, None); }
	}
}

fn willpowerHandler(scope: &Scope<TrackProps>, index: usize)
{
	match scope.props.tracker.values.get(index)
	{
		Some(ts) => { updateWillpower(scope, *ts, Some(index)); }
		None => { updateWillpower(scope, TrackerState::Two, None); }
	}
}
