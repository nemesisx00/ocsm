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
		traits::{
			CoreAttributeType,
			CoreSkillType,
		},
		tracks::TrackerState,
		state::{
			CharacterAdvantages,
			CharacterAttributes,
			CharacterSkills,
			updateBaseHealth,
			updateBaseWillpower,
		},
		vtr2e::{
			advantages::TemplateAdvantageType,
			disciplines::DisciplineType,
			state::{
				KindredAdvantages,
				KindredDisciplines,
				updateTemplateAdvantage,
				updateVitae,
			}
		}
	},
};

/// The UI Component handling a Vampire: The Requiem 2e Kindred's Core and Kindred Advantages.
pub fn Advantages(cx: Scope) -> Element
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let attributesRef = use_atom_ref(&cx, CharacterAttributes);
	let disciplinesRef = use_atom_ref(&cx, KindredDisciplines);
	let skillsRef = use_atom_ref(&cx, CharacterSkills);
	let templateRef = use_atom_ref(&cx, KindredAdvantages);
	
	let mut advantages = advantagesRef.write();
	let attributes = attributesRef.read();
	let disciplines = disciplinesRef.read();
	let skills = skillsRef.read();
	let template = templateRef.read();
	
	let size = advantages.size;
	
	match disciplines.get(&DisciplineType::Celerity)
	{
		Some(celerity) =>
		{
			if celerity > &0
			{
				let attrDef = match attributes[&CoreAttributeType::Dexterity] <= attributes[&CoreAttributeType::Wits]
				{
					true => { attributes[&CoreAttributeType::Dexterity] }
					false => { attributes[&CoreAttributeType::Wits] }
				};
				advantages.defense = attrDef + skills[&CoreSkillType::Athletics] + celerity;
			}
		}
		None => {}
	}
	
	match disciplines.get(&DisciplineType::Resilience)
	{
		Some(resilience) =>
		{
			if resilience > &0
			{
				advantages.health.updateMax(size + attributes[&CoreAttributeType::Stamina] + resilience);
			}
		}
		None => {}
	}
	
	match disciplines.get(&DisciplineType::Vigor)
	{
		Some(vigor) =>
		{
			if vigor > &0
			{
				advantages.speed = size + attributes[&CoreAttributeType::Dexterity] + attributes[&CoreAttributeType::Strength] + vigor;
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

/// Event handler triggered when a dot in the Blood Potency Track is clicked.
fn bloodPotencyHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	updateTemplateAdvantage(cx, TemplateAdvantageType::BloodPotency, clickedValue);
}

/// Event handler triggered when a box in the Health Track is clicked.
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

/// Event handler triggered when a dot in the Humanity Track is clicked.
fn humanityHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	updateTemplateAdvantage(cx, TemplateAdvantageType::Humanity, clickedValue);
}

/// Event handler triggered when a box in the Vitae Track is clicked.
fn vitaeHandler(cx: &Scope<TrackProps>, index: usize)
{
	updateVitae(cx, index);
}

/// Event handler triggered when a box in the Willpower Track is clicked.
fn willpowerHandler(cx: &Scope<TrackProps>, index: usize)
{
	updateBaseWillpower(cx, index);
}
