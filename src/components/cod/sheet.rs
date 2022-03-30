#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	events::FormEvent,
	prelude::*
};
use crate::{
	cod::{
		state::{
			CharacterAspirations,
			CharacterConditions,
			CharacterSpecialties,
		},
	},
	components::{
		cod::{
			advantages::Advantages,
			details::Details,
			experience::Experience,
			list::SimpleEntryList,
			merits::Merits,
			traits::{
				Attributes,
				Skills,
			},
		},
	},
};

/// The UI Component defining the layout of a Chronicles of Darkness Mortal's character sheet.
pub fn MortalSheet(cx: Scope) -> Element
{
	let aspirationsRef = use_atom_ref(&cx, CharacterAspirations);
	let conditionsRef = use_atom_ref(&cx, CharacterConditions);
	let specialtiesRef = use_atom_ref(&cx, CharacterSpecialties);
	
	let mut aspirations = vec![];
	aspirationsRef.read().iter().for_each(|a| aspirations.push(a.clone()));
	let mut conditions = vec![];
	conditionsRef.read().iter().for_each(|c| conditions.push(c.clone()));
	let mut specialties = vec![];
	specialtiesRef.read().iter().for_each(|s| specialties.push(s.clone()));
	
	return cx.render(rsx!
	{	
		div
		{
			class: "sheet cod core column",
			
			h1 { "Chronicles of Darkness" }
			//h3 { }
			hr { class: "row" }
			
			div
			{
				class: "row",
				
				Details
				{
					virtue: "Virtue".to_string(),
					vice: "Vice".to_string(),
					typePrimary: "Age".to_string(),
					typeSecondary: "Group Name".to_string(),
					faction: "Faction".to_string()
				}
				
				Advantages { integrity: "Integrity".to_string() }
			}
			
			hr { class: "row" }
			
			div
			{
				class: "row spacedOut",
				
				SimpleEntryList
				{
					class: "aspirations".to_string(),
					data: aspirations.clone(),
					label: "Aspirations".to_string(),
					entryUpdateHandler: aspirationUpdateHandler,
					entryRemoveHandler: aspirationRemoveClickHandler,
				}
				
				SimpleEntryList
				{
					class: "conditions".to_string(),
					data: conditions.clone(),
					label: "Conditions".to_string(),
					entryUpdateHandler: conditionUpdateHandler,
					entryRemoveHandler: conditionRemoveClickHandler,
				}
				
				Experience {}
			}
			
			hr { class: "row" }
			div { class: "row", Attributes {} }
			hr { class: "row" }
			div { class: "row", Skills {} }
			hr { class: "row" }
			
			div
			{
				class: "row",
				
				SimpleEntryList
				{
					class: "specialties".to_string(),
					data: specialties.clone(),
					label: "Specialties".to_string(),
					entryUpdateHandler: skillSpecialtyUpdateHandler,
					entryRemoveHandler: skillSpecialtyRemoveClickHandler,
				}
				
				Merits {}
			}
		}
	});
}

/// Event handler triggered by clicking the "Remove" button after
/// right-clicking a Aspiration row.
fn aspirationRemoveClickHandler<T>(cx: &Scope<T>, index: usize)
{
	let aspirationsRef = use_atom_ref(&cx, CharacterAspirations);
	let mut aspirations = aspirationsRef.write();
	
	if index < aspirations.len()
	{
		aspirations.remove(index);
	}
}

/// Event handler triggered when a Aspiration's value changes.
fn aspirationUpdateHandler<T>(e: FormEvent, cx: &Scope<T>, index: Option<usize>)
{
	let aspirationsRef = use_atom_ref(&cx, CharacterAspirations);
	let mut aspirations = aspirationsRef.write();
	
	match index
	{
		Some(i) => { aspirations[i] = e.value.to_string(); }
		None => { aspirations.push(e.value.to_string()); }
	}
}

/// Event handler triggered by clicking the "Remove" button after
/// right-clicking a Condition row.
fn conditionRemoveClickHandler<T>(cx: &Scope<T>, index: usize)
{
	let conditionsRef = use_atom_ref(&cx, CharacterConditions);
	let mut conditions = conditionsRef.write();
	
	if index < conditions.len()
	{
		conditions.remove(index);
	}
}

/// Event handler triggered when a Condition's value changes.
fn conditionUpdateHandler<T>(e: FormEvent, cx: &Scope<T>, index: Option<usize>)
{
	let conditionsRef = use_atom_ref(&cx, CharacterConditions);
	let mut conditions = conditionsRef.write();
	
	match index
	{
		Some(i) => { conditions[i] = e.value.to_string(); }
		None => { conditions.push(e.value.to_string()); }
	}
}

/// Event handler triggered by clicking the "Remove" button after
/// right-clicking a Skill Specialty row.
fn skillSpecialtyRemoveClickHandler<T>(cx: &Scope<T>, index: usize)
{
	let skillSpecialtiesRef = use_atom_ref(&cx, CharacterSpecialties);
	let mut skillSpecialties = skillSpecialtiesRef.write();
	
	if index < skillSpecialties.len()
	{
		skillSpecialties.remove(index);
	}
}

/// Event handler triggered when a Skill Specialty's value changes.
fn skillSpecialtyUpdateHandler<T>(e: FormEvent, cx: &Scope<T>, index: Option<usize>)
{
	let skillSpecialtiesRef = use_atom_ref(&cx, CharacterSpecialties);
	let mut skillSpecialties = skillSpecialtiesRef.write();
	
	match index
	{
		Some(i) => { skillSpecialties[i] = e.value.to_string(); }
		None => { skillSpecialties.push(e.value.to_string()); }
	}
}
