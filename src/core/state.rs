#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::{
	Atom,
	Scope,
};
use serde::{
	de::DeserializeOwned,
	Serialize,
};
use crate::{
	cod::{
		state::resetGlobalStateCod,
		ctl2e::state::resetGlobalStateCtl2e,
		mta2e::state::resetGlobalStateMta2e,
		vtr2e::state::resetGlobalStateVtr2e,
	},
	core::enums::GameSystem,
};

/// Control switch used to signal all Menu components to hide their submenus.
/// 
/// Default value is `true`. Set to `false` to close all Menus.
pub static MainMenuState: Atom<bool> = |_| true;
/// The most recently selected file path.
/// 
/// Updated by loading or saving a character sheet.
pub static CurrentFilePath: Atom<Option<String>> = |_| None;
/// The active `GameSystem` determining which character sheet is rendered.
pub static CurrentGameSystem: Atom<GameSystem> = |_| GameSystem::None;

// --------------------------------------------------

/// Reset every stateful value in the application, regardless of game system.
pub fn resetGlobalState<T>(cx: &Scope<T>)
{
	resetGlobalStateCod(cx);
	resetGlobalStateCtl2e(cx);
	resetGlobalStateMta2e(cx);
	resetGlobalStateVtr2e(cx);
}

// --------------------------------------------------

/// Trait defining methods for interacting with the application's global state.
pub trait StatefulTemplate: DeserializeOwned + Serialize
{
	/// Pull the global state down into this template.
	fn pull<T>(&mut self, cx: &Scope<T>);
	
	/// Push this template up into the global state.
	fn push<T>(&mut self, cx: &Scope<T>);
	
	/// Detect and resolve any internal data errors.
	fn validate(&mut self);
}
