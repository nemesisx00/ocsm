#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	events::FormEvent,
	prelude::*,
};
use std::collections::HashMap;
use crate::{
	cod::{
		enums::CoreSkill,
		mta2e::{
			enums::{
				Arcana,
				PraxisField,
				RoteField,
			},
			structs::{
				Praxis,
				Rote,
			},
			state::{
				MagePraxes,
				MageRotes,
			},
		},
	},
	components::cod::dots::{
		Dots,
		DotsProps,
	},
	core::util::{
		RemovePopUpXOffset,
		RemovePopUpYOffset,
	},
};

/// The properties struct for `Praxis`.
#[derive(PartialEq, Props)]
pub struct PraxesProps
{
	#[props(optional)]
	class: Option<String>,
}

/// A generic UI Component for an editable list of text inputs.
pub fn Praxes(cx: Scope<PraxesProps>) -> Element
{
	let praxesRef = use_atom_ref(&cx, MagePraxes);
	let praxes = praxesRef.read();
	
	let clickedX = use_state(&cx, || 0);
	let clickedY = use_state(&cx, || 0);
	let lastIndex = use_state(&cx, || 0);
	let showRemove = use_state(&cx, || false);
	
	let posX = *clickedX.get() - RemovePopUpXOffset;
	let posY = *clickedY.get() - RemovePopUpYOffset;
	
	let className = match &cx.props.class
	{
		Some(c) => c.clone(),
		None => "".to_string()
	};
	
	return cx.render(rsx!
	{
		div
		{
			class: "simpleEntryListWrapper column praxes justEven {className}",
			
			div { class: "simpleEntryListLabel", "Praxes" },
			
			div
			{
				class: "sublabel row justEven",
				
				div { "Spell" }
				div { "Arcanum" }
				div { "Level" }
			}
			
			div
			{
				class: "simpleEntryList praxes column justEven",
				
				praxes.iter().enumerate().map(|(i, praxis)| {
					
					let mut arcanaOptions = vec![];
					let mut arcanaSelected = HashMap::new();
					Arcana::asMap().iter().for_each(|(_, name)|
					{
						arcanaOptions.push(name.to_string());
						if let Some(arcanum) = praxis.arcanum
						{
							let selected = match arcanum.as_ref().to_string() == name.to_string()
							{
								true => "true",
								false => "false"
							};
							arcanaSelected.insert(name.to_string(), selected);
						}
						else
						{
							arcanaSelected.insert(name.to_string(), "false");
						}
					});
					
					rsx!(cx, div
					{
						class: "row justEven praxis",
						key: "{i}",
						oncontextmenu: move |e|
						{
							e.cancel_bubble();
							clickedX.set(e.data.client_x);
							clickedY.set(e.data.client_y);
							lastIndex.set(i);
							showRemove.set(true);
						},
						prevent_default: "oncontextmenu",
						
						input
						{
							r#type: "text",
							value: "{praxis.name}",
							onchange: move |e| praxisUpdateHandler(e, &cx, Some(i), PraxisField::Name),
							oncontextmenu: move |e|
							{
								e.cancel_bubble();
								clickedX.set(e.data.client_x);
								clickedY.set(e.data.client_y);
								lastIndex.set(i);
								showRemove.set(true);
							},
							prevent_default: "oncontextmenu",
						}
						
						// select Arcanum
						select
						{
							onchange: move |e| praxisUpdateHandler(e, &cx, Some(i), PraxisField::Arcanum),
							oncontextmenu: move |e| e.cancel_bubble(),
							prevent_default: "oncontextmenu",
							
							option { value: "", selected: "true", "Choose an Arcanum" }
							arcanaOptions.iter().enumerate().map(|(i, name)|
							{
								let selected = arcanaSelected[name];
								rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
							})
						}
						
						Dots { max: 5, value: praxis.level, handler: praxisDotHandler, handlerKey: i }
					})
				})
				
				div
				{
					class: "row justEven",
					input { r#type: "text", value: "", placeholder: "Enter new a Praxis", onchange: move |e| praxisUpdateHandler(e, &cx, None, PraxisField::Name), oncontextmenu: move |e| e.cancel_bubble(), prevent_default: "oncontextmenu" }
				}
				
				showRemove.then(|| rsx!{
					div
					{
						class: "removePopUpWrapper column justEven",
						style: "left: {posX}px; top: {posY}px;",
						onclick: move |e| { e.cancel_bubble(); showRemove.set(false); },
						prevent_default: "onclick",
						
						div
						{
							class: "removePopUp column justEven",
							
							div { class: "row justEven", "Are you sure you want to remove this Praxis?" }
							div
							{
								class: "row justEven",
								
								button { onclick: move |e| { e.cancel_bubble(); praxisRemoveClickHandler(&cx, *(lastIndex.get())); showRemove.set(false); }, prevent_default: "onclick", "Remove" }
								button { onclick: move |e| { e.cancel_bubble(); showRemove.set(false); }, prevent_default: "onclick", "Cancel" }
							}
						}
					}
				})
			}
		}
	});
}

/// Event handler triggered when a Praxis Level value changes.
fn praxisDotHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	let praxesRef = use_atom_ref(&cx, MagePraxes);
	let mut praxes = praxesRef.write();
	
	if let Some(i) = &cx.props.handlerKey
	{
		let ref mut praxis = praxes[*i];
		praxis.level = clickedValue;
	}
}

