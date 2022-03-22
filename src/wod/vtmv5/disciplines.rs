#![allow(non_snake_case, non_upper_case_globals)]

///A vampiric Discipline
#[derive(Clone, Default)]
pub struct Discipline
{
	pub name: String,
	pub r#type: String,
	pub masqueradeThreat: String,
	pub bloodResonance: String,
	pub powers: Vec<DisciplinePower>,
}

///An individual Power of a Discipline
#[derive(Clone, Default)]
pub struct DisciplinePower
{
	pub cost: String,
	pub dicePools: String,
	pub level: i8,
	pub name: String,
	pub system: String,
	pub duration: String,
}
