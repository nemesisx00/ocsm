pub mod widgets;

use std::sync::LazyLock;

#[derive(Clone, Copy, Debug, Eq, PartialEq, PartialOrd, Ord)]
pub enum TraitCategory
{
	Mental,
	Physical,
	Social,
}

#[derive(Clone, Copy, Debug, Eq, PartialEq, PartialOrd, Ord)]
pub enum TraitKind
{
	Attribute,
	Skill,
}

#[derive(Clone, Debug, Eq, PartialEq, PartialOrd, Ord)]
pub struct Trait
{
	pub category: TraitCategory,
	pub kind: TraitKind,
	pub name: String,
}

impl Trait
{
	pub fn new(category: TraitCategory, kind: TraitKind, name: String) -> Self
	{
		return Self
		{
			category,
			kind,
			name,
		};
	}
}

#[derive(Clone, Debug, PartialEq, PartialOrd)]
pub struct TraitDots
{
	pub r#trait: Trait,
	pub value: u8,
}

pub fn GenerateAttributes() -> Vec<Trait>
{
	return vec![
		Trait::new(TraitCategory::Social, TraitKind::Attribute, "Composure".into()),
		Trait::new(TraitCategory::Physical, TraitKind::Attribute, "Dexterity".into()),
		Trait::new(TraitCategory::Mental, TraitKind::Attribute, "Intelligence".into()),
		Trait::new(TraitCategory::Social, TraitKind::Attribute, "Manipulation".into()),
		Trait::new(TraitCategory::Social, TraitKind::Attribute, "Presence".into()),
		Trait::new(TraitCategory::Mental, TraitKind::Attribute, "Resolve".into()),
		Trait::new(TraitCategory::Physical, TraitKind::Attribute, "Stamina".into()),
		Trait::new(TraitCategory::Physical, TraitKind::Attribute, "Strength".into()),
		Trait::new(TraitCategory::Mental, TraitKind::Attribute, "Wits".into()),
	];
}

pub const Composure: LazyLock<Trait> = LazyLock::new(|| Trait
{
	category: TraitCategory::Social,
	kind: TraitKind::Attribute,
	name: "Composure".into(),
});
