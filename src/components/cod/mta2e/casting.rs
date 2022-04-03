#![allow(non_snake_case, non_upper_case_globals)]

use atoi::atoi;
use dioxus::{
	events::FormEvent,
	prelude::*,
};
use std::{
	collections::HashMap,
	convert::TryInto,
};
use strum::IntoEnumIterator;
use crate::{
	cod::{
		enums::CoreSkill,
		mta2e::{
			enums::{
				SpellCastingMethod,
				SpellFactorType,
				SpellYantras,
			},
			state::MageArcana,
		},
		state::{
			CharacterAdvantages,
			CharacterSkills,
		},
	},
	components::{
		cod::dots::{
			Dots,
			DotsProps,
		},
		core::events::{
			hideRemovePopUp,
			showRemovePopUpWithIndex,
		},
	},
	core::util::{
		RemovePopUpXOffset,
		RemovePopUpYOffset,
		generateSelectedValue,
	},
};

/// The properties struct for `Spellcasting`.
#[derive(PartialEq, Props)]
pub struct SpellcastingProps
{
	#[props(optional)]
	class: Option<String>,
}

/// A UI Component used to calculate dice pools and resource costs
/// of the complex Mage: The Awakening 2e spellcasting system
/// based on the currently loaded Sheet.
pub fn Spellcasting(cx: Scope<SpellcastingProps>) -> Element
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let arcanaRef = use_atom_ref(&cx, MageArcana);
	let skillsRef = use_atom_ref(&cx, CharacterSkills);
	let skills = skillsRef.read();
	
	let className = match &cx.props.class
	{
		Some(c) => c.clone(),
		None => "".to_string()
	};
	
	let clickedX = use_state(&cx, || 0);
	let clickedY = use_state(&cx, || 0);
	let lastIndex = use_state(&cx, || 0);
	let showRemove = use_state(&cx, || false);
	
	let posX = *clickedX.get() - RemovePopUpXOffset;
	let posY = *clickedY.get() - RemovePopUpYOffset;
	
	let castingMethod = use_state(&cx, || SpellCastingMethod::Improvised.as_ref().to_string());
	let castingDuration = use_state(&cx, || SpellFactorType::Standard.as_ref().to_string());
	let castingPotency = use_state(&cx, || SpellFactorType::Standard.as_ref().to_string());
	let castingRange = use_state(&cx, || SpellFactorType::Standard.as_ref().to_string());
	let castingScale = use_state(&cx, || SpellFactorType::Standard.as_ref().to_string());
	let castingSkill = use_state(&cx, || "".to_string());
	let castingTime = use_state(&cx, || SpellFactorType::Standard.as_ref().to_string());
	let highestArcanum = use_state(&cx, || "".to_string());
	let highestArcanumDots = use_state(&cx, || 0);
	let totalDuration = use_state(&cx, || 1);
	let totalPotency = use_state(&cx, || 1);
	let totalScale = use_state(&cx, || 1);
	let totalTime = use_state(&cx, || 1);
	let totalReaches = use_state(&cx, || 0);
	let usedYantras = use_state(&cx, || Vec::<String>::new());
	let resetLocalState = use_state(&cx, || false);
	
	if resetLocalState.get() == &true
	{
		castingMethod.set(SpellCastingMethod::Improvised.as_ref().to_string());
		castingDuration.set(SpellFactorType::Standard.as_ref().to_string());
		castingPotency.set(SpellFactorType::Standard.as_ref().to_string());
		castingRange.set(SpellFactorType::Standard.as_ref().to_string());
		castingScale.set(SpellFactorType::Standard.as_ref().to_string());
		castingSkill.set("".to_string());
		castingTime.set(SpellFactorType::Standard.as_ref().to_string());
		highestArcanum.set("".to_string());
		highestArcanumDots.set(0);
		totalDuration.set(1);
		totalPotency.set(1);
		totalScale.set(1);
		totalTime.set(1);
		totalReaches.set(0);
		usedYantras.set(Vec::<String>::new());
		resetLocalState.set(false);
	}
	
	let mut arcanaOptions = vec![];
	let mut arcanaSelected = HashMap::new();
	arcanaRef.read().iter().for_each(|(arcana, _)|
	{
		arcanaOptions.push(arcana.as_ref().to_string());
		arcanaSelected.insert(arcana.as_ref().to_string(), generateSelectedValue::<String>(arcana.as_ref().to_string(), highestArcanum.get().clone()).clone());
	});
	
	let mut castingMethodOptions = vec![];
	let mut castingMethodSelected = HashMap::<String, String>::new();
	for scm in SpellCastingMethod::iter()
	{
		castingMethodOptions.push(scm.as_ref().to_string());
		castingMethodSelected.insert(scm.as_ref().to_string(), generateSelectedValue::<String>(scm.as_ref().to_string(), castingMethod.get().clone()).clone());
	}
	
	let methodIsRote = castingMethod.get().clone() == SpellCastingMethod::Rote.as_ref().to_string();
	let mut roteSkillOptions = vec![];
	let mut roteSkillOptionsSelected = HashMap::<String, String>::new();
	CoreSkill::asMap().iter().for_each(|(cs, _)|
	{
		let skillName = CoreSkill::getSkillName(*cs);
		roteSkillOptions.push(skillName.clone());
		roteSkillOptionsSelected.insert(skillName.clone(), generateSelectedValue::<String>(skillName.clone(), castingSkill.get().clone()));
	});
	
	let mut factorTypeOptions = vec![];
	let mut castingDurationSelected = HashMap::new();
	let mut castingPotencySelected = HashMap::new();
	let mut castingRangeSelected = HashMap::new();
	let mut castingScaleSelected = HashMap::new();
	let mut castingTimeSelected = HashMap::new();
	for sft in SpellFactorType::iter()
	{
		if sft == SpellFactorType::AdvancedFree
		{
			let modifiedName = "Advanced (No Reach)".to_string();
			factorTypeOptions.push(modifiedName.clone());
			castingDurationSelected.insert(modifiedName.clone(), generateSelectedValue::<String>(modifiedName.clone(), castingDuration.get().to_string()).clone());
			castingPotencySelected.insert(modifiedName.clone(), generateSelectedValue::<String>(modifiedName.clone(), castingPotency.get().to_string()).clone());
			castingRangeSelected.insert(modifiedName.clone(), generateSelectedValue::<String>(modifiedName.clone(), castingRange.get().to_string()).clone());
			castingScaleSelected.insert(modifiedName.clone(), generateSelectedValue::<String>(modifiedName.clone(), castingScale.get().to_string()).clone());
			castingTimeSelected.insert(modifiedName.clone(), generateSelectedValue::<String>(modifiedName.clone(), castingTime.get().to_string()).clone());
		}
		else
		{
			factorTypeOptions.push(sft.as_ref().to_string());
			castingDurationSelected.insert(sft.as_ref().to_string(), generateSelectedValue::<String>(sft.as_ref().to_string(), castingDuration.get().to_string()).clone());
			castingPotencySelected.insert(sft.as_ref().to_string(), generateSelectedValue::<String>(sft.as_ref().to_string(), castingPotency.get().to_string()).clone());
			castingRangeSelected.insert(sft.as_ref().to_string(), generateSelectedValue::<String>(sft.as_ref().to_string(), castingRange.get().to_string()).clone());
			castingScaleSelected.insert(sft.as_ref().to_string(), generateSelectedValue::<String>(sft.as_ref().to_string(), castingScale.get().to_string()).clone());
			castingTimeSelected.insert(sft.as_ref().to_string(), generateSelectedValue::<String>(sft.as_ref().to_string(), castingTime.get().to_string()).clone());
		}
	}
	
	let autoReach = detectAutoReach(castingDuration.get().to_string())
					+ detectAutoReach(castingPotency.get().to_string())
					+ detectAutoReach(castingRange.get().to_string())
					+ detectAutoReach(castingScale.get().to_string())
					+ detectAutoReach(castingTime.get().to_string());
	
	if totalReaches.get() < &autoReach
	{
		totalReaches.set(autoReach);
	}
	
	let currentHighestArcanum = highestArcanumDots.get();
	let currentTotalDuration = totalDuration.get();
	let currentTotalPotency = totalPotency.get();
	let currentTotalReaches = totalReaches.get();
	let currentTotalScale = totalScale.get();
	let currentTotalTime = totalTime.get();
	
	let mut buildYantras = vec![];
	usedYantras.get().iter().for_each(|yantra| buildYantras.push(yantra.clone()));
	let currentlyUsedYantras1 = buildYantras.clone();
	let currentlyUsedYantras2 = buildYantras.clone();
	
	let mut canCastSpell = false;
	let mut spellcastingTime = "1 Turn".to_string();
	let mut spellcastingDicePool = 0;
	let mut spellcastingFreeReach = 1;
	let mut spellcastingParadoxDicePool = 0;
	
	if let Some((arcana, _)) = arcanaSelected.iter().filter(|(_, sel)| sel.to_string() == "true".to_string()).next()
	{
		let mut gnosis: usize = 1;
		if let Some(num) = advantagesRef.read().power
		{
			gnosis = num;
		}
		
		let mut characterArcanum: usize = 0;
		if let Some((_, dots)) = arcanaRef.read().iter().filter(|(a, _)| a.as_ref().to_string() == arcana.to_string()).next()
		{
			characterArcanum = *dots;
		}
		
		let currentRoteSkill = castingSkill.get().clone();
		let mut roteSkill = None;
		if let Some(actualRoteSkill) = CoreSkill::getByName(currentRoteSkill.clone())
		{
			roteSkill = Some(skills[&actualRoteSkill]);
		}
		spellcastingDicePool = calculateDicePool(
								castingMethod.get().clone(),
								roteSkill.clone(),
								gnosis,
								characterArcanum,
								*currentTotalPotency,
								*currentTotalDuration,
								*currentTotalScale,
								*currentTotalTime,
								castingTime.get().clone(),
								buildYantras.clone());
		
		let (reach, paradox) = calculateReachAndParadoxPool(castingMethod.get().clone(), gnosis, characterArcanum, *currentHighestArcanum, *currentTotalReaches);
		spellcastingFreeReach = reach;
		spellcastingParadoxDicePool = paradox;
		spellcastingTime = calculateCastingTime(gnosis, castingTime.get().clone(), *currentTotalTime, castingMethod.get().clone(), usedYantras.get().len());
		
		canCastSpell = currentHighestArcanum <= &characterArcanum && spellcastingDicePool > -6;
	}
	
	// Make sure an Arcanum and level has been selected for the to-be-cast spell
	canCastSpell = canCastSpell && highestArcanum.get().clone() != "".to_string() && highestArcanumDots.get() > &0;
	
	let chanceDie = spellcastingDicePool <= 0 && spellcastingDicePool > -6;
	let showConcentration = buildYantras.contains(&SpellYantras::Concentration.as_ref().to_string());
	let showRunes = buildYantras.contains(&SpellYantras::Runes.as_ref().to_string());
	
	return cx.render(rsx!
	{
		div
		{
			class: "column justEven spellcasting {className}",
			
			div { class: "label", "Spellcasting Calculator" },
			div
			{
				class: "sublabel",
				button { onclick: move |e| { e.cancel_bubble(); resetLocalState.set(true); }, prevent_default: "oncontextmenu", "Reset" }
			}
			
			div
			{
				class: "row justEven",
				
				div
				{
					class: "column justStart spellFactors1",
					
					div
					{
						class: "row justEnd",
						
						div { class: "label", "Spell Arcanum:" }
						
						div
						{
							class: "row justStart",
							
							select
							{
								class: "arcanum",
								onchange: move |e| { e.cancel_bubble(); highestArcanum.set(e.value.to_string()); },
								prevent_default: "oncontextmenu",
								
								option { value: "", "Select Arcanum" }
								arcanaOptions.iter().enumerate().map(|(i, name)|
								{
									let selected = &arcanaSelected[name];
									rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
								})
							}
						}
					}
					
					div
					{
						class: "row justEnd",
						
						div { class: "label", "Spell Level:" }
						
						div
						{
							class: "row justStart",
							
							Dots { max: 5, value: *currentHighestArcanum, handler: highestArcanumDotHandler, handlerKey: highestArcanumDots }
						}
					}
					
					div
					{
						class: "row justEnd",
						
						div { class: "label", "Casting Method:" }
					
						select
						{
							onchange: move |e| { e.cancel_bubble(); castingMethod.set(e.value.to_string()); },
							prevent_default: "oncontextmenu",
							
							castingMethodOptions.iter().enumerate().map(|(i, name)|
							{
								let selected = &castingMethodSelected[name];
								rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
							})
						}
					}
					
					methodIsRote.then(|| rsx!(div
					{
						class: "row justEnd",
						
						div { class: "label", "Rote Skill:" }
						
						select
						{
							onchange: move |e| { e.cancel_bubble(); castingSkill.set(e.value.to_string()); },
							prevent_default: "oncontextmenu",
							
							roteSkillOptions.iter().enumerate().map(|(i, name)|
							{
								let selected = &roteSkillOptionsSelected[name];
								rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
							})
						}
					}))
					
					div
					{
						class: "row justEnd",
						
						div { class: "label", "Total Reach:" }
						
						input
						{
							r#type: "text",
							onchange: move |e|
							{
								e.cancel_bubble();
								match atoi::<usize>(e.value.as_bytes())
								{
									Some(reaches) => totalReaches.set(reaches),
									None => totalReaches.set(*currentTotalReaches),
								}
							},
							prevent_default: "oncontextmenu",
							value: "{currentTotalReaches}"
						}
					}
					
					div
					{
						class: "row justEnd",
						
						div { class: "label", "Casting Range:" }
						
						select
						{
							onchange: move |e| { e.cancel_bubble(); castingRange.set(e.value.to_string()); },
							prevent_default: "oncontextmenu",
							
							factorTypeOptions.iter().enumerate().map(|(i, name)|
							{
								let selected = &castingRangeSelected[name];
								rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
							})
						}
					}
				}
				
				div
				{
					class: "column justStart spellFactors2",
					
					div
					{
						class: "row justCenter",
						
						div { class: "label", "Casting Time:" }
							
						div
						{
							class: "row justEnd",
						
							select
							{
								onchange: move |e| { e.cancel_bubble(); castingTime.set(e.value.to_string()); },
								prevent_default: "oncontextmenu",
								
								factorTypeOptions.iter().enumerate().map(|(i, name)|
								{
									let selected = &castingTimeSelected[name];
									rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
								})
							}
							
							input
							{
								class: "factorStep",
								r#type: "text",
								onchange: move |e|
								{
									e.cancel_bubble();
									match atoi::<usize>(e.value.as_bytes())
									{
										Some(time) =>
										{
											if time < 1
											{
												totalTime.set(*currentTotalTime);
											}
											else
											{
												totalTime.set(time);
											}
										},
										None => totalTime.set(*currentTotalTime),
									}
								},
								prevent_default: "oncontextmenu",
								value: "{currentTotalTime}"
							}
						}
					}
					
					div
					{
						class: "row justCenter",
						
						div { class: "label", "Casting Potency:" }
							
						div
						{
							class: "row justEnd",
							
							select
							{
								onchange: move |e| { e.cancel_bubble(); castingPotency.set(e.value.to_string()); },
								prevent_default: "oncontextmenu",
								
								factorTypeOptions.iter().enumerate().map(|(i, name)|
								{
									let selected = &castingPotencySelected[name];
									rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
								})
							}
							
							input
							{
								class: "factorStep",
								r#type: "text",
								onchange: move |e|
								{
									e.cancel_bubble();
									match atoi::<usize>(e.value.as_bytes())
									{
										Some(potency) =>
										{
											if potency < 1
											{
												totalPotency.set(*currentTotalPotency);
											}
											else
											{
												totalPotency.set(potency);
											}
										},
										None => totalPotency.set(*currentTotalPotency),
									}
								},
								prevent_default: "oncontextmenu",
								value: "{currentTotalPotency}"
							}
						}
					}
					
					div
					{
						class: "row justCenter",
						
						div { class: "label", "Casting Duration:" }
							
						div
						{
							class: "row justEnd",
							
							select
							{
								onchange: move |e| { e.cancel_bubble(); castingDuration.set(e.value.to_string()); },
								prevent_default: "oncontextmenu",
								
								factorTypeOptions.iter().enumerate().map(|(i, name)|
								{
									let selected = &castingDurationSelected[name];
									rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
								})
							}
							
							input
							{
								class: "factorStep",
								r#type: "text",
								onchange: move |e|
								{
									e.cancel_bubble();
									match atoi::<usize>(e.value.as_bytes())
									{
										Some(duration) =>
										{
											if duration < 1
											{
												totalDuration.set(*currentTotalDuration);
											}
											else
											{
												totalDuration.set(duration);
											}
										},
										None => totalDuration.set(*currentTotalDuration),
									}
								},
								prevent_default: "oncontextmenu",
								value: "{currentTotalDuration}"
							}
						}
					}
					
					div
					{
						class: "row justCenter",
						
						div { class: "label", "Casting Scale:" }
							
						div
						{
							class: "row justEnd",
							
							select
							{
								onchange: move |e| { e.cancel_bubble(); castingScale.set(e.value.to_string()); },
								prevent_default: "oncontextmenu",
								
								factorTypeOptions.iter().enumerate().map(|(i, name)|
								{
									let selected = &castingScaleSelected[name];
									rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
								})
							}
							
							input
							{
								class: "factorStep",
								r#type: "text",
								onchange: move |e|
								{
									e.cancel_bubble();
									match atoi::<usize>(e.value.as_bytes())
									{
										Some(scale) =>
										{
											if scale < 1
											{
												totalScale.set(*currentTotalScale);
											}
											else
											{
												totalScale.set(scale);
											}
										},
										None => totalScale.set(*currentTotalScale),
									}
								},
								prevent_default: "oncontextmenu",
								value: "{currentTotalScale}"
							}
						}
					}
					
					div
					{
						class: "column justCenter yantras",
						
						div
						{
							class: "row justEven",
							
							div { class: "label", "Yantras" }
							button { class: "clear", onclick: move |e| { hideRemovePopUp(e, &showRemove); yantraRemoveAllHandler(&usedYantras); }, prevent_default: "oncontextmenu", "Clear Yantras" }
						}
						
						select
						{
							class: "new",
							onchange: move |e| 
							{
								e.cancel_bubble();
								let mut yantras = currentlyUsedYantras1.clone();
								yantraSelectChangeHandler(e, &mut yantras, &usedYantras, None);
							},
							prevent_default: "oncontextmenu",
							
							option { value: "", selected: "true", "Select Yantra" }
							(SpellYantras::asStringVec()).iter().enumerate().map(|(i, name)| rsx!(cx, option { key: "{i}", value: "{name}", "{name}" }))
						}
						
						currentlyUsedYantras2.iter().enumerate().map(|(i, used)|
						{
							let mut yantras = currentlyUsedYantras2.clone();
							rsx!(select
							{
								onchange: move |e| yantraSelectChangeHandler(e, &mut yantras, &usedYantras, Some(i)),
								oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &lastIndex, &showRemove, i),
								prevent_default: "oncontextmenu",
								
								(SpellYantras::asStringVec()).iter().enumerate().map(|(j, name)|
								{
									let selected = generateSelectedValue(used.clone(), name.clone());
									rsx!(cx, option { key: "{j}", value: "{name}", selected: "{selected}", "{name}" })
								})
							})
						})
						
						showRemove.then(||
						{
							let mut yantras = currentlyUsedYantras2.clone();
							rsx!(
								div
								{
									class: "removePopUpWrapper column justEven",
									style: "left: {posX}px; top: {posY}px;",
									onclick: move |e| hideRemovePopUp(e, &showRemove),
									prevent_default: "oncontextmenu",
									
									div
									{
										class: "removePopUp column justEven",
										
										div { class: "row justEven", "Are you sure you want to remove this Yantra?" }
										div
										{
											class: "row justEven",
											
											button { onclick: move |e| { hideRemovePopUp(e, &showRemove); yantraRemoveHandler(&mut yantras, &usedYantras, *(lastIndex.get())); }, prevent_default: "oncontextmenu", "Remove" }
											button { onclick: move |e| hideRemovePopUp(e, &showRemove), prevent_default: "oncontextmenu", "Cancel" }
										}
									}
								}
							)
						})
					}
				}
				
				div
				{
					class: "column justStart results",
					
					div
					{
						class: "row justStart",
						
						div { class: "label", "Casting Time:" }
						(!canCastSpell).then(|| rsx!(div { class: "castTime", "-" }))
						canCastSpell.then(|| rsx!(div { class: "castTime", "{spellcastingTime}" }))
					}
					
					div
					{
						class: "row justStart",
						
						div { class: "label", "Dice Pool:" }
						(!canCastSpell).then(|| rsx!(div { class: "dicePool", "-" }))
						canCastSpell.then(|| rsx!(
							(!chanceDie).then(|| rsx!(div { class: "dicePool", "{spellcastingDicePool}" }))
							chanceDie.then(|| rsx!(div { class: "dicePool", "Chance Die" }))
						))
					}
					
					div
					{
						class: "row justStart",
						
						div { class: "label", "Paradox Dice Pool:" }
						(!canCastSpell).then(|| rsx!(div { class: "paradoxDice", "-" }))
						canCastSpell.then(|| rsx!(div { class: "paradoxDice", "{spellcastingParadoxDicePool}" }))
					}
					
					div
					{
						class: "row justStart",
						
						div { class: "label", "Free Reach:" }
						(!canCastSpell).then(|| rsx!(div { class: "freeReach", "-" }))
						canCastSpell.then(|| rsx!(div { class: "freeReach", "{spellcastingFreeReach}" }))
					}
					
					div
					{
						class: "row justStart",
						
						div { class: "label", "Mana Cost:" }
						(!canCastSpell).then(|| rsx!(div { class: "manaCost", "-" }))
						canCastSpell.then(|| rsx!(div { class: "manaCost", "1" }))
					}
					
					div
					{
						class: "row justStart",
						
						div { class: "label", "Willpower Cost:" }
						(!canCastSpell).then(|| rsx!(div { class: "willpowerCost", "-" }))
						canCastSpell.then(|| rsx!(div { class: "willpowerCost", "0" }))
					}
					
					showConcentration.then(|| rsx!(div
					{
						class: "row justEven",
						
						div { class: "yantra concentration", "Taking damage or a non-reflexive action cancels the spell."}
					}))
					
					showRunes.then(|| rsx!(div
					{
						class: "row justEven",
						
						div { class: "yantra runes", "Breaking the runes cancels the spell."}
					}))
				}
			}
		}
	});
}

