#![allow(non_snake_case, non_upper_case_globals)]

use serde::{Serialize, Deserialize};
use crate::cod::tracks::Tracker;

#[derive(Clone, Copy, Deserialize, PartialEq, Serialize)]
pub enum AdvantageType
{
	Defense,
	Health,
	Initiative,
	Size,
	Speed,
	Willpower,
}

#[derive(Clone, Deserialize, Serialize)]
pub struct Advantages
{
	pub defense: usize,
	pub health: Tracker,
	pub initiative: usize,
	pub size: usize,
	pub speed: usize,
	pub willpower: Tracker,
}

impl Default for Advantages
{
	fn default() -> Self
	{
		Self
		{
			defense: 1,
			health: Tracker::new(6),
			initiative: 2,
			size: 5,
			speed: 7,
			willpower: Tracker::new(2),
		}
	}
}
