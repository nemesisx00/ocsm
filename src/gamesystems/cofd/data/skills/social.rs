use serde::{Deserialize, Serialize};
use super::skills::Skills;

#[derive(Clone, Copy, Debug, Default, Deserialize, Serialize)]
pub struct SkillsSocial
{
	pub animalKen: u32,
	pub empathy: u32,
	pub expression: u32,
	pub intimidation: u32,
	pub persuasion: u32,
	pub socialize: u32,
	pub streetwise: u32,
	pub subterfuge: u32,
}

impl From<Skills> for SkillsSocial
{
	fn from(value: Skills) -> Self
	{
		return Self
		{
			animalKen: value.animalKen,
			empathy: value.empathy,
			expression: value.expression,
			intimidation: value.intimidation,
			persuasion: value.persuasion,
			socialize: value.socialize,
			streetwise: value.streetwise,
			subterfuge: value.subterfuge,
		};
	}
}
