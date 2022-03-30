#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::{
	cod::{
		enums::{
			CoreAdvantage,
			TrackerState,
		},
		state::{
			CharacterAdvantages,
			updateCoreAdvantage,
			updateCoreHealth,
			updateCoreResource,
			updateCoreWillpower
		},
	},
	components::{
		cod::{
			dots::{
				Dots,
				DotsProps,
			},
			tracks::{
				Track,
				TrackProps,
			},
		},
	},
};

/// The properties struct for `Advantages`.
#[derive(Props)]
pub struct AdvantagesProps
{
	integrity: String,
	
	#[props(optional)]
	power: Option<String>,
	
	#[props(optional)]
	resource: Option<String>,
	
	#[props(optional)]
	handleTemplateBonuses: Option<fn(&Scope<AdvantagesProps>)>,
}

impl PartialEq for AdvantagesProps
{
	fn eq(&self, other: &Self) -> bool
	{
		let easyEq = self.integrity == other.integrity
			&& self.power == other.power
			&& self.resource == other.resource;
		
		let handlerEq = match self.handleTemplateBonuses
		{
			Some(_) => match other.handleTemplateBonuses
			{
				Some(_) => true,
				None => false,
			},
			None => match other.handleTemplateBonuses
			{
				Some(_) => false,
				None => true,
			}
		};
		
		return easyEq && handlerEq;
	}
}

/// The UI Component handling a Changeling: The Lost 2e Changeling's Core and Changeling Advantages.
pub fn Advantages(cx: Scope<AdvantagesProps>) -> Element
{
	if let Some(handler) = &cx.props.handleTemplateBonuses
	{
		handler(&cx);
	}
	
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let advantages = advantagesRef.read();
	// This feels like a hack to get around the move in the showPower/showResource .then()'s
	// but it works for now.
	let otherAdvantages = advantagesRef.read();
	
	let showPower = match cx.props.power
	{
		Some(_) => true,
		None => false,
	};
	
	let showResource = match cx.props.resource
	{
		Some(_) => true,
		None => false,
	};
	
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
				Dots { label: cx.props.integrity.clone(), max: 10, value: advantages.integrity, handler: clarityHandler }
				
				showPower.then(|| rsx!(
					Dots { label: cx.props.power.as_ref().unwrap().clone(), max: 10, value: advantages.power.as_ref().unwrap().clone(), handler: wyrdHandler }
				))
				showResource.then(|| rsx!(
					Track { label: cx.props.resource.as_ref().unwrap().clone(), tracker: otherAdvantages.resource.as_ref().unwrap().clone(), handler: glamourHandler }
				))
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
