#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::{
	Atom,
	AtomRef,
	Scope,
	use_atom_ref,
	use_set,
};
use crate::{
	cod::{
		ctl2e::{
			enums::Regalia,
			structs::Contract,
		},
	},
};

/// The value of the bonus to initiative and speed provided by the Beast Seeming Blessing.
pub const BeastBonus: usize = 3;

/// A Changeling: The Lost 2e Changeling's list of Contracts.
pub static ChangelingContracts: AtomRef<Vec<Contract>> = |_| Vec::<Contract>::new();
/// A Changeling: The Lost 2e Changeling's chosen Favored Regalia.
pub static ChangelingFavoredRegalia: Atom<Option<Regalia>> = |_| None;
/// A Changeling: The Lost 2e Changeling's list of Frailties.
pub static ChangelingFrailties: AtomRef<Vec<String>> = |_| Vec::<String>::new();
/// A Changeling: The Lost 2e Changeling's list of Touchstones.
pub static ChangelingTouchstones: AtomRef<Vec<String>> = |_| Vec::<String>::new();

/// Reset all `cod::ctl2e::state` global state values.
pub fn resetGlobalStateCtl2e<T>(cx: &Scope<T>)
{
	let changelingContracts = use_atom_ref(cx, ChangelingContracts);
	let changelingFavoredRegalia = use_set(cx, ChangelingFavoredRegalia);
	let changelingFrailties = use_atom_ref(cx, ChangelingFrailties);
	let changelingTouchstones = use_atom_ref(cx, ChangelingTouchstones);
	
	(*changelingContracts.write()) = Vec::<Contract>::new();
	changelingFavoredRegalia(None);
	(*changelingFrailties.write()) = Vec::<String>::new();
	(*changelingTouchstones.write()) = Vec::<String>::new();
}
