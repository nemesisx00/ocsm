#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
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
	class: Option<String>,
	
	#[props(optional)]
	handler: Option<fn(&Scope<TrackProps>, usize)>,
}

impl PartialEq for TrackProps
{
	fn eq(&self, other: &Self) -> bool
	{
		let labelEq = self.label == other.label;
		let trackerEq = self.tracker == other.tracker;
		
		return labelEq && trackerEq;
	}
}

pub fn Track(cx: Scope<TrackProps>) -> Element
{
	let max = cx.props.tracker.clone().getMax();
	
	let class = match &cx.props.class
	{
		Some(cn) => { format!("tracker {}", cn) }
		None => { "tracker".to_string() }
	};
	
	return cx.render(rsx!
	{
		div
		{
			class: "{class}",
			
			div { class: "label", "{cx.props.label}" }
			
			div
			{
				class: "checkerLine row",
				
				cx.props.tracker.values.iter().enumerate().map(|(i, ts)| rsx!(cx, CheckLine
				{
					key: "{i}",
					lineState: getLineState(ts),
					onclick: move |_| clickHandler(&cx, i)
				})),
				
				((cx.props.tracker.values.len())..max).map(|i| rsx!(cx, CheckLine
				{
					key: "{i}",
					lineState: CheckLineState::None,
					onclick: move |_| clickHandler(&cx, i)
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

fn clickHandler(cx: &Scope<TrackProps>, index: usize)
{
	match &cx.props.handler
	{
		Some(h) => { h(&cx, index); }
		None => {}
	}
}
