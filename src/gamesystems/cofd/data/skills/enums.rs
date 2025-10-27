use serde::{Deserialize, Serialize};
use strum_macros::{AsRefStr, EnumIter};

#[derive(AsRefStr, Clone, Copy, Debug, Default, Deserialize, EnumIter, Eq, PartialEq, PartialOrd, Ord, Serialize)]
pub enum Skill
{
	#[default]
	Academics,
	#[strum(serialize = "Animal Ken")]
	AnimalKen,
	Athletics,
	Brawl,
	Computer,
	Crafts,
	Drive,
	Empathy,
	Expression,
	Firearms,
	Intimidation,
	Investigation,
	Larceny,
	Medicine,
	Occult,
	Persuasion,
	Politics,
	Science,
	Socialize,
	Stealth,
	Streetwise,
	Subterfuge,
	Survival,
	Weaponry,
}