// --------------------------------------------------

fn yantraSelectChangeHandler<'a>(e: FormEvent, yantras: &mut Vec<String>, usedYantras: &'a UseState<Vec<String>>, index: Option<usize>)
{
	e.cancel_bubble();
	
	match index
	{
		Some(i) => { yantras[i] = e.value.to_string(); }
		None =>
		{
			if let None = yantras.iter().enumerate().filter(|(_, inside)| inside.to_string() == e.value.to_string()).next()
			{
				yantras.push(e.value.to_string());
			}
		}
	}
	usedYantras.set(yantras.clone());
}

fn yantraRemoveHandler<'a>(yantras: &mut Vec<String>, usedYantras: &'a UseState<Vec<String>>, i: usize)
{
	yantras.remove(i);
	usedYantras.set(yantras.clone());
}

fn yantraRemoveAllHandler<'a>(usedYantras: &'a UseState<Vec<String>>)
{
	usedYantras.set(Vec::<String>::new());
}

fn highestArcanumDotHandler<'a>(cx: &Scope<DotsProps<&'a UseState<usize>>>, clickedValue: usize)
{
	if let Some(highestArcanumDots) = &cx.props.handlerKey
	{
		highestArcanumDots.set(clickedValue);
	}
}

// --------------------------------------------------

