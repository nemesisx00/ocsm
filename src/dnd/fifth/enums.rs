#![allow(non_snake_case, non_upper_case_globals)]

use serde::{
	Deserialize,
	Serialize,
};
use strum_macros::{
	AsRefStr,
	EnumCount,
	EnumIter,
};
use std::fmt::{
	Display,
	Formatter,
	Result
};
use crate::core::util::spaceOutCapitals;

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum Ability
{
	Strength,
	Dexterity,
	Constitution,
	Intelligence,
	Wisdom,
	Charisma,
}

impl Ability
{
	pub fn getShortName(&self) -> String
	{
		return self.as_ref()[..2].to_string();
	}
}

impl Display for Ability
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		return write!(f, "{}", self.as_ref());
	}
}

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, Eq, EnumCount, EnumIter, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum ActionType
{
	Action,
	Bonus,
	Reaction,
}

impl Default for ActionType
{
	fn default() -> Self
	{
		return Self::Action;
	}
}

impl Display for ActionType
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		return write!(f, "{}", self.as_ref());
	}
}

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, Eq, EnumCount, EnumIter, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum Advantage
{
	None,
	Advantage,
	Disadvantage,
}

impl Default for Advantage
{
	fn default() -> Self
	{
		return Self::None;
	}
}

impl Display for Advantage
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		return write!(f, "{}", self.as_ref());
	}
}

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, Eq, EnumCount, EnumIter, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum Alignment
{
	LawfulGood,
	NeutralGood,
	ChaoticGood,
	LawfulNeutral,
	Neutral,
	ChaoticNeutral,
	LawfulEvil,
	NeutralEvil,
	ChaoticEvil,
}

impl Default for Alignment
{
	fn default() -> Self
	{
		return Self::Neutral;
	}
}

impl Display for Alignment
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		return write!(f, "{}", spaceOutCapitals(self.as_ref()));
	}
}

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, Eq, EnumCount, EnumIter, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum Armor
{
	Light,
	Medium,
	Heavy,
	Shield,
}

impl Default for Armor
{
	fn default() -> Self
	{
		return Self::Light;
	}
}

impl Display for Armor
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		return write!(f, "{}", self.as_ref());
	}
}

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum DamageType
{
	Acid,
	Bludgeoning,
	Cold,
	Fire,
	Force,
	Lightning,
	Necrotic,
	Piercing,
	Poison,
	Psychic,
	Radiant,
	Slashing,
	Thunder,
}

impl Default for DamageType
{
	fn default() -> Self
	{
		return Self::Acid;
	}
}

impl Display for DamageType
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		return write!(f, "{}", self.as_ref());
	}
}

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum DamageReaction
{
	Vulnerable,
	None,
	Resistant,
	Immune,
}

impl Default for DamageReaction
{
	fn default() -> Self
	{
		return Self::None;
	}
}

impl Display for DamageReaction
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		return write!(f, "{}", self.as_ref());
	}
}

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum Die
{
	Four,
	Six,
	Eight,
	Ten,
	Twelve,
	Twenty,
	Hundred,
}

impl Die
{
	pub fn toNum(self) -> isize
	{
		return match self
		{
			Self::Four => 4,
			Self::Six => 6,
			Self::Eight => 8,
			Self::Ten => 10,
			Self::Twelve => 12,
			Self::Twenty => 20,
			Self::Hundred => 100,
		};
	}
}

impl Default for Die
{
	fn default() -> Self
	{
		return Self::Twenty;
	}
}

impl Display for Die
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		return write!(f, "{}", self.as_ref());
	}
}

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum FeatureType
{
	Background,
	Class,
	ClassShared,
	Feat,
	Race,
	RaceShared,
}

impl Default for FeatureType
{
	fn default() -> Self
	{
		return Self::Background;
	}
}

impl Display for FeatureType
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		return write!(f, "{}", spaceOutCapitals(self.as_ref()));
	}
}

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum ItemType
{
	AdventuringGear,
	Armor(Armor),
	//Container,
	Miscellaneous,
	Tool(Tool),
	Weapon(WeaponProficiency, WeaponRange),
}

impl Default for ItemType
{
	fn default() -> Self
	{
		return Self::Miscellaneous;
	}
}

