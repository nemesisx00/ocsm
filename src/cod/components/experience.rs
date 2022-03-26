#![allow(non_snake_case, non_upper_case_globals)]

use atoi::atoi;
use dioxus::{
	events::FormEvent,
	prelude::*,
};
use crate::cod::{
	components::tracks::{
		Track,
		TrackProps,
	},
	state::{
		CharacterBeats,
		CharacterExperience,
		updateBaseBeats,
	},
};

pub fn Experience(cx: Scope) -> Element
{
	let beats = use_atom_ref(&cx, CharacterBeats);
	let experience = use_read(&cx, CharacterExperience);
	
	return cx.render(rsx!
	{
		div
		{
			class: "experienceWrapper cod column",
			
			div { class: "experienceLabel", "Experience" },
			
			div
			{
				class: "column",
				
				Track { class: "row".to_string(), label: "Beats:".to_string(), tracker: beats.read().clone(), handler: beatsHandler }
				div
				{
					class: "row",
					
					div { class: "label", "Experience:" }
					input { r#type: "text", value: "{experience}", onchange: move |e| experienceHandler(e, &cx), oncontextmenu: move |e| e.cancel_bubble(), prevent_default: "oncontextmenu" }
				}
			}
		}
	});
}

fn beatsHandler(cx: &Scope<TrackProps>, index: usize)
{
	let experience = use_read(cx, CharacterExperience);
	let previousExperience = *experience;
	let setExperience = use_set(cx, CharacterExperience);
	
	match index + 1 > 4
	{
		true => { setExperience(previousExperience + 1); updateBaseBeats(cx, 0, true); }
		false => { updateBaseBeats(cx, index, false); }
	}
}

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
