#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	events::FormEvent,
	prelude::*
};
use strum::IntoEnumIterator;
use crate::{
	cod::{
		mta2e::{
			enums::{
				Arcana,
				Path,
			},
			state::{
				MageActiveSpells,
				MageArcana,
				MageDedicatedTools,
				MageObsessions,
			},
		},
		state::{
			CharacterAdvantages,
			CharacterAspirations,
			CharacterConditions,
			CharacterMerits,
			CharacterSpecialties,
			getTraitMax,
		},
	},
	components::{
		cod::{
			advantages::{
				Advantages,
				AdvantagesProps,
			},
			details::Details,
			dots::DotsProps,
			experience::Experience,
			list::{
				DotEntryList,
				SimpleEntryList,
			},
			mta2e::{
				casting::Spellcasting,
				spells::{
					Praxes,
					Rotes,
				},
			},
			sheet::{
				aspirationRemoveClickHandler,
				aspirationUpdateHandler,
				conditionRemoveClickHandler,
				conditionUpdateHandler,
				meritDotHandler,
				meritRemoveClickHandler,
				meritUpdateHandler,
				skillSpecialtyRemoveClickHandler,
				skillSpecialtyUpdateHandler,
			},
			traits::{
				Attributes,
				Skills,
			},
		},
	},
};

