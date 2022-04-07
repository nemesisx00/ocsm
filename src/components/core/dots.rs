#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::components::core::check::CheckCircle;

/// The properties struct for `Dots`.
#[derive(Props)]
pub struct DotsProps<T>
{
	pub max: usize,
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
		let labelEq = self.label == other.label;
		let classEq = self.class == other.class;
		
		return maxEq && valueEq && labelEq && classEq;
	}
}

/// The UI Component handling a single set of dots used to represent and manipulate
/// the numeric value of several different Chronicles of Darkness Traits.
pub fn Dots<T>(cx: Scope<DotsProps<T>>) -> Element
{
	let class = match &cx.props.class
	{
		Some(cn) => { format!("tracker {}", cn) }
		None => { "tracker".to_string() }
	};
	
	let label = match &cx.props.label
	{
		Some(l) => { l }
		None => { "" }
	};
	
	return cx.render(rsx!{
		div
		{
			class: "{class}",
			
			div { class: "label", "{label}" }
			
			div
			{
				class: "checkerLine row justEven",
				
				(0..cx.props.max).map(|i|
				{
					rsx!(cx, CheckCircle
					{
						key: "{i}",
						checked: i < cx.props.value,
						onclick: move |_| clickHandler(&cx, i + 1)
					})
				})
			}
		}
	});
}

/// Event handler triggered when a dot in `Dots` is clicked.
fn clickHandler<T>(cx: &Scope<DotsProps<T>>, clickedValue: usize)
{
	match &cx.props.handler
	{
		Some(h) => { h(&cx, clickedValue); }
		None => {}
	}
}
