use serde::{Deserialize, Serialize};
use super::mental::SkillsMental;
use super::physical::SkillsPhysical;
use super::social::SkillsSocial;

#[derive(Clone, Copy, Debug, Default, Deserialize, PartialEq, Serialize)]
pub struct Skills
{
	pub academics: u32,
	pub animalKen: u32,
	pub athletics: u32,
	pub brawl: u32,
	pub computer: u32,
	pub crafts: u32,
	pub drive: u32,
	pub empathy: u32,
	pub expression: u32,
	pub firearms: u32,
	pub intimidation: u32,
	pub investigation: u32,
	pub larceny: u32,
	pub medicine: u32,
	pub occult: u32,
	pub persuasion: u32,
	pub politics: u32,
	pub science: u32,
	pub socialize: u32,
	pub stealth: u32,
	pub streetwise: u32,
	pub subterfuge: u32,
	pub survival: u32,
	pub weaponry: u32,
}

impl From<SkillsMental> for Skills
{
	fn from(value: SkillsMental) -> Self
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
			..Default::default()
		};
	}
}

impl From<SkillsPhysical> for Skills
{
	fn from(value: SkillsPhysical) -> Self
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
			..Default::default()
		};
	}
}

impl From<SkillsSocial> for Skills
{
	fn from(value: SkillsSocial) -> Self
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
			..Default::default()
		};
	}
}

impl Skills
{
	pub fn updateMental(&mut self, value: SkillsMental)
	{
		self.academics = value.academics;
		self.computer = value.computer;
		self.crafts = value.crafts;
		self.investigation = value.investigation;
		self.medicine = value.medicine;
		self.occult = value.occult;
		self.politics = value.politics;
		self.science = value.science;
	}
	
	pub fn updatePhysical(&mut self, value: SkillsPhysical)
	{
		self.athletics = value.athletics;
		self.brawl = value.brawl;
		self.drive = value.drive;
		self.firearms = value.firearms;
		self.larceny = value.larceny;
		self.stealth = value.stealth;
		self.survival = value.survival;
		self.weaponry = value.weaponry;
	}
	
	pub fn updateSocial(&mut self, value: SkillsSocial)
	{
		self.animalKen = value.animalKen;
		self.empathy = value.empathy;
		self.expression = value.expression;
		self.intimidation = value.intimidation;
		self.persuasion = value.persuasion;
		self.socialize = value.socialize;
		self.streetwise = value.streetwise;
		self.subterfuge = value.subterfuge;
	}
}