/// The UI Component defining the layout of a Mage: The Awakening 2e Mage's character sheet.
pub fn MageSheet(cx: Scope) -> Element
{
	let activeSpellsRef = use_atom_ref(&cx, MageActiveSpells);
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let arcanaRef = use_atom_ref(&cx, MageArcana);
	let aspirationsRef = use_atom_ref(&cx, CharacterAspirations);
	let conditionsRef = use_atom_ref(&cx, CharacterConditions);
	let dedicatedToolsRef = use_atom_ref(&cx, MageDedicatedTools);
	let meritsRef = use_atom_ref(&cx, CharacterMerits);
	let obsessionsRef = use_atom_ref(&cx, MageObsessions);
	let specialtiesRef = use_atom_ref(&cx, CharacterSpecialties);
	let traitMax = match advantagesRef.read().power
	{
		Some(p) => getTraitMax(p),
		None => 5
	};
	
	let mut paths = Vec::<String>::new();
	for p in Path::iter()
	{
		paths.push(p.as_ref().to_string());
	}
	
	let mut activeSpells = vec![];
	activeSpellsRef.read().iter().for_each(|s| activeSpells.push(s.clone()));
	let mut arcana = vec![];
	arcanaRef.read().iter().for_each(|(a, v)| arcana.push((a.as_ref().to_string(), *v)));
	let mut arcanaSelectOptions = vec![];
	for a in Arcana::iter()
	{
		arcanaSelectOptions.push(a.as_ref().to_string());
	}
	let mut aspirations = vec![];
	aspirationsRef.read().iter().for_each(|a| aspirations.push(a.clone()));
	let mut conditions = vec![];
	conditionsRef.read().iter().for_each(|c| conditions.push(c.clone()));
	let mut dedicatedTools = vec![];
	dedicatedToolsRef.read().iter().for_each(|dt| dedicatedTools.push(dt.clone()));
	let mut merits = vec![];
	meritsRef.read().iter().for_each(|m| merits.push(m.clone()));
	let mut specialties = vec![];
	specialtiesRef.read().iter().for_each(|s| specialties.push(s.clone()));
	let mut obsessions = vec![];
	obsessionsRef.read().iter().for_each(|t| obsessions.push(t.clone()));
	
	return cx.render(rsx!
	{	
		div
		{
			class: "sheet cod mta2e column justEven",
			
			h1 { "Mage: The Awakening" }
			h3 { "Second Edition" }
			hr { class: "row justEven" }
			
			div
			{
				class: "row justEven",
				
				Details
				{
					virtue: "Virtue".to_string(),
					vice: "Vice".to_string(),
					typePrimary: "Path".to_string(),
					typePrimaryOptions: paths.clone(),
					typeSecondary: "Legacy".to_string(),
					faction: "Order".to_string()
				}
				
				Advantages
				{
					integrity: "Wisdom".to_string(),
					power: "Gnosis".to_string(),
					resource: "Mana".to_string(),
					handleTemplateBonuses: templateBonusesHandler
				}
			}
			
			hr { class: "row justEven" }
			
			div
			{
				class: "row justEven spacedOut",
				
				SimpleEntryList
				{
					class: "aspirations".to_string(),
					data: aspirations.clone(),
					label: "Aspirations".to_string(),
					entryRemoveHandler: aspirationRemoveClickHandler,
					entryUpdateHandler: aspirationUpdateHandler,
				}
				
				div
				{
					class: "column justStart",
					
					SimpleEntryList
					{
						class: "conditions".to_string(),
						data: conditions.clone(),
						label: "Conditions".to_string(),
						entryRemoveHandler: conditionRemoveClickHandler,
						entryUpdateHandler: conditionUpdateHandler,
					}
					
					SimpleEntryList
					{
						class: "obsessions".to_string(),
						data: obsessions.clone(),
						label: "Obsessions".to_string(),
						entryRemoveHandler: obsessionRemoveClickHandler,
						entryUpdateHandler: obsessionUpdateHandler,
					}
				}
				
				Experience {}
			}
			
			hr { class: "row justEven" }
			div { class: "row justEven", Attributes { traitMax: traitMax } }
			hr { class: "row justEven" }
			div { class: "row justEven", Skills { traitMax: traitMax } }
			hr { class: "row justEven" }
			
			div
			{
				class: "row justEven",
				
				DotEntryList
				{
					class: "arcana".to_string(),
					data: arcana.clone(),
					label: "Arcana".to_string(),
					selectOptions: arcanaSelectOptions.clone(),
					entryDotHandler: arcanaDotHandler,
					entryUpdateHandler: arcanasUpdateHandler,
					entryRemoveHandler: arcanaRemoveClickHandler,
				}
				
				DotEntryList
				{
					class: "merits".to_string(),
					data: merits.clone(),
					label: "Merits".to_string(),
					entryDotHandler: meritDotHandler,
					entryUpdateHandler: meritUpdateHandler,
					entryRemoveHandler: meritRemoveClickHandler,
				}
				
				SimpleEntryList
				{
					class: "specialties".to_string(),
					data: specialties.clone(),
					label: "Specialties".to_string(),
					entryRemoveHandler: skillSpecialtyRemoveClickHandler,
					entryUpdateHandler: skillSpecialtyUpdateHandler,
				}
			}
			
			hr { class: "row justEven" }
			
			div
			{
				class: "row justEven",
			
				SimpleEntryList
				{
					class: "activeSpells".to_string(),
					data: activeSpells.clone(),
					label: "Active Spells".to_string(),
					entryRemoveHandler: activeSpellsRemoveClickHandler,
					entryUpdateHandler: activeSpellsUpdateHandler,
				}
				
				SimpleEntryList
				{
					class: "dedicatedTools".to_string(),
					data: dedicatedTools.clone(),
					label: "Dedicated Tools".to_string(),
					entryRemoveHandler: dedicatedToolsRemoveClickHandler,
					entryUpdateHandler: dedicatedToolsUpdateHandler,
				}
				
				Praxes {}
			}
			
			hr { class: "row justEven" }
			div { class: "row justEven", Rotes {} }
			hr { class: "row justEven" }
			div { class: "row justEven", Spellcasting {} }
			hr { class: "row justEven" }
			
			div
			{
				class: "row justEven",
				
				
			}
		}
	});
}

/// Event handler triggered when a Discipline value changes.
fn arcanaDotHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	let arcanasRef = use_atom_ref(&cx, MageArcana);
	let mut arcanas = arcanasRef.write();
	
	if let Some(i) = &cx.props.handlerKey
	{
		let (_, ref mut value) = arcanas[*i];
		*value = clickedValue;
	}
}

