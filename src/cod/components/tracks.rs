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

const DefaultLineMax: usize = 10;

#[derive(Props)]
pub struct TrackProps
{
	label: String,
	
	#[props(optional)]
	lineMax: Option<usize>,
	
	pub tracker: Tracker,
	
	#[props(optional)]
	handler: Option<fn(&Scope<TrackProps>, usize)>,
}

impl PartialEq for TrackProps
{
	fn eq(&self, other: &Self) -> bool
	{
		let labelEq = self.label == other.label;
		
		let lineMaxEq = match &self.lineMax
		{
			Some(lm1) => match &other.lineMax
			{
				Some(lm2) => { lm1 == lm2 }
				None => { false }
			}
			None => match &other.lineMax
			{
				Some(_h2) => { false }
				None => { true }
			}
		};
		
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
		
		return labelEq && lineMaxEq && trackerEq && handlerEq;
	}
}

pub fn Track(scope: Scope<TrackProps>) -> Element
{
	let lineMax = match scope.props.lineMax
	{
		Some(lm) => { lm }
		None => { DefaultLineMax }
	};
	
	let max = scope.props.tracker.clone().getMax();
	
	let lines = max / lineMax;
	
	let lastStart = lines * lineMax;
	let lastEnd = lastStart + (max % lineMax);
	
	return scope.render(rsx!
	{
		div
		{
			class: "tracker",
			
			div { class: "label", "{scope.props.label}" },
			
			(0..lines).map(|l|
			{
				let start = l * lineMax;
				let end = start + lineMax;
				
				rsx!(scope, div
				{
					class: "checkerLine row",
					
					(start..end).map(|i|
					{
						rsx!(scope, CheckLine
						{
							key: "{i}",
							lineState: getLineState(scope.props.tracker.clone().getValue(i)),
							onclick: move |e| clickHandler(e, &scope, i)
						})
					})
				})
			})
			
			div
			{
				class: "checkerLine row",
				
				(lastStart..lastEnd).map(|i|
				{
					rsx!(scope, CheckLine
					{
						key: "{i}",
						lineState: getLineState(scope.props.tracker.clone().getValue(i)),
						onclick: move |e| clickHandler(e, &scope, i)
					})
				})
			}
		}
	});
}

fn getLineState(ts: Option<TrackerState>) -> CheckLineState
{
	return match ts
	{
		Some(s) =>
		{
			match s
			{
				TrackerState::One => CheckLineState::Single,
				TrackerState::Two => CheckLineState::Double,
				TrackerState::Three => CheckLineState::Triple,
			}
		},
		None => CheckLineState::None
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
