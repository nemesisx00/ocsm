#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use dioxus::events::{
	FormEvent
};
use crate::cod::{
	components::dots::{
		Dots,
		DotsProps,
	},
	merits::Merit,
	state::{
		CharacterMerits,
	}
};

pub fn Merits(cx: Scope) -> Element
{
	let meritsRef = use_atom_ref(&cx, CharacterMerits);
	let merits = meritsRef.read();
	
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
					
					input { r#type: "text", value: "{merit.name}", onchange: move |e| inputHandler(e, &cx, Some(i)) }
					Dots { max: 5, value: merit.value, handler: dotsHandler, handlerKey: i }
				}))
				
				div
				{
					class: "entry row",
					
					input { r#type: "text", value: "", placeholder: "Type new Merits here", onchange: move |e| inputHandler(e, &cx, None) }
				}
			}
		}
	});
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
