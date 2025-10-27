use std::cmp::Ordering;

use serde::{Deserialize, Serialize};
use crate::gamesystems::cofd::data::Skill;

#[derive(Clone, Debug, Default, Deserialize, Eq, PartialEq, Ord, Serialize)]
pub struct Specialty
{
	pub skill: Skill,
	pub specialty: String,
}

impl PartialOrd for Specialty
{
	fn partial_cmp(&self, other: &Self) -> Option<std::cmp::Ordering>
	{
		return match self.skill.partial_cmp(&other.skill)
		{
			None => self.specialty.partial_cmp(&other.specialty),
			Some(ord) => match ord
			{
				Ordering::Equal => self.specialty.partial_cmp(&other.specialty),
				_ => Some(ord),
			}
		};
	}
}
