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
			CharacterAttributes,
			CharacterDetails,
			getTraitMax,
		},
	},
	components::{
		cod::{
			ctl2e::{
				frailties::Frailties,
				regalia::{
					Contracts,
					FavoredRegalia,
				},
			},
			advantages::{
				Advantages,
				AdvantagesProps,
			},
			aspirations::Aspirations,
			details::Details,
			experience::Experience,
			list::SimpleEntryList,
			merits::Merits,
			traits::{
				Attributes,
				Skills,
				SkillSpecialties,
			},
		},
	},
};

/// The UI Component defining the layout of a Changeling: The Lost 2e Changeling's character sheet.
pub fn ChangelingSheet(cx: Scope) -> Element
{
	let advantages = use_atom_ref(&cx, CharacterAdvantages);
	let touchstonesRef = use_atom_ref(&cx, ChangelingTouchstones);
	let traitMax = getTraitMax(advantages.read().power.unwrap());
	
	let mut seemings = Vec::<String>::new();
	for s in Seeming::iter()
	{
		seemings.push(s.as_ref().to_string());
	}
	
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
				Aspirations {}
				div
				{
					class: "column",
					SimpleEntryList
					{
						data: touchstones.clone(),
						label: "Touchstone".to_string(),
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
			div { class: "row", Frailties {} SkillSpecialties {} Merits {} }
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
