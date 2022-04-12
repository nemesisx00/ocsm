#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::core::{
	enums::GameSystem,
	state::CurrentGameSystem,
};

/// The default UI to display when no Game System is currently selected.
pub fn HomeScreen(cx: Scope) -> Element
{
	return cx.render(rsx!
	{
		div
		{
			
			class: "home column justEven",
			
			h1 { "Open Character Sheet Manager" }
			h4 { "Version 0.3.0" }
			hr { class: "row justEven" }
			h2 { "Select a Game System" }
			
			div
			{
				class: "row justEven chooser",
				
				div
				{
					class: "column justStart",
					
					div { class: "row label", "Chronicles of Darkness" }
					div { class: "row", button { onclick: move |_| gameSystemClickHandler(&cx, GameSystem::CodMortal), "Core" } }
					div { class: "row", button { onclick: move |_| gameSystemClickHandler(&cx, GameSystem::CodChangeling2e), "Changeling: The Lost 2e" } }
					div { class: "row", button { onclick: move |_| gameSystemClickHandler(&cx, GameSystem::CodMage2e), "Mage: The Awakening 2e" } }
					div { class: "row", button { onclick: move |_| gameSystemClickHandler(&cx, GameSystem::CodVampire2e), "Vampire: The Requiem 2e" } }
				}
				
				div
				{
					class: "column justStart",
					
					div { class: "row label", "Dungeons & Dragons" }
					div { class: "row", button { onclick: move |_| gameSystemClickHandler(&cx, GameSystem::Dnd5e), "Fifth Edition" } }
				}
			}
		}
	});
}

fn gameSystemClickHandler(cx: &Scope, gameSystem: GameSystem)
{
	let setCurrentGameSystem = use_set(cx, CurrentGameSystem);
	setCurrentGameSystem(gameSystem);
}
