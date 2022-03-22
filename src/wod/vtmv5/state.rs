#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::{
	wod::{
		tracks::Tracker,
		vtmv5::character::Vampire
	}
};

pub static VampireCharacter: Atom<Vampire> = |_| Vampire::default();
pub static CurrentHealth: Atom<Tracker> = |_| Tracker::default();
