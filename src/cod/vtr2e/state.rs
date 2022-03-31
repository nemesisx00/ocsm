#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::{
	cod::{
		structs::ActiveAbility,
		vtr2e::enums::Discipline,
	},
};

/// The maximum Vitae capacity of a Vampire: The Requiem 2e Kindred at Blood Potency 0
pub const BP0VitaeMax: usize = 5;
#[allow(dead_code)]
/// The maximum Vitae per turn of a Vampire: The Requiem 2e Kindred at Blood Potency 0
pub const BP0VitaePerTurn: usize = 1;

/// A Vampire: The Requiem 2e Kindred's list of Disciplines.
pub static KindredDisciplines: AtomRef<Vec<(Discipline, usize)>> = |_| Vec::<(Discipline, usize)>::new();
/// A Vampire: The Requiem 2e Kindred's list of Devotions and Discipline Powers.
pub static KindredPowers: AtomRef<Vec<ActiveAbility>> = |_| Vec::<ActiveAbility>::new();
/// A Vampire: The Requiem 2e Kindred's list of Touchstones.
pub static KindredTouchstones: AtomRef<Vec<String>> = |_| Vec::<String>::new();

/// Reset all `cod::vtr2e::state` global state values.
pub fn resetGlobalStateVtr2e<T>(cx: &Scope<T>)
{
	let kindredDevotions = use_atom_ref(cx, KindredPowers);
	let kindredDisciplines = use_atom_ref(cx, KindredDisciplines);
	let kindredTouchstones = use_atom_ref(cx, KindredTouchstones);
	
	(*kindredDevotions.write()) = Vec::<ActiveAbility>::new();
	(*kindredDisciplines.write()) = Vec::<(Discipline, usize)>::new();
	(*kindredTouchstones.write()) = Vec::<String>::new();
}
