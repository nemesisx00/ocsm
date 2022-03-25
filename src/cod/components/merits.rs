#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use dioxus::events::{
	FormEvent,
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
};

const DefaultLastIndex: usize = 1000;

pub fn Merits(cx: Scope) -> Element
{
	let meritsRef = use_atom_ref(&cx, CharacterMerits);
	let merits = meritsRef.read();
	
	let lastIndex = use_state(&cx, || DefaultLastIndex);
	let showRemove = lastIndex.get() < &merits.len();
	
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
					oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); },
					prevent_default: "oncontextmenu",
					
					input { r#type: "text", value: "{merit.name}", onchange: move |e| inputHandler(e, &cx, Some(i)), oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); }, prevent_default: "oncontextmenu" }
					Dots { max: 5, value: merit.value, handler: dotsHandler, handlerKey: i }
				}))
				
				div
				{
					class: "entry row",
					input { r#type: "text", value: "", placeholder: "Type new Merits here", onchange: move |e| inputHandler(e, &cx, None), oncontextmenu: move |e| e.cancel_bubble(), prevent_default: "oncontextmenu" }
				}
				
				showRemove.then(|| rsx!{
					div { class: "removePopUpOverlay column" }
					
					div
					{
						class: "removePopUpWrapper column",
						
						div
						{
							class: "removePopUp column",
							
							div { class: "row", "Are you sure you want to remove this Merit?" }
							div
							{
								class: "row",
								
								button { onclick: move |e| { e.cancel_bubble(); removeClickHandler(&cx, *(lastIndex.get())); lastIndex.set(DefaultLastIndex); }, prevent_default: "onclick", "Remove" }
								button { onclick: move |e| { e.cancel_bubble(); lastIndex.set(DefaultLastIndex); }, prevent_default: "onclick", "Cancel" }
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
	let meritsRef = use_atom_ref(&cx, CharacterMerits);
	let mut merits = meritsRef.write();
	
	if index < merits.len()
	{
		merits.remove(index);
	}
}

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