/// Event handler triggered by clicking the "Remove" button after
/// right-clicking a Praxis row.
fn praxisRemoveClickHandler(cx: &Scope<PraxesProps>, index: usize)
{
	let praxesRef = use_atom_ref(&cx, MagePraxes);
	let mut praxes = praxesRef.write();
	
	if index < praxes.len()
	{
		praxes.remove(index);
	}
}

/// Event handler triggered when a Praxis's value changes.
fn praxisUpdateHandler(e: FormEvent, cx: &Scope<PraxesProps>, index: Option<usize>, field: PraxisField)
{
	let praxesRef = use_atom_ref(&cx, MagePraxes);
	let mut praxes = praxesRef.write();
	
	match index
	{
		Some(i) =>
		{
			match field
			{
				PraxisField::Arcanum => if let Some(a) = Arcana::getByName(e.value.clone()) { praxes[i].arcanum = Some(a); },
				PraxisField::Name => praxes[i].name = e.value.clone(),
				_ => {}
			}
		}
		None =>
		{
			match field
			{
				PraxisField::Arcanum => if let Some(a) = Arcana::getByName(e.value.clone()) { praxes.push(Praxis { arcanum: Some(a), ..Default::default() }); },
				PraxisField::Name => praxes.push(Praxis { name: e.value.clone(), ..Default::default() }),
				_ => {}
			}
		}
	}
}

// --------------------------------------------------

/// The properties struct for `Rote`.
#[derive(PartialEq, Props)]
pub struct RotesProps
{
	#[props(optional)]
	class: Option<String>,
}

