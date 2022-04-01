#![allow(non_snake_case, non_upper_case_globals)]

use serde::{
	Deserialize,
	Serialize,
};
use crate::core::enums::GameSystem;

#[derive(Clone, Debug, Deserialize, Eq, PartialEq, PartialOrd, Serialize, Ord)]
pub struct SaveData
{
	pub game: GameSystem,
	pub sheet: String,
}

impl SaveData
{
	pub fn new(g: GameSystem, s: String) -> Self
	{
		return Self
		{
			game: g,
			sheet: s,
		};
	}
}
