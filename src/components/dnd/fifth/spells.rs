#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use strum::IntoEnumIterator;
use crate::{
	components::core::tracks::Track,
	core::structs::Tracker,
	dnd::fifth::util::getSpellSlots,
};

#[derive(PartialEq, Props)]
pub struct SpellSlotsProps
{
	pub characterLevel: usize,
}

/// The UI Component defining the layout of a D&D5e Adventurer's character details.
pub fn SpellSlots(cx: Scope<SpellSlotsProps>) -> Element
{
	let slots = getSpellSlots(cx.props.characterLevel);
	
	return cx.render(rsx!
	{
		div
		{
			class: "column justStart",
			
			div { class: "label", "Spell Slots" }
			
			div
			{
				class: "row justEven",
				
				slots.iter().map(|(spellLevel, spellSlots)| rsx!(cx, div
				{
					class: "column justEven",
					
					Track { label: spellLevel.to_string(), tracker: Tracker::new(*spellSlots) }
				}))
			}
		}
	});
}
