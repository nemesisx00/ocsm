#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	events::FormEvent,
	prelude::*,
};
use crate::cod::vtr2e::state::KindredTouchstones;

pub fn Touchstones(cx: Scope) -> Element
{
	let touchstonesRef = use_atom_ref(&cx, KindredTouchstones);
	let touchstones = touchstonesRef.read();
	
	let showRemove = use_state(&cx, || false);
	let lastIndex = use_state(&cx, || 0);
	
	return cx.render(rsx!
	{
		div
		{
			class: "touchstonesWrapper cod column",
			
			div { class: "touchstonesLabel", "Touchstones" },
			
			div
			{
				class: "touchstones column",
				
				touchstones.iter().enumerate().map(|(i, touchstone)| {
					rsx!(cx, div
					{
						class: "row",
						key: "{i}",
						input { r#type: "text", value: "{touchstone}", onchange: move |e| touchstoneHandler(e, &cx, Some(i)), oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); showRemove.set(true); }, prevent_default: "oncontextmenu" }
					})
				})
				
				div
				{
					class: "row",
					input { r#type: "text", value: "", placeholder: "Enter new a Touchstone", onchange: move |e| touchstoneHandler(e, &cx, None), oncontextmenu: move |e| e.cancel_bubble(), prevent_default: "oncontextmenu" }
				}
				
				showRemove.then(|| rsx!{
					div { class: "removePopUpOverlay column" }
					
					div
					{
						class: "removePopUpWrapper column",
						
						div
						{
							class: "removePopUp column",
							
							div { class: "row", "Are you sure you want to remove this Touchstone?" }
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
	let touchstonesRef = use_atom_ref(&cx, KindredTouchstones);
	let mut touchstones = touchstonesRef.write();
	
	if index < touchstones.len()
	{
		touchstones.remove(index);
	}
}

fn touchstoneHandler(e: FormEvent, cx: &Scope, index: Option<usize>)
{
	let touchstonesRef = use_atom_ref(&cx, KindredTouchstones);
	let mut touchstones = touchstonesRef.write();
	
	match index
	{
		Some(i) => { touchstones[i] = e.value.to_string(); }
		None => { touchstones.push(e.value.to_string()); }
	}
}