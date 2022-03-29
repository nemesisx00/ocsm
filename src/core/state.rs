#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::{
	cod::{
		state::resetGlobalStateCod,
		ctl2e::state::resetGlobalStateCtl2e,
		vtr2e::state::resetGlobalStateVtr2e,
	},
	core::io::getUserDocumentsDir
};

/// The most recently selected file path.
/// 
/// Updated by loading or saving a character sheet.
pub static CurrentFilePath: Atom<Option<String>> = |_| None;

// -----

/// Reset every stateful value in the application, regardless of game system.
pub fn resetGlobalState<T>(cx: &Scope<T>)
{
	let setCurrentFilePath = use_set(cx, CurrentFilePath);
	setCurrentFilePath(Some(getUserDocumentsDir()));
	
	resetGlobalStateCod(cx);
	resetGlobalStateCtl2e(cx);
	resetGlobalStateVtr2e(cx);
}
