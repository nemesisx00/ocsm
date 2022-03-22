#![allow(non_snake_case, non_upper_case_globals)]

use crate::{
	wod::{
		tracks::Tracker,
		traits::{
			Attribute,
			Attributes,
			Skills
		},
		vtmv5::disciplines::Discipline
	}
};

///A complete Vampire character
#[derive(Clone)]
pub struct Vampire
{
	pub ambition: String,
	pub chronicle: String,
	pub concept: String,
	pub desire: String,
	pub name: String,
	pub player: String,
	pub predator: String,
	pub sire: String,
	
	pub health: Tracker,
	pub willpower: Tracker,
	pub humanity: i8,
	
	pub attributes: Attributes,
	pub disciplines: Vec<Discipline>,
	pub skills: Skills,
}

impl Default for Vampire
{
	fn default() -> Self
	{
		return Vampire
		{
			ambition: String::default(),
			chronicle: String::default(),
			concept: String::default(),
			desire: String::default(),
			name: String::default(),
			player: String::default(),
			predator: String::default(),
			sire: String::default(),
			
			health: Tracker::default(),
			willpower: Tracker::default(),
			humanity: 7,
			
			attributes: Attributes {
				presence: Attribute { name: "Charisma".to_string(), ..Default::default() },
				..Default::default()
			},
			disciplines: vec![],
			skills: Skills::default()
		};
	}
}
