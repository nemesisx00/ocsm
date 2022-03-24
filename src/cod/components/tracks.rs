#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use dioxus::events::{
	MouseEvent,
};
use crate::{
	cod::{
		tracks::{
			Tracker,
			TrackerState,
		},
	},
	core::components::check::{
		CheckLine,
		CheckLineState,
	},
};

#[derive(Props)]
pub struct TrackProps
{
	label: String,
	
	pub tracker: Tracker,
	
	#[props(optional)]
	handler: Option<fn(&Scope<TrackProps>, usize)>,
}

impl PartialEq for TrackProps
{
	fn eq(&self, other: &Self) -> bool
	{
		let labelEq = self.label == other.label;
		
		let trackerEq = self.tracker == other.tracker;
		
		let handlerEq = match &self.handler
		{
			Some(_h1) => match &other.handler
			{
				Some(_h2) => { true }
				None => { false }
			}
			None => match &other.handler
			{
				Some(_h2) => { false }
				None => { true }
			}
		};
		
		return labelEq && trackerEq && handlerEq;
	}
}

pub fn Track(scope: Scope<TrackProps>) -> Element
{
	let max = scope.props.tracker.clone().getMax();
	
	return scope.render(rsx!
	{
		div
		{
			class: "tracker",
			
			div { class: "label", "{scope.props.label}" }
			
			div
			{
				class: "checkerLine row",
				
				scope.props.tracker.values.iter().enumerate().map(|(i, ts)| rsx!(scope, CheckLine
				{
					key: "{i}",
					lineState: getLineState(ts),
					onclick: move |e| clickHandler(e, &scope, i)
				})),
				
				((scope.props.tracker.values.len())..max).map(|i| rsx!(scope, CheckLine
				{
					key: "{i}",
					lineState: CheckLineState::None,
					onclick: move |e| clickHandler(e, &scope, i)
				}))
			}
		}
	});
}

fn getLineState(ts: &TrackerState) -> CheckLineState
{
	return match ts
	{
		TrackerState::One => CheckLineState::Single,
		TrackerState::Two => CheckLineState::Double,
		TrackerState::Three => CheckLineState::Triple,
	};
}

fn clickHandler(e: MouseEvent, scope: &Scope<TrackProps>, index: usize)
{
	e.cancel_bubble();
	
	match &scope.props.handler
	{
		Some(h) => { h(&scope, index); }
		None => {}
	}
}
