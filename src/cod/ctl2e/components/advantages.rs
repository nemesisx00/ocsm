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
		advantages::BaseSpeed,
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
			advantages::TemplateAdvantageType,
			details::{
				BeastBonus,
				DetailType,
				Seeming,
			},
			state::{
				ChangelingAdvantages,
				ChangelingDetails,
				ChangelingFrailties,
				updateTemplateAdvantage,
				updateGlamour,
			},
		},
		traits::{
			BaseAttributeType,
		},
		tracks::TrackerState,
		state::{
			CharacterAdvantages,
			CharacterAttributes,
			updateBaseHealth,
			updateBaseWillpower,
		}
	},
};

pub fn Advantages(cx: Scope) -> Element
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let attributesRef = use_atom_ref(&cx, CharacterAttributes);
	let detailsRef = use_atom_ref(&cx, ChangelingDetails);
	let templateRef = use_atom_ref(&cx, ChangelingAdvantages);
	
	let mut advantages = advantagesRef.write();
	let attributes = attributesRef.read();
	let details = detailsRef.read();
	let template = templateRef.read();
	
	if details[&DetailType::Seeming] == Seeming::Beast.as_ref().to_string()
	{
		advantages.initiative = attributes[&BaseAttributeType::Dexterity] + attributes[&BaseAttributeType::Composure] + BeastBonus;
		advantages.speed = BaseSpeed + attributes[&BaseAttributeType::Dexterity] + attributes[&BaseAttributeType::Strength] + BeastBonus;
	}
	else
	{
		advantages.initiative = attributes[&BaseAttributeType::Dexterity] + attributes[&BaseAttributeType::Composure];
		advantages.speed = BaseSpeed + attributes[&BaseAttributeType::Dexterity] + attributes[&BaseAttributeType::Strength];
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
				Dots { label: "Clarity".to_string(), max: 10, value: template.clarity, handler: clarityHandler }
				Dots { label: "Wyrd".to_string(), max: 10, value: template.wyrd, handler: wyrdHandler }
				Track { label: "Glamour".to_string(), tracker: template.glamour.clone(), handler: glamourHandler }
			}
		}
	});
}

fn wyrdHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	updateTemplateAdvantage(cx, TemplateAdvantageType::Wyrd, clickedValue);
}

fn healthHandler(cx: &Scope<TrackProps>, index: usize)
{
	let value = cx.props.tracker.clone().getValue(index);
	match value
	{
		Some(ts) =>
		{
			match ts
			{
				TrackerState::One => { updateBaseHealth(&cx, TrackerState::Two, false, Some(index)); }
				TrackerState::Two => { updateBaseHealth(&cx, TrackerState::Three, false, Some(index)); }
				TrackerState::Three => { updateBaseHealth(&cx, TrackerState::Three, true, Some(index)); }
			}
		}
		None => { updateBaseHealth(&cx, TrackerState::One, false, None); }
	}
}

fn clarityHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	updateTemplateAdvantage(cx, TemplateAdvantageType::Clarity, clickedValue);
}

fn glamourHandler(cx: &Scope<TrackProps>, index: usize)
{
	updateGlamour(cx, index);
}

fn willpowerHandler(cx: &Scope<TrackProps>, index: usize)
{
	updateBaseWillpower(cx, index);
}

// -----

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

fn removeClickHandler(cx: &Scope, index: usize)
{
	let frailtiesRef = use_atom_ref(&cx, ChangelingFrailties);
	let mut frailties = frailtiesRef.write();
	
	if index < frailties.len()
	{
		frailties.remove(index);
	}
}

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
