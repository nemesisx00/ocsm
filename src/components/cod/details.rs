#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::BTreeMap;
use dioxus::prelude::*;
use dioxus::events::FormEvent;
use crate::{
	cod::{
		enums::{
			CoreAdvantage,
			CoreDetail,
		},
		state::{
			CharacterAdvantages,
			CharacterDetails,
			updateCoreAdvantage,
			updateCoreDetail,
		},
	},
	core::{
		enums::GameSystem,
		state::CurrentGameSystem,
		util::generateSelectedValue,
	},
};

/// The properties struct for `Details`.
#[derive(PartialEq, Props)]
pub struct DetailsProps
{
	virtue: String,
	vice: String,
	typePrimary: String,
	typeSecondary: String,
	faction: String,
	
	#[props(optional)]
	typePrimaryOptions: Option<Vec<String>>,
}

/// The UI Component handling a Vampire: The Requiem 2e Kindred's Details.
pub fn Details(cx: Scope<DetailsProps>) -> Element
{
	let currentGameSystem = use_read(&cx, CurrentGameSystem);
	let advantages = use_atom_ref(&cx, CharacterAdvantages);
	let detailsRef = use_atom_ref(&cx, CharacterDetails);
	let details = detailsRef.read();
	
	let armor = advantages.read().armor;
	let defense = advantages.read().defense;
	let initiative = advantages.read().initiative;
	let speed = advantages.read().speed;
	
	let showPrimaryText = cx.props.typePrimaryOptions == None;
	let showPrimarySelect = cx.props.typePrimaryOptions != None;
	let typePrimaryValue1 = details[&CoreDetail::TypePrimary].clone();
	let typePrimaryValue2 = details[&CoreDetail::TypePrimary].clone();
	
	let details1 = details.clone();
	let details2 = details.clone();
	
	let isMortal = match currentGameSystem
	{
		GameSystem::CodMortal => true,
		_ => false,
	};
	let notIsMortal = !isMortal;
	
	return cx.render(rsx!
	{
		div
		{
			class: "detailsWrapper column justEven",
			
			div { class: "detailsLabel", "Details" },
			
			div
			{
				class: "details row justEven",
				
				isMortal.then(|| rsx!(
					div
					{
						class: "column justEven",
						
						DetailInput { label: "Player:".to_string(), value: (&details1[&CoreDetail::Player]).clone(), select: false, handler: detailHandler, handlerKey: CoreDetail::Player, }
						DetailInput { label: "Chronicle:".to_string(), value: (&details1[&CoreDetail::Chronicle]).clone(), select: false, handler: detailHandler, handlerKey: CoreDetail::Chronicle, }
						DetailInput { label: "Name:".to_string(), value: (&details1[&CoreDetail::Name]).clone(), select: false, handler: detailHandler, handlerKey: CoreDetail::Name, }
						DetailInput { label: format!("{}:", &cx.props.typePrimary.clone()), value: (&details1[&CoreDetail::TypePrimary]).clone(), select: false, handler: detailHandler, handlerKey: CoreDetail::TypePrimary, }
						DetailInput { label: "Size:".to_string(), value: format!("{}", advantages.read().size), select: true, selectOptions: vec!["4".to_string(), "5".to_string(), "6".to_string()], handler: advantageHandler, handlerKey: CoreAdvantage::Size, }
					}
					
					div
					{
						class: "column justEven",
						
						DetailInput { label: "Concept:".to_string(), value: (&details1[&CoreDetail::Concept]).clone(), select: false, handler: detailHandler, handlerKey: CoreDetail::Concept, }
						DetailInput { label: format!("{}:", &cx.props.virtue.clone()), value: (&details1[&CoreDetail::Virtue]).clone(), select: false, handler: detailHandler, handlerKey: CoreDetail::Virtue, }
						DetailInput { label: format!("{}:", &cx.props.vice.clone()), value: (&details1[&CoreDetail::Vice]).clone(), select: false, handler: detailHandler, handlerKey: CoreDetail::Vice, }
						DetailInput { label: format!("{}:", &cx.props.typeSecondary.clone()), value: (&details1[&CoreDetail::TypeSecondary]).clone(), select: false, handler: detailHandler, handlerKey: CoreDetail::TypeSecondary, }
						DetailInput { label: format!("{}:", &cx.props.faction.clone()), value: (&details1[&CoreDetail::Faction]).clone(), select: false, handler: detailHandler, handlerKey: CoreDetail::Faction, }
					}
				))
				
				notIsMortal.then(|| rsx!(
					div
					{
						class: "column justEven",
						
						DetailInput { label: "Player:".to_string(), value: (&details2[&CoreDetail::Player]).clone(), select: false, handler: detailHandler, handlerKey: CoreDetail::Player, }
						DetailInput { label: "Chronicle:".to_string(), value: (&details2[&CoreDetail::Chronicle]).clone(), select: false, handler: detailHandler, handlerKey: CoreDetail::Chronicle, }
						DetailInput { label: "Name:".to_string(), value: (&details2[&CoreDetail::Name]).clone(), select: false, handler: detailHandler, handlerKey: CoreDetail::Name, }
						DetailInput { label: "Concept:".to_string(), value: (&details2[&CoreDetail::Concept]).clone(), select: false, handler: detailHandler, handlerKey: CoreDetail::Concept, }
						DetailInput { label: "Size:".to_string(), value: format!("{}", advantages.read().size), select: true, selectOptions: vec!["4".to_string(), "5".to_string(), "6".to_string()], handler: advantageHandler, handlerKey: CoreAdvantage::Size, }
					}
					
					div
					{
						class: "column justEven",
						
						DetailInput { label: format!("{}:", &cx.props.virtue.clone()), value: (&details2[&CoreDetail::Virtue]).clone(), select: false, handler: detailHandler, handlerKey: CoreDetail::Virtue, }
						DetailInput { label: format!("{}:", &cx.props.vice.clone()), value: (&details2[&CoreDetail::Vice]).clone(), select: false, handler: detailHandler, handlerKey: CoreDetail::Vice, }
						
						showPrimaryText.then(|| rsx!(
							DetailInput { label: format!("{}:", &cx.props.typePrimary.clone()), value: typePrimaryValue1.clone(), select: false, handler: detailHandler, handlerKey: CoreDetail::TypePrimary, }
						))
						showPrimarySelect.then(||
						{
							let options = match &cx.props.typePrimaryOptions
							{
								Some(opt) => opt.clone(),
								None => Vec::<String>::new()
							};
							
							rsx!(DetailInput
							{
								label: format!("{}:", &cx.props.typePrimary.clone()),
								value: typePrimaryValue2.clone(),
								select: true,
								selectNoneLabel: format!("{} {}", "Choose a".to_string(), &cx.props.typePrimary.clone()),
								selectOptions: options,
								handler: detailHandler,
								handlerKey: CoreDetail::TypePrimary,
							})
						})
						
						DetailInput { label: format!("{}:", &cx.props.typeSecondary.clone()), value: (&details2[&CoreDetail::TypeSecondary]).clone(), select: false, handler: detailHandler, handlerKey: CoreDetail::TypeSecondary, }
						DetailInput { label: format!("{}:", &cx.props.faction.clone()), value: (&details2[&CoreDetail::Faction]).clone(), select: false, handler: detailHandler, handlerKey: CoreDetail::Faction, }
					}
				))
			}
			
			div
			{
				class: "calculated row justEven",
				
				div { class: "row justEven", label { "Armor:" } div { "{armor}" } }
				div { class: "row justEven", label { "Defense:" } div { "{defense}" } }
				div { class: "row justEven", label { "Initiative:" } div { "{initiative}" } }
				div { class: "row justEven", label { "Speed:" } div { "{speed}" } }
			}
		}
	});
}

/// Event handler triggered when a `DetailInput`'s value changes.
fn detailHandler(cx: &Scope<DetailInputProps<CoreDetail>>, value: String)
{
	match cx.props.handlerKey
	{
		Some(df) => { updateCoreDetail(cx, df, value); }
		None => {}
	}
}

/// Event handler triggered when the Size input's value changes.
fn advantageHandler(cx: &Scope<DetailInputProps<CoreAdvantage>>, value: String)
{
	match usize::from_str_radix(&value, 10)
	{
		Ok(num) =>
		{
			match cx.props.handlerKey
			{
				Some(at) => { updateCoreAdvantage(cx, at, num); }
				None => {}
			}
		}
		Err(_) => {}
	};
}

// --------------------------------------------------

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
			class: "row justEven",
			
			label { "{label}" }
			
			showText.then(|| rsx!
			{
				input
				{
					r#type: "text",
					value: "{value}",
					oninput:  move |e| inputHandler(e, &cx),
					prevent_default: "oncontextmenu",
				}
			})
			
			showSelect.then(|| rsx!
			{
				select
				{
					onchange: move |e| inputHandler(e, &cx),
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
