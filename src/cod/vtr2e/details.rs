#![allow(non_snake_case, non_upper_case_globals)]

#[allow(dead_code)]
#[derive(Clone, Copy, Debug, PartialEq)]
pub enum DetailsField
{
	Bloodline,
	Chronicle,
	Clan,
	Concept,
	Covenant,
	Dirge,
	Mask,
	Name,
	Player,
}

#[derive(Clone, Default)]
pub struct Details
{
	pub bloodline: String,
	pub chronicle: String,
	pub clan: String,
	pub concept: String,
	pub covenant: String,
	pub dirge: String,
	pub mask: String,
	pub name: String,
	pub player: String,
}
