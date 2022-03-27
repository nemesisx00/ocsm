#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use dioxus::events::FormEvent;
use crate::cod::{
	advantages::BaseAdvantageType,
	state::{
		CharacterAdvantages,
		updateBaseAdvantage,
	},
	ctl2e::{
		details::DetailType,
		state::{
			ChangelingDetails,
			updateDetail,
		},
	},
};

pub fn Details(cx: Scope) -> Element
{
	let advantages = use_atom_ref(&cx, CharacterAdvantages);
	let detailsRef = use_atom_ref(&cx, ChangelingDetails);
	let details = detailsRef.read();
	
	let defense = advantages.read().defense;
	let initiative = advantages.read().initiative;
	let speed = advantages.read().speed;
	
	return cx.render(rsx!
	{
		div
		{
			class: "detailsWrapper column",
			
			div { class: "detailsLabel", "Details" },
			
			div
			{
				class: "details row",
				
				div
				{
					class: "column",
					
					DetailInput { label: "Player:".to_string(), value: (&details[&DetailType::Player]).clone(), handler: detailHandler, handlerKey: DetailType::Player, }
					DetailInput { label: "Chronicle:".to_string(), value: (&details[&DetailType::Chronicle]).clone(), handler: detailHandler, handlerKey: DetailType::Chronicle, }
					DetailInput { label: "Name:".to_string(), value: (&details[&DetailType::Name]).clone(), handler: detailHandler, handlerKey: DetailType::Name, }
					DetailInput { label: "Concept:".to_string(), value: (&details[&DetailType::Concept]).clone(), handler: detailHandler, handlerKey: DetailType::Concept, }
					DetailNumInput { label: "Size:".to_string(), value: advantages.read().size, handler: advantageHandler, handlerKey: BaseAdvantageType::Size, }
				}
				
				div
				{
					class: "column",
					
					DetailInput { label: "Needle:".to_string(), value: (&details[&DetailType::Needle]).clone(), handler: detailHandler, handlerKey: DetailType::Needle, }
					DetailInput { label: "Thread:".to_string(), value: (&details[&DetailType::Thread]).clone(), handler: detailHandler, handlerKey: DetailType::Thread, }
					DetailInput { label: "Seeming:".to_string(), value: (&details[&DetailType::Seeming]).clone(), handler: detailHandler, handlerKey: DetailType::Seeming, }
					DetailInput { label: "Kith:".to_string(), value: (&details[&DetailType::Kith]).clone(), handler: detailHandler, handlerKey: DetailType::Kith, }
					DetailInput { label: "Court:".to_string(), value: (&details[&DetailType::Court]).clone(), handler: detailHandler, handlerKey: DetailType::Court, }
				}
			}
			
			div
			{
				class: "calculated row",
				
				div { class: "row", label { "Defense:" } div { "{defense}" } }
				div { class: "row", label { "Initiative:" } div { "{initiative}" } }
				div { class: "row", label { "Speed:" } div { "{speed}" } }
			}
		}
	});
}

fn detailHandler(cx: &Scope<DetailInputProps<DetailType>>, value: String)
{
	match cx.props.handlerKey
	{
		Some(df) => { updateDetail(cx, df, value); }
		None => {}
	}
}

fn advantageHandler(cx: &Scope<DetailInputNumProps<BaseAdvantageType>>, value: String)
{
	let num = match usize::from_str_radix(&value, 10)
	{
		Ok(i) => { i }
		Err(_) => { cx.props.value }
	};
	
	match cx.props.handlerKey
	{
		Some(at) => { updateBaseAdvantage(cx, at, num); }
		None => {}
	}
}

// -----

#[derive(Props)]
struct DetailInputProps<T>
{
	label: String,
	value: String,
	
	#[props(optional)]
	handler: Option<fn(&Scope<DetailInputProps<T>>, String)>,
	
	#[props(optional)]
	pub handlerKey: Option<T>,
}

impl<T> PartialEq for DetailInputProps<T>
{
	fn eq(&self, other: &Self) -> bool
	{
		let labelEq = self.label == other.label;
		let valueEq = self.value == other.value;
		
		return labelEq && valueEq;
	}
}

fn DetailInput<T>(cx: Scope<DetailInputProps<T>>) -> Element
{
	let label = &cx.props.label;
	let value = &cx.props.value;
	
	return cx.render(rsx!
	{
		div
		{
			class: "row",
			
			label { "{label}" }
			
			input
			{
				r#type: "text",
				value: "{value}",
				oninput:  move |e| inputHandler(e, &cx),
				oncontextmenu: move |e| e.cancel_bubble(),
				prevent_default: "oncontextmenu",
			}
		}
	});
}

fn inputHandler<T>(e: FormEvent, cx: &Scope<DetailInputProps<T>>)
{
	e.cancel_bubble();
	
	match &cx.props.handler
	{
		Some(h) => { h(&cx, e.value.clone()); }
		None => {}
	}
}

// -----

#[derive(Props)]
struct DetailInputNumProps<T>
{
	label: String,
	value: usize,
	
	#[props(optional)]
	handler: Option<fn(&Scope<DetailInputNumProps<T>>, String)>,
	
	#[props(optional)]
	pub handlerKey: Option<T>,
}

impl<T> PartialEq for DetailInputNumProps<T>
{
	fn eq(&self, other: &Self) -> bool
	{
		let labelEq = self.label == other.label;
		let valueEq = self.value == other.value;
		
		return labelEq && valueEq;
	}
}

fn DetailNumInput<T>(cx: Scope<DetailInputNumProps<T>>) -> Element
{
	let label = &cx.props.label;
	let value = &cx.props.value;
	
	let four = generateSelectedValue(4, *value);
	let five = generateSelectedValue(5, *value);
	let six = generateSelectedValue(6, *value);
	
	return cx.render(rsx!
	{
		div
		{
			class: "row",
			
			label { "{label}" }
			
			select
			{
				onchange: move |e| inputNumHandler(e, &cx),
				oncontextmenu: move |e| e.cancel_bubble(),
				prevent_default: "oncontextmenu",
				
				option { value: "4", selected: "{four}", "4" }
				option { value: "5", selected: "{five}", "5" }
				option { value: "6", selected: "{six}", "6" }
			}
		}
	});
}

fn inputNumHandler<T>(e: FormEvent, cx: &Scope<DetailInputNumProps<T>>)
{
	e.cancel_bubble();
	
	match &cx.props.handler
	{
		Some(h) => { h(cx, e.value.clone()); }
		None => {}
	}
}

fn generateSelectedValue(a: usize, b: usize) -> String
{
	return match a == b
	{
		true => { String::from("true") }
		false => { String::from("false") }
	};
}
