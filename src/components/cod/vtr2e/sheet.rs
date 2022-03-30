#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use strum::IntoEnumIterator;
use crate::{
	cod::{
		state::{
			CharacterAdvantages,
			getTraitMax,
		},
		vtr2e::enums::Clan,
	},
	components::{
		cod::{
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
				advantages::Advantages,
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
				Advantages {}
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
