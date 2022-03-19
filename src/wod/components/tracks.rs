#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use dioxus::events::MouseEvent;
use crate::{
	core::components::check::{
		CheckLine,
		CheckLineState,
		getNextLineState
	},
	wod::{
		tracks::{
			DamageType,
			Tracker
		},
		vampire::state::{
			CurrentHealth,
			VampireCharacter
		}
	}
};

#[derive(Props)]
pub struct TrackProps<'a>
{
	#[props(optional)]
	onclick: Option<EventHandler<'a, MouseEvent>>
}

impl<'a> PartialEq for TrackProps<'a>
{
	fn eq(&self, other: &Self) -> bool
	{
		return true;
	}
}

pub fn Track<'a>(cx: Scope<'a, TrackProps<'a>>) -> Element
{
	let currentHealth = use_read(&cx, CurrentHealth);
	
	return cx.render(rsx!{
		div
		{
			class: "tracker",
			
			div
			{
				class: "checkerLine",
				
				(0..currentHealth.max).map(|i|
				{
					rsx!(cx, CheckLine
					{
						key: "{i}",
						lineState: getLineState(currentHealth.aggravated, currentHealth.getDamageTotal(), i),
						onclick: move |e| clickHandler(e, &cx, getLineState(currentHealth.aggravated, currentHealth.getDamageTotal(), i))
					})
				})
			}
		}
	});
}

fn clickHandler(e: MouseEvent, cx: &Scope<TrackProps>, currentState: CheckLineState)
{
	e.cancel_bubble();
	let cHealth = use_read(&cx, CurrentHealth);
	let setCurrentHealth = use_set(&cx, CurrentHealth);
	
	let mut newHealth = Tracker { max: cHealth.max, superficial: cHealth.superficial, aggravated: cHealth.aggravated };
	
	println!("newHealth: {}", cHealth.superficial);
	match currentState
	{
		CheckLineState::None => cHealth.increaseDamage(DamageType::Superficial),
		/*
		CheckLineState::Single => vamp.health.increaseDamage(DamageType::Aggravated),
		CheckLineState::Triple => vamp.health.decreaseDamage(DamageType::Aggravated),
		*/
		_ => {}
	}
	setCurrentHealth(*cHealth);
	println!("newHealth: {}", cHealth.superficial);
	
	match &cx.props.onclick
	{
		Some(handler) => handler.call(e),
		None => {}
	}
}

fn getLineState(aggravated: i8, total: i8, key: i8) -> CheckLineState
{
	return match aggravated > key
	{
		true => CheckLineState::Triple,
		false => match total > key
		{
			true => CheckLineState::Single,
			false => CheckLineState::None
		}
	};
}
