use serde::{Deserialize, Serialize};
use super::skills::Skills;

#[derive(Clone, Copy, Debug, Default, Deserialize, Serialize)]
pub struct SkillsMental
{
	pub academics: u32,
	pub computer: u32,
	pub crafts: u32,
	pub investigation: u32,
	pub medicine: u32,
	pub occult: u32,
	pub politics: u32,
	pub science: u32,
}

impl From<Skills> for SkillsMental
{
	fn from(value: Skills) -> Self
	{
		return Self
		{
			academics: value.academics,
			computer: value.computer,
			crafts: value.crafts,
			investigation: value.investigation,
			medicine: value.medicine,
			occult: value.occult,
			politics: value.politics,
			science: value.science,
		};
	}
}
