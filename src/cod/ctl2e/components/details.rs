#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::BTreeMap;
use dioxus::prelude::*;
use dioxus::events::FormEvent;
use strum::IntoEnumIterator;
use crate::{
	cod::{
		advantages::CoreAdvantageType,
		state::{
			CharacterAdvantages,
			updateBaseAdvantage,
		},
		ctl2e::{
			details::{
				Seeming,
				DetailType,
			},
			state::{
				ChangelingDetails,
				updateDetail,
			},
		},
	},
	core::util::generateSelectedValue,
};

/// The UI Component handling a Changeling: The Lost 2e Changeling's Details.
pub fn Details(cx: Scope) -> Element
{
	let advantages = use_atom_ref(&cx, CharacterAdvantages);
	let detailsRef = use_atom_ref(&cx, ChangelingDetails);
	let details = detailsRef.read();
	
	let defense = advantages.read().defense;
	let initiative = advantages.read().initiative;
	let speed = advantages.read().speed;
	
	let mut seemings = Vec::<String>::new();
	for s in Seeming::iter()
	{
		seemings.push(s.as_ref().to_string());
	}
	
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
					
					DetailInput { label: "Player:".to_string(), value: (&details[&DetailType::Player]).clone(), select: false, handler: detailHandler, handlerKey: DetailType::Player, }
					DetailInput { label: "Chronicle:".to_string(), value: (&details[&DetailType::Chronicle]).clone(), select: false, handler: detailHandler, handlerKey: DetailType::Chronicle, }
					DetailInput { label: "Name:".to_string(), value: (&details[&DetailType::Name]).clone(), select: false, handler: detailHandler, handlerKey: DetailType::Name, }
					DetailInput { label: "Concept:".to_string(), value: (&details[&DetailType::Concept]).clone(), select: false, handler: detailHandler, handlerKey: DetailType::Concept, }
					DetailInput { label: "Size:".to_string(), value: format!("{}", advantages.read().size), select: true, selectOptions: vec!["4".to_string(), "5".to_string(), "6".to_string()], handler: advantageHandler, handlerKey: CoreAdvantageType::Size, }
				}
				
				div
				{
					class: "column",
					
					DetailInput { label: "Needle:".to_string(), value: (&details[&DetailType::Needle]).clone(), select: false, handler: detailHandler, handlerKey: DetailType::Needle, }
					DetailInput { label: "Thread:".to_string(), value: (&details[&DetailType::Thread]).clone(), select: false, handler: detailHandler, handlerKey: DetailType::Thread, }
					DetailInput { label: "Seeming:".to_string(), value: (&details[&DetailType::Seeming]).clone(), select: true, selectNoneLabel: "Choose a Seeming".to_string(), selectOptions: seemings.clone(), handler: detailHandler, handlerKey: DetailType::Seeming, }
					DetailInput { label: "Kith:".to_string(), value: (&details[&DetailType::Kith]).clone(), select: false, handler: detailHandler, handlerKey: DetailType::Kith, }
					DetailInput { label: "Court:".to_string(), value: (&details[&DetailType::Court]).clone(), select: false, handler: detailHandler, handlerKey: DetailType::Court, }
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

/// Event handler triggered when a `DetailInput`'s value changes.
fn detailHandler(cx: &Scope<DetailInputProps<DetailType>>, value: String)
{
	match cx.props.handlerKey
	{
		Some(df) => { updateDetail(cx, df, value); }
		None => {}
	}
}

/// Event handler triggered when the Size input's value changes.
fn advantageHandler(cx: &Scope<DetailInputProps<CoreAdvantageType>>, value: String)
{
	match usize::from_str_radix(&value, 10)
	{
		Ok(num) =>
		{
			match cx.props.handlerKey
			{
				Some(at) => { updateBaseAdvantage(cx, at, num); }
				None => {}
			}
		}
		Err(_) => {}
	};
}

// -----

/// The properties struct for `DetailInput`
#[derive(Props)]
struct DetailInputProps<T>
{
	label: String,
	value: String,
	select: bool,
	
	#[props(optional)]
	selectNoneLabel: Option<String>,
	
	#[props(optional)]
	selectOptions: Option<Vec<String>>,
	
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

// The UI Component defining the layout and functionality of a single Detail input.
fn DetailInput<T>(cx: Scope<DetailInputProps<T>>) -> Element
{
	let label = &cx.props.label;
	let value = &cx.props.value;
	
	let mut showText = false;
	let mut showSelect = false;
	match &cx.props.select
	{
		true => { showSelect = true; }
		false => { showText = true; }
	}
	
	let mut selectOptions = BTreeMap::<String, String>::new();
	match &cx.props.selectOptions
	{
		Some(v) =>
		{
			v.iter().for_each(|op|
			{
				selectOptions.insert(op.clone(), generateSelectedValue(op, value));
			})
		}
		None => {}
	}
	
	let selectNoneLabel = match &cx.props.selectNoneLabel
	{
		Some(snl) => { snl.clone() }
		None => { "".to_string() }
	};
	let showSelectNoneLabel = selectNoneLabel != "".to_string();
	
	return cx.render(rsx!
	{
		div
		{
			class: "row",
			
			label { "{label}" }
			
			showText.then(|| rsx!
			{
				input
				{
					r#type: "text",
					value: "{value}",
					oninput:  move |e| inputHandler(e, &cx),
					oncontextmenu: move |e| e.cancel_bubble(),
					prevent_default: "oncontextmenu",
				}
			})
			
			showSelect.then(|| rsx!
			{
				select
				{
					onchange: move |e| inputHandler(e, &cx),
					oncontextmenu: move |e| e.cancel_bubble(),
					prevent_default: "oncontextmenu",
					
					showSelectNoneLabel.then(|| rsx!(option { value: "", "{selectNoneLabel}" }))
					
					selectOptions.iter().enumerate().map(|(i, (so, selected))| {
						rsx!(cx, option { key: "{i}", value: "{so}", selected: "{selected}", "{so}" })
					})
				}
			})
		}
	});
}

/// Event handler triggered when the `DetailInput`'s input value changes.
fn inputHandler<T>(e: FormEvent, cx: &Scope<DetailInputProps<T>>)
{
	e.cancel_bubble();
	
	match &cx.props.handler
	{
		Some(h) => { h(&cx, e.value.clone()); }
		None => {}
	}
}
