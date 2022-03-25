#![allow(non_snake_case, non_upper_case_globals)]

use serde::{
	Serialize,
	Deserialize,
};
use std::iter::Iterator;
use strum_macros::{
	AsRefStr,
	EnumCount,
	EnumIter
};

#[allow(dead_code)]
#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub enum DetailsField
{
	Bloodline,
	Chronicle,
	Clan,
	Concept,
	Covenant,
	Dirge,
	Mask,
	Name,
	Player,
}

#[derive(Clone, Debug, Default, Deserialize, Serialize)]
pub struct Details
{
	pub bloodline: String,
	pub chronicle: String,
	pub clan: String,
	pub concept: String,
	pub covenant: String,
	pub dirge: String,
	pub mask: String,
	pub name: String,
	pub player: String,
}
