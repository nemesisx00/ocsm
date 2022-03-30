#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::{
	cod::{
		enums::{
			CoreAdvantage,
			CoreAttribute,
			CoreSkill,
			TrackerState,
		},
		state::{
			CharacterAdvantages,
			CharacterAttributes,
			CharacterSkills,
			updateCoreAdvantage,
			updateCoreHealth,
			updateCoreResource,
			updateCoreWillpower,
		},
		vtr2e::{
			enums::Discipline,
			state::KindredDisciplines,
		},
	},
	components::{
		cod::{
			dots::{
				Dots,
				DotsProps,
			},
			tracks::{
				Track,
				TrackProps,
			},
		},
	},
};

/// The UI Component handling a Vampire: The Requiem 2e Kindred's Core and Kindred Advantages.
pub fn Advantages(cx: Scope) -> Element
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let attributesRef = use_atom_ref(&cx, CharacterAttributes);
	let disciplinesRef = use_atom_ref(&cx, KindredDisciplines);
	let skillsRef = use_atom_ref(&cx, CharacterSkills);
	
	let mut advantages = advantagesRef.write();
	let attributes = attributesRef.read();
	let disciplines = disciplinesRef.read();
	let skills = skillsRef.read();
	
	let size = advantages.size;
	
	match disciplines.get(&Discipline::Celerity)
	{
		Some(celerity) =>
		{
			if celerity > &0
			{
				let attrDef = match attributes[&CoreAttribute::Dexterity] <= attributes[&CoreAttribute::Wits]
				{
					true => { attributes[&CoreAttribute::Dexterity] }
					false => { attributes[&CoreAttribute::Wits] }
				};
				advantages.defense = attrDef + skills[&CoreSkill::Athletics] + celerity;
			}
		}
		None => {}
	}
	
	match disciplines.get(&Discipline::Resilience)
	{
		Some(resilience) =>
		{
			if resilience > &0
			{
				advantages.health.updateMax(size + attributes[&CoreAttribute::Stamina] + resilience);
			}
		}
		None => {}
	}
	
	match disciplines.get(&Discipline::Vigor)
	{
		Some(vigor) =>
		{
			if vigor > &0
			{
				advantages.speed = size + attributes[&CoreAttribute::Dexterity] + attributes[&CoreAttribute::Strength] + vigor;
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
				Dots { label: "Humanity".to_string(), max: 10, value: advantages.integrity, handler: humanityHandler }
				Dots { label: "Blood Potency".to_string(), max: 10, value: advantages.power.clone().unwrap(), handler: bloodPotencyHandler }
				Track { label: "Vitae".to_string(), tracker: advantages.resource.clone().unwrap().clone(), handler: vitaeHandler }
			}
		}
	});
}

/// Event handler triggered when a dot in the Blood Potency Track is clicked.
fn bloodPotencyHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	updateCoreAdvantage(cx, CoreAdvantage::Power, clickedValue);
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
				TrackerState::One => { updateCoreHealth(&cx, TrackerState::Two, false, Some(index)); }
				TrackerState::Two => { updateCoreHealth(&cx, TrackerState::Three, false, Some(index)); }
				TrackerState::Three => { updateCoreHealth(&cx, TrackerState::Three, true, Some(index)); }
			}
		}
		None => { updateCoreHealth(&cx, TrackerState::One, false, None); }
	}
}

/// Event handler triggered when a dot in the Humanity Track is clicked.
fn humanityHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	updateCoreAdvantage(cx, CoreAdvantage::Integrity, clickedValue);
}

/// Event handler triggered when a box in the Vitae Track is clicked.
fn vitaeHandler(cx: &Scope<TrackProps>, index: usize)
{
	updateCoreResource(cx, index);
}

/// Event handler triggered when a box in the Willpower Track is clicked.
fn willpowerHandler(cx: &Scope<TrackProps>, index: usize)
{
	updateCoreWillpower(cx, index);
}
