#![allow(non_snake_case, non_upper_case_globals)]

use serde::{
	Deserialize,
	Serialize,
};

#[derive(Clone, Debug, Default, Deserialize, PartialEq, Eq, Serialize, PartialOrd, Ord)]
pub struct Merit
{
	#[serde(default)]
	pub name: String,
	#[serde(default)]
	pub value: usize,
}
