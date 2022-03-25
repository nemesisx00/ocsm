#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::{
	cod::{
		components::{
			merits::Merits,
			traits::{
				Attributes,
				Skills,
			}
		},
		vtr2e::{
			components::{
				advantages::Advantages,
				details::Details,
				disciplines::{
					Disciplines,
					Devotions,
				},
			},
		},
	},
};

pub fn Sheet(cx: Scope) -> Element
{
	return cx.render(rsx!
	{	
		div
		{
			class: "sheet cod column",
			
			h1 { "Vampire: The Requiem" }
			h2 { "Second Edition" }
			hr { class: "row spacedOut" }
			div { class: "row", Details {} Advantages {} }
			hr { class: "row spacedOut" }
			div { class: "row", Attributes {} }
			hr { class: "row spacedOut" }
			div { class: "row", Skills {} }
			hr { class: "row spacedOut" }
			div { class: "row", Disciplines {} Merits {} }
			hr { class: "row spacedOut" }
			div { class: "row", Devotions {} }
		}
	});
}