impl From<Armor> for ItemType
{
	fn from(a: Armor) -> Self
	{
		return Self::Armor(a);
	}
}

impl From<Tool> for ItemType
{
	fn from(t: Tool) -> Self
	{
		return Self::Tool(t);
	}
}

impl From<WeaponProficiency> for ItemType
{
	fn from(wp: WeaponProficiency) -> Self
	{
		return Self::Weapon(wp, WeaponRange::Melee);
	}
}

impl From<WeaponRange> for ItemType
{
	fn from(wr: WeaponRange) -> Self
	{
		return Self::Weapon(WeaponProficiency::Simple, wr);
	}
}

impl Display for ItemType
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		return write!(f, "{}", spaceOutCapitals(self.as_ref()));
	}
}

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum Proficiency
{
	None,
	Half,
	Proficient,
	Double,
}

impl Proficiency
{
	pub fn asTitle(&self) -> String
	{
		return match self
		{
			Self::None => "No Proficiency".to_string(),
			Self::Half => "Half Proficiency".to_string(),
			Self::Proficient => "Proficiency".to_string(),
			Self::Double => "Expertise".to_string(),
		};
	}
}

impl Default for Proficiency
{
	fn default() -> Self
	{
		return Self::None;
	}
}

impl Display for Proficiency
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		return write!(f, "{}", self.as_ref());
	}
}

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum ProficiencyType
{
	Armor,
	Language,
	Skill,
	Tool,
	Weapon,
}

impl Default for ProficiencyType
{
	fn default() -> Self
	{
		return Self::Armor;
	}
}

impl Display for ProficiencyType
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		return write!(f, "{}", self.as_ref());
	}
}

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum Skill
{
	Acrobatics,
	AnimalHandling,
	Arcana,
	Athletics,
	Deception,
	History,
	Insight,
	Intimidation,
	Investigation,
	Medicine,
	Nature,
	Perception,
	Performance,
	Persuasion,
	Religion,
	SleightOfHand,
	Stealth,
	Survival,
}

impl Default for Skill
{
	fn default() -> Self
	{
		return Self::Acrobatics;
	}
}

impl Display for Skill
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		return write!(f, "{}", spaceOutCapitals(self.as_ref()));
	}
}

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, Eq, EnumCount, EnumIter, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum Tool
{
	Artisan,
	Gaming,
	Herbalism,
	Music,
	Navigator,
	Poisoner,
	Thief,
	Vehicle(VehicleType),
}

impl From<VehicleType> for Tool
{
	fn from(vt: VehicleType) -> Self
	{
		return Self::Vehicle(vt);
	}
}

impl Default for Tool
{
	fn default() -> Self
	{
		return Self::Artisan;
	}
}

impl Display for Tool
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		return write!(f, "{}", self.as_ref());
	}
}

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum VehicleType
{
	Land,
	Water,
}

impl Default for VehicleType
{
	fn default() -> Self
	{
		return Self::Land;
	}
}

impl Display for VehicleType
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		return write!(f, "{}", self.as_ref());
	}
}

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum WeaponProficiency
{
	Simple,
	Martial,
}

impl Default for WeaponProficiency
{
	fn default() -> Self
	{
		return Self::Simple;
	}
}

impl Display for WeaponProficiency
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		return write!(f, "{}", self.as_ref());
	}
}

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum WeaponProperty
{
	Ammunition,
	Finesse,
	Heavy,
	Light,
	Loading,
	Range,
	Reach,
	Special,
	Thrown,
	TwoHanded,
	Versatile,
}

impl Default for WeaponProperty
{
	fn default() -> Self
	{
		return Self::Ammunition;
	}
}

impl Display for WeaponProperty
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		return match self
		{
			WeaponProperty::TwoHanded => write!(f, "{}", "Two-Handed"),
			_ => write!(f, "{}", self.as_ref()),
		};
	}
}

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum WeaponRange
{
	Melee,
	Ranged,
}

impl Default for WeaponRange
{
	fn default() -> Self
	{
		return Self::Melee;
	}
}

impl Display for WeaponRange
{
	fn fmt(&self, f: &mut Formatter<'_>) -> Result
	{
		return write!(f, "{}", self.as_ref());
	}
}
