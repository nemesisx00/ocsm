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
				MageArcana,
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
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let arcanaRef = use_atom_ref(&cx, MageArcana);
	let aspirationsRef = use_atom_ref(&cx, CharacterAspirations);
	let conditionsRef = use_atom_ref(&cx, CharacterConditions);
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
					entryUpdateHandler: aspirationUpdateHandler,
					entryRemoveHandler: aspirationRemoveClickHandler,
				}
				
				div
				{
					class: "column justStart",
					
					SimpleEntryList
					{
						class: "conditions".to_string(),
						data: conditions.clone(),
						label: "Conditions".to_string(),
						entryUpdateHandler: conditionUpdateHandler,
						entryRemoveHandler: conditionRemoveClickHandler,
					}
					
					SimpleEntryList
					{
						class: "obsessions".to_string(),
						data: obsessions.clone(),
						label: "Obsessions".to_string(),
						entryUpdateHandler: obsessionUpdateHandler,
						entryRemoveHandler: obsessionRemoveClickHandler,
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
					entryRemoveHandler: arcanaRemoveClickHandler,
					entryUpdateHandler: arcanasUpdateHandler,
				}
				
				DotEntryList
				{
					class: "merits".to_string(),
					data: merits.clone(),
					label: "Merits".to_string(),
					entryDotHandler: meritDotHandler,
					entryRemoveHandler: meritRemoveClickHandler,
					entryUpdateHandler: meritUpdateHandler,
				}
				
				SimpleEntryList
				{
					class: "specialties".to_string(),
					data: specialties.clone(),
					label: "Specialties".to_string(),
					entryUpdateHandler: skillSpecialtyUpdateHandler,
					entryRemoveHandler: skillSpecialtyRemoveClickHandler,
				}
			}
			/*
			hr { class: "row justEven" }
			
			div
			{
				class: "row justBetween",
				
				ActiveAbilities
				{
					class: "contracts".to_string(),
					data: contracts.clone(),
					label: "Contracts".to_string(),
					entryRemoveHandler: contractsRemoveClickHandler,
					entryUpdateHandler: contractsUpdateHandler,
				}
			}
			*/
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

/*
/// Event handler triggered by clicking the "Remove" button after
/// right-clicking a Contract row.
fn contractsRemoveClickHandler<T>(cx: &Scope<T>, index: usize)
{
	let contractsRef = use_atom_ref(&cx, ChangelingContracts);
	let mut contracts = contractsRef.write();
	
	if index < contracts.len()
	{
		contracts.remove(index);
	}
}

/// Event handler triggered when a Contract's value changes.
fn contractsUpdateHandler<T>(e: FormEvent, cx: &Scope<T>, index: Option<usize>, field: ActiveAbilityField)
{
	let contractsRef = use_atom_ref(&cx, ChangelingContracts);
	let mut contracts = contractsRef.write();
	
	match index
	{
		Some(i) =>
		{
			match field
			{
				ActiveAbilityField::Action => { contracts[i].action = e.value.clone(); }
				ActiveAbilityField::Cost => { contracts[i].cost = e.value.clone(); }
				ActiveAbilityField::Description => { contracts[i].description = e.value.clone(); }
				ActiveAbilityField::DicePool => { contracts[i].dicePool = e.value.clone(); }
				ActiveAbilityField::Duration => { contracts[i].duration = e.value.clone(); }
				ActiveAbilityField::Effects => { contracts[i].effects = e.value.clone(); }
				ActiveAbilityField::Name => { contracts[i].name = e.value.clone(); }
				ActiveAbilityField::Requirements => { contracts[i].requirements = e.value.clone(); }
			}
		}
		None =>
		{
			match field
			{
				ActiveAbilityField::Action => { contracts.push(ActiveAbility { action: e.value.clone(), ..Default::default() }); }
				ActiveAbilityField::Cost => { contracts.push(ActiveAbility { cost: e.value.clone(), ..Default::default() }); }
				ActiveAbilityField::Description => { contracts.push(ActiveAbility { description: e.value.clone(), ..Default::default() }); }
				ActiveAbilityField::DicePool => { contracts.push(ActiveAbility { dicePool: e.value.clone(), ..Default::default() }); }
				ActiveAbilityField::Duration => { contracts.push(ActiveAbility { duration: e.value.clone(), ..Default::default() }); }
				ActiveAbilityField::Effects => { contracts.push(ActiveAbility { effects: e.value.clone(), ..Default::default() }); }
				ActiveAbilityField::Name => { contracts.push(ActiveAbility { name: e.value.clone(), ..Default::default() }); }
				ActiveAbilityField::Requirements => { contracts.push(ActiveAbility { requirements: e.value.clone(), ..Default::default() }); }
			}
		}
	}
}
*/

fn templateBonusesHandler(_cx: &Scope<AdvantagesProps>)
{
}

/// Event handler triggered by clicking the "Remove" button after
/// right-clicking a Touchstone row.
fn obsessionRemoveClickHandler<T>(cx: &Scope<T>, index: usize)
{
	let obsessionsRef = use_atom_ref(&cx, MageObsessions);
	let mut obsessions = obsessionsRef.write();
	
	if index < obsessions.len()
	{
		obsessions.remove(index);
	}
}

/// Event handler triggered when a Touchstone's value changes.
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
