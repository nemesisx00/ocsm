#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::{
	cod::{
		components::{
			aspirations::Aspirations,
			experience::Experience,
			merits::Merits,
			traits::{
				Attributes,
				Skills,
				SkillSpecialties,
			}
		},
		state::{
			CharacterAdvantages,
			getTraitMax,
		},
		vtr2e::{
			components::{
				advantages::Advantages,
				details::Details,
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
	
	return cx.render(rsx!
	{	
		div
		{
			class: "sheet cod vtr2e column",
			
			h1 { "Vampire: The Requiem" }
			h3 { "Second Edition" }
			hr { class: "row" }
			div { class: "row", Details {} Advantages {} }
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
