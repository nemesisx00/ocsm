#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::{
	AtomRef,
	Scope,
	use_atom_ref,
};
use crate::{
	cod::{
		mta2e::{
			enums::Arcana,
			structs::{
				Praxis,
				Rote,
				Spell,
			},
		},
	},
};

/// A Mage: The Awakening 2e Mage's list of maintained Active Spells.
pub static MageActiveSpells: AtomRef<Vec<String>> = |_| Vec::<String>::new();
/// A Mage: The Awakening 2e Mage's list of Arcana.
pub static MageArcana: AtomRef<Vec<(Arcana, usize)>> = |_| Vec::<(Arcana, usize)>::new();
/// A Mage: The Awakening 2e Mage's list of Dedicated Magical Tools.
pub static MageDedicatedTools: AtomRef<Vec<String>> = |_| Vec::<String>::new();
/// A Mage: The Awakening 2e Mage's list of Obsessions.
pub static MageObsessions: AtomRef<Vec<String>> = |_| Vec::<String>::new();
/// A Mage: The Awakening 2e Mage's list of known Praxes.
pub static MagePraxes: AtomRef<Vec<Praxis>> = |_| Vec::<Praxis>::new();
/// A Mage: The Awakening 2e Mage's list of known Praxes.
pub static MageRotes: AtomRef<Vec<Rote>> = |_| Vec::<Rote>::new();
///A Mage: The Awakening 2e Mage's non-exhaustive list of possible Spells and their details.
pub static MageSpells: AtomRef<Vec<Spell>> = |_| Vec::<Spell>::new();

/// Reset all `cod::mta2e::state` global state values.
pub fn resetGlobalStateMta2e<T>(cx: &Scope<T>)
{
	let mageActiveSpells = use_atom_ref(cx, MageActiveSpells);
	let mageArcana = use_atom_ref(cx, MageArcana);
	let mageDedicatedTools = use_atom_ref(cx, MageDedicatedTools);
	let mageObsessions = use_atom_ref(cx, MageObsessions);
	let magePraxes = use_atom_ref(cx, MagePraxes);
	let mageRotes = use_atom_ref(cx, MageRotes);
	let mageSpells = use_atom_ref(cx, MageSpells);
	
	(*mageActiveSpells.write()) = Vec::<String>::new();
	(*mageArcana.write()) = Vec::<(Arcana, usize)>::new();
	(*mageDedicatedTools.write()) = Vec::<String>::new();
	(*mageObsessions.write()) = Vec::<String>::new();
	(*magePraxes.write()) = Vec::<Praxis>::new();
	(*mageRotes.write()) = Vec::<Rote>::new();
	(*mageSpells.write()) = Vec::<Spell>::new();
}
