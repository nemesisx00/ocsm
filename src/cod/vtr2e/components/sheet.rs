#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::cod::{
	components::{
		tracks::Track,
		traits::{
			Attributes,
			Skills,
		}
	},
	state::{
		CharacterAttributes,
		CharacterHealth,
		CharacterSkills,
		CharacterWillpower,
	},
	vtr2e::{
		components::{
			details::{
				Details,
			},
		},
	},
};

pub fn Sheet(scope: Scope) -> Element
{
	let attributes = use_atom_ref(&scope, CharacterAttributes);
	let skills = use_atom_ref(&scope, CharacterSkills);
	let health = use_atom_ref(&scope, CharacterHealth);
	let willpower = use_atom_ref(&scope, CharacterWillpower);
	
	return scope.render(rsx!
	{	
		div
		{
			class: "sheet cod column",
			
			h1 { "Vampire: The Requiem" }
			h2 { "Second Edition" }
			
			div
			{
				class: "row",
				
				Details { }
				
				div
				{
					class: "advantages",
					
					div
					{
						class: "trackers",
						
						Track
						{
							label: "Health".to_string(),
							tracker: (*health.read()).clone(),
							healthHandler: true
						}
						
						Track
						{
							label: "Willpower".to_string(),
							tracker: (*willpower.read()).clone(),
							willpowerHandler: true
						}
					}
				}
			}
			hr { class: "row spacedOut" }
			div { class: "row", Attributes { attributes: (*attributes.read()).clone(), label: "Attributes".to_string() } }
			hr { class: "row spacedOut" }
			div { class: "row", Skills { label: "Skills".to_string(), skills: (*skills.read()).clone() } }
		}
	});
}
