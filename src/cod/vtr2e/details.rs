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
	#[serde(default)]
	pub bloodline: String,
	#[serde(default)]
	pub chronicle: String,
	#[serde(default)]
	pub clan: String,
	#[serde(default)]
	pub concept: String,
	#[serde(default)]
	pub covenant: String,
	#[serde(default)]
	pub dirge: String,
	#[serde(default)]
	pub mask: String,
	#[serde(default)]
	pub name: String,
	#[serde(default)]
	pub player: String,
}
