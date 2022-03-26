#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::core::components::check::CheckCircle;

#[derive(Props)]
pub struct DotsProps<T>
{
	max: usize,
	pub value: usize,
	
	#[props(optional)]
	dioxusKey: Option<usize>,
	
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
		let dioxusKeyEq = self.dioxusKey == other.dioxusKey;
		let labelEq = self.label == other.label;
		let classEq = self.class == other.class;
		
		return maxEq && valueEq && dioxusKeyEq && labelEq && classEq;
	}
}

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
	
	let dioxusKey = match &cx.props.dioxusKey
	{
		Some(dk) => { *dk }
		None => { 10000 }
	};
	
	return cx.render(rsx!{
		div
		{
			key: "{dioxusKey}",
			class: "{class}",
			
			div { class: "label", "{label}" }
			
			div
			{
				class: "checkerLine row",
				
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

fn clickHandler<T>(cx: &Scope<DotsProps<T>>, clickedValue: usize)
{
	match &cx.props.handler
	{
		Some(h) => { h(&cx, clickedValue); }
		None => {}
	}
}
