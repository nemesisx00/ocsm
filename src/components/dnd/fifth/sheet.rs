#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use std::collections::{
	BTreeMap,
	HashMap,
};
use crate::{
	components::dnd::fifth::{
		abilities::{
			AbilityScores,
			SavingThrows,
			Skills,
		},
		details::{
			CharacterDetails,
			CombatDetails,
			DeathSaves,
		},
		spells::{
			PreparedSpells,
			SpellSlots,
		},
	},
	dnd::fifth::enums::{
		Ability,
		Proficiency,
		Skill,
	},
};

/// The UI Component defining the layout of a Chronicles of Darkness Mortal's character sheet.
pub fn Dnd5eAdventurerSheet(cx: Scope) -> Element
{
	let characterLevel = 20;
	
	let mut abilityScores = HashMap::new();
	abilityScores.insert(Ability::Strength, 16);
	abilityScores.insert(Ability::Dexterity, 13);
	abilityScores.insert(Ability::Constitution, 14);
	abilityScores.insert(Ability::Intelligence, 10);
	abilityScores.insert(Ability::Wisdom, 14);
	abilityScores.insert(Ability::Charisma, 8);
	
	let mut savingThrowProficiences = HashMap::new();
	savingThrowProficiences.insert(Ability::Strength, Proficiency::Proficient);
	savingThrowProficiences.insert(Ability::Dexterity, Proficiency::Proficient);
	savingThrowProficiences.insert(Ability::Constitution, Proficiency::None);
	savingThrowProficiences.insert(Ability::Intelligence, Proficiency::None);
	savingThrowProficiences.insert(Ability::Wisdom, Proficiency::None);
	savingThrowProficiences.insert(Ability::Charisma, Proficiency::None);
	
	let mut skillProficiencies = HashMap::new();
	skillProficiencies.insert(Skill::Acrobatics, Proficiency::None);
	skillProficiencies.insert(Skill::AnimalHandling, Proficiency::None);
	skillProficiencies.insert(Skill::Arcana, Proficiency::None);
	skillProficiencies.insert(Skill::Athletics, Proficiency::Proficient);
	skillProficiencies.insert(Skill::Deception, Proficiency::Proficient);
	skillProficiencies.insert(Skill::History, Proficiency::None);
	skillProficiencies.insert(Skill::Insight, Proficiency::None);
	skillProficiencies.insert(Skill::Intimidation, Proficiency::None);
	skillProficiencies.insert(Skill::Investigation, Proficiency::None);
	skillProficiencies.insert(Skill::Medicine, Proficiency::None);
	skillProficiencies.insert(Skill::Nature, Proficiency::None);
	skillProficiencies.insert(Skill::Perception, Proficiency::Proficient);
	skillProficiencies.insert(Skill::Performance, Proficiency::None);
	skillProficiencies.insert(Skill::Persuasion, Proficiency::None);
	skillProficiencies.insert(Skill::Religion, Proficiency::None);
	skillProficiencies.insert(Skill::SleightOfHand, Proficiency::None);
	skillProficiencies.insert(Skill::Stealth, Proficiency::Proficient);
	skillProficiencies.insert(Skill::Survival, Proficiency::Proficient);
	
	let mut spells = BTreeMap::new();
	spells.insert(0, vec!["Chill Touch".to_string(), "Message".to_string(), "Minor Illusion".to_string(), "Dancing Lights".to_string()]);
	spells.insert(1, vec!["Magic Missile".to_string(), "Chromatic Orb".to_string()]);
	spells.insert(2, vec!["Suggestion".to_string(), "Flaming Sphere".to_string()]);
	spells.insert(3, vec!["Animate Dead".to_string(), "Fireball".to_string()]);
	spells.insert(4, vec!["Stoneskin".to_string(), "Dimension Door".to_string()]);
	spells.insert(5, vec!["Teleportation Circle".to_string(), "Geas".to_string()]);
	spells.insert(6, vec!["Chain Lightning".to_string(), "Programmed Illusion".to_string()]);
	spells.insert(7, vec!["Prismatic Spray".to_string(), "Simulacrum".to_string()]);
	spells.insert(8, vec!["Clone".to_string(), "Incendiary Cloud".to_string()]);
	spells.insert(9, vec!["True Polymorph".to_string(), "Wish".to_string()]);
	
	return cx.render(rsx!
	{
		div
		{
			class: "sheet dnd fifth column justEven",
			
			h1 { "Dungeons & Dragons" }
			h3 { "Fifth Edition" }
			hr { class: "row justEven" }
			
			div { class: "row justEven", CharacterDetails {} }
			hr { class: "row justEven" }
			div { class: "row justEven", CombatDetails {} }
			hr { class: "row justEven" }
			
			div
			{
				class: "row justEven",
				
				AbilityScores { abilityScores: abilityScores.clone() }
				DeathSaves {}
				SavingThrows { abilityScores: abilityScores.clone(), proficiencies: savingThrowProficiences.clone() }
			}
			
			hr { class: "row justEven" }
			
			div
			{
				class: "row justEven",
				Skills { abilityScores: abilityScores.clone(), proficiencies: skillProficiencies.clone() }
			}
			
			hr { class: "row justEven" }
			div { class: "row justEven", SpellSlots { characterLevel: characterLevel } }
			hr { class: "row justEven" }
			div { class: "row justEven", PreparedSpells { spells: spells.clone() } }
			
			/*
			Equipment
			Passive Skills, Proficiencies/Languages, Characteristics (Ideals/Flaws/etc)
			Features & Traits
			Known Spell Details (use this list to generate available spells in the Prepared Spells component above)
			*/
		}
	});
}