/// Event handler triggered by clicking the "Remove" button after right-clicking a
/// Discipline row.
fn arcanaRemoveClickHandler<T>(cx: &Scope<T>, index: usize)
{
	let arcanasRef = use_atom_ref(&cx, MageArcana);
	let mut arcanas = arcanasRef.write();
	
	if let Some((i, (_, _))) = arcanas.clone().iter().enumerate().filter(|(i, (_, _))| *i == index).next()
	{
		arcanas.remove(i);
	}
}

/// Event handler triggered when the New Discipline select input's value changes.
fn arcanasUpdateHandler<T>(e: FormEvent, cx: &Scope<T>, _index: Option<usize>)
{
	let arcanasRef = use_atom_ref(&cx, MageArcana);
	let mut arcanas = arcanasRef.write();
	
	if let Some((d, _)) = Arcana::asMap().iter().filter(|(_, name)| *name == &e.value.to_string()).next()
	{
		if let None = arcanas.iter().filter(|(arcana, _)| arcana == d).next()
		{
			arcanas.push((*d, 0));
		}
	}
}

fn templateBonusesHandler(_cx: &Scope<AdvantagesProps>)
{
}

/// Event handler triggered by clicking the "Remove" button after
/// right-clicking an Active Spell row.
fn activeSpellsRemoveClickHandler<T>(cx: &Scope<T>, index: usize)
{
	let activeSpellsRef = use_atom_ref(&cx, MageActiveSpells);
	let mut activeSpells = activeSpellsRef.write();
	
	if index < activeSpells.len()
	{
		activeSpells.remove(index);
	}
}

/// Event handler triggered when an Active Spell's value changes.
fn activeSpellsUpdateHandler<T>(e: FormEvent, cx: &Scope<T>, index: Option<usize>)
{
	let activeSpellsRef = use_atom_ref(&cx, MageActiveSpells);
	let mut activeSpells = activeSpellsRef.write();
	
	match index
	{
		Some(i) => { activeSpells[i] = e.value.to_string(); }
		None => { activeSpells.push(e.value.to_string()); }
	}
}

/// Event handler triggered by clicking the "Remove" button after
/// right-clicking a Dedicated Magical Tool row.
fn dedicatedToolsRemoveClickHandler<T>(cx: &Scope<T>, index: usize)
{
	let dedicatedToolsRef = use_atom_ref(&cx, MageDedicatedTools);
	let mut dedicatedTools = dedicatedToolsRef.write();
	
	if index < dedicatedTools.len()
	{
		dedicatedTools.remove(index);
	}
}

/// Event handler triggered when a Dedicated Magical Tool's value changes.
fn dedicatedToolsUpdateHandler<T>(e: FormEvent, cx: &Scope<T>, index: Option<usize>)
{
	let dedicatedToolsRef = use_atom_ref(&cx, MageDedicatedTools);
	let mut dedicatedTools = dedicatedToolsRef.write();
	
	match index
	{
		Some(i) => { dedicatedTools[i] = e.value.to_string(); }
		None => { dedicatedTools.push(e.value.to_string()); }
	}
}

/// Event handler triggered by clicking the "Remove" button after
/// right-clicking a Obsession row.
fn obsessionRemoveClickHandler<T>(cx: &Scope<T>, index: usize)
{
	let obsessionsRef = use_atom_ref(&cx, MageObsessions);
	let mut obsessions = obsessionsRef.write();
	
	if index < obsessions.len()
	{
		obsessions.remove(index);
	}
}

/// Event handler triggered when a Obsession's value changes.
fn obsessionUpdateHandler<T>(e: FormEvent, cx: &Scope<T>, index: Option<usize>)
{
	let obsessionsRef = use_atom_ref(&cx, MageObsessions);
	let mut obsessions = obsessionsRef.write();
	
	match index
	{
		Some(i) => { obsessions[i] = e.value.to_string(); }
		None => { obsessions.push(e.value.to_string()); }
	}
}