/// A generic UI Component for an editable list of text inputs.
pub fn Rotes(cx: Scope<RotesProps>) -> Element
{
	let rotesRef = use_atom_ref(&cx, MageRotes);
	let rotes = rotesRef.read();
	
	let clickedX = use_state(&cx, || 0);
	let clickedY = use_state(&cx, || 0);
	let lastIndex = use_state(&cx, || 0);
	let showRemove = use_state(&cx, || false);
	
	let posX = *clickedX.get() - RemovePopUpXOffset;
	let posY = *clickedY.get() - RemovePopUpYOffset;
	
	let className = match &cx.props.class
	{
		Some(c) => c.clone(),
		None => "".to_string()
	};
	
	return cx.render(rsx!
	{
		div
		{
			class: "simpleEntryListWrapper column rotes justEven {className}",
			
			div { class: "simpleEntryListLabel", "Rotes" },
			
			div
			{
				class: "sublabel row justEven",
				
				div { "Spell" }
				div { "Arcanum" }
				div { "Level" }
				div { "Creator" }
				div { "Rote Skill" }
			}
			
			div
			{
				class: "simpleEntryList rote column justEven",
				
				rotes.iter().enumerate().map(|(i, rote)| {
					
					let mut arcanaOptions = vec![];
					let mut arcanaSelected = HashMap::new();
					Arcana::asMap().iter().for_each(|(_, name)|
					{
						arcanaOptions.push(name.to_string());
						if let Some(arcanum) = rote.arcanum
						{
							let selected = match arcanum.as_ref().to_string() == name.to_string()
							{
								true => "true",
								false => "false"
							};
							arcanaSelected.insert(name.to_string(), selected);
						}
						else
						{
							arcanaSelected.insert(name.to_string(), "false");
						}
					});
					let mut skillOptions = vec![];
					let mut skillSelected = HashMap::new();
					CoreSkill::asMap().iter().for_each(|(cs, _)|
					{
						skillOptions.push(CoreSkill::getSkillName(*cs));
						if let Some(skill) = rote.skill
						{
							let selected = match CoreSkill::getSkillName(skill) == CoreSkill::getSkillName(*cs)
							{
								true => "true",
								false => "false"
							};
							skillSelected.insert(CoreSkill::getSkillName(*cs), selected);
						}
						else
						{
							skillSelected.insert(CoreSkill::getSkillName(*cs), "false");
						}
					});
					
					rsx!(cx, div
					{
						class: "row justEven rote",
						key: "{i}",
						oncontextmenu: move |e|
						{
							e.cancel_bubble();
							clickedX.set(e.data.client_x);
							clickedY.set(e.data.client_y);
							lastIndex.set(i);
							showRemove.set(true);
						},
						prevent_default: "oncontextmenu",
						
						input
						{
							r#type: "text",
							value: "{rote.name}",
							onchange: move |e| roteUpdateHandler(e, &cx, Some(i), RoteField::Name),
							oncontextmenu: move |e|
							{
								e.cancel_bubble();
								clickedX.set(e.data.client_x);
								clickedY.set(e.data.client_y);
								lastIndex.set(i);
								showRemove.set(true);
							},
							prevent_default: "oncontextmenu",
						}
						
						select
						{
							class: "arcanum",
							onchange: move |e| roteUpdateHandler(e, &cx, Some(i), RoteField::Arcanum),
							oncontextmenu: move |e| e.cancel_bubble(),
							prevent_default: "oncontextmenu",
							
							option { value: "", selected: "true", "Choose an Arcanum" }
							arcanaOptions.iter().enumerate().map(|(i, name)|
							{
								let selected = arcanaSelected[name];
								rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
							})
						}
						
						Dots { max: 5, value: rote.level, handler: roteDotHandler, handlerKey: i }
						
						select
						{
							class: "skill",
							onchange: move |e| roteUpdateHandler(e, &cx, Some(i), RoteField::Skill),
							oncontextmenu: move |e| e.cancel_bubble(),
							prevent_default: "oncontextmenu",
							
							option { value: "", selected: "true", "Choose a Rote Skill" }
							skillOptions.iter().enumerate().map(|(i, name)|
							{
								let selected = skillSelected[name];
								rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
							})
						}
						
						input
						{
							r#type: "text",
							value: "{rote.creator}",
							onchange: move |e| roteUpdateHandler(e, &cx, Some(i), RoteField::Creator),
							oncontextmenu: move |e|
							{
								e.cancel_bubble();
								clickedX.set(e.data.client_x);
								clickedY.set(e.data.client_y);
								lastIndex.set(i);
								showRemove.set(true);
							},
							prevent_default: "oncontextmenu",
						}
					})
				})
				
				div
				{
					class: "row justEven",
					input { class: "newRote", r#type: "text", value: "", placeholder: "Enter new a Rote", onchange: move |e| roteUpdateHandler(e, &cx, None, RoteField::Name), oncontextmenu: move |e| e.cancel_bubble(), prevent_default: "oncontextmenu" }
				}
				
				showRemove.then(|| rsx!{
					div
					{
						class: "removePopUpWrapper column justEven",
						style: "left: {posX}px; top: {posY}px;",
						onclick: move |e| { e.cancel_bubble(); showRemove.set(false); },
						prevent_default: "onclick",
						
						div
						{
							class: "removePopUp column justEven",
							
							div { class: "row justEven", "Are you sure you want to remove this Rote?" }
							div
							{
								class: "row justEven",
								
								button { onclick: move |e| { e.cancel_bubble(); roteRemoveClickHandler(&cx, *(lastIndex.get())); showRemove.set(false); }, prevent_default: "onclick", "Remove" }
								button { onclick: move |e| { e.cancel_bubble(); showRemove.set(false); }, prevent_default: "onclick", "Cancel" }
							}
						}
					}
				})
			}
		}
	});
}

/// Event handler triggered when a Rote Level value changes.
fn roteDotHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	let rotesRef = use_atom_ref(&cx, MageRotes);
	let mut rotes = rotesRef.write();
	
	if let Some(i) = &cx.props.handlerKey
	{
		let ref mut rote = rotes[*i];
		rote.level = clickedValue;
	}
}

/// Event handler triggered by clicking the "Remove" button after
/// right-clicking a Rote row.
fn roteRemoveClickHandler(cx: &Scope<RotesProps>, index: usize)
{
	let rotesRef = use_atom_ref(&cx, MageRotes);
	let mut rotes = rotesRef.write();
	
	if index < rotes.len()
	{
		rotes.remove(index);
	}
}

/// Event handler triggered when a Rote field's value changes.
fn roteUpdateHandler(e: FormEvent, cx: &Scope<RotesProps>, index: Option<usize>, field: RoteField)
{
	let rotesRef = use_atom_ref(&cx, MageRotes);
	let mut rotes = rotesRef.write();
	
	match index
	{
		Some(i) =>
		{
			match field
			{
				RoteField::Arcanum => if let Some(a) = Arcana::getByName(e.value.clone()) { rotes[i].arcanum = Some(a); },
				RoteField::Creator => rotes[i].creator = e.value.clone(),
				RoteField::Name => rotes[i].name = e.value.clone(),
				RoteField::Skill => if let Some(cs) = CoreSkill::getByName(e.value.clone()) { rotes[i].skill = Some(cs); },
				_ => {}
			}
		}
		None =>
		{
			match field
			{
				RoteField::Arcanum => if let Some(a) = Arcana::getByName(e.value.clone()) { rotes.push(Rote { arcanum: Some(a), ..Default::default() }); },
				RoteField::Creator => rotes.push(Rote { creator: e.value.clone(), ..Default::default() }),
				RoteField::Name => rotes.push(Rote { name: e.value.clone(), ..Default::default() }),
				RoteField::Skill => if let Some(cs) = CoreSkill::getByName(e.value.clone()) { rotes.push(Rote { skill: Some(cs), ..Default::default() }); },
				_ => {}
			}
		}
	}
}
