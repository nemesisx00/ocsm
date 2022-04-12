#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::BTreeMap;
use crate::dnd::fifth::enums::{
	Proficiency,
};

pub const AbilityModifierSubtractor: isize = 5;
pub const DefaultProficiencyBonus: isize = 2;
pub const MinimumSpellLevel: usize = 1;
pub const MaximumSpellLevel: usize = 9;

/// Generate the Ability Modifier value based on an Ability Score value.
pub fn calculateAbilityModifier(score: isize) -> isize
{
	return (score / 2) - AbilityModifierSubtractor;
}

/// Generate human-readable output for a Modifier value.
pub fn displayModifier(modifier: isize) -> String
{
	return match modifier >= 0
	{
		true => format!("+{}", modifier),
		false => format!("{}", modifier),
	};
}

/// Generate the numeric Proficiency modifier value based on a Proficiency Bonus and a Proficiency.
pub fn getProficiencyModifier(proficiencyBonus: isize, proficiency: Proficiency) -> isize
{
	return match proficiency
	{
		Proficiency::Proficient => proficiencyBonus,
		Proficiency::Double => proficiencyBonus * 2,
		Proficiency::Half => proficiencyBonus / 2,
		Proficiency::None => 0,
	};
}

/// Generate a map of the amount of available Spell Slots per Spell Level for a Character Level.
pub fn getSpellSlots(level: usize) -> BTreeMap<usize, usize>
{
	let mut slots = BTreeMap::new();
	for sl in MinimumSpellLevel..=MaximumSpellLevel
	{
		slots.insert(sl, getSpellSlotsForLevel(level, sl));
	}
	return slots;
}

/// Generate the amount of available Spell Slots for a Character Level and Spell Level.
pub fn getSpellSlotsForLevel(characterLevel: usize, spellLevel: usize) -> usize
{
	return match spellLevel
	{
		1 => match characterLevel
		{
			0 => 0,
			1 => 2,
			2 => 3,
			_ => 4,
		},
		
		2 => match characterLevel
		{
			0..=2 => 0,
			3 => 2,
			_ => 3,
		},
		
		3 => match characterLevel
		{
			0..=4 => 0,
			5 => 2,
			_ => 3,
		},
		
		4 => match characterLevel
		{
			0..=6 => 0,
			7 => 1,
			8 => 2,
			_ => 3,
		},
		
		5 => match characterLevel
		{
			0..=8 => 0,
			9 => 1,
			10..=16 => 2,
			_ => 3,
		},
		
		6 => match characterLevel
		{
			0..=10 => 0,
			11..=18 => 1,
			_ => 2,
		},
		
		7 => match characterLevel
		{
			0..=12 => 0,
			13..=19 => 1,
			_ => 2,
		},
		
		8 => match characterLevel
		{
			0..=14 => 0,
			_ => 1,
		},
		
		9 => match characterLevel
		{
			0..=16 => 0,
			_ => 1,
		},
		
		_ => 0,
	};
}
