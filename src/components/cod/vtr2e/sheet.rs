#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	events::FormEvent,
	prelude::*
};
use strum::IntoEnumIterator;
use crate::{
	cod::{
		enums::{
			ActiveAbilityField,
			CoreAttribute,
			CoreSkill,
		},
		state::{
			CharacterAdvantages,
			CharacterAspirations,
			CharacterAttributes,
			CharacterConditions,
			CharacterMerits,
			CharacterSkills,
			CharacterSpecialties,
			getTraitMax,
		},
		structs::ActiveAbility,
		vtr2e::{
			enums::{
				Clan,
				Discipline,
			},
			state::{
				BP0VitaeMax,
				KindredDisciplines,
				KindredPowers,
				KindredTouchstones,
			},
		},
	},
	components::{
		cod::{
			advantages::{
				Advantages,
				AdvantagesProps,
			},
			details::Details,
			experience::Experience,
			list::{
				ActiveAbilities,
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
			vtr2e::disciplines::Disciplines,
		},
	},
};

/// The UI Component defining the layout of a Vampire: The Requiem 2e Kindred's character sheet.
pub fn VampireSheet(cx: Scope) -> Element
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let aspirationsRef = use_atom_ref(&cx, CharacterAspirations);
	let conditionsRef = use_atom_ref(&cx, CharacterConditions);
	let meritsRef = use_atom_ref(&cx, CharacterMerits);
	let powersRef = use_atom_ref(&cx, KindredPowers);
	let specialtiesRef = use_atom_ref(&cx, CharacterSpecialties);
	let touchstonesRef = use_atom_ref(&cx, KindredTouchstones);
	let traitMax = match advantagesRef.read().power
	{
		Some(p) => getTraitMax(p),
		None => 5
	};
	
	let mut clans = Vec::<String>::new();
	for c in Clan::iter()
	{
		clans.push(c.as_ref().to_string());
	}
	
	let mut aspirations = vec![];
	aspirationsRef.read().iter().for_each(|a| aspirations.push(a.clone()));
	let mut conditions = vec![];
	conditionsRef.read().iter().for_each(|c| conditions.push(c.clone()));
	let mut merits = vec![];
	meritsRef.read().iter().for_each(|m| merits.push(m.clone()));
	let mut powers = vec![];
	powersRef.read().iter().for_each(|p| powers.push(p.clone()));
	let mut specialties = vec![];
	specialtiesRef.read().iter().for_each(|s| specialties.push(s.clone()));
	let mut touchstones = vec![];
	touchstonesRef.read().iter().for_each(|t| touchstones.push(t.clone()));
	
	return cx.render(rsx!
	{	
		div
		{
			class: "sheet cod vtr2e column",
			
			h1 { "Vampire: The Requiem" }
			h3 { "Second Edition" }
			hr { class: "row" }
			
			div
			{
				class: "row",
				
				Details
				{
					virtue: "Mask".to_string(),
					vice: "Dirge".to_string(),
					typePrimary: "Clan".to_string(),
					typePrimaryOptions: clans.clone(),
					typeSecondary: "Bloodline".to_string(),
					faction: "Covenant".to_string()
				}
				
				Advantages
				{
					integrity: "Humanity".to_string(),
					power: "Blood Potency".to_string(),
					resource: "Vitae".to_string(),
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
				
				SimpleEntryList
				{
					class: "touchstones".to_string(),
					data: touchstones.clone(),
					label: "Touchstones".to_string(),
					entryUpdateHandler: touchstoneUpdateHandler,
					entryRemoveHandler: touchstoneRemoveClickHandler,
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
				
				Disciplines {}
				
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
			
			div
			{
				class: "row",
				
				ActiveAbilities
				{
					class: "powers".to_string(),
					data: powers.clone(),
					label: "Powers".to_string(),
					entryRemoveHandler: powersRemoveClickHandler,
					entryUpdateHandler: powersUpdateHandler,
				}
			}
		}
	});
}

fn templateBonusesHandler(cx: &Scope<AdvantagesProps>)
{
	let advantagesRef = use_atom_ref(cx, CharacterAdvantages);
	let attributesRef = use_atom_ref(cx, CharacterAttributes);
	let disciplinesRef = use_atom_ref(cx, KindredDisciplines);
	let skillsRef = use_atom_ref(cx, CharacterSkills);
	let mut advantages = advantagesRef.write();
	let attributes = attributesRef.read();
	let disciplines = disciplinesRef.read();
	let skills = skillsRef.read();
	
	let size = advantages.size;
	
	if advantages.power == Some(0)
	{
		if let Some(ref mut resource) = advantages.resource
		{
			resource.updateMax(BP0VitaeMax);
		}
	}
	
	if let Some(celerity) = disciplines.get(&Discipline::Celerity)
	{
		if celerity > &0
		{
			let attrDef = match attributes[&CoreAttribute::Dexterity] <= attributes[&CoreAttribute::Wits]
			{
				true => attributes[&CoreAttribute::Dexterity],
				false => attributes[&CoreAttribute::Wits]
			};
			advantages.defense = attrDef + skills[&CoreSkill::Athletics] + celerity;
		}
	}
	
	if let Some(resilience) = disciplines.get(&Discipline::Resilience)
	{
		if resilience > &0
		{
			advantages.health.updateMax(size + attributes[&CoreAttribute::Stamina] + resilience);
		}
	}
	
	if let Some(vigor) = disciplines.get(&Discipline::Vigor)
	{
		if vigor > &0
		{
			advantages.speed = size + attributes[&CoreAttribute::Dexterity] + attributes[&CoreAttribute::Strength] + vigor;
		}
	}
}

/// Event handler triggered by clicking the "Remove" button after
/// right-clicking a Touchstone row.
fn touchstoneRemoveClickHandler<T>(cx: &Scope<T>, index: usize)
{
	let touchstonesRef = use_atom_ref(&cx, KindredTouchstones);
	let mut touchstones = touchstonesRef.write();
	
	if index < touchstones.len()
	{
		touchstones.remove(index);
	}
}

/// Event handler triggered when a Touchstone's value changes.
fn touchstoneUpdateHandler<T>(e: FormEvent, cx: &Scope<T>, index: Option<usize>)
{
	let touchstonesRef = use_atom_ref(&cx, KindredTouchstones);
	let mut touchstones = touchstonesRef.write();
	
	match index
	{
		Some(i) => { touchstones[i] = e.value.to_string(); }
		None => { touchstones.push(e.value.to_string()); }
	}
}

/// Event handler triggered when a `Devotion` input's value changes.
fn powersUpdateHandler<T>(e: FormEvent, cx: &Scope<T>, index: Option<usize>, prop: ActiveAbilityField)
{
	let powersRef = use_atom_ref(&cx, KindredPowers);
	let mut powers = powersRef.write();
	
	match index
	{
		Some(i) =>
		{
			match prop
			{
				ActiveAbilityField::Action => { powers[i].action = e.value.clone(); }
				ActiveAbilityField::Cost => { powers[i].cost = e.value.clone(); }
				ActiveAbilityField::Description => { powers[i].description = e.value.clone(); }
				ActiveAbilityField::DicePool => { powers[i].dicePool = e.value.clone(); }
				ActiveAbilityField::Duration => { powers[i].duration = e.value.clone(); }
				ActiveAbilityField::Effects => { powers[i].effects = e.value.clone(); }
				ActiveAbilityField::Name => { powers[i].name = e.value.clone(); }
				ActiveAbilityField::Requirements => { powers[i].requirements = e.value.clone(); }
			}
		}
		None =>
		{
			match prop
			{
				ActiveAbilityField::Action => { powers.push(ActiveAbility { action: e.value.clone(), ..Default::default() }); }
				ActiveAbilityField::Cost => { powers.push(ActiveAbility { cost: e.value.clone(), ..Default::default() }); }
				ActiveAbilityField::Description => { powers.push(ActiveAbility { description: e.value.clone(), ..Default::default() }); }
				ActiveAbilityField::DicePool => { powers.push(ActiveAbility { dicePool: e.value.clone(), ..Default::default() }); }
				ActiveAbilityField::Duration => { powers.push(ActiveAbility { duration: e.value.clone(), ..Default::default() }); }
				ActiveAbilityField::Effects => { powers.push(ActiveAbility { effects: e.value.clone(), ..Default::default() }); }
				ActiveAbilityField::Name => { powers.push(ActiveAbility { name: e.value.clone(), ..Default::default() }); }
				ActiveAbilityField::Requirements => { powers.push(ActiveAbility { requirements: e.value.clone(), ..Default::default() }); }
			}
		}
	}
}

/// Event handler triggered by clicking the "Remove" button after right-clicking a
/// Devotion row.
fn powersRemoveClickHandler<T>(cx: &Scope<T>, index: usize)
{
	let powersRef = use_atom_ref(&cx, KindredPowers);
	let mut powers = powersRef.write();
	
	if index < powers.len()
	{
		powers.remove(index);
	}
}
