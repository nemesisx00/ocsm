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
		state::{
			updateHealth,
			updateWillpower,
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
	
	tracker: Tracker,
	
	//TODO: Figure out how to allow a generic function handler as a property, rather than these two bools
	#[props(optional)]
	healthHandler: Option<bool>,
	
	#[props(optional)]
	willpowerHandler: Option<bool>,
}

impl PartialEq for TrackProps
{
	fn eq(&self, other: &Self) -> bool
	{
		let labelEq = self.label == other.label;
		
		let trackerEq = self.tracker == other.tracker;
		
		let healthHandlerEq = match &self.healthHandler
		{
			Some(_h1) => match &other.healthHandler
			{
				Some(_h2) => { true }
				None => { false }
			}
			None => match &other.healthHandler
			{
				Some(_h2) => { false }
				None => { true }
			}
		};
		
		let willpowerHandlerEq = match &self.willpowerHandler
		{
			Some(_h1) => match &other.willpowerHandler
			{
				Some(_h2) => { true }
				None => { false }
			}
			None => match &other.willpowerHandler
			{
				Some(_h2) => { false }
				None => { true }
			}
		};
		
		return labelEq && trackerEq && healthHandlerEq && willpowerHandlerEq;
	}
}

pub fn Track(scope: Scope<TrackProps>) -> Element
{
	return scope.render(rsx!
	{
		div
		{
			class: "tracker",
			
			div
			{
				class: "label",
				
				"{scope.props.label}"
			},
			
			div
			{
				class: "checkerLine row",
				
				(0..scope.props.tracker.max).map(|i|
				{
					rsx!(scope, CheckLine
					{
						key: "{i}",
						lineState: getLineState(scope.props.tracker.values.get(i)),
						onclick: move |e| clickHandler(e, &scope, i, scope.props.tracker.values.get(i))
					})
				})
			}
		}
	});
}

fn getLineState(ts: Option<&TrackerState>) -> CheckLineState
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

fn clickHandler(e: MouseEvent, scope: &Scope<TrackProps>, index: usize, currentState: Option<&TrackerState>)
{
	e.cancel_bubble();
	
	match &scope.props.healthHandler
	{
		Some(isHandler) =>
		{
			if *isHandler
			{
				match currentState
				{
					Some(dt) =>
					{
						match dt
						{
							TrackerState::One => { updateHealth(&scope, TrackerState::Two, false, Some(index)); }
							TrackerState::Two => { updateHealth(&scope, TrackerState::Three, false, Some(index)); }
							TrackerState::Three => { updateHealth(&scope, TrackerState::Three, true, Some(index)); }
						}
					}
					None => { updateHealth(&scope, TrackerState::One, false, None); }
				}
			}
		}
		None => {}
	}
	
	match &scope.props.willpowerHandler
	{
		Some(isHandler) =>
		{
			if *isHandler
			{
				match currentState
				{
					Some(ts) => { updateWillpower(scope, *ts, Some(index)); }
					None => { updateWillpower(scope, TrackerState::Two, None); }
				}
			}
		}
		None => {}
	}
}
