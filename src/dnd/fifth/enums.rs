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

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum FeatureType
{
	Background,
	Class,
	Feat,
	Racial,
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
		return Self::Weapon(wp, WeaponRange::default());
	}
}

impl From<WeaponRange> for ItemType
{
	fn from(wr: WeaponRange) -> Self
	{
		return Self::Weapon(WeaponProficiency::default(), wr);
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

impl Default for Proficiency
{
	fn default() -> Self
	{
		return Self::None;
	}
}

// --------------------------------------------------

#[derive(AsRefStr, Clone, Copy, Debug, Deserialize, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Serialize, Ord)]
pub enum ProficiencyType
{
	Armor,
	Language,
	Tool,
	Weapon,
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

impl Default for Tool
{
	fn default() -> Self
	{
		return Self::Artisan;
	}
}

impl From<VehicleType> for Tool
{
	fn from(vt: VehicleType) -> Self
	{
		return Self::Vehicle(vt);
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
