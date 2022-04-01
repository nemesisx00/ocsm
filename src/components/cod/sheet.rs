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
			CharacterMerits,
			CharacterSpecialties,
		},
	},
	components::{
		cod::{
			advantages::Advantages,
			details::Details,
			dots::DotsProps,
			experience::Experience,
			list::{
				DotEntryList,
				SimpleEntryList,
			},
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
	let meritsRef = use_atom_ref(&cx, CharacterMerits);
	let specialtiesRef = use_atom_ref(&cx, CharacterSpecialties);
	
	let mut aspirations = vec![];
	aspirationsRef.read().iter().for_each(|a| aspirations.push(a.clone()));
	let mut conditions = vec![];
	conditionsRef.read().iter().for_each(|c| conditions.push(c.clone()));
	let mut merits = vec![];
	meritsRef.read().iter().for_each(|m| merits.push(m.clone()));
	let mut specialties = vec![];
	specialtiesRef.read().iter().for_each(|s| specialties.push(s.clone()));
	
	return cx.render(rsx!
	{	
		div
		{
			class: "sheet cod core column justEven",
			
			h1 { "Chronicles of Darkness" }
			hr { class: "row justEven" }
			
			div
			{
				class: "row justEven",
				
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
			
			hr { class: "row justEven" }
			div { class: "row justEven", Attributes {} }
			hr { class: "row justEven" }
			div { class: "row justEven", Skills {} }
			hr { class: "row justEven" }
			
			div
			{
				class: "row justEven",
				
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
		}
	});
}

/// Event handler triggered by clicking the "Remove" button after
/// right-clicking a Aspiration row.
pub fn aspirationRemoveClickHandler<T>(cx: &Scope<T>, index: usize)
{
	let aspirationsRef = use_atom_ref(&cx, CharacterAspirations);
	let mut aspirations = aspirationsRef.write();
	
	if index < aspirations.len()
	{
		aspirations.remove(index);
	}
}

/// Event handler triggered when a Aspiration's value changes.
pub fn aspirationUpdateHandler<T>(e: FormEvent, cx: &Scope<T>, index: Option<usize>)
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
pub fn conditionRemoveClickHandler<T>(cx: &Scope<T>, index: usize)
{
	let conditionsRef = use_atom_ref(&cx, CharacterConditions);
	let mut conditions = conditionsRef.write();
	
	if index < conditions.len()
	{
		conditions.remove(index);
	}
}

/// Event handler triggered when a Condition's value changes.
pub fn conditionUpdateHandler<T>(e: FormEvent, cx: &Scope<T>, index: Option<usize>)
{
	let conditionsRef = use_atom_ref(&cx, CharacterConditions);
	let mut conditions = conditionsRef.write();
	
	match index
	{
		Some(i) => { conditions[i] = e.value.to_string(); }
		None => { conditions.push(e.value.to_string()); }
	}
}

/// Event handler triggered when a `Merit`'s `Dots` is clicked.
pub fn meritDotHandler(cx: &Scope<DotsProps<usize>>, clickedValue: usize)
{
	let meritsRef = use_atom_ref(&cx, CharacterMerits);
	let mut merits = meritsRef.write();
	
	if let Some(index) = &cx.props.handlerKey
	{
		let (_, ref mut value) = merits[*index];
		*value = clickedValue;
	}
}

/// Event handler triggered by clicking the "Remove" button after right-clicking a Merit row.
pub fn meritRemoveClickHandler<T>(cx: &Scope<T>, index: usize)
{
	let meritsRef = use_atom_ref(&cx, CharacterMerits);
	let mut merits = meritsRef.write();
	
	if index < merits.len()
	{
		merits.remove(index);
	}
}

/// Event handler triggered when the `Merit` label input's value changes.
pub fn meritUpdateHandler<T>(e: FormEvent, cx: &Scope<T>, index: Option<usize>)
{
	let meritsRef = use_atom_ref(&cx, CharacterMerits);
	let mut merits = meritsRef.write();
	
	match index
	{
		Some(i) =>
		{
			let (ref mut name, _) = merits[i];
			*name = e.value.clone();
		},
		None => merits.push((e.value.clone(), 0))
	}
}

/// Event handler triggered by clicking the "Remove" button after
/// right-clicking a Skill Specialty row.
pub fn skillSpecialtyRemoveClickHandler<T>(cx: &Scope<T>, index: usize)
{
	let skillSpecialtiesRef = use_atom_ref(&cx, CharacterSpecialties);
	let mut skillSpecialties = skillSpecialtiesRef.write();
	
	if index < skillSpecialties.len()
	{
		skillSpecialties.remove(index);
	}
}

/// Event handler triggered when a Skill Specialty's value changes.
pub fn skillSpecialtyUpdateHandler<T>(e: FormEvent, cx: &Scope<T>, index: Option<usize>)
{
	let skillSpecialtiesRef = use_atom_ref(&cx, CharacterSpecialties);
	let mut skillSpecialties = skillSpecialtiesRef.write();
	
	match index
	{
		Some(i) => { skillSpecialties[i] = e.value.to_string(); }
		None => { skillSpecialties.push(e.value.to_string()); }
	}
}
