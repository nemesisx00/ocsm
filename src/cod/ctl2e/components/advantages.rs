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
		ctl2e::{
			advantages::TemplateAdvantageType,
			state::{
				ChangelingAdvantages,
				updateTemplateAdvantage,
				updateGlamour,
			},
		},
		traits::{
			BaseAttributeType,
			BaseSkillType,
		},
		tracks::TrackerState,
		state::{
			CharacterAdvantages,
			CharacterAttributes,
			CharacterSkills,
			updateBaseHealth,
			updateBaseWillpower,
		}
	},
};

pub fn Advantages(cx: Scope) -> Element
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let attributesRef = use_atom_ref(&cx, CharacterAttributes);
	let skillsRef = use_atom_ref(&cx, CharacterSkills);
	let templateRef = use_atom_ref(&cx, ChangelingAdvantages);
	
	let mut advantages = advantagesRef.write();
	let attributes = attributesRef.read();
	let skills = skillsRef.read();
	let template = templateRef.read();
	
	let size = advantages.size;
	
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
				Dots { label: "Clarity".to_string(), max: 10, value: template.clarity, handler: clarityHandler }
				Dots { label: "Wyrd".to_string(), max: 10, value: template.wyrd, handler: wyrdHandler }
				Track { label: "Glamour".to_string(), tracker: template.glamour.clone(), handler: glamourHandler }
			}
		}
	});
}

fn wyrdHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	updateTemplateAdvantage(cx, TemplateAdvantageType::Wyrd, clickedValue);
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

fn clarityHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	updateTemplateAdvantage(cx, TemplateAdvantageType::Clarity, clickedValue);
}

fn glamourHandler(cx: &Scope<TrackProps>, index: usize)
{
	updateGlamour(cx, index);
}

fn willpowerHandler(cx: &Scope<TrackProps>, index: usize)
{
	updateBaseWillpower(cx, index);
}
