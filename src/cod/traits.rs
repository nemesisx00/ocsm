#![allow(non_snake_case, non_upper_case_globals)]

use serde::{Serialize, Deserialize};

#[derive(Clone, Copy, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub enum BaseAttributeType
{
	Composure,
	Dexterity,
	Intelligence,
	Manipulation,
	Presence,
	Resolve,
	Stamina,
	Strength,
	Wits,
}

#[derive(Clone, Debug, Deserialize, PartialEq, Serialize)]
pub struct BaseAttribute
{
	pub name: String,
	pub value: usize,
}

impl Default for BaseAttribute
{
	fn default() -> Self
	{
		return BaseAttribute { name: "".to_string(), value: 1 };
	}
}

#[derive(Clone, Debug, Deserialize, PartialEq, Serialize)]
pub struct BaseAttributes
{
	pub composure: BaseAttribute,
	pub dexterity: BaseAttribute,
	pub intelligence: BaseAttribute,
	pub manipulation: BaseAttribute,
	pub presence: BaseAttribute,
	pub resolve: BaseAttribute,
	pub stamina: BaseAttribute,
	pub strength: BaseAttribute,
	pub wits: BaseAttribute,
}

impl Default for BaseAttributes
{
	fn default() -> Self
	{
		return Self
		{
			composure: BaseAttribute { name: "Composure".to_string(), ..Default::default() },
			dexterity: BaseAttribute { name: "Dexterity".to_string(), ..Default::default() },
			intelligence: BaseAttribute { name: "Intelligence".to_string(), ..Default::default() },
			manipulation: BaseAttribute { name: "Manipulation".to_string(), ..Default::default() },
			presence: BaseAttribute { name: "Presence".to_string(), ..Default::default() },
			resolve: BaseAttribute { name: "Resolve".to_string(), ..Default::default() },
			stamina: BaseAttribute { name: "Stamina".to_string(), ..Default::default() },
			strength: BaseAttribute { name: "Strength".to_string(), ..Default::default() },
			wits: BaseAttribute { name: "Wits".to_string(), ..Default::default() }
		};
	}
}

#[derive(Clone, Copy, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub enum BaseSkillType
{
	Academics,
	AnimalKen,
	Athletics,
	Brawl,
	Computer,
	Crafts,
	Drive,
	Empathy,
	Expression,
	Firearms,
	Investigation,
	Intimidation,
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

#[derive(Clone, Debug, Default, Deserialize, Serialize)]
pub struct BaseSkill
{
	pub name: String,
	pub value: usize,
	pub specialties: Vec<String>
}

impl PartialEq for BaseSkill
{
	fn eq(&self, other: &Self) -> bool
	{
		let nameEq = self.name == other.name;
		
		let valueEq = self.value == other.value;
		
		return nameEq && valueEq;
	}
}

#[derive(Clone, Debug, Deserialize, PartialEq, Serialize)]
pub struct BaseSkills
{
	pub academics: BaseSkill,
	pub animalKen: BaseSkill,
	pub athletics: BaseSkill,
	pub brawl: BaseSkill,
	pub computer: BaseSkill,
	pub crafts: BaseSkill,
	pub drive: BaseSkill,
	pub empathy: BaseSkill,
	pub expression: BaseSkill,
	pub firearms: BaseSkill,
	pub investigation: BaseSkill,
	pub intimidation: BaseSkill,
	pub larceny: BaseSkill,
	pub medicine: BaseSkill,
	pub occult: BaseSkill,
	pub persuasion: BaseSkill,
	pub politics: BaseSkill,
	pub science: BaseSkill,
	pub socialize: BaseSkill,
	pub stealth: BaseSkill,
	pub streetwise: BaseSkill,
	pub subterfuge: BaseSkill,
	pub survival: BaseSkill,
	pub weaponry: BaseSkill,
}

impl Default for BaseSkills
{
	fn default() -> Self
	{
		return Self
		{
			academics: BaseSkill { name: "Academics".to_string(), ..Default::default() },
			animalKen: BaseSkill { name: "Animal Ken".to_string(), ..Default::default() },
			athletics: BaseSkill { name: "Athletics".to_string(), ..Default::default() },
			brawl: BaseSkill { name: "Brawl".to_string(), ..Default::default() },
			computer: BaseSkill{ name: "Computer".to_string(), ..Default::default() },
			crafts: BaseSkill { name: "Crafts".to_string(), ..Default::default() },
			drive: BaseSkill { name: "Drive".to_string(), ..Default::default() },
			empathy: BaseSkill { name: "Empathy".to_string(), ..Default::default() },
			expression: BaseSkill { name: "Expression".to_string(), ..Default::default() },
			firearms: BaseSkill { name: "Firearms".to_string(), ..Default::default() },
			investigation: BaseSkill { name: "Investigation".to_string(), ..Default::default() },
			intimidation: BaseSkill { name: "Intimidation".to_string(), ..Default::default() },
			larceny: BaseSkill { name: "Larceny".to_string(), ..Default::default() },
			medicine: BaseSkill { name: "Medicine".to_string(), ..Default::default() },
			occult: BaseSkill { name: "Occult".to_string(), ..Default::default() },
			persuasion: BaseSkill { name: "Persuasion".to_string(), ..Default::default() },
			politics: BaseSkill { name: "Politics".to_string(), ..Default::default() },
			science: BaseSkill { name: "Science".to_string(), ..Default::default() },
			socialize: BaseSkill { name: "Socialize".to_string(), ..Default::default() },
			stealth: BaseSkill { name: "Stealth".to_string(), ..Default::default() },
			streetwise: BaseSkill { name: "Streetwise".to_string(), ..Default::default() },
			subterfuge: BaseSkill { name: "Subterfuge".to_string(), ..Default::default() },
			survival: BaseSkill { name: "Survival".to_string(), ..Default::default() },
			weaponry: BaseSkill { name: "Weaponry".to_string(), ..Default::default() },
		};
	}
}
