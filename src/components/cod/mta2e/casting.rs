#![allow(non_snake_case, non_upper_case_globals)]

use atoi::atoi;
use dioxus::prelude::*;
use std::collections::HashMap;
use std::convert::TryInto;
use strum::IntoEnumIterator;
use crate::{
	cod::{
		mta2e::{
			enums::{
				SpellCastingMethod,
				SpellFactorType,
				SpellYantras,
			},
			state::MageArcana,
		},
		state::CharacterAdvantages,
	},
	components::cod::dots::{
		Dots,
		DotsProps,
	},
	core::util::generateSelectedValue,
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
	
	let className = match &cx.props.class
	{
		Some(c) => c.clone(),
		None => "".to_string()
	};
	
	let castingMethod = use_state(&cx, || SpellCastingMethod::Improvised.as_ref().to_string());
	let castingDuration = use_state(&cx, || SpellFactorType::Standard.as_ref().to_string());
	let castingPotency = use_state(&cx, || SpellFactorType::Standard.as_ref().to_string());
	let castingRange = use_state(&cx, || SpellFactorType::Standard.as_ref().to_string());
	let castingScale = use_state(&cx, || SpellFactorType::Standard.as_ref().to_string());
	let castingTime = use_state(&cx, || SpellFactorType::Standard.as_ref().to_string());
	let highestArcanum = use_state(&cx, || "".to_string());
	let highestArcanumDots = use_state(&cx, || 0);
	let totalDuration = use_state(&cx, || 1);
	let totalPotency = use_state(&cx, || 1);
	let totalScale = use_state(&cx, || 1);
	let totalReaches = use_state(&cx, || 0);
	let usedYantras = use_state(&cx, || Vec::<String>::new());
	
	let mut arcanaOptions = vec![];
	let mut arcanaSelected = HashMap::new();
	arcanaRef.read().iter().for_each(|(arcana, _)|
	{
		arcanaOptions.push(arcana.as_ref().to_string());
		arcanaSelected.insert(arcana.as_ref().to_string(), generateSelectedValue::<String>(arcana.as_ref().to_string(), highestArcanum.get().to_string()).clone());
	});
	
	let mut castingMethodOptions = vec![];
	let mut castingMethodSelected = HashMap::<String, String>::new();
	for scm in SpellCastingMethod::iter()
	{
		castingMethodOptions.push(scm.as_ref().to_string());
		castingMethodSelected.insert(scm.as_ref().to_string(), generateSelectedValue::<String>(scm.as_ref().to_string(), castingMethod.get().to_string()).clone());
	}
	
	let mut factorTypeOptions = vec![];
	let mut castingDurationSelected = HashMap::new();
	let mut castingPotencySelected = HashMap::new();
	let mut castingRangeSelected = HashMap::new();
	let mut castingScaleSelected = HashMap::new();
	let mut castingTimeSelected = HashMap::new();
	for sft in SpellFactorType::iter()
	{
		factorTypeOptions.push(sft.as_ref().to_string());
		castingDurationSelected.insert(sft.as_ref().to_string(), generateSelectedValue::<String>(sft.as_ref().to_string(), castingDuration.get().to_string()).clone());
		castingPotencySelected.insert(sft.as_ref().to_string(), generateSelectedValue::<String>(sft.as_ref().to_string(), castingPotency.get().to_string()).clone());
		castingRangeSelected.insert(sft.as_ref().to_string(), generateSelectedValue::<String>(sft.as_ref().to_string(), castingRange.get().to_string()).clone());
		castingScaleSelected.insert(sft.as_ref().to_string(), generateSelectedValue::<String>(sft.as_ref().to_string(), castingScale.get().to_string()).clone());
		castingTimeSelected.insert(sft.as_ref().to_string(), generateSelectedValue::<String>(sft.as_ref().to_string(), castingTime.get().to_string()).clone());
	}
	
	let mut autoReach = 0;
	if castingDuration.get().to_string() == SpellFactorType::Advanced.as_ref().to_string()
	{
		autoReach += 1;
	}
	if castingPotency.get().to_string() == SpellFactorType::Advanced.as_ref().to_string()
	{
		autoReach += 1;
	}
	if castingRange.get().to_string() == SpellFactorType::Advanced.as_ref().to_string()
	{
		autoReach += 1;
	}
	if castingScale.get().to_string() == SpellFactorType::Advanced.as_ref().to_string()
	{
		autoReach += 1;
	}
	if castingTime.get().to_string() == SpellFactorType::Advanced.as_ref().to_string()
	{
		autoReach += 1;
	}
	
	if totalReaches.get() < &autoReach
	{
		totalReaches.set(autoReach);
	}
	
	let currentHighestArcanum = highestArcanumDots.get();
	let currentTotalDuration = totalDuration.get();
	let currentTotalPotency = totalPotency.get();
	let currentTotalReaches = totalReaches.get();
	let currentTotalScale = totalScale.get();
	
	let yantraOptions = SpellYantras::asStringVec();
	let mut yantraSelected = HashMap::<String, String>::new();
	let mut currentlyUsedYantras = vec![];
	usedYantras.get().iter().for_each(|yantra| currentlyUsedYantras.push(yantra.clone()));
	
	yantraOptions.iter().for_each(|yo|
	{
		match currentlyUsedYantras.iter().filter(|used| used.to_string() == yo.to_string()).next()
		{
			Some(_) => yantraSelected.insert(yo.to_string(), "true".to_string()),
			None => yantraSelected.insert(yo.to_string(), "false".to_string()),
		};
	});
	
	let mut canCastSpell = false;
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
		
		spellcastingDicePool = calculateDicePool(castingMethod.get().clone(), gnosis, characterArcanum, *currentTotalPotency, *currentTotalDuration, *currentTotalScale, currentlyUsedYantras.clone());
		
		let (reach, paradox) = calculateReachAndParadoxPool(castingMethod.get().clone(), gnosis, characterArcanum, *currentHighestArcanum, *currentTotalReaches);
		spellcastingFreeReach = reach;
		spellcastingParadoxDicePool = paradox;
		
		canCastSpell = currentHighestArcanum <= &characterArcanum && spellcastingDicePool > -6;
	}
	
	let chanceDie = spellcastingDicePool <= 0 && spellcastingDicePool > -6;
	
	return cx.render(rsx!
	{
		div
		{
			class: "column justEven spellcasting {className}",
			
			div { class: "label", "Spellcasting Calculations" },
			
			div
			{
				class: "row justEven",
				
				div
				{
					class: "column justEven",
					
					div
					{
						class: "row justStart",
						select
						{
							class: "arcanum",
							onchange: move |e| { e.cancel_bubble(); highestArcanum.set(e.value.to_string()); },
							oncontextmenu: move |e| e.cancel_bubble(),
							prevent_default: "oncontextmenu",
							
							option { value: "", "Select Arcanum" }
							arcanaOptions.iter().enumerate().map(|(i, name)|
							{
								let selected = &arcanaSelected[name];
								rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
							})
						}
					}
					
					div { class: "row justStart", div { class: "label", "Casting Method" } }
					div { class: "row justStart", div { class: "label", "Total Number of Reaches:" } }
					div { class: "row justStart", div { class: "label", "Casting Time" } }
					div { class: "row justStart", div { class: "label", "Casting Range" } }
				}
				
				div
				{
					class: "column justEven",
					
					Dots { max: 5, value: *currentHighestArcanum, handler: highestArcanumDotHandler, handlerKey: highestArcanumDots }
					
					select
					{
						onchange: move |e| { e.cancel_bubble(); castingMethod.set(e.value.to_string()); },
						oncontextmenu: move |e| e.cancel_bubble(),
						prevent_default: "oncontextmenu",
						
						castingMethodOptions.iter().enumerate().map(|(i, name)|
						{
							let selected = &castingMethodSelected[name];
							rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
						})
					}
					
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
						value: "{currentTotalReaches}"
					}
					
					select
					{
						onchange: move |e| { e.cancel_bubble(); castingTime.set(e.value.to_string()); },
						oncontextmenu: move |e| e.cancel_bubble(),
						prevent_default: "oncontextmenu",
						
						factorTypeOptions.iter().enumerate().map(|(i, name)|
						{
							let selected = &castingTimeSelected[name];
							rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
						})
					}
					
					select
					{
						onchange: move |e| { e.cancel_bubble(); castingRange.set(e.value.to_string()); },
						oncontextmenu: move |e| e.cancel_bubble(),
						prevent_default: "oncontextmenu",
						
						factorTypeOptions.iter().enumerate().map(|(i, name)|
						{
							let selected = &castingRangeSelected[name];
							rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
						})
					}
				}
				
				div
				{
					class: "column justEven",
					
					div
					{
						class: "row justEven",
						
						div
						{
							class: "column justEven",
							
							div
							{
								class: "row justStart",
								
								div { class: "label", "Casting Potency" }
							}
							
							div
							{
								class: "row justStart",
								
								div { class: "label", "Casting Duration" }
							}
							
							div
							{
								class: "row justStart",
								
								div { class: "label", "Casting Scale" }
							}
						}
						
						div
						{
							class: "column justEven",
								
							select
							{
								onchange: move |e| { e.cancel_bubble(); castingPotency.set(e.value.to_string()); },
								oncontextmenu: move |e| e.cancel_bubble(),
								prevent_default: "oncontextmenu",
								
								factorTypeOptions.iter().enumerate().map(|(i, name)|
								{
									let selected = &castingPotencySelected[name];
									rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
								})
							}
							
							input
							{
								r#type: "text",
								onchange: move |e|
								{
									e.cancel_bubble();
									match atoi::<usize>(e.value.as_bytes())
									{
										Some(potency) => totalPotency.set(potency),
										None => totalPotency.set(*currentTotalPotency),
									}
								},
								value: "{currentTotalPotency}"
							}
							
							select
							{
								onchange: move |e| { e.cancel_bubble(); castingDuration.set(e.value.to_string()); },
								oncontextmenu: move |e| e.cancel_bubble(),
								prevent_default: "oncontextmenu",
								
								factorTypeOptions.iter().enumerate().map(|(i, name)|
								{
									let selected = &castingDurationSelected[name];
									rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
								})
							}
							
							input
							{
								r#type: "text",
								onchange: move |e|
								{
									e.cancel_bubble();
									match atoi::<usize>(e.value.as_bytes())
									{
										Some(duration) => totalDuration.set(duration),
										None => totalDuration.set(*currentTotalDuration),
									}
								},
								value: "{currentTotalDuration}"
							}
							
							select
							{
								onchange: move |e| { e.cancel_bubble(); castingScale.set(e.value.to_string()); },
								oncontextmenu: move |e| e.cancel_bubble(),
								prevent_default: "oncontextmenu",
								
								factorTypeOptions.iter().enumerate().map(|(i, name)|
								{
									let selected = &castingScaleSelected[name];
									rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
								})
							}
							
							input
							{
								r#type: "text",
								onchange: move |e|
								{
									e.cancel_bubble();
									match atoi::<usize>(e.value.as_bytes())
									{
										Some(scale) => totalScale.set(scale),
										None => totalScale.set(*currentTotalScale),
									}
								},
								value: "{currentTotalScale}"
							}
						}
					}
					
					div { class: "label", "Yantras" }
					
					select
					{
						class: "yantras",
						onchange: move |e| { e.cancel_bubble(); if let None = currentlyUsedYantras.iter().filter(|inside| inside.to_string() == e.value.to_string()).next() { currentlyUsedYantras.push(e.value.to_string()); } usedYantras.set(currentlyUsedYantras.clone()); },
						oncontextmenu: move |e| e.cancel_bubble(),
						prevent_default: "oncontextmenu",
						
						option { value: "", "Choose a Yantra" }
						yantraOptions.iter().enumerate().map(|(i, name)|
						{
							let selected = &yantraSelected[name];
							rsx!(cx, option { key: "{i}", value: "{name}", selected: "{selected}", "{name}" })
						})
					}
				}
				
				div
				{
					class: "column justEven results",
					
					div { class: "label", "Casting Time:" }
					div { class: "label", "Dice Pool:" }
					div { class: "label", "Paradox Dice Pool:" }
					div { class: "label", "Free Reach:" }
					div { class: "label", "Mana Cost:" }
					div { class: "label", "Willpower Cost:" }
				}
				
				div
				{
					class: "column justEven results",
					
					(!canCastSpell).then(|| rsx!(
						div { class: "castTime", "-" }
						div { class: "dicePool", "-" }
						div { class: "paradoxDice", "-" }
						div { class: "freeReach", "-" }
						div { class: "manaCost", "-" }
						div { class: "willpowerCost", "-" }
					))
					canCastSpell.then(|| rsx!(
						div { class: "castTime", "1" }
						(!chanceDie).then(|| rsx!(div { class: "dicePool", "{spellcastingDicePool}" }))
						chanceDie.then(|| rsx!(div { class: "dicePool", "Chance Die" }))
						div { class: "paradoxDice", "{spellcastingParadoxDicePool}" }
						div { class: "freeReach", "{spellcastingFreeReach}" }
						div { class: "manaCost", "1" }
						div { class: "willpowerCost", "0" }
					))
				}
			}
		}
	});
}

