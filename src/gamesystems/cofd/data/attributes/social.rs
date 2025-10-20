use serde::{Deserialize, Serialize};
use super::attributes::Attributes;

#[derive(Clone, Copy, Debug, Deserialize, PartialEq, Serialize)]
pub struct AttributesSocial
{
	pub composure: u32,
	pub presence: u32,
	pub manipulation: u32,
}

impl Default for AttributesSocial
{
	fn default() -> Self
	{
		return Self
		{
			composure: 1,
			manipulation: 1,
			presence: 1,
		};
	}
}

impl From<Attributes> for AttributesSocial
{
	fn from(value: Attributes) -> Self
	{
		return Self
		{
			composure: value.composure,
			presence: value.presence,
			manipulation: value.manipulation,
		};
	}
}
