use serde::{Deserialize, Serialize};
use super::mental::AttributesMental;
use super::physical::AttributesPhysical;
use super::social::AttributesSocial;

#[derive(Clone, Copy, Debug, Deserialize, PartialEq, Serialize)]
pub struct Attributes
{
	pub composure: u32,
	pub dexterity: u32,
	pub intelligence: u32,
	pub manipulation: u32,
	pub presence: u32,
	pub resolve: u32,
	pub stamina: u32,
	pub strength: u32,
	pub wits: u32,
}

impl Default for Attributes
{
	fn default() -> Self
	{
		return Self
		{
			composure: 1,
			dexterity: 1,
			intelligence: 1,
			manipulation: 1,
			presence: 1,
			resolve: 1,
			stamina: 1,
			strength: 1,
			wits: 1,
		};
	}
}

impl From<AttributesMental> for Attributes
{
	fn from(value: AttributesMental) -> Self
	{
		return Self
		{
			intelligence: value.intelligence,
			resolve: value.resolve,
			wits: value.wits,
			..Default::default()
		};
	}
}

impl From<AttributesPhysical> for Attributes
{
	fn from(value: AttributesPhysical) -> Self
	{
		return Self
		{
			dexterity: value.dexterity,
			stamina: value.stamina,
			strength: value.strength,
			..Default::default()
		};
	}
}

impl From<AttributesSocial> for Attributes
{
	fn from(value: AttributesSocial) -> Self
	{
		return Self
		{
			composure: value.composure,
			manipulation: value.manipulation,
			presence: value.presence,
			..Default::default()
		};
	}
}

impl Attributes
{
	pub fn updateMental(&mut self, mental: AttributesMental)
	{
		self.intelligence = mental.intelligence;
		self.resolve = mental.resolve;
		self.wits = mental.wits;
	}
	
	pub fn updatePhysical(&mut self, physical: AttributesPhysical)
	{
		self.dexterity = physical.dexterity;
		self.stamina = physical.stamina;
		self.strength = physical.strength;
	}
	
	pub fn updateSocial(&mut self, social: AttributesSocial)
	{
		self.composure = social.composure;
		self.manipulation = social.manipulation;
		self.presence = social.presence;
	}
}
