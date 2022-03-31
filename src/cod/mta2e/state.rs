#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::{
	AtomRef,
	Scope,
	use_atom_ref,
};
use crate::{
	cod::{
		mta2e::enums::Arcana,
	},
};

/// A Mage: The Awakening 2e Mage's list of Arcana.
pub static MageArcana: AtomRef<Vec<(Arcana, usize)>> = |_| Vec::<(Arcana, usize)>::new();
/// A Mage: The Awakening 2e Mage's list of Obsessions.
pub static MageObsessions: AtomRef<Vec<String>> = |_| Vec::<String>::new();

/// Reset all `cod::mta2e::state` global state values.
pub fn resetGlobalStateMta2e<T>(cx: &Scope<T>)
{
	let mageArcana = use_atom_ref(cx, MageArcana);
	let mageObsessions = use_atom_ref(cx, MageObsessions);
	
	(*mageArcana.write()) = Vec::<(Arcana, usize)>::new();
	(*mageObsessions.write()) = Vec::<String>::new();
}
