#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::{
	Atom,
	AtomRef,
	Scope,
	use_atom_ref,
};
use serde::{
	de::DeserializeOwned,
	Serialize,
};
use std::collections::HashMap;
use crate::{
	dnd::fifth::{
		enums::{
			Ability,
			CasterWeight,
			Proficiency,
			Skill,
		},
		structs::{
			ClassLevel,
			Spell,
		},
	},
};

///
pub static AdventurerAbilityScores: AtomRef<HashMap<Ability, isize>> = |_| HashMap::new();
///
pub static AdventurerKnownSpells: AtomRef<Vec<Spell>> = |_| Vec::new();
///
pub static AdventurerClassLevels: AtomRef<Vec<ClassLevel>> = |_| Vec::new();
///
pub static AdventurerSavingThrowProficiencies: AtomRef<HashMap<Ability, Proficiency>> = |_| HashMap::new();
///
pub static AdventurerSkillProficiencies: AtomRef<HashMap<Skill, Proficiency>> = |_| HashMap::new();

// --------------------------------------------------

/// Reset all `dnd::state` global state values.
pub fn resetGlobalStateDnd5e<T>(cx: &Scope<T>)
{
	let falseLevels = vec![
		ClassLevel { class: "Wizard".to_string(), level: 1, caster: Some(CasterWeight::Full) },
		ClassLevel { class: "Wizard".to_string(), level: 2, caster: Some(CasterWeight::Full) },
		ClassLevel { class: "Rogue".to_string(), level: 1, caster: None },
		ClassLevel { class: "Wizard".to_string(), level: 3, caster: Some(CasterWeight::Full) },
		ClassLevel { class: "Rogue".to_string(), level: 2, caster: None },
		ClassLevel { class: "Ranger".to_string(), level: 1, caster: Some(CasterWeight::Half) },
		ClassLevel { class: "Rogue".to_string(), level: 3, caster: Some(CasterWeight::Third) },
		ClassLevel { class: "Ranger".to_string(), level: 2, caster: Some(CasterWeight::Half) },
	];
	
	let abilityScores = use_atom_ref(cx, AdventurerAbilityScores);
	let knownSpells = use_atom_ref(cx, AdventurerKnownSpells);
	let classLevels = use_atom_ref(cx, AdventurerClassLevels);
	let savingThrowProficiences = use_atom_ref(cx, AdventurerSavingThrowProficiencies);
	let skillProficiencies = use_atom_ref(cx, AdventurerSkillProficiencies);
	
	(*abilityScores.write()) = HashMap::new();
	(*knownSpells.write()) = Vec::new();
	(*classLevels.write()) = falseLevels.clone(); //Vec::new();
	(*savingThrowProficiences.write()) = HashMap::new();
	(*skillProficiencies.write()) = HashMap::new();
}
