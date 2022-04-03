#![allow(non_snake_case, non_upper_case_globals)]

use serde::Deserialize;
use std::fs::File;
use std::path::Path;
use crate::{
	core::io::loadFromFile,
	dnd::fifth::{
		enums::{
			Armor,
			ItemType,
		},
		structs::{
			Item,
		},
	},
};

#[derive(Debug, Default, Deserialize)]
pub struct Metadata
{
	pub items: Vec<Item>
}

pub fn loadMetadata(path: &Path) -> Result<Metadata, String>
{
	return match loadFromFile::<Metadata>(path)
	{
		Ok(data) => Ok(data),
		Err(e) => Err(format!("Error loading metadata: {}", e)),
	};
}
