#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use dioxus::events::{
	FormEvent,
};
use crate::cod::{
	advantages::{
		BaseAdvantageType,
	},
	state::{
		CharacterAdvantages,
		updateBaseAdvantage,
	},
	vtr2e::{
		details::{
			DetailsField,
		},
		state::{
			KindredDetails,
			updateDetail,
		},
	},
};

pub fn Details(scope: Scope) -> Element
{
	let advantages = use_atom_ref(&scope, CharacterAdvantages);
	let detailsRef = use_atom_ref(&scope, KindredDetails);
	let details = detailsRef.read();
	
	let bloodlineLabel = "Bloodline:".to_string();
	let chronicleLabel = "Chronicle:".to_string();
	let clanLabel = "Clan:".to_string();
	let conceptLabel = "Concept:".to_string();
	let covenantLabel = "Covenant:".to_string();
	let dirgeLabel = "Dirge:".to_string();
	let maskLabel = "Mask:".to_string();
	let nameLabel = "Name:".to_string();
	let playerLabel = "Player:".to_string();
	let sizeLabel = "Size:".to_string();
	
	let defense = advantages.read().defense;
	let initiative = advantages.read().initiative;
	let speed = advantages.read().speed;
	
	// I'm leaving DetailsField::Player out of this component for now.
	// I'm building this for players (read: myself) first and foremost
	// so the Player field is a bit redundant. Maybe one day this will
	// have grown to the point of being able to open multiple sheets
	// so GMs/STs can keep track of their players' characters more easily.
	// But it is not this day.
	
	return scope.render(rsx!
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
					
					DetailInput { label: playerLabel, value: (&details.player).clone(), handler: detailHandler, handlerKey: DetailsField::Player, }
					DetailInput { label: chronicleLabel, value: (&details.chronicle).clone(), handler: detailHandler, handlerKey: DetailsField::Chronicle, }
					DetailInput { label: nameLabel, value: (&details.name).clone(), handler: detailHandler, handlerKey: DetailsField::Name, }
					DetailInput { label: conceptLabel, value: (&details.concept).clone(), handler: detailHandler, handlerKey: DetailsField::Concept, }
					DetailNumInput { label: sizeLabel, value: advantages.read().size, handler: advantageHandler, handlerKey: BaseAdvantageType::Size, }
				}
				
				div
				{
					class: "column",
					
					DetailInput { label: maskLabel, value: (&details.mask).clone(), handler: detailHandler, handlerKey: DetailsField::Mask, }
					DetailInput { label: dirgeLabel, value: (&details.dirge).clone(), handler: detailHandler, handlerKey: DetailsField::Dirge, }
					DetailInput { label: clanLabel, value: (&details.clan).clone(), handler: detailHandler, handlerKey: DetailsField::Clan, }
					DetailInput { label: bloodlineLabel, value: (&details.bloodline).clone(), handler: detailHandler, handlerKey: DetailsField::Bloodline, }
					DetailInput { label: covenantLabel, value: (&details.covenant).clone(), handler: detailHandler, handlerKey: DetailsField::Covenant, }
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

fn detailHandler(scope: &Scope<DetailInputProps<DetailsField>>, value: String)
{
	match scope.props.handlerKey
	{
		Some(df) => { updateDetail(scope, df, value); }
		None => {}
	}
}

fn advantageHandler(scope: &Scope<DetailInputNumProps<BaseAdvantageType>>, value: String)
{
	let num = match usize::from_str_radix(&value, 10)
	{
		Ok(i) => { i }
		Err(_) => { scope.props.value }
	};
	
	match scope.props.handlerKey
	{
		Some(at) => { updateBaseAdvantage(scope, at, num); }
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

fn DetailInput<T>(scope: Scope<DetailInputProps<T>>) -> Element
{
	let label = &scope.props.label;
	let value = &scope.props.value;
	
	return scope.render(rsx!
	{
		div
		{
			class: "row",
			
			label { "{label}" }
			
			input
			{
				r#type: "text",
				value: "{value}",
				oninput:  move |e| inputHandler(e, &scope),
			}
		}
	});
}

fn inputHandler<T>(e: FormEvent, scope: &Scope<DetailInputProps<T>>)
{
	e.cancel_bubble();
	
	match &scope.props.handler
	{
		Some(h) => { h(&scope, e.value.clone()); }
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

fn DetailNumInput<T>(scope: Scope<DetailInputNumProps<T>>) -> Element
{
	let label = &scope.props.label;
	let value = &scope.props.value;
	
	let four = generateSelectedValue(4, *value);
	let five = generateSelectedValue(5, *value);
	let six = generateSelectedValue(6, *value);
	
	return scope.render(rsx!
	{
		div
		{
			class: "row",
			
			label { "{label}" }
			
			select
			{
				onchange: move |e| inputNumHandler(e, &scope),
				
				option { value: "4", selected: "{four}", "4" }
				option { value: "5", selected: "{five}", "5" }
				option { value: "6", selected: "{six}", "6" }
			}
		}
	});
}

fn inputNumHandler<T>(e: FormEvent, scope: &Scope<DetailInputNumProps<T>>)
{
	e.cancel_bubble();
	
	match &scope.props.handler
	{
		Some(h) => { h(scope, e.value.clone()); }
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
