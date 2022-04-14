#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use strum::IntoEnumIterator;
use crate::{
	components::core::{
		check::{
			CheckLine,
			CheckLineState,
		},
		tracks::{
			Track,
		}
	},
	core::structs::Tracker,
	dnd::fifth::enums::{
		Ability,
		Alignment,
	},
};

/// The UI Component defining the layout of a D&D5e Adventurer's character details.
pub fn CharacterDetails(cx: Scope) -> Element
{
	let bardicInspiration = use_state(&cx, || CheckLineState::None);
	let inspiration = use_state(&cx, || CheckLineState::None);
	
	let mut alignmentOptions = vec![];
	for al in Alignment::iter()
	{
		alignmentOptions.push(al);
	}
	
	return cx.render(rsx!
	{
		div
		{
			class: "column justStart",
			
			h3 { class: "row justEven", "Character Details" }
			
			div
			{
				class: "row justEven characterDetails",
				
				div
				{
					class: "column justEven",
					
					div
					{
						class: "row justStart",
						div { class: "label", "Player:" }
						input { r#type: "text" }
					}
					
					div
					{
						class: "row justStart",
						div { class: "label", "Name:" }
						input { r#type: "text" }
					}
					
					div
					{
						class: "row justStart",
						div { class: "label", "Alignment:" }
						select
						{
							option { value: "", "Select Alignment" }
							alignmentOptions.iter().enumerate().map(|(i, val)| rsx!(option { key: "{i}", value: "{val}", "{val}" }))
						}
					}
				}
				
				div
				{
					class: "column justEven",
					
					div
					{
						class: "row justStart",
						div { class: "label", "Race:" }
						input { r#type: "text" }
					}
					
					div
					{
						class: "row justStart",
						div { class: "label", "Background:" }
						input { r#type: "text" }
					}
					
					div
					{
						class: "row justStart",
						div { class: "label", "Class(es):" }
						input { r#type: "text" }
					}
				}
				
				div
				{
					class: "column justEven",
					
					div
					{
						class: "row justStart",
						div { class: "label long", "Inspiration:" }
						CheckLine { lineState: *inspiration.get(), onclick: move |_| inspiration.set(flipCheckLineState(*inspiration.get())) }
					}
					
					div
					{
						class: "row justStart",
						div { class: "label long", "Bardic Inspiration:" }
						CheckLine { lineState: *bardicInspiration.get(), onclick: move |_| bardicInspiration.set(flipCheckLineState(*bardicInspiration.get())) }
					}
					
					div
					{
						class: "row justStart",
						div { class: "label", "Experience:" }
						input { r#type: "text" }
					}
				}
			}
		}
	});
}

fn flipCheckLineState(current: CheckLineState) -> CheckLineState
{
	return match current
	{
		CheckLineState::None => CheckLineState::Double,
		_ => CheckLineState::None,
	};
}

// --------------------------------------------------

/// The UI Component defining the layout of a D&D5e Adventurer's combat details.
pub fn CombatDetails(cx: Scope) -> Element
{
	let concentration = use_state(&cx, || CheckLineState::None);
	
	let mut spellcastingAbilityOptions = vec![];
	spellcastingAbilityOptions.push(Ability::Intelligence);
	spellcastingAbilityOptions.push(Ability::Wisdom);
	spellcastingAbilityOptions.push(Ability::Charisma);
	
	return cx.render(rsx!
	{
		div
		{
			class: "column justStart",
			
			div
			{
				class: "row justEven combatDetails",
				
				div
				{
					class: "column justStart",
					
					div
					{
						class: "row justStart",
						
						div { class: "label", "Proficiency Bonus:" }
						input { r#type: "text", value: "" }
					}
					
					div
					{
						class: "row justStart",
						
						div { class: "label", "Armor Class:" }
						input { r#type: "text", value: "" }
					}
					
					div
					{
						class: "row justStart",
						
						div { class: "label", "Initiative:" }
						input { r#type: "text", value: "" }
					}
					
					div
					{
						class: "row justStart",
						
						div { class: "label", "Speed:" }
						input { r#type: "text", value: "" }
					}
				}
				
				div
				{
					class: "column justStart",
					
					div
					{
						class: "row justStart",
						
						div { class: "label", "Spellcasting Ability:" }
						select
						{
							option { value: "", "Select Ability" }
							spellcastingAbilityOptions.iter().enumerate().map(|(i, val)| rsx!(option { key: "{i}", value: "{val}", "{val}" }))
						}
					}
					
					div
					{
						class: "row justStart",
						
						div { class: "label", "Spell Save DC:" }
						input { r#type: "text", value: "" }
					}
					
					div
					{
						class: "row justStart",
						
						div { class: "label", "Spell Attack Bonus:" }
						input { r#type: "text", value: "" }
					}
					
					div
					{
						class: "row justStart",
						
						div { class: "label", "Concentration:" }
						CheckLine { lineState: *concentration.get(), onclick: move |_| concentration.set(flipCheckLineState(*concentration.get())) }
					}
				}
				
				div
				{
					class: "column justStart hp",
					
					div
					{
						class: "row justEven",
						
						div { class: "label", "Hit Points & Hit Dice" }
					}
					
					div
					{
						class: "row justCenter hpRow",
						
						input { r#type: "text", title: "Current Hit Points", value: "19" }
						div { class: "hpSlash", "/" }
						input { r#type: "text", title: "Maximum Hit Points", value: "24" }
					}
					
					div
					{
						class: "row justCenter hpRow",
						
						input { class: "tempHp", r#type: "text", title: "Temporary Hit Points", value: "" }
					}
					
					div
					{
						class: "row justCenter hpRow",
						
						input { r#type: "text", title: "Available Hit Dice", value: "5" }
						div { class: "hpSlash", "/" }
						input { r#type: "text", title: "Total Hit Dice", value: "6" }
					}
				}
			}
		}
	});
}

// --------------------------------------------------

/// The UI Component defining the layout of a D&D5e Adventurer's combat details.
pub fn DeathSaves(cx: Scope) -> Element
{
	return cx.render(rsx!
	{
		
		div
		{
			class: "column justStart",
			
			h3 { class: "row justEven", "Death Saves" }
			
			div
			{
				class: "column justEven deathSaves",
				
				Track { class: "row justEven".to_string(), label: "Successes".to_string(), tracker: Tracker::new(3) }
				Track { class: "row justEven".to_string(), label: "Failures".to_string(), tracker: Tracker::new(3) }
			}
		}
	});
}
