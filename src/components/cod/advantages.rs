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
		structs::Tracker,
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
	
	let powerLabel = match &cx.props.power
	{
		Some(p) => p.to_string(),
		None => "".to_string()
	};
	let resourceLabel = match &cx.props.resource
	{
		Some(t) => t.clone(),
		None => "".to_string()
	};
	
	let power = match advantages.power
	{
		Some(p) => p,
		None => 0
	};
	let resource = match &advantages.resource
	{
		Some(t) => t.clone(),
		None => Tracker::new(5)
	};
	
	let showPower = match cx.props.power
	{
		Some(_) => true,
		None => false,
	};
	
	let showResource = match &cx.props.resource
	{
		Some(_) => true,
		None => false,
	};
	
	return cx.render(rsx!
	{		
		div
		{
			class: "advantages row justEven",
			
			div
			{
				class: "column justEven",
				
				Track { label: "Health".to_string(), tracker: advantages.health.clone(), handler: healthHandler }
				Track { label: "Willpower".to_string(), tracker: advantages.willpower.clone(), handler: willpowerHandler }
				Dots { label: cx.props.integrity.clone(), max: 10, value: advantages.integrity, handler: integrityHandler }
				
				showPower.then(|| rsx!(
					Dots { label: powerLabel.clone(), max: 10, value: power.clone(), handler: powerHandler }
				))
				showResource.then(|| rsx!(
					Track { label: resourceLabel.clone(), tracker: resource.clone(), handler: resourceHandler }
				))
			}
		}
	});
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

/// Event handler triggered when a dot in the Clarity Track is clicked.
fn integrityHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	updateCoreAdvantage(cx, CoreAdvantage::Integrity, clickedValue);
}

/// Event handler triggered when a dot in the Wyrd Track is clicked.
fn powerHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	updateCoreAdvantage(cx, CoreAdvantage::Power, clickedValue);
}

/// Event handler triggered when a box in the Glamour Track is clicked.
fn resourceHandler(cx: &Scope<TrackProps>, index: usize)
{
	updateCoreResource(cx, index);
}

/// Event handler triggered when a box in the Willpower Track is clicked.
fn willpowerHandler(cx: &Scope<TrackProps>, index: usize)
{
	updateCoreWillpower(cx, index);
}
