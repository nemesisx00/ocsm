#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use std::collections::BTreeMap;
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
			
			h3 { class: "row justEven", "Spell Slots" }
			
			div
			{
				class: "row justEven",
				
				slots.iter().map(|(level, slots)| rsx!(cx, Track { class: "column".to_string(), label: level.to_string(), tracker: Tracker::new(*slots) }))
			}
		}
	});
}

// --------------------------------------------------

#[derive(PartialEq, Props)]
pub struct PreparedSpellsProps
{
	pub spells: BTreeMap<usize, Vec<String>>,
}

pub fn PreparedSpells(cx: Scope<PreparedSpellsProps>) -> Element
{
	return cx.render(rsx!
	{
		div
		{
			class: "column justEven preparedSpells",
			
			h3 { class: "row justEven", "Prepared Spells" }
			
			div
			{
				class: "row justEven",
				
				cx.props.spells.iter()
					.filter(|(level, _)| **level < 5)
					.map(|(level, names)|
					{
						let isCantrip = *level == 0;
						let isSpell = *level > 0;
						rsx!(div
						{
							class: "column justStart",
							
							isCantrip.then(|| rsx!(div { class: "label", "Cantrips" }))
							isSpell.then(|| rsx!(div { class: "label", "Level {level}" }))
							select
							{
								option { value: "", "Prepare a Spell" }
								names.iter().map(|name| rsx!(option { value: "{name}", "{name}" }))
							}
						})
					})
			}
			
			div
			{
				class: "row justEven",
				
				cx.props.spells.iter()
					.filter(|(level, _)| **level > 4)
					.map(|(level, names)| rsx!(div
					{
						class: "column justStart",
						
						div { class: "label", "Level {level}" }
						select
						{
							option { value: "", "Prepare a Spell" }
							names.iter().map(|name| rsx!(option { value: "{name}", "{name}" }))
						}
					}))
			}
		}
	});
}
