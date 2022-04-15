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
			KnownSpells,
			PreparedSpells,
			SpellSlots,
		},
	},
	dnd::fifth::{
		enums::{
			Ability,
			MagicSchool,
			Proficiency,
			Skill,
		},
		structs::{
			Spell,
			SpellComponents,
		}
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
	
	let spells = vec![
		Spell
		{
			castingTime: Some("1 action".to_string()),
			components: SpellComponents { verbal: true, somatic: true, material: false },
			concentration: false,
			description: "You create a ghostly, skeletal hand in the space of a creature within range. Make a ranged spell attack against the creature to assail it with the chill of the grave. On a hit, the target takes 1d8 necrotic damage, and it can't regain hit points until the start of your next turn. Until then, the hand clings to the target.\n\nIf you hit an undead target, it also has disadvantage on attack rolls against you until the end of your next turn.\n\nThis spell's damage increases by 1d8 when you reach 5th level (2d8), 11th level (3d8), and 17th level (4d8).".to_string(),
			duration: Some("1 round".to_string()),
			level: 0,
			materials: None,
			name: "Chill Touch".to_string(),
			range: Some("120 feet".to_string()),
			school: MagicSchool::Necromancy,
		},
		
		Spell
		{
			castingTime: Some("1 action".to_string()),
			components: SpellComponents { verbal: true, somatic: true, material: true },
			concentration: false,
			description: "You hurl a 4-inch-diameter sphere of energy at a creature that you can see within range. You choose acid, cold, fire, lightning, poison, or thunder for the type of orb you create, and then make a ranged spell attack against the target. If the attack hits, the creature takes 3d8 damage of the type you chose.\n\nAt Higher Levels: When you cast this spell using a spell slots of 2nd level or higher, the dammage increases by 1d8 for each slot level above 1st.".to_string(),
			duration: None,
			level: 1,
			materials: Some("A diamond worth at least 50 gp".to_string()),
			name: "Chromatic Orb".to_string(),
			range: Some("90 feet".to_string()),
			school: MagicSchool::Evocation,
		}
	];
	
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
			hr { class: "row justEven" }
			div { class: "row justEven", KnownSpells { spells: spells.clone() } }
			
			/*
			Equipment
			Passive Skills, Proficiencies/Languages, Characteristics (Ideals/Flaws/etc)
			Features & Traits
			Known Spell Details (use this list to generate available spells in the Prepared Spells component above)
			*/
		}
	});
}
