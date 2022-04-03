#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use std::path::Path;
use crate::{
	core::io::getUserDocumentsDir,
	dnd::fifth::{
		enums::{
			ItemType,
			WeaponProficiency,
			WeaponRange,
		},
		meta::{
			Metadata,
			loadMetadata,
		},
		structs::Item,
	},
};

/// The UI Component defining the layout of a Chronicles of Darkness Mortal's character sheet.
pub fn Dnd5eSheet(cx: Scope) -> Element
{
	return cx.render(rsx!
	{
		h1 { "D&D!" }
	});
}
