#![allow(non_snake_case, non_upper_case_globals)]

#[derive(Clone, Default, PartialEq)]
pub struct Attribute
{
	pub name: String,
	pub value: i8,
}

#[derive(Clone, PartialEq)]
pub struct Attributes
{
	pub composure: Attribute,
	pub dexterity: Attribute,
	pub intelligence: Attribute,
	pub manipulation: Attribute,
	pub presence: Attribute,
	pub resolve: Attribute,
	pub stamina: Attribute,
	pub strength: Attribute,
	pub wits: Attribute,
}

impl Default for Attributes
{
	fn default() -> Self
	{
		return Attributes
		{
			composure: Attribute { name: "Composure".to_string(), ..Default::default() },
			dexterity: Attribute { name: "Dexterity".to_string(), ..Default::default() },
			intelligence: Attribute { name: "Intelligence".to_string(), ..Default::default() },
			manipulation: Attribute { name: "Manipulation".to_string(), ..Default::default() },
			presence: Attribute { name: "Presence".to_string(), ..Default::default() },
			resolve: Attribute { name: "Resolve".to_string(), ..Default::default() },
			stamina: Attribute { name: "Stamina".to_string(), ..Default::default() },
			strength: Attribute { name: "Strength".to_string(), ..Default::default() },
			wits: Attribute { name: "Wits".to_string(), ..Default::default() }
		};
	}
}

#[derive(Clone, Default)]
pub struct Skill
{
	pub name: String,
	pub value: i8,
	pub specialties: Vec<String>
}

#[derive(Clone)]
pub struct Skills
{
	pub academics: Skill,
	pub computer: Skill,
	pub crafts: Skill,
	pub investigation: Skill,
	pub medicine: Skill,
	pub occult: Skill,
	pub politics: Skill,
	pub science: Skill,
	pub athletics: Skill,
	pub brawl: Skill,
	pub drive: Skill,
	pub firearms: Skill,
	pub larceny: Skill,
	pub stealth: Skill,
	pub survival: Skill,
	pub weaponry: Skill,
	pub animalKen: Skill,
	pub empathy: Skill,
	pub expression: Skill,
	pub intimidation: Skill,
	pub persuasion: Skill,
	pub socialize: Skill,
	pub streetwise: Skill,
	pub subterfuge: Skill
}

impl Default for Skills
{
	fn default() -> Self
	{
		return Skills
		{
			academics: Skill { name: "Academics".to_string(), ..Default::default() },
			computer: Skill { name: "Computer".to_string(), ..Default::default() },
			crafts: Skill { name: "Crafts".to_string(), ..Default::default() },
			investigation: Skill { name: "Investigation".to_string(), ..Default::default() },
			medicine: Skill { name: "Medicine".to_string(), ..Default::default() },
			occult: Skill { name: "Occult".to_string(), ..Default::default() },
			politics: Skill { name: "Politics".to_string(), ..Default::default() },
			science: Skill { name: "Science".to_string(), ..Default::default() },
			athletics: Skill { name: "Athletics".to_string(), ..Default::default() },
			brawl: Skill { name: "Brawl".to_string(), ..Default::default() },
			drive: Skill { name: "Drive".to_string(), ..Default::default() },
			firearms: Skill { name: "Firearms".to_string(), ..Default::default() },
			larceny: Skill { name: "Larceny".to_string(), ..Default::default() },
			stealth: Skill { name: "Stealth".to_string(), ..Default::default() },
			survival: Skill { name: "Survival".to_string(), ..Default::default() },
			weaponry: Skill { name: "Weaponry".to_string(), ..Default::default() },
			animalKen: Skill { name: "Animal Ken".to_string(), ..Default::default() },
			empathy: Skill { name: "Empathy".to_string(), ..Default::default() },
			expression: Skill { name: "Expression".to_string(), ..Default::default() },
			intimidation: Skill { name: "Intimidation".to_string(), ..Default::default() },
			persuasion: Skill { name: "Persuasion".to_string(), ..Default::default() },
			socialize: Skill { name: "Socialize".to_string(), ..Default::default() },
			streetwise: Skill { name: "Streetwise".to_string(), ..Default::default() },
			subterfuge: Skill { name: "Subterfuge".to_string(), ..Default::default() }
		};
	}
}
