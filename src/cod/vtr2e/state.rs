#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::BTreeMap;
use dioxus::prelude::*;
use crate::{
	cod::{
		vtr2e::{
			enums::Discipline,
			structs::Devotion,
		},
	},
};

/// The maximum Vitae capacity of a Vampire: The Requiem 2e Kindred at Blood Potency 0
pub const BP0VitaeMax: usize = 5;
#[allow(dead_code)]
/// The maximum Vitae per turn of a Vampire: The Requiem 2e Kindred at Blood Potency 0
pub const BP0VitaePerTurn: usize = 1;

/// A Vampire: The Requiem 2e Kindred's list of Devotions and Discipline Powers.
pub static KindredDevotions: AtomRef<Vec<Devotion>> = |_| Vec::<Devotion>::new();
/// A Vampire: The Requiem 2e Kindred's list of Disciplines.
pub static KindredDisciplines: AtomRef<BTreeMap<Discipline, usize>> = |_| BTreeMap::<Discipline, usize>::new();
/// A Vampire: The Requiem 2e Kindred's list of Touchstones.
pub static KindredTouchstones: AtomRef<Vec<String>> = |_| Vec::<String>::new();

/// Reset all `cod::vtr2e::state` global state values.
pub fn resetGlobalStateVtr2e<T>(cx: &Scope<T>)
{
	let kindredDevotions = use_atom_ref(cx, KindredDevotions);
	let kindredDisciplines = use_atom_ref(cx, KindredDisciplines);
	let kindredTouchstones = use_atom_ref(cx, KindredTouchstones);
	
	(*kindredDevotions.write()) = Vec::<Devotion>::new();
	(*kindredDisciplines.write()) = BTreeMap::<Discipline, usize>::new();
	(*kindredTouchstones.write()) = Vec::<String>::new();
}
