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
		ctl2e::{
			components::{
				details::Details,
			},
		},
	},
};

pub fn ChangelingSheet(cx: Scope) -> Element
{
	//let advantages = use_atom_ref(&cx, ChangelingAdvantages);
	//let wyrd = advantages.read().wyrd;
	let traitMax = 5;//bloodPotencyAttributeMax(bloodPotency);
	
	return cx.render(rsx!
	{	
		div
		{
			class: "sheet cod ctl2e column",
			
			h1 { "Changeling: The Lost" }
			h3 { "Second Edition" }
			hr { class: "row" }
			div { class: "row", Details {} /*Advantages {}*/ }
			hr { class: "row" }
			div { class: "row spacedOut", Aspirations {} /*Touchstones {}*/ Experience {} }
			hr { class: "row" }
			div { class: "row", Attributes { traitMax: traitMax } }
			hr { class: "row" }
			div { class: "row", Skills { traitMax: traitMax } }
			hr { class: "row" }
			div { class: "row", /*Disciplines {}*/ SkillSpecialties {} Merits {} }
		}
	});
}
