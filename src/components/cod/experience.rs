#![allow(non_snake_case, non_upper_case_globals)]

use atoi::atoi;
use dioxus::{
	events::FormEvent,
	prelude::*,
};
use crate::{
	cod::{
		state::{
			CharacterBeats,
			CharacterExperience,
			updateCoreBeats,
		},
	},
	components::core::tracks::{
		Track,
		TrackProps,
	},
};

/// The UI Component handling a Chronicles of Darkness character's Experience.
pub fn Experience(cx: Scope) -> Element
{
	let beats = use_atom_ref(&cx, CharacterBeats);
	let experience = use_read(&cx, CharacterExperience);
	
	return cx.render(rsx!
	{
		div
		{
			class: "experienceWrapper cod column justEven",
			
			div { class: "experienceLabel", "Experience" },
			
			div
			{
				class: "column justEven",
				
				Track { class: "row justEven".to_string(), label: "Beats:".to_string(), tracker: beats.read().clone(), handler: beatsHandler }
				div
				{
					class: "row justEven",
					
					div { class: "label", "Experience:" }
					input { r#type: "text", value: "{experience}", onchange: move |e| experienceHandler(e, &cx), oncontextmenu: move |e| e.cancel_bubble(), prevent_default: "oncontextmenu" }
				}
			}
		}
	});
}

/// Event handler triggered when the Beats `Dots` is clicked.
fn beatsHandler(cx: &Scope<TrackProps>, index: usize)
{
	let experience = use_read(cx, CharacterExperience);
	let previousExperience = *experience;
	let setExperience = use_set(cx, CharacterExperience);
	
	match index + 1 > 4
	{
		true => { setExperience(previousExperience + 1); updateCoreBeats(cx, 0, true); }
		false => { updateCoreBeats(cx, index, false); }
	}
}

/// Event handler triggered when the Experience input's value changes.
fn experienceHandler(e: FormEvent, cx: &Scope)
{
	let experience = use_read(cx, CharacterExperience);
	let previousExperience = *experience;
	let setExperience = use_set(cx, CharacterExperience);
	
	match atoi::<usize>(e.value.as_bytes())
	{
		Some(xp) => { setExperience(xp); }
		None => { setExperience(previousExperience); }
	}
}
