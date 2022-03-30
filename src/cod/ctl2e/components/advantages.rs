#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	events::FormEvent,
	prelude::*
};
use crate::{
	core::util::{
		RemovePopUpXOffset,
		RemovePopUpYOffset,
	},
	cod::{
		components::{
			dots::{
				Dots,
				DotsProps,
			},
			tracks::{
				Track,
				TrackProps,
			},
		},
		ctl2e::{
			enums::Seeming,
			state::{
				BeastBonus,
				ChangelingFrailties,
			},
		},
		enums::{
			CoreAttribute,
			CoreAdvantage,
			CoreDetail,
			TrackerState,
		},
		state::{
			BaseSpeed,
			CharacterAdvantages,
			CharacterAttributes,
			CharacterDetails,
			updateCoreAdvantage,
			updateCoreHealth,
			updateCoreResource,
			updateCoreWillpower
		}
	},
};

/// The UI Component handling a Changeling: The Lost 2e Changeling's Core and Changeling Advantages.
pub fn Advantages(cx: Scope) -> Element
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let attributesRef = use_atom_ref(&cx, CharacterAttributes);
	let detailsRef = use_atom_ref(&cx, CharacterDetails);
	
	let mut advantages = advantagesRef.write();
	let attributes = attributesRef.read();
	let details = detailsRef.read();
	
	if details[&CoreDetail::TypePrimary] == Seeming::Beast.as_ref().to_string()
	{
		advantages.initiative = attributes[&CoreAttribute::Dexterity] + attributes[&CoreAttribute::Composure] + BeastBonus;
		advantages.speed = BaseSpeed + attributes[&CoreAttribute::Dexterity] + attributes[&CoreAttribute::Strength] + BeastBonus;
	}
	else
	{
		advantages.initiative = attributes[&CoreAttribute::Dexterity] + attributes[&CoreAttribute::Composure];
		advantages.speed = BaseSpeed + attributes[&CoreAttribute::Dexterity] + attributes[&CoreAttribute::Strength];
	}
	
	return cx.render(rsx!
	{		
		div
		{
			class: "advantages row",
			
			div
			{
				class: "column",
				
				Track { label: "Health".to_string(), tracker: advantages.health.clone(), handler: healthHandler }
				Track { label: "Willpower".to_string(), tracker: advantages.willpower.clone(), handler: willpowerHandler }
				Dots { label: "Clarity".to_string(), max: 10, value: advantages.integrity, handler: clarityHandler }
				Dots { label: "Wyrd".to_string(), max: 10, value: advantages.power.unwrap(), handler: wyrdHandler }
				Track { label: "Glamour".to_string(), tracker: advantages.resource.clone().unwrap().clone(), handler: glamourHandler }
			}
		}
	});
}

/// Event handler triggered when a dot in the Clarity Track is clicked.
fn clarityHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	updateCoreAdvantage(cx, CoreAdvantage::Integrity, clickedValue);
}

/// Event handler triggered when a box in the Health Track is clicked.
fn healthHandler(cx: &Scope<TrackProps>, index: usize)
{
	let value = cx.props.tracker.clone().getValue(index);
	match value
	{
		Some(ts) =>
		{
			match ts
			{
				TrackerState::One => { updateCoreHealth(&cx, TrackerState::Two, false, Some(index)); }
				TrackerState::Two => { updateCoreHealth(&cx, TrackerState::Three, false, Some(index)); }
				TrackerState::Three => { updateCoreHealth(&cx, TrackerState::Three, true, Some(index)); }
			}
		}
		None => { updateCoreHealth(&cx, TrackerState::One, false, None); }
	}
}

/// Event handler triggered when a box in the Glamour Track is clicked.
fn glamourHandler(cx: &Scope<TrackProps>, index: usize)
{
	updateCoreResource(cx, index);
}

/// Event handler triggered when a box in the Willpower Track is clicked.
fn willpowerHandler(cx: &Scope<TrackProps>, index: usize)
{
	updateCoreWillpower(cx, index);
}

/// Event handler triggered when a dot in the Wyrd Track is clicked.
fn wyrdHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	updateCoreAdvantage(cx, CoreAdvantage::Power, clickedValue);
}

// -----

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
