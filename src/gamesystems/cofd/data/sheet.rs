use anyhow::Result;
use serde::{Deserialize, Serialize};
use crate::{components::StateValue, data::CharacterSheet};
use crate::data::SheetId;
use super::{Attributes, Merit, Skills};

#[derive(Clone, Debug, Deserialize, PartialEq, Serialize)]
pub struct Sheet
{
	pub id: SheetId,
	pub age: u32,
	pub aspirations: Vec<String>,
	pub attributes: Attributes,
	pub chronicle: String,
	pub concept: String,
	pub conditions: Vec<String>,
	pub faction: String,
	pub group: String,
	pub health: StateValue,
	pub integrity: u32,
	pub merits: Vec<Merit>,
	pub name: String,
	pub player: String,
	pub size: u32,
	pub skills: Skills,
	pub vice: String,
	pub virtue: String,
	pub willpower: u32,
}

impl CharacterSheet for Sheet
{
	fn serialize(&self) -> Result<String>
	{
		return Ok(serde_json::to_string(self)?);
	}
	
	fn update(&mut self, other: &Sheet)
	{
		self.attributes = other.attributes;
		self.name = other.name.to_owned();
		self.health = other.health;
		self.skills = other.skills;
	}
}

impl Default for Sheet
{
	fn default() -> Self
	{
		return Self
		{
			id: SheetId::default(),
			age: 0,
			aspirations: vec![],
			attributes: Attributes::default(),
			chronicle: String::default(),
			concept: String::default(),
			conditions: vec![],
			faction: String::default(),
			group: String::default(),
			health: StateValue::default(),
			integrity: 7,
			merits: vec![],
			name: String::default(),
			player: String::default(),
			size: 5,
			skills: Skills::default(),
			vice: String::default(),
			virtue: String::default(),
			willpower: 0,
		};
	}
}

impl Sheet
{
	pub fn calculateMaxHealth(&self) -> u32
	{
		return self.size + self.attributes.stamina;
	}
	
	pub fn calculateMaxWillpower(&self) -> u32
	{
		return self.attributes.resolve + self.attributes.composure;
	}
}
