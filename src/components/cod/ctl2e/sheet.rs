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
				ChangelingContracts,
				ChangelingFrailties,
				ChangelingTouchstones,
			},
		},
		enums::{
			ActiveAbilityField,
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
		structs::ActiveAbility,
	},
	components::{
		cod::{
			ctl2e::regalia::FavoredRegalia,
			advantages::{
				Advantages,
				AdvantagesProps,
			},
			details::Details,
			experience::Experience,
			list::ActiveAbilities,
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
		core::list::{
			DotEntryList,
			SimpleEntryList,
		},
	},
};

/// The UI Component defining the layout of a Changeling: The Lost 2e Changeling's character sheet.
pub fn ChangelingSheet(cx: Scope) -> Element
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let aspirationsRef = use_atom_ref(&cx, CharacterAspirations);
	let conditionsRef = use_atom_ref(&cx, CharacterConditions);
	let contractsRef = use_atom_ref(&cx, ChangelingContracts);
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
	let mut contracts = vec![];
	contractsRef.read().iter().for_each(|c| contracts.push(c.clone()));
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
			class: "sheet cod ctl2e column justEven",
			
			h1 { "Changeling: The Lost" }
			h3 { "Second Edition" }
			hr { class: "row justEven" }
			
			div
			{
				class: "row justEven",
				
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
						class: "touchstones".to_string(),
						data: touchstones.clone(),
						label: "Touchstones".to_string(),
						entryUpdateHandler: touchstoneUpdateHandler,
						entryRemoveHandler: touchstoneRemoveClickHandler,
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
				
				div
				{
					class: "column justStart",
				
					SimpleEntryList
					{
						class: "frailties".to_string(),
						data: frailties.clone(),
						label: "Frailties".to_string(),
						entryUpdateHandler: frailtyUpdateHandler,
						entryRemoveHandler: frailtyRemoveClickHandler,
					}
					
					FavoredRegalia {}
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
		}
	});
}

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
