use serde::{Deserialize, Serialize};
use super::skills::Skills;

#[derive(Clone, Copy, Debug, Default, Deserialize, Serialize)]
pub struct SkillsPhysical
{
	pub athletics: u32,
	pub brawl: u32,
	pub drive: u32,
	pub firearms: u32,
	pub larceny: u32,
	pub stealth: u32,
	pub survival: u32,
	pub weaponry: u32,
}

impl From<Skills> for SkillsPhysical
{
	fn from(value: Skills) -> Self
	{
		return Self
		{
			athletics: value.athletics,
			brawl: value.brawl,
			drive: value.drive,
			firearms: value.firearms,
			larceny: value.larceny,
			stealth: value.stealth,
			survival: value.survival,
			weaponry: value.weaponry,
		};
	}
}