fn calculateReachAndParadoxPool(castingMethod: String, gnosis: usize, characterLevel: usize, spellLevel: usize, totalReaches: usize) -> (usize, usize)
{
	let mut freeReach = 1;
	// There should be logic preventing this function from being called if this isn't true, but better safe than sorry.
	if characterLevel >= spellLevel
	{
		let mut effectiveCasterLevel = characterLevel;
		// Rotes get free reach as though the caster has 5 dots in the Arcanum.
		if castingMethod.to_string() == SpellCastingMethod::Rote.as_ref().to_string()
		{
			effectiveCasterLevel = 5;
		}
		
		freeReach = effectiveCasterLevel - spellLevel + 1;
	}
	
	let mut paradoxSteps = 0;
	if totalReaches > freeReach
	{
		paradoxSteps = totalReaches - freeReach;
	}
	
	let paradoxDice = paradoxSteps * paradoxDicePerGnosis(gnosis);
	
	return (freeReach, paradoxDice);
}

/// Calculate the Spellcasting Dice Pool based on the choices the user makes.
fn calculateDicePool(castingMethod:String, roteSkill: Option<usize>, characterGnosis: usize, characterLevel: usize, chosenPotency: usize, chosenDuration: usize, chosenScale: usize, chosenTime: usize, timeFactor: String, yantras: Vec<String>) -> isize
{
	// The Dice Pool starts out as Gnosis + Arcanum
	let mut dice: isize = 0;
	
	let gnosis: isize = match characterGnosis.try_into()
	{
		Ok(num) => num,
		Err(_) => 0,
	};
	dice += gnosis;
	
	let dots: isize = match characterLevel.clone().try_into()
	{
		Ok(num) => num,
		Err(_) => 0,
	};
	dice += dots;
	
	// Collect the Spell Factor Dice Penalty separately from the Dice Pool, so we can calculate the Yantra bonus correctly.
	let mut stepPenalty = 0;
	let potency: isize = match chosenPotency.try_into()
	{
		Ok(num) => num,
		Err(_) => 0,
	};
	stepPenalty -= changeFactorStepToDice(potency);
	
	let duration: isize = match chosenDuration.try_into()
	{
		Ok(num) => num,
		Err(_) => 0,
	};
	stepPenalty -= changeFactorStepToDice(duration);
	
	let scale: isize = match chosenScale.try_into()
	{
		Ok(num) => num,
		Err(_) => 0,
	};
	stepPenalty -= changeFactorStepToDice(scale);
	
	// Ritual casting, every interval beyond the first grants additional dice
	if timeFactor == SpellFactorType::Standard.as_ref().to_string()
	{
		let time: isize = match chosenTime.try_into()
		{
			Ok(num) => num,
			Err(_) => 0,
		};
		
		if time > 1
		{
			let mut timeDice = time - 1;
			if timeDice > 5
			{
				timeDice = 5;
			}
			stepPenalty += timeDice;
		}
	}
	
	// The Yantra bonuses can be used to offset the penalties from increasing Spell Factors.
	let mut yantraBonus = stepPenalty;
	// The number of Yantras that can be used is limited by Gnosis
	let mut yantrasUsed = 0;
	let yantrasMax = yantrasPerGnosis(gnosis);
	let allYantras = SpellYantras::asMap();
	allYantras.iter().for_each(|(yantra, name)|
	{
		if yantrasUsed <= yantrasMax
		{
			if let Some(_) = yantras.iter().filter(|y| y.to_string() == name.to_string()).next()
			{
				yantrasUsed += 1;
				let roteSkillValue: isize = match roteSkill
				{
					Some(num) => match num.clone().try_into()
					{
						Ok(num2) => num2,
						Err(_) => 0
					},
					None => 0,
				};
				match yantra
				{
					SpellYantras::Concentration => yantraBonus += 2,
					SpellYantras::Demesne => yantraBonus += 2,
					SpellYantras::Environment => yantraBonus += 1,
					// Only applies to Rotes
					SpellYantras::Mudra => { if castingMethod.clone() == SpellCastingMethod::Rote.as_ref().to_string() { yantraBonus += roteSkillValue } },
					SpellYantras::OrderTool => yantraBonus += 1,
					SpellYantras::PathTool => yantraBonus += 1,
					SpellYantras::Runes => yantraBonus += 2,
					SpellYantras::Sacrament => yantraBonus += 2,
					SpellYantras::Sympathy => yantraBonus += 2,
				}
			}
		}
	});
	
	// The bonus from Yantras is limited to +5, after overcoming Spell Factor penalties.
	if yantraBonus > 5
	{
		yantraBonus = 5;
	}
	
	return dice + yantraBonus;
}

