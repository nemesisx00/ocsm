#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::wod::{
	components::{
		tracks::Track,
		traits::Attributes
	},
	tracks::Tracker,
	vtmv5::state::*
};

pub fn Sheet(scope: Scope) -> Element
{
	let currentHealth = use_read(&scope, CurrentHealth);
	
	return scope.render(rsx!
	{	
		div
		{
			class: "sheet mageTheAwakening",
			
			div
			{
				class: "column",
				
				//Attributes { attributes: vamp.attributes }
			}
			
			div
			{
				class: "column right",
				
				div
				{
					class: "trackers",
					
					p
					{
						Track
						{
							onclick: move |e|
							{
								println!("{:?}", currentHealth.superficial);
							}
						}
					}
					p { "Willpower" }
				}
			}
		}
	});
}
