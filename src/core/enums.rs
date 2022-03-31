#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::BTreeMap;
use strum::IntoEnumIterator;
use strum_macros::{
	AsRefStr,
	EnumCount,
	EnumIter,
};

/// Game systems for which character sheets have been implemented.
#[derive(AsRefStr, Clone, Copy, Debug, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Ord)]
pub enum GameSystem
{
	CodMortal,
		//CodBeast,
	CodChangeling2e,
		//CodDemon,
		//CodGeist,
		//CodHunter,
	CodMage2e,
		//CodMummy,
		//CodPromethean,
	CodVampire2e,
		//CodWerewolf,
	//Dnd5e,
	//WodVampireV5,
}

impl GameSystem
{
	/// Generate a collection mapping GameSystem to their full human-readable titles.
	pub fn asMap() -> BTreeMap<Self, String>
	{
		let mut map = BTreeMap::new();
		map.insert(GameSystem::CodMortal, "Chronicles of Darkness".to_string());
		map.insert(GameSystem::CodChangeling2e, "Changeling: The Lost 2e".to_string());
		map.insert(GameSystem::CodMage2e, "Mage: The Awakening 2e".to_string());
		map.insert(GameSystem::CodVampire2e, "Vampire: The Requiem 2e".to_string());
		//map.insert(GameSystem::Dnd5e, "Dungeons & Dragons 5E".to_string());
		//map.insert(GameSystem::WodVampireV5, "Vampire: The Masquerade V5".to_string());
		return map;
	}
	
	/// Generate a collection mapping GameSystem to a boolean value.
	/// 
	/// Used in tandem with `CurrentGameSystem` to determine whether or not to display
	/// UI elements in the `App` component.
	pub fn showMap() -> BTreeMap<Self, bool>
	{
		let mut map = BTreeMap::new();
		for gs in GameSystem::iter()
		{
			map.insert(gs, false);
		}
		return map;
	}
}