fn calculateCastingTime(gnosis: usize, time: String, timeSteps: usize, _method: String, yantrasCount: usize) -> String
{
	if time == SpellFactorType::Standard.as_ref().to_string()
	{
		//Ritual
		let (interval, intervalLabel) = ritualIntervalPerGnosis(gnosis);
		let quantity = interval * timeSteps;
		
		if quantity > 1
		{
			return format!("{} {}s", quantity, intervalLabel);
		}
		else
		{
			return format!("1 {}", intervalLabel);
		}
	}
	else
	{
		//Instant
		let mut turns = 1;
		if yantrasCount > 1
		{
			turns += yantrasCount - 1;
		}
		if turns > 1
		{
			return format!("{} Turns", turns);
		}
		return "1 Turn".to_string();
	}
}

/// Calculate the dice penalty for the number of steps in a particular Spell Factor.
/// 
/// Each step beyond the first is worth 2 dice. The first step is free.
/// There are no negative steps.
fn changeFactorStepToDice(steps: isize) -> isize
{
	return (steps - 1) * 2;
}

fn detectAutoReach(factor: String) -> usize
{
	let mut autoReach = 0;
	if factor.to_string() == SpellFactorType::Advanced.as_ref().to_string()
	{
		autoReach += 1;
	}
	return autoReach;
}

fn paradoxDicePerGnosis(gnosis: usize) -> usize
{
	return match gnosis
	{
		1 => 1,
		2 => 1,
		3 => 2,
		4 => 2,
		5 => 3,
		6 => 3,
		7 => 4,
		8 => 4,
		_ => 5,
	};
}

fn ritualIntervalPerGnosis(gnosis: usize) -> (usize, String)
{
	return match gnosis
	{
		1 => (3, "Hour".to_string()),
		2 => (3, "Hour".to_string()),
		3 => (1, "Hour".to_string()),
		4 => (1, "Hour".to_string()),
		5 => (30, "Minute".to_string()),
		6 => (30, "Minute".to_string()),
		7 => (10, "Minute".to_string()),
		8 => (10, "Minute".to_string()),
	 	_ => (1, "Minute".to_string()),
	};
}

fn yantrasPerGnosis(gnosis: isize) -> isize
{
	return match gnosis
	{
		1 => 2,
		2 => 2,
		3 => 3,
		4 => 3,
		5 => 4,
		6 => 4,
		7 => 5,
		8 => 5,
		_ => 6,
	};
}
