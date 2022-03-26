#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	events::FormEvent,
	prelude::*,
};
use crate::cod::state::CharacterAspirations;

pub fn Aspirations(cx: Scope) -> Element
{
	let aspirationsRef = use_atom_ref(&cx, CharacterAspirations);
	let aspirations = aspirationsRef.read();
	
	let showRemove = use_state(&cx, || false);
	let lastIndex = use_state(&cx, || 0);
	
	return cx.render(rsx!
	{
		div
		{
			class: "aspirationsWrapper cod column",
			
			div { class: "aspirationsLabel", "Aspirations" },
			
			div
			{
				class: "aspirations column",
				
				aspirations.iter().enumerate().map(|(i, aspiration)| {
					rsx!(cx, div
					{
						class: "row",
						key: "{i}",
						input { r#type: "text", value: "{aspiration}", onchange: move |e| aspirationHandler(e, &cx, Some(i)), oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); showRemove.set(true); }, prevent_default: "oncontextmenu" }
					})
				})
				
				div
				{
					class: "row",
					input { r#type: "text", value: "", placeholder: "Enter new a Aspiration", onchange: move |e| aspirationHandler(e, &cx, None), oncontextmenu: move |e| e.cancel_bubble(), prevent_default: "oncontextmenu" }
				}
				
				showRemove.then(|| rsx!{
					div { class: "removePopUpOverlay column" }
					
					div
					{
						class: "removePopUpWrapper column",
						
						div
						{
							class: "removePopUp column",
							
							div { class: "row", "Are you sure you want to remove this Aspiration?" }
							div
							{
								class: "row",
								
								button { onclick: move |e| { e.cancel_bubble(); removeClickHandler(&cx, *(lastIndex.get())); showRemove.set(false); }, prevent_default: "onclick", "Remove" }
								button { onclick: move |e| { e.cancel_bubble(); showRemove.set(false); }, prevent_default: "onclick", "Cancel" }
							}
						}
					}
				})
			}
		}
	});
}

fn removeClickHandler(cx: &Scope, index: usize)
{
	let aspirationsRef = use_atom_ref(&cx, CharacterAspirations);
	let mut aspirations = aspirationsRef.write();
	
	if index < aspirations.len()
	{
		aspirations.remove(index);
	}
}

fn aspirationHandler(e: FormEvent, cx: &Scope, index: Option<usize>)
{
	let aspirationsRef = use_atom_ref(&cx, CharacterAspirations);
	let mut aspirations = aspirationsRef.write();
	
	match index
	{
		Some(i) => { aspirations[i] = e.value.to_string(); }
		None => { aspirations.push(e.value.to_string()); }
	}
}
