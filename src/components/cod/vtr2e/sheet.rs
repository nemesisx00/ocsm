#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use strum::IntoEnumIterator;
use crate::{
	cod::{
		enums::{
			CoreAttribute,
			CoreSkill,
		},
		state::{
			CharacterAdvantages,
			CharacterAttributes,
			CharacterSkills,
			getTraitMax,
		},
		vtr2e::{
			enums::{
				Clan,
				Discipline,
			},
			state::{
				BP0VitaeMax,
				KindredDisciplines,
			},
		},
	},
	components::{
		cod::{
			advantages::{
				Advantages,
				AdvantagesProps,
			},
			aspirations::Aspirations,
			details::Details,
			experience::Experience,
			merits::Merits,
			traits::{
				Attributes,
				Skills,
				SkillSpecialties,
			},
			vtr2e::{
				disciplines::{
					Disciplines,
					Devotions,
				},
				touchstones::Touchstones,
			},
		},
	},
};

/// The UI Component defining the layout of a Vampire: The Requiem 2e Kindred's character sheet.
pub fn VampireSheet(cx: Scope) -> Element
{
	let advantages = use_atom_ref(&cx, CharacterAdvantages);
	let traitMax = getTraitMax(advantages.read().power.unwrap());
	
	let mut clans = Vec::<String>::new();
	for c in Clan::iter()
	{
		clans.push(c.as_ref().to_string());
	}
	
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
			div { class: "row spacedOut", Aspirations {} Touchstones {} Experience {} }
			hr { class: "row" }
			div { class: "row", Attributes { traitMax: traitMax } }
			hr { class: "row" }
			div { class: "row", Skills { traitMax: traitMax } }
			hr { class: "row" }
			div { class: "row", Disciplines {} SkillSpecialties {} Merits {} }
			hr { class: "row" }
			div { class: "row", Devotions {} }
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
