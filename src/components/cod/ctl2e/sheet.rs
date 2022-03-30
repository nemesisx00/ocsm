#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::{
	cod::state::{
		CharacterAdvantages,
		getTraitMax,
	},
	components::{
		cod::{
			ctl2e::{
				advantages::{
					Advantages,
					Frailties,
				},
				details::Details,
				regalia::{
					Contracts,
					FavoredRegalia,
				},
				touchstones::Touchstones,
			},
			aspirations::Aspirations,
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
	
	return cx.render(rsx!
	{	
		div
		{
			class: "sheet cod ctl2e column",
			
			h1 { "Changeling: The Lost" }
			h3 { "Second Edition" }
			hr { class: "row" }
			div { class: "row", Details {} Advantages {} }
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
