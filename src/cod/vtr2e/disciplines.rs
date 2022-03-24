#![allow(non_snake_case, non_upper_case_globals)]

use serde::{
	Deserialize,
	Serialize,
};

#[derive(Clone, Eq, PartialEq, Deserialize, Serialize, PartialOrd, Ord)]
pub enum DisciplineType
{
	Animalism,
	Auspex,
	Celerity,
	Dominate,
	Majesty,
	Nightmare,
	Obfuscate,
	Protean,
	Resilience,
	Vigor,
}

#[derive(Clone, Default, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub struct Discipline
{
	pub name: String,
	pub value: usize,
}

#[derive(Clone, Default, Deserialize, Serialize)]
pub struct Disciplines
{
	pub animalism: Discipline,
	pub auspex: Discipline,
	pub celerity: Discipline,
	pub dominate: Discipline,
	pub majesty: Discipline,
	pub nightmare: Discipline,
	pub obfuscate: Discipline,
	pub protean: Discipline,
	pub resilience: Discipline,
	pub vigor: Discipline,
}

#[derive(Clone, Eq, PartialEq, Deserialize, Serialize, PartialOrd, Ord)]
pub enum DevotionProperty
{
	Cost,
	DicePool,
	Disciplines,
	Name,
	Reference,
}

#[derive(Clone, Default, Deserialize, Eq, PartialEq, Serialize, PartialOrd, Ord)]
pub struct Devotion
{
	pub cost: String,
	pub dicePool: String,
	pub disciplines: String,
	pub name: String,
	pub reference: String,
}
