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

pub fn Merits(scope: Scope) -> Element
{
	let meritsRef = use_atom_ref(&scope, CharacterMerits);
	let merits = meritsRef.read();
	
	return scope.render(rsx!
	{
		div
		{
			class: "merits entryListWrapper column",
			
			div { class: "entryListLabel", "Merits" }
			
			div
			{
				class: "entryList column",
				
				merits.iter().enumerate().map(|(i, merit)| rsx!(scope, div
				{
					key: "{i}",
					class: "entry row",
					
					input { r#type: "text", value: "{merit.name}", onchange: move |e| inputHandler(e, &scope, Some(i)) }
					Dots { max: 5, value: merit.value, handler: dotsHandler, handlerKey: i }
				}))
				
				div
				{
					class: "entry row",
					
					input { r#type: "text", value: "", onchange: move |e| inputHandler(e, &scope, None) }
					Dots { max: 5, value: 0, handler: dotsHandler }
				}
			}
		}
	});
}

fn dotsHandler(scope: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	let meritsRef = use_atom_ref(&scope, CharacterMerits);
	let mut merits = meritsRef.write();
	
	match &scope.props.handlerKey
	{
		Some(index) => { merits[*index].value = clickedValue; }
		None => { merits.push(Merit { value: clickedValue, ..Default::default() }); }
	}
}

fn inputHandler(e: FormEvent, scope: &Scope, index: Option<usize>)
{
	let meritsRef = use_atom_ref(&scope, CharacterMerits);
	let mut merits = meritsRef.write();
	
	match index
	{
		Some(i) => { merits[i].name = e.value.clone(); }
		None => { merits.push(Merit { name: e.value.clone(), ..Default::default() }); }
	}
}
