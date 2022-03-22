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
		vtmv5::state::{
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

pub fn Track<'a>(scope: Scope<'a, TrackProps<'a>>) -> Element
{
	let currentHealth = use_read(&scope, CurrentHealth);
	
	return scope.render(rsx!{
		div
		{
			class: "tracker",
			
			div
			{
				class: "checkerLine",
				
				(0..currentHealth.max).map(|i|
				{
					rsx!(scope, CheckLine
					{
						key: "{i}",
						lineState: getLineState(currentHealth.aggravated, currentHealth.getDamageTotal(), i),
						onclick: move |e| clickHandler(e, &scope, getLineState(currentHealth.aggravated, currentHealth.getDamageTotal(), i))
					})
				})
			}
		}
	});
}

fn clickHandler(e: MouseEvent, scope: &Scope<TrackProps>, currentState: CheckLineState)
{
	e.cancel_bubble();
	let cHealth = use_read(&scope, CurrentHealth);
	let setCurrentHealth = use_set(&scope, CurrentHealth);
	
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
	
	match &scope.props.onclick
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