fn highestArcanumDotHandler<'a>(cx: &Scope<DotsProps<&'a UseState<usize>>>, clickedValue: usize)
{
	if let Some(highestArcanumDots) = &cx.props.handlerKey
	{
		highestArcanumDots.set(clickedValue);
	}
}

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
fn calculateDicePool(castingMethod:String, characterGnosis: usize, characterLevel: usize, potency: usize, duration: usize, scale: usize, yantras: Vec<String>) -> isize
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
	let iPotency: isize = match potency.try_into()
	{
		Ok(num) => num,
		Err(_) => 0,
	};
	stepPenalty -= changeFactorStepToDice(iPotency);
	
	let iDuration: isize = match duration.try_into()
	{
		Ok(num) => num,
		Err(_) => 0,
	};
	stepPenalty -= changeFactorStepToDice(iDuration);
	
	let iScale: isize = match scale.try_into()
	{
		Ok(num) => num,
		Err(_) => 0,
	};
	stepPenalty -= changeFactorStepToDice(iScale);
	
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
				match yantra
				{
					SpellYantras::Concentration => yantraBonus += 2,
					SpellYantras::Demesne => yantraBonus += 2,
					SpellYantras::Environment => yantraBonus += 1,
					// Only applies to Rotes
					SpellYantras::Mudra => { if castingMethod.clone() == SpellCastingMethod::Rote.as_ref().to_string() { yantraBonus += 1 } },
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

/// Calculate the dice penalty for the number of steps in a particular Spell Factor.
/// 
/// Each step beyond the first is worth 2 dice. The first step is free.
/// There are no negative steps.
fn changeFactorStepToDice(steps: isize) -> isize
{
	return (steps - 1) * 2;
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
