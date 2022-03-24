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
			disciplines::{
				DisciplineType,
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

pub fn Advantages(cx: Scope) -> Element
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let attributesRef = use_atom_ref(&cx, CharacterAttributes);
	let disciplinesRef = use_atom_ref(&cx, KindredDisciplines);
	let templateRef = use_atom_ref(&cx, KindredAdvantages);
	
	let mut advantages = advantagesRef.write();
	let attributes = attributesRef.read();
	let disciplines = disciplinesRef.read();
	let template = templateRef.read();
	
	let size = advantages.size;
	
	match disciplines.iter().filter(|d| d.name.as_str() == DisciplineType::Resilience.as_ref()).next()
	{
		Some(resilience) =>
		{
			if resilience.value > 0
			{
				advantages.health.updateMax(size + attributes.stamina.value + resilience.value);
			}
		}
		None => {}
	}
	
	match disciplines.iter().filter(|d| d.name.as_str() == DisciplineType::Vigor.as_ref()).next()
	{
		Some(vigor) =>
		{
			if vigor.value > 0
			{
				advantages.speed = size + attributes.strength.value + attributes.dexterity.value + vigor.value;
			}
		}
		None => {}
	}
	
	return cx.render(rsx!
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

fn bloodPotencyHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	updateTemplateAdvantage(cx, TemplateAdvantageType::BloodPotency, clickedValue);
}

fn healthHandler(cx: &Scope<TrackProps>, index: usize)
{
	let value = cx.props.tracker.clone().getValue(index);
	match value
	{
		Some(ts) =>
		{
			match ts
			{
				TrackerState::One => { updateBaseHealth(&cx, TrackerState::Two, false, Some(index)); }
				TrackerState::Two => { updateBaseHealth(&cx, TrackerState::Three, false, Some(index)); }
				TrackerState::Three => { updateBaseHealth(&cx, TrackerState::Three, true, Some(index)); }
			}
		}
		None => { updateBaseHealth(&cx, TrackerState::One, false, None); }
	}
}

fn humanityHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	updateTemplateAdvantage(cx, TemplateAdvantageType::Humanity, clickedValue);
}

fn vitaeHandler(cx: &Scope<TrackProps>, index: usize)
{
	updateVitae(cx, index);
}

fn willpowerHandler(cx: &Scope<TrackProps>, index: usize)
{
	updateBaseWillpower(cx, index);
}
