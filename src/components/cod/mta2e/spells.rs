#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	events::FormEvent,
	prelude::*,
};
use std::collections::HashMap;
use crate::{
	cod::{
		enums::{
			CoreAttribute,
			CoreSkill,
		},
		mta2e::{
			enums::{
				Arcana,
				PraxisField,
				RoteField,
				SpellFactor,
				SpellField,
				SpellPractice,
			},
			structs::{
				Praxis,
				Rote,
				Spell,
			},
			state::{
				MagePraxes,
				MageRotes,
				MageSpells,
			},
		},
	},
	components::{
		core::{
			dots::{
				Dots,
				DotsProps,
			},
			events::{
				hideRemovePopUp,
				showRemovePopUpWithIndex,
			},
		},
	},
	core::util::{
		RemovePopUpXOffset,
		RemovePopUpYOffset,
		generateSelectedValue,
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
						oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
						prevent_default: "oncontextmenu",
						
						input
						{
							r#type: "text",
							value: "{praxis.name}",
							onchange: move |e| praxisUpdateHandler(e, &cx, Some(i), PraxisField::Name),
							oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
							prevent_default: "oncontextmenu",
							placeholder: "Spell Name",
						}
						
						select
						{
							class: "arcanum",
							onchange: move |e| praxisUpdateHandler(e, &cx, Some(i), PraxisField::Arcanum),
							oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
							prevent_default: "oncontextmenu",
							
							option { value: "", selected: "true", "Select Arcanum" }
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
					class: "new row justEven",
					input { r#type: "text", value: "", placeholder: "Enter new a Praxis", onchange: move |e| praxisUpdateHandler(e, &cx, None, PraxisField::Name), prevent_default: "oncontextmenu" }
				}
				
				showRemove.then(|| rsx!{
					div
					{
						class: "removePopUpWrapper column justEven",
						style: "left: {posX}px; top: {posY}px;",
						onclick: move |e| hideRemovePopUp(e, &showRemove),
						prevent_default: "oncontextmenu",
						
						div
						{
							class: "removePopUp column justEven",
							
							div { class: "row justEven", "Are you sure you want to remove this Praxis?" }
							div
							{
								class: "row justEven",
								
								button { onclick: move |e| { hideRemovePopUp(e, &showRemove); praxisRemoveClickHandler(&cx, *(lastIndex.get())); }, prevent_default: "oncontextmenu", "Remove" }
								button { onclick: move |e| hideRemovePopUp(e, &showRemove), prevent_default: "oncontextmenu", "Cancel" }
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
						oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
						prevent_default: "oncontextmenu",
						
						input
						{
							r#type: "text",
							value: "{rote.name}",
							onchange: move |e| roteUpdateHandler(e, &cx, Some(i), RoteField::Name),
							oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
							prevent_default: "oncontextmenu",
							placeholder: "Spell Name",
						}
						
						select
						{
							class: "arcanum",
							onchange: move |e| roteUpdateHandler(e, &cx, Some(i), RoteField::Arcanum),
							oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
							prevent_default: "oncontextmenu",
							
							option { value: "", selected: "true", "Select Arcanum" }
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
							oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
							prevent_default: "oncontextmenu",
							
							option { value: "", selected: "true", "Select Skill" }
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
							oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
							prevent_default: "oncontextmenu",
							placeholder: "Creator",
						}
					})
				})
				
				div
				{
					class: "new row justEven",
					input { class: "newRote", r#type: "text", value: "", placeholder: "Enter new a Rote", onchange: move |e| roteUpdateHandler(e, &cx, None, RoteField::Name), prevent_default: "oncontextmenu" }
				}
				
				showRemove.then(|| rsx!{
					div
					{
						class: "removePopUpWrapper column justEven",
						style: "left: {posX}px; top: {posY}px;",
						onclick: move |e| hideRemovePopUp(e, &showRemove),
						prevent_default: "oncontextmenu",
						
						div
						{
							class: "removePopUp column justEven",
							
							div { class: "row justEven", "Are you sure you want to remove this Rote?" }
							div
							{
								class: "row justEven",
								
								button { onclick: move |e| { hideRemovePopUp(e, &showRemove); roteRemoveClickHandler(&cx, *(lastIndex.get())); }, prevent_default: "oncontextmenu", "Remove" }
								button { onclick: move |e| hideRemovePopUp(e, &showRemove), prevent_default: "oncontextmenu", "Cancel" }
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

// --------------------------------------------------

/// The UI Component handling a Mage: The Awakening 2e Mage's list of available Spells.
pub fn SpellDetails(cx: Scope) -> Element
{
	let spellsRef = use_atom_ref(&cx, MageSpells);
	let spells = spellsRef.read();
	
	let clickedX = use_state(&cx, || 0);
	let clickedY = use_state(&cx, || 0);
	let lastIndex = use_state(&cx, || 0);
	let showRemove = use_state(&cx, || false);
	
	let posX = *clickedX.get() - RemovePopUpXOffset;
	let posY = *clickedY.get() - RemovePopUpYOffset;
	
	return cx.render(rsx!
	{
		div
		{
			class: "simpleEntryListWrapper spells column justEven",
			
			div { class: "simpleEntryListLabel", "Spells" }
			
			div
			{
				class: "simpleEntryList column justEven",
				
				spells.iter().enumerate().map(|(i, spell)| rsx!(
					(i > 0).then(|| rsx!(cx, hr { class: "row justEven thin" }))
					div
					{
						class: "entry column justStart",
						oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
						prevent_default: "oncontextmenu",
						
						div
						{
							class: "row justEven",
							oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
							prevent_default: "oncontextmenu",
							
							div { class: "label spellName", "Name:" }
							input
							{
								class: "spellName",
								r#type: "text",
								value: "{spell.name}",
								onchange: move |e| spellUpdateHandler(e, &cx, Some(i), SpellField::Name),
								oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
								prevent_default: "oncontextmenu"
							}
							
							select
							{
								class: "spellArcanum",
								onchange: move |e| spellUpdateHandler(e, &cx, Some(i), SpellField::Arcanum),
								oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
								prevent_default: "oncontextmenu",
								
								option { value: "", "Select Arcanum" }
								(Arcana::asMap()).iter().enumerate().map(|(j, (a, name))|
								{
									let mut selected = "false".to_string();
									if let Some(arcanum) = spell.arcanum
									{
										selected = generateSelectedValue(arcanum, *a);
									}
									rsx!(cx, option { key: "{j}", value: "{name}", selected: "{selected}", "{name}" })
								})
							}
						
							Dots { class: "spellDots".to_string(), max: 5, value: SpellPractice::getValue(spell.practice), handlerKey: Some(i) }
						}
						
						div
						{
							class: "row",
							
							div { class: "label spellPractice", "Practice:" }
							
							select
							{
								class: "spellPractice",
								onchange: move |e| spellUpdateHandler(e, &cx, Some(i), SpellField::Practice),
								oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
								prevent_default: "oncontextmenu",
								
								option { value: "", "Select Practice" }
								(SpellPractice::asMap()).iter().enumerate().map(|(j, (sp, name))|
								{
									let mut selected = "false".to_string();
									if let Some(practice) = spell.practice
									{
										selected = generateSelectedValue(practice, *sp);
									}
									rsx!(cx, option { key: "{j}", value: "{name}", selected: "{selected}", "{name}" })
								})
							}
							
							div { class: "label spellFactor", "Primary Factor:" }
							
							select
							{
								class: "spellFactor",
								onchange: move |e| spellUpdateHandler(e, &cx, Some(i), SpellField::PrimaryFactor),
								oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
								prevent_default: "oncontextmenu",
								
								option { value: "", "Select Primary Factor" }
								(SpellFactor::asMap()).iter().enumerate().map(|(j, (sf, name))|
								{
									let mut selected = "false".to_string();
									if let Some(factor) = spell.primaryFactor
									{
										selected = generateSelectedValue(factor, *sf);
									}
									rsx!(cx, option { key: "{j}", value: "{name}", selected: "{selected}", "{name}" })
								})
							}
							
							div { class: "label spellWithstand", "Withstand:" }
							
							
							select
							{
								class: "spellWithstand",
								onchange: move |e| spellUpdateHandler(e, &cx, Some(i), SpellField::Withstand),
								oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
								prevent_default: "oncontextmenu",
								
								option { value: "", "Select Withstand" }
								(CoreAttribute::asMap()).iter().enumerate().map(|(j, (ca, _))|
								{
									let name = ca.as_ref().to_string();
									let mut selected = "false".to_string();
									if let Some(withstand) = spell.withstand
									{
										selected = generateSelectedValue(withstand, *ca);
									}
									rsx!(cx, option { key: "{j}", value: "{name}", selected: "{selected}", "{name}" })
								})
							}
						}
						
						div
						{
							class: "column justEven",
							div { class: "label", "Intent:" }
							textarea
							{
								//autosize: "true",
								onchange: move |e| spellUpdateHandler(e, &cx, Some(i), SpellField::Intent),
								oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
								prevent_default: "oncontextmenu",
								
								"{spell.intent}"
							}
						}
						
						div
						{
							class: "column justEven",
							div { class: "label", "Effects:" }
							textarea
							{
								//autosize: "true",
								onchange: move |e| spellUpdateHandler(e, &cx, Some(i), SpellField::Effects),
								oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
								prevent_default: "oncontextmenu",
								
								"{spell.effects}"
							}
						}
					}
				))
				
				div
				{
					class: "new row justEven",
					input { r#type: "text", value: "", placeholder: "Enter a new Spell", onchange: move |e| spellUpdateHandler(e, &cx, None, SpellField::Name), prevent_default: "oncontextmenu" }
				}
			}
			
			showRemove.then(|| rsx!
			{
				div
				{
					class: "removePopUpWrapper column justEven",
					style: "left: {posX}px; top: {posY}px;",
					onclick: move |e| hideRemovePopUp(e, &showRemove),
					prevent_default: "oncontextmenu",
					
					div
					{
						class: "removePopUp column justEven",
						
						div { class: "row justEven", "Are you sure you want to remove this Spell?" }
						div
						{
							class: "row justEven",
							
							button { onclick: move |e| { hideRemovePopUp(e, &showRemove); spellRemoveHandler(&cx, *(lastIndex.get())); }, prevent_default: "oncontextmenu", "Remove" }
							button { onclick: move |e| hideRemovePopUp(e, &showRemove), prevent_default: "oncontextmenu", "Cancel" }
						}
					}
				}
			})
		}
	});
}

/// Event handler triggered by clicking the "Remove" button after
/// right-clicking a Spell row.
fn spellRemoveHandler(cx: &Scope, index: usize)
{
	let spellsRef = use_atom_ref(&cx, MageSpells);
	let mut spells = spellsRef.write();
	
	if index < spells.len()
	{
		spells.remove(index);
	}
}

fn spellUpdateHandler(event: FormEvent, cx: &Scope, index: Option<usize>, field: SpellField)
{
	let spellsRef = use_atom_ref(&cx, MageSpells);
	let mut spells = spellsRef.write();
	
	match index
	{
		Some(i) =>
		{
			match field
			{
				SpellField::Arcanum => if let Some(a) = Arcana::getByName(event.value.clone()) { spells[i].arcanum = Some(a); },
				SpellField::Effects => spells[i].effects = event.value.clone(),
				SpellField::Intent => spells[i].intent = event.value.clone(),
				SpellField::Name => spells[i].name = event.value.clone(),
				SpellField::Practice => if let Some(sp) = SpellPractice::getByName(event.value.clone()) { spells[i].practice = Some(sp); },
				SpellField::PrimaryFactor => if let Some(sf) = SpellFactor::getByName(event.value.clone()) { spells[i].primaryFactor = Some(sf); },
				SpellField::Withstand => if let Some(ca) = CoreAttribute::getByName(event.value.clone()) { spells[i].withstand = Some(ca); },
			}
		}
		None =>
		{
			match field
			{
				SpellField::Arcanum => if let Some(a) = Arcana::getByName(event.value.clone()) { spells.push(Spell { arcanum: Some(a), ..Default::default() }); },
				SpellField::Effects => spells.push(Spell { effects: event.value.clone(), ..Default::default() }),
				SpellField::Intent => spells.push(Spell { intent: event.value.clone(), ..Default::default() }),
				SpellField::Name => spells.push(Spell { name: event.value.clone(), ..Default::default() }),
				SpellField::Practice => if let Some(sp) = SpellPractice::getByName(event.value.clone()) { spells.push(Spell { practice: Some(sp), ..Default::default() }); },
				SpellField::PrimaryFactor => if let Some(sf) = SpellFactor::getByName(event.value.clone()) { spells.push(Spell { primaryFactor: Some(sf), ..Default::default() }); },
				SpellField::Withstand => if let Some(ca) = CoreAttribute::getByName(event.value.clone()) { spells.push(Spell { withstand: Some(ca), ..Default::default() }); },
			}
		}
	}
}
