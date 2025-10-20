use serde::{Deserialize, Serialize};
use super::attributes::Attributes;

#[derive(Clone, Copy, Debug, Deserialize, PartialEq, Serialize)]
pub struct AttributesMental
{
	pub intelligence: u32,
	pub resolve: u32,
	pub wits: u32,
}

impl Default for AttributesMental
{
	fn default() -> Self
	{
		return Self
		{
			intelligence: 1,
			resolve: 1,
			wits: 1,
		};
	}
}

impl From<Attributes> for AttributesMental
{
	fn from(value: Attributes) -> Self
	{
		return Self
		{
			intelligence: value.intelligence,
			resolve: value.resolve,
			wits: value.wits,
		};
	}
}
