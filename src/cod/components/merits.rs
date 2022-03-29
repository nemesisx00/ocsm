#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	prelude::*,
	events::{
		FormEvent,
	},
};
use crate::{
	cod::{
		components::dots::{
			Dots,
			DotsProps,
		},
		merits::Merit,
		state::{
			CharacterMerits,
		},
	},
	core::util::{
		RemovePopUpXOffset,
		RemovePopUpYOffset,
	}
};

/// The UI Component handling a Chronicles of Darkness character's list of Merits.
pub fn Merits(cx: Scope) -> Element
{
	let meritsRef = use_atom_ref(&cx, CharacterMerits);
	let merits = meritsRef.read();
	
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
			class: "merits entryListWrapper column",
			
			div { class: "entryListLabel", "Merits" }
			
			div
			{
				class: "entryList column",
				
				merits.iter().enumerate().map(|(i, merit)| rsx!(cx, div
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
						value: "{merit.name}",
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
					Dots { max: 5, value: merit.value, handler: dotsHandler, handlerKey: i }
				}))
				
				div
				{
					class: "entry row",
					input { r#type: "text", value: "", placeholder: "Enter new a Merit", onchange: move |e| inputHandler(e, &cx, None), oncontextmenu: move |e| e.cancel_bubble(), prevent_default: "oncontextmenu" }
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
							
							div { class: "row", "Are you sure you want to remove this Merit?" }
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

/// Event handler triggered by clicking the "Remove" button after right-clicking a Merit row.
fn removeClickHandler(cx: &Scope, index: usize)
{
	let meritsRef = use_atom_ref(&cx, CharacterMerits);
	let mut merits = meritsRef.write();
	
	if index < merits.len()
	{
		merits.remove(index);
	}
}

/// Event handler triggered when a `Merit`'s `Dots` is clicked.
fn dotsHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	let meritsRef = use_atom_ref(&cx, CharacterMerits);
	let mut merits = meritsRef.write();
	
	match &cx.props.handlerKey
	{
		Some(index) => { merits[*index].value = clickedValue; }
		None => {}
	}
}

/// Event handler triggered when the `Merit` label input's value changes.
fn inputHandler(e: FormEvent, cx: &Scope, index: Option<usize>)
{
	let meritsRef = use_atom_ref(&cx, CharacterMerits);
	let mut merits = meritsRef.write();
	
	match index
	{
		Some(i) => { merits[i].name = e.value.clone(); }
		None => { merits.push(Merit { name: e.value.clone(), ..Default::default() }); }
	}
}
