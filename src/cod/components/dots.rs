#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use dioxus::events::MouseEvent;
use crate::core::components::check::CheckCircle;

#[derive(Props)]
pub struct DotsProps<T>
{
	max: usize,
	pub value: usize,
	
	#[props(optional)]
	label: Option<String>,
	
	#[props(optional)]
	class: Option<String>,
	
	#[props(optional)]
	handler: Option<fn(&Scope<DotsProps<T>>, usize)>,
	
	#[props(optional)]
	pub handlerKey: Option<T>,
}

impl<T> PartialEq for DotsProps<T>
{
	fn eq(&self, other: &Self) -> bool
	{
		let maxEq = self.max == other.max;
		
		let valueEq = self.value == other.value;
		
		let labelEq = match &self.label
		{
			Some(l1) => match &other.label
			{
				Some(l2) => { l1 == l2 }
				None => { false }
			}
			None => match &other.label
			{
				Some(_l2) => { false }
				None => { true }
			}
		};
		
		let classEq = match &self.class
		{
			Some(c1) => match &other.class
			{
				Some(c2) => { c1 == c2 }
				None => { false }
			},
			None => match &other.class
			{
				Some(_c2) => { false }
				None => { true }
			}
		};
		
		return maxEq && valueEq && labelEq && classEq;
	}
}

pub fn Dots<T>(scope: Scope<DotsProps<T>>) -> Element
{
	let class = match &scope.props.class
	{
		Some(cn) => { format!("tracker {}", cn) },
		None => { String::from("tracker") }
	};
	
	let label = match &scope.props.label
	{
		Some(l) => { l }
		None => { "" }
	};
	
	return scope.render(rsx!{
		div
		{
			class: "{class}",
			
			div { class: "label", "{label}" }
			
			div
			{
				class: "checkerLine row",
				
				(0..scope.props.max).map(|i|
				{
					rsx!(scope, CheckCircle
					{
						key: "{i}",
						checked: i < scope.props.value,
						onclick: move |e| clickHandler(e, &scope, i + 1)
					})
				})
			}
		}
	});
}

fn clickHandler<T>(e: MouseEvent, scope: &Scope<DotsProps<T>>, clickedValue: usize)
{
	e.cancel_bubble();
	
	match &scope.props.handler
	{
		Some(h) => { h(&scope, clickedValue); }
		None => {}
	}
}
