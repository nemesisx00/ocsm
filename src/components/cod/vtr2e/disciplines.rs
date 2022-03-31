#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	prelude::*,
	events::FormEvent
};
use strum::IntoEnumIterator;
use crate::{
	cod::{
		vtr2e::{
			enums::Discipline,
			state::KindredDisciplines,
		},
	},
	core::util::{
		RemovePopUpXOffset,
		RemovePopUpYOffset,
	},
	components::cod::dots::{
		Dots,
		DotsProps,
	},
};

/// The UI Component handling a Vampire: The Requiem 2e Kindred's Disciplines.
pub fn Disciplines(cx: Scope) -> Element
{
	let disciplinesRef = use_atom_ref(&cx, KindredDisciplines);
	let disciplines = disciplinesRef.read();
	
	let clickedX = use_state(&cx, || 0);
	let clickedY = use_state(&cx, || 0);
	let lastIndex = use_state(&cx, || 0);
	let showRemove = use_state(&cx, || false);
	
	let posX = *clickedX.get() - RemovePopUpXOffset;
	let posY = *clickedY.get() - RemovePopUpYOffset;
	
	let mut optionNames = Vec::<String>::new();
	for dt in Discipline::iter()
	{
		optionNames.push(dt.as_ref().to_string());
	}
	
	return cx.render(rsx!
	{
		div
		{
			class: "disciplines entryListWrapper column",
			
			div { class: "entryListLabel", "Disciplines" }
			
			div
			{
				class: "entryList column",
				
				disciplines.iter().enumerate().map(|(i, (dt, value))|
				{
					let name = dt.as_ref().to_string();
					rsx!(cx, div
					{
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
						
						Dots { class: "row".to_string(), key: "{i}", label: name.clone(), max: 5, value: *value, handler: dotsHandler, handlerKey: *dt }
					})
				})
				
				div
				{
					class: "entry row",
					
					select
					{
						onchange: move |e| selectHandler(e, &cx),
						oncontextmenu: move |e| { e.cancel_bubble(); },
						prevent_default: "oncontextmenu",
						
						option { value: "", selected: "true", "Add a Discipline" }
						optionNames.iter().enumerate().map(|(i, name)| rsx!(cx, option { option { key: "{i}", value: "{name}", "{name}" } }))
					}
				}
			}
			
			showRemove.then(|| rsx!
			{
				div
				{
					class: "removePopUpWrapper column",
					style: "left: {posX}px; top: {posY}px;",
					onclick: move |e| { e.cancel_bubble(); showRemove.set(false); },
					prevent_default: "onclick",
					
					div
					{
						class: "removePopUp column",
						
						div { class: "row", "Are you sure you want to remove this Discipline?" }
						div
						{
							class: "row",
							
							button { onclick: move |e| { e.cancel_bubble(); removeDisciplineClickHandler(&cx, *(lastIndex.get())); showRemove.set(false); }, prevent_default: "onclick", "Remove" }
							button { onclick: move |e| { e.cancel_bubble(); showRemove.set(false); }, prevent_default: "onclick", "Cancel" }
						}
					}
				}
			})
		}
	});
}

/// Event handler triggered when a Discipline value changes.
fn dotsHandler(cx: &Scope<DotsProps<Discipline>>, clickedValue: usize)
{
	let disciplinesRef = use_atom_ref(&cx, KindredDisciplines);
	let mut disciplines = disciplinesRef.write();
	
	match &cx.props.handlerKey
	{
		Some(disciplineType) =>
		{
			match disciplines.get_mut(disciplineType)
			{
				Some(discipline) => { *discipline = clickedValue; }
				None => {}
			}
		}
		None => {}
	}
}

/// Event handler triggered by clicking the "Remove" button after right-clicking a
/// Discipline row.
fn removeDisciplineClickHandler(cx: &Scope, index: usize)
{
	let disciplinesRef = use_atom_ref(&cx, KindredDisciplines);
	let mut disciplines = disciplinesRef.write();
	
	match disciplines.clone().iter().enumerate().filter(|(i, _)| *i == index).next()
	{
		Some((_, (dt, _))) => { disciplines.remove(dt); }
		None => {}
	}
}

/// Event handler triggered when the New Discipline select input's value changes.
fn selectHandler(e: FormEvent, cx: &Scope)
{
	let disciplinesRef = use_atom_ref(&cx, KindredDisciplines);
	let mut disciplines = disciplinesRef.write();
	
	let disciplineType = e.value.to_string();
	
	match Discipline::asMap().iter().filter(|(_, name)| *name == &disciplineType).next()
	{
		Some((dt, _)) =>
		{
			match disciplines.get(dt)
			{
				Some(_) => {}
				None => { disciplines.insert(*dt, 0); }
			}
		}
		None => {}
	}
}
