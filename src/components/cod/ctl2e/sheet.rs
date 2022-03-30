#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	events::FormEvent,
	prelude::*
};
use strum::IntoEnumIterator;
use crate::{
	cod::{
		ctl2e::{
			enums::Seeming,
			state::{
				BeastBonus,
				ChangelingFrailties,
				ChangelingTouchstones,
			},
		},
		enums::{
			CoreAttribute,
			CoreDetail,
		},
		state::{
			BaseSpeed,
			CharacterAdvantages,
			CharacterAspirations,
			CharacterAttributes,
			CharacterConditions,
			CharacterDetails,
			CharacterMerits,
			CharacterSpecialties,
			getTraitMax,
		},
	},
	components::{
		cod::{
			ctl2e::{
				regalia::{
					Contracts,
					FavoredRegalia,
				},
			},
			advantages::{
				Advantages,
				AdvantagesProps,
			},
			details::Details,
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

/// The UI Component defining the layout of a Changeling: The Lost 2e Changeling's character sheet.
pub fn ChangelingSheet(cx: Scope) -> Element
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let aspirationsRef = use_atom_ref(&cx, CharacterAspirations);
	let conditionsRef = use_atom_ref(&cx, CharacterConditions);
	let frailtiesRef = use_atom_ref(&cx, ChangelingFrailties);
	let meritsRef = use_atom_ref(&cx, CharacterMerits);
	let specialtiesRef = use_atom_ref(&cx, CharacterSpecialties);
	let touchstonesRef = use_atom_ref(&cx, ChangelingTouchstones);
	let traitMax = match advantagesRef.read().power
	{
		Some(p) => getTraitMax(p),
		None => 5
	};
	
	let mut seemings = Vec::<String>::new();
	for s in Seeming::iter()
	{
		seemings.push(s.as_ref().to_string());
	}
	
	let mut aspirations = vec![];
	aspirationsRef.read().iter().for_each(|a| aspirations.push(a.clone()));
	let mut conditions = vec![];
	conditionsRef.read().iter().for_each(|c| conditions.push(c.clone()));
	let mut frailties = vec![];
	frailtiesRef.read().iter().for_each(|f| frailties.push(f.clone()));
	let mut merits = vec![];
	meritsRef.read().iter().for_each(|m| merits.push(m.clone()));
	let mut specialties = vec![];
	specialtiesRef.read().iter().for_each(|s| specialties.push(s.clone()));
	let mut touchstones = vec![];
	touchstonesRef.read().iter().for_each(|t| touchstones.push(t.clone()));
	
	return cx.render(rsx!
	{	
		div
		{
			class: "sheet cod ctl2e column",
			
			h1 { "Changeling: The Lost" }
			h3 { "Second Edition" }
			hr { class: "row" }
			
			div
			{
				class: "row",
				
				Details
				{
					virtue: "Needle".to_string(),
					vice: "Thread".to_string(),
					typePrimary: "Seeming".to_string(),
					typePrimaryOptions: seemings.clone(),
					typeSecondary: "Kith".to_string(),
					faction: "Court".to_string()
				}
				
				Advantages
				{
					integrity: "Clarity".to_string(),
					power: "Wyrd".to_string(),
					resource: "Glamour".to_string(),
					handleTemplateBonuses: templateBonusesHandler
				}
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
				
				div
				{
					class: "column",
					
					SimpleEntryList
					{
						class: "touchstones".to_string(),
						data: touchstones.clone(),
						label: "Touchstones".to_string(),
						entryUpdateHandler: touchstoneUpdateHandler,
						entryRemoveHandler: touchstoneRemoveClickHandler,
					}
					
					FavoredRegalia {}
				}
				
				Experience {}
			}
			
			hr { class: "row" }
			div { class: "row", Attributes { traitMax: traitMax } }
			hr { class: "row" }
			div { class: "row", Skills { traitMax: traitMax } }
			hr { class: "row" }
			
			div
			{
				class: "row",
				
				SimpleEntryList
				{
					class: "frailties".to_string(),
					data: frailties.clone(),
					label: "Frailties".to_string(),
					entryUpdateHandler: frailtyUpdateHandler,
					entryRemoveHandler: frailtyRemoveClickHandler,
				}
				
				div
				{
					class: "column",
					
					SimpleEntryList
					{
						class: "specialties".to_string(),
						data: specialties.clone(),
						label: "Specialties".to_string(),
						entryUpdateHandler: skillSpecialtyUpdateHandler,
						entryRemoveHandler: skillSpecialtyRemoveClickHandler,
					}
					
					SimpleEntryList
					{
						class: "conditions".to_string(),
						data: conditions.clone(),
						label: "Conditions".to_string(),
						entryUpdateHandler: conditionUpdateHandler,
						entryRemoveHandler: conditionRemoveClickHandler,
					}
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
			}
			
			hr { class: "row" }
			div { class: "row", Contracts {} }
		}
	});
}

fn templateBonusesHandler(cx: &Scope<AdvantagesProps>)
{
	let advantagesRef = use_atom_ref(cx, CharacterAdvantages);
	let attributesRef = use_atom_ref(cx, CharacterAttributes);
	let detailsRef = use_atom_ref(cx, CharacterDetails);
	let mut advantages = advantagesRef.write();
	let attributes = attributesRef.read();
	let details = detailsRef.read();
	
	if details[&CoreDetail::TypePrimary] == Seeming::Beast.as_ref().to_string()
	{
		advantages.initiative = attributes[&CoreAttribute::Dexterity] + attributes[&CoreAttribute::Composure] + BeastBonus;
		advantages.speed = BaseSpeed + attributes[&CoreAttribute::Dexterity] + attributes[&CoreAttribute::Strength] + BeastBonus;
	}
	else
	{
		advantages.initiative = attributes[&CoreAttribute::Dexterity] + attributes[&CoreAttribute::Composure];
		advantages.speed = BaseSpeed + attributes[&CoreAttribute::Dexterity] + attributes[&CoreAttribute::Strength];
	}
}

/// Event handler triggered by clicking the "Remove" button after
/// right-clicking a Aspiration row.
fn frailtyRemoveClickHandler<T>(cx: &Scope<T>, index: usize)
{
	let frailtiesRef = use_atom_ref(&cx, ChangelingFrailties);
	let mut frailties = frailtiesRef.write();
	
	if index < frailties.len()
	{
		frailties.remove(index);
	}
}

/// Event handler triggered when a Aspiration's value changes.
fn frailtyUpdateHandler<T>(e: FormEvent, cx: &Scope<T>, index: Option<usize>)
{
	let frailtiesRef = use_atom_ref(&cx, ChangelingFrailties);
	let mut frailties = frailtiesRef.write();
	
	match index
	{
		Some(i) => { frailties[i] = e.value.to_string(); }
		None => { frailties.push(e.value.to_string()); }
	}
}

/// Event handler triggered by clicking the "Remove" button after
/// right-clicking a Touchstone row.
fn touchstoneRemoveClickHandler<T>(cx: &Scope<T>, index: usize)
{
	let touchstonesRef = use_atom_ref(&cx, ChangelingTouchstones);
	let mut touchstones = touchstonesRef.write();
	
	if index < touchstones.len()
	{
		touchstones.remove(index);
	}
}

/// Event handler triggered when a Touchstone's value changes.
fn touchstoneUpdateHandler<T>(e: FormEvent, cx: &Scope<T>, index: Option<usize>)
{
	let touchstonesRef = use_atom_ref(&cx, ChangelingTouchstones);
	let mut touchstones = touchstonesRef.write();
	
	match index
	{
		Some(i) => { touchstones[i] = e.value.to_string(); }
		None => { touchstones.push(e.value.to_string()); }
	}
}
