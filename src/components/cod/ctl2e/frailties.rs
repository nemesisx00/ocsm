#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	events::FormEvent,
	prelude::*
};
use crate::{
	cod::{
		ctl2e::state::ChangelingFrailties,
	},
	core::util::{
		RemovePopUpXOffset,
		RemovePopUpYOffset,
	},
};

/// The UI Component handling a Changeling: The Lost 2e Changeling's list of Frailties.
pub fn Frailties(cx: Scope) -> Element
{
	let frailtiesRef = use_atom_ref(&cx, ChangelingFrailties);
	let frailties = frailtiesRef.read();
	
	let clickedX = use_state(&cx, || 0);
	let clickedY = use_state(&cx, || 0);
	let lastIndex = use_state(&cx, || 0);
	let showRemove = use_state(&cx, || false);
	
	let posX = *clickedX.get() - RemovePopUpXOffset;
	let posY = *clickedY.get() - RemovePopUpYOffset;
	
	return cx.render(rsx!
	{
		div
		{
			class: "frailties entryListWrapper column",
			
			div { class: "entryListLabel", "Frailties" }
			
			div
			{
				class: "entryList column",
				
				frailties.iter().enumerate().map(|(i, frailty)| rsx!(cx, div
				{
					key: "{i}",
					class: "entry row",
					oncontextmenu: move |e|
					{
						e.cancel_bubble();
						clickedX.set(e.data.client_x);
						clickedY.set(e.data.client_y);
						lastIndex.set(i);
						showRemove.set(true);
					},
					prevent_default: "oncontextmenu",
					
					input
					{
						r#type: "text",
						value: "{frailty}",
						onchange: move |e| inputHandler(e, &cx, Some(i)),
						oncontextmenu: move |e|
						{
							e.cancel_bubble();
							clickedX.set(e.data.client_x);
							clickedY.set(e.data.client_y);
							lastIndex.set(i);
							showRemove.set(true);
						},
						prevent_default: "oncontextmenu"
					}
				}))
				
				div
				{
					class: "entry row",
					input { r#type: "text", value: "", placeholder: "Enter new a Frailty", onchange: move |e| inputHandler(e, &cx, None), oncontextmenu: move |e| e.cancel_bubble(), prevent_default: "oncontextmenu" }
				}
				
				showRemove.then(|| rsx!{
					div
					{
						class: "removePopUpWrapper column",
						style: "left: {posX}px; top: {posY}px;",
						onclick: move |e| { e.cancel_bubble(); showRemove.set(false); },
						prevent_default: "onclick",
						
						div
						{
							class: "removePopUp column",
							
							div { class: "row", "Are you sure you want to remove this Frailty?" }
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

/// Event handler triggered by clicking the "Remove" button after right-clicking a Frailty row.
fn removeClickHandler(cx: &Scope, index: usize)
{
	let frailtiesRef = use_atom_ref(&cx, ChangelingFrailties);
	let mut frailties = frailtiesRef.write();
	
	if index < frailties.len()
	{
		frailties.remove(index);
	}
}

/// Event handler triggered when a Frailty input's value changes
/// or when the "Add a New Frailty" input's value changes.
fn inputHandler(e: FormEvent, cx: &Scope, index: Option<usize>)
{
	let frailtiesRef = use_atom_ref(&cx, ChangelingFrailties);
	let mut frailties = frailtiesRef.write();
	
	match index
	{
		Some(i) => { frailties[i] = e.value.clone(); }
		None => { frailties.push(e.value.clone()); }
	}
}
