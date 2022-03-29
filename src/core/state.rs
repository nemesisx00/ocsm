#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::{
	cod::{
		state::resetGlobalStateCod,
		ctl2e::state::resetGlobalStateCtl2e,
		vtr2e::state::resetGlobalStateVtr2e,
	}
};

pub static CurrentFilePath: Atom<Option<String>> = |_| None;

// -----

/*
Memory capacity/usage isn't really an issue for this app. Even with all
the currently planned game systems implemented, I would guess the memory
footprint would still be trivial.

That said, there's no reason to waste memory with loose fragments of
game system data floating around that aren't being used in the currently
loaded game system.
*/

/// Reset every stateful value in the application, regardless of game system.
pub fn resetGlobalState<T>(cx: &Scope<T>)
{
	resetGlobalStateCod(cx);
	resetGlobalStateCtl2e(cx);
	resetGlobalStateVtr2e(cx);
}
