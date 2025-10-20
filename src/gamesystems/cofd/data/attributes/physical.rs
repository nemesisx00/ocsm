use serde::{Deserialize, Serialize};
use super::attributes::Attributes;

#[derive(Clone, Copy, Debug, Deserialize, PartialEq, Serialize)]
pub struct AttributesPhysical
{
	pub dexterity: u32,
	pub stamina: u32,
	pub strength: u32,
}

impl Default for AttributesPhysical
{
	fn default() -> Self
	{
		return Self
		{
			dexterity: 1,
			stamina: 1,
			strength: 1,
		};
	}
}

impl From<Attributes> for AttributesPhysical
{
	fn from(value: Attributes) -> Self
	{
		return Self
		{
			dexterity: value.dexterity,
			stamina: value.stamina,
			strength: value.strength,
		};
	}
}
