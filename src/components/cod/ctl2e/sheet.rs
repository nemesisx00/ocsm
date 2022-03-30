#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use strum::IntoEnumIterator;
use crate::{
	cod::{
		ctl2e::{
			enums::Seeming,
			state::BeastBonus,
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
				touchstones::Touchstones,
			},
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
		},
	},
};

/// The UI Component defining the layout of a Changeling: The Lost 2e Changeling's character sheet.
pub fn ChangelingSheet(cx: Scope) -> Element
{
	let advantages = use_atom_ref(&cx, CharacterAdvantages);
	let traitMax = getTraitMax(advantages.read().power.unwrap());
	
	let mut seemings = Vec::<String>::new();
	for s in Seeming::iter()
	{
		seemings.push(s.as_ref().to_string());
	}
	
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
			div { class: "row spacedOut", Aspirations {} div { class: "column", Touchstones {} FavoredRegalia {} } Experience {} }
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
