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
			CharacterAttributes,
			updateBaseHealth,
			updateBaseWillpower,
		},
		vtr2e::{
			advantages::{
				TemplateAdvantageType,
			},
			state::{
				KindredAdvantages,
				KindredDisciplines,
				updateTemplateAdvantage,
				updateVitae,
			}
		}
	},
};

pub fn Advantages(scope: Scope) -> Element
{
	let advantagesRef = use_atom_ref(&scope, CharacterAdvantages);
	let attributesRef = use_atom_ref(&scope, CharacterAttributes);
	let templateRef = use_atom_ref(&scope, KindredAdvantages);
	let disciplinesRef = use_atom_ref(&scope, KindredDisciplines);
	
	let mut advantages = advantagesRef.write();
	let attributes = attributesRef.read();
	let template = templateRef.read();
	let disciplines = disciplinesRef.read();
	
	let size = advantages.size;
	
	if disciplines.resilience.value > 0
	{
		advantages.health.updateMax(size + attributes.stamina.value + disciplines.resilience.value);
	}
	
	if disciplines.vigor.value > 0
	{
		advantages.speed = size + attributes.strength.value + attributes.dexterity.value + disciplines.vigor.value;
	}
	
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
	updateTemplateAdvantage(scope, TemplateAdvantageType::BloodPotency, clickedValue);
}

fn healthHandler(scope: &Scope<TrackProps>, index: usize)
{
	let value = scope.props.tracker.clone().getValue(index);
	match value
	{
		Some(ts) =>
		{
			match ts
			{
				TrackerState::One => { updateBaseHealth(&scope, TrackerState::Two, false, Some(index)); }
				TrackerState::Two => { updateBaseHealth(&scope, TrackerState::Three, false, Some(index)); }
				TrackerState::Three => { updateBaseHealth(&scope, TrackerState::Three, true, Some(index)); }
			}
		}
		None => { updateBaseHealth(&scope, TrackerState::One, false, None); }
	}
}

fn humanityHandler(scope: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	updateTemplateAdvantage(scope, TemplateAdvantageType::Humanity, clickedValue);
}

fn vitaeHandler(scope: &Scope<TrackProps>, index: usize)
{
	updateVitae(scope, index);
}

fn willpowerHandler(scope: &Scope<TrackProps>, index: usize)
{
	updateBaseWillpower(scope, index);
}
