#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	prelude::*,
	events::FormEvent
};
use strum::IntoEnumIterator;
use crate::{
	cod::{
		components::dots::{
			Dots,
			DotsProps,
		},
		vtr2e::{
			enums::{
				DevotionField,
				Discipline,
			},
			structs::Devotion,
			state::{
				KindredDisciplines,
				KindredDevotions,
			}
		},
	},
	core::util::{
		RemovePopUpXOffset,
		RemovePopUpYOffset,
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

// -----

/// The UI Component handling a Vampire: The Requiem 2e Kindred's Devotions and
/// Discipline Powers.
pub fn Devotions(cx: Scope) -> Element
{
	let devotionsRef = use_atom_ref(&cx, KindredDevotions);
	let devotions = devotionsRef.read();
	
	let showRemove = use_state(&cx, || false);
	let lastIndex = use_state(&cx, || 0);
	
	return cx.render(rsx!
	{
		div
		{
			class: "devotions entryListWrapper column",
			
			div { class: "entryListLabel", "Devotions & Powers" }
			
			div
			{
				class: "entryList column",
				
				devotions.iter().enumerate().map(|(i, dev)| rsx!(cx, div
				{
					class: "entry column",
					oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); },
					prevent_default: "oncontextmenu",
					
					div
					{
						class: "row",
						
						div { class: "label first", "Name:" }
						input { r#type: "text", value: "{dev.name}", onchange: move |e| inputHandler(e, &cx, Some(i), DevotionField::Name),  oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); showRemove.set(true); }, prevent_default: "oncontextmenu" }
						div { class: "label second", "Cost:" }
						input { r#type: "text", value: "{dev.cost}", onchange: move |e| inputHandler(e, &cx, Some(i), DevotionField::Cost),  oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); showRemove.set(true); }, prevent_default: "oncontextmenu" }
					}
					
					div
					{
						class: "row",
						
						div { class: "label first", "Dice Pool:" }
						input { r#type: "text", value: "{dev.dicePool}", onchange: move |e| inputHandler(e, &cx, Some(i), DevotionField::DicePool),  oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); showRemove.set(true); }, prevent_default: "oncontextmenu" }
						div { class: "label second", "Action:" }
						input { r#type: "text", value: "{dev.action}", onchange: move |e| inputHandler(e, &cx, Some(i), DevotionField::Action),  oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); showRemove.set(true); }, prevent_default: "oncontextmenu" }
					}
					
					div
					{
						class: "row",
						
						div { class: "label first", "Requirements:" }
						input { r#type: "text", value: "{dev.disciplines}", onchange: move |e| inputHandler(e, &cx, Some(i), DevotionField::Disciplines),  oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); showRemove.set(true); }, prevent_default: "oncontextmenu" }
						div { class: "label second", "Duration:" }
						input { r#type: "text", value: "{dev.duration}", onchange: move |e| inputHandler(e, &cx, Some(i), DevotionField::Duration),  oncontextmenu: move |e| { e.cancel_bubble(); lastIndex.set(i); showRemove.set(true); }, prevent_default: "oncontextmenu" }
					}
				}))
				
				div
				{
					class: "new entry row",
					input { r#type: "text", value: "", placeholder: "Enter a new Name", onchange: move |e| inputHandler(e, &cx, None, DevotionField::Name), oncontextmenu: move |e| e.cancel_bubble(), prevent_default: "oncontextmenu" }
				}
			}
			
			showRemove.then(|| rsx!
			{
				div { class: "removePopUpOverlay column" }
				
				div
				{
					class: "removePopUpWrapper column",
					
					div
					{
						class: "removePopUp column",
						
						div { class: "row", "Are you sure you want to remove this Devotion?" }
						div
						{
							class: "row",
							
							button { onclick: move |e| { e.cancel_bubble(); removeDevotionClickHandler(&cx, *(lastIndex.get())); showRemove.set(false); }, prevent_default: "onclick", "Remove" }
							button { onclick: move |e| { e.cancel_bubble(); showRemove.set(false); }, prevent_default: "onclick", "Cancel" }
						}
					}
				}
			})
		}
	});
}

/// Event handler triggered when a `Devotion` input's value changes.
fn inputHandler(e: FormEvent, cx: &Scope, index: Option<usize>, prop: DevotionField)
{
	let devotionsRef = use_atom_ref(&cx, KindredDevotions);
	let mut devotions = devotionsRef.write();
	
	match index
	{
		Some(i) =>
		{
			match prop
			{
				DevotionField::Action => { devotions[i].action = e.value.clone(); }
				DevotionField::Cost => { devotions[i].cost = e.value.clone(); }
				DevotionField::DicePool => { devotions[i].dicePool = e.value.clone(); }
				DevotionField::Disciplines => { devotions[i].disciplines = e.value.clone(); }
				DevotionField::Duration => { devotions[i].duration = e.value.clone(); }
				DevotionField::Name => { devotions[i].name = e.value.clone(); }
			}
		}
		None =>
		{
			match prop
			{
				DevotionField::Action => { devotions.push(Devotion { action: e.value.clone(), ..Default::default() }); }
				DevotionField::Cost => { devotions.push(Devotion { cost: e.value.clone(), ..Default::default() }); }
				DevotionField::DicePool => { devotions.push(Devotion { dicePool: e.value.clone(), ..Default::default() }); }
				DevotionField::Disciplines => { devotions.push(Devotion { disciplines: e.value.clone(), ..Default::default() }); }
				DevotionField::Duration => { devotions.push(Devotion { duration: e.value.clone(), ..Default::default() }); }
				DevotionField::Name => { devotions.push(Devotion { name: e.value.clone(), ..Default::default() }); }
			}
		}
	}
}

/// Event handler triggered by clicking the "Remove" button after right-clicking a
/// Devotion row.
fn removeDevotionClickHandler(cx: &Scope, index: usize)
{
	let devotionsRef = use_atom_ref(&cx, KindredDevotions);
	let mut devotions = devotionsRef.write();
	
	if index < devotions.len()
	{
		devotions.remove(index);
	}
}
