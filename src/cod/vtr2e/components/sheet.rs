#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::{
	cod::{
		components::{
			traits::{
				Attributes,
				Skills,
			}
		},
		state::{
			CharacterAttributes,
			CharacterSkills,
		},
		vtr2e::{
			components::{
				advantages::{
					Advantages,
				},
				details::{
					Details,
				},
				disciplines::{
					Disciplines,
				},
			},
			template::{
				Kindred,
			},
		},
	},
};

pub fn Sheet(scope: Scope) -> Element
{
	let attributes = use_atom_ref(&scope, CharacterAttributes);
	let skills = use_atom_ref(&scope, CharacterSkills);
	
	return scope.render(rsx!
	{	
		div
		{
			class: "sheet cod column",
			
			h1 { "Vampire: The Requiem" }
			h2 { "Second Edition" }
			hr { class: "row spacedOut" }
			
			div
			{
				class: "row",
				
				Details { }
				Advantages { }
			}
			
			hr { class: "row spacedOut" }
			div { class: "row", Attributes { attributes: attributes.read().clone(), label: "Attributes".to_string() } }
			hr { class: "row spacedOut" }
			div { class: "row", Skills { label: "Skills".to_string(), skills: skills.read().clone() } }
			hr { class: "row spacedOut" }
			
			Disciplines { }
		}
	});
}
