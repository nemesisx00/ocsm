use std::collections::HashMap;
use anyhow::Result;
use serde::{Deserialize, Serialize};
use crate::components::StateValue;
use crate::gamesystems::cofd::data::{Attributes, Merit, Skills};
use crate::data::{CharacterSheet, SheetId};
use super::Discipline;

#[derive(Clone, Debug, Deserialize, PartialEq, Serialize)]
pub struct Sheet
{
	pub id: SheetId,
	pub attributes: Attributes,
	pub aspirations: Vec<String>,
	pub banes: Vec<String>,
	pub bloodline: String,
	pub bloodPotency: u32,
	pub chronicle: String,
	pub clan: String,
	pub concept: String,
	pub covenant: String,
	pub dirge: String,
	pub disciplines: HashMap<Discipline, u32>,
	pub health: StateValue,
	pub humanity: u32,
	pub mask: String,
	pub merits: Vec<Merit>,
	pub name: String,
	pub player: String,
	pub size: u32,
	pub skills: Skills,
	pub vitae: u32,
	pub willpower: u32,
}

impl CharacterSheet for Sheet
{
	fn save(&self) -> Result<String>
	{
		return Ok(serde_json::to_string(self)?);
	}
	
	fn update(&mut self, other: &Sheet)
	{
		self.aspirations = other.aspirations.to_owned();
		self.attributes = other.attributes;
		self.banes = other.banes.to_owned();
		self.bloodline = other.bloodline.to_owned();
		self.bloodPotency = other.bloodPotency;
		self.chronicle = other.chronicle.to_owned();
		self.clan = other.clan.to_owned();
		self.concept = other.concept.to_owned();
		self.covenant = other.covenant.to_owned();
		self.dirge = other.dirge.to_owned();
		self.disciplines = other.disciplines.to_owned();
		self.health = other.health;
		self.mask = other.mask.to_owned();
		self.merits = other.merits.to_owned();
		self.name = other.name.to_owned();
		self.player = other.player.to_owned();
		self.skills = other.skills;
		self.vitae = other.vitae;
		self.willpower = other.willpower;
	}
}

impl Default for Sheet
{
	fn default() -> Self
	{
		return Self
		{
			aspirations: vec![],
			attributes: Attributes::default(),
			banes: vec![],
			bloodPotency: 1,
			bloodline: String::default(),
			chronicle: String::default(),
			clan: String::default(),
			concept: String::default(),
			covenant: String::default(),
			dirge: String::default(),
			disciplines: HashMap::default(),
			health: StateValue::default(),
			humanity: 7,
			id: SheetId::default(),
			mask: String::default(),
			merits: vec![],
			name: String::default(),
			player: String::default(),
			size: 5,
			skills: Skills::default(),
			vitae: 0,
			willpower: 0,
		};
	}
}

impl Sheet
{
	pub fn calculateMaxHealth(&self) -> u32
	{
		let resilience = match self.disciplines.get(&Discipline::Resilience)
		{
			None => 0,
			Some(res) => *res,
		};
		
		return self.size + self.attributes.stamina + resilience;
	}
	
	pub fn calculateMaxTrait(&self) -> u32
	{
		return match self.bloodPotency
		{
			6 => 6,
			7 => 7,
			8 => 8,
			9 => 9,
			10 => 10,
			
			_ => 5,
		};
	}
	
	pub fn calculateMaxVitae(&self) -> u32
	{
		return match self.bloodPotency
		{
			1 => 10,
			2 => 11,
			3 => 12,
			4 => 13,
			5 => 15,
			6 => 20,
			7 => 25,
			8 => 30,
			9 => 50,
			10 => 75,
			// 0
			_ => self.attributes.stamina,
		};
	}
	
	pub fn calculateMaxVitaePerTurn(&self) -> u32
	{
		return match self.bloodPotency
		{
			2 => 2,
			3 => 3,
			4 => 4,
			5 => 5,
			6 => 6,
			7 => 7,
			8 => 8,
			9 => 10,
			10 => 15,
			
			// 0
			// 1
			_ => 1,
		};
	}
	
	pub fn calculateMaxWillpower(&self) -> u32
	{
		return self.attributes.resolve + self.attributes.composure;
	}
}
