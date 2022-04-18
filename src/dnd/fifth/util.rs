#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::{
	BTreeMap,
	HashMap,
};
use crate::dnd::fifth::{
	enums::{
		CasterWeight,
		Proficiency,
	},
	structs::{
		ClassLevel,
	},
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

/// Calculate the total Caster Level based on the classes' CasterWeight.
pub fn calculateCasterLevel(levels: Vec<ClassLevel>) -> isize
{
	let mut full = HashMap::<String, isize>::new();
	let mut half = HashMap::<String, isize>::new();
	let mut third = HashMap::<String, isize>::new();
	
	levels.iter().for_each(|cl|
	{
		if let Some(weight) = cl.caster
		{
			match weight
			{
				CasterWeight::Full =>
				{
					if let Some(level) = full.get_mut(&cl.class)
					{
						if cl.level > *level
						{
							*level = cl.level;
						}
					}
					else
					{
						full.insert(cl.class.clone(), cl.level);
					}
				},
				CasterWeight::Half =>
				{
					if let Some(level) = half.get_mut(&cl.class)
					{
						if cl.level > *level
						{
							*level = cl.level;
						}
					}
					else
					{
						half.insert(cl.class.clone(), cl.level);
					}
				},
				CasterWeight::Third =>
				{
					if let Some(level) = third.get_mut(&cl.class)
					{
						if cl.level > *level
						{
							*level = cl.level;
						}
					}
					else
					{
						third.insert(cl.class.clone(), cl.level);
					}
				},
			}
		}
	});
	
	let mut fullTotal = 0;
	let mut halfTotal = 0;
	let mut thirdTotal = 0;
	
	full.iter().for_each(|(_, level)| fullTotal += level);
	half.iter().for_each(|(_, level)| halfTotal += level);
	third.iter().for_each(|(_, level)| thirdTotal += level);
	
	return fullTotal + (halfTotal / 2) + (thirdTotal / 3);
}

/// Calculate the total Character Level.
pub fn calculateCharacterLevel(levels: Vec<ClassLevel>) -> isize
{
	let classes = getHighestLevels(levels);
	let mut characterLevel = 0;
	classes.iter().for_each(|(_, level)| characterLevel += level);
	
	return characterLevel;
}

/// Extract the highest level for each class.
pub fn getHighestLevels(levels: Vec<ClassLevel>) -> BTreeMap<String, isize>
{
	let mut classes = BTreeMap::<String, isize>::new();
	levels.iter().for_each(|cl|
	{
		if let Some(level) = classes.get_mut(&cl.class)
		{
			if cl.level > *level
			{
				*level = cl.level;
			}
		}
		else
		{
			classes.insert(cl.class.clone(), cl.level);
		}
	});
	
	return classes.clone();
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
pub fn getSpellSlots(level: isize) -> BTreeMap<usize, usize>
{
	let mut slots = BTreeMap::new();
	for sl in MinimumSpellLevel..=MaximumSpellLevel
	{
		slots.insert(sl, getSpellSlotsForLevel(level, sl));
	}
	return slots;
}

/// Generate the amount of available Spell Slots for a Character Level and Spell Level.
pub fn getSpellSlotsForLevel(characterLevel: isize, spellLevel: usize) -> usize
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

// --------------------------------------------------

#[cfg(test)]
mod tests
{
	use super::*;
	use std::collections::HashMap;
	
	fn getTestClassLevels() -> Vec<ClassLevel>
	{
		// Ranger (2) / Rogue - Arcane Trickster (3) / Wizard (3)
		return vec![
			ClassLevel { class: "Wizard".to_string(), level: 1, caster: Some(CasterWeight::Full) },
			ClassLevel { class: "Wizard".to_string(), level: 2, caster: Some(CasterWeight::Full) },
			ClassLevel { class: "Ranger".to_string(), level: 2, caster: Some(CasterWeight::Half) },
			ClassLevel { class: "Rogue".to_string(), level: 1, caster: None },
			ClassLevel { class: "Wizard".to_string(), level: 3, caster: Some(CasterWeight::Full) },
			ClassLevel { class: "Rogue".to_string(), level: 2, caster: None },
			ClassLevel { class: "Ranger".to_string(), level: 1, caster: Some(CasterWeight::Half) },
			ClassLevel { class: "Rogue".to_string(), level: 3, caster: Some(CasterWeight::Third) },
		];
	}
	
	#[test]
	fn fn_calculateCasterLevel()
	{
		let expected = 5;
		let result = calculateCasterLevel(getTestClassLevels().clone());
		assert_eq!(expected, result);
	}
	
	#[test]
	fn fn_calculateCharacterLevel()
	{
		let expected = 8;
		let result = calculateCharacterLevel(getTestClassLevels().clone());
		assert_eq!(expected, result);
	}
	
	#[test]
	fn fn_getHighestLevels()
	{
		let mut expected = BTreeMap::new();
		expected.insert("Ranger".to_string(), 2);
		expected.insert("Rogue".to_string(), 3);
		expected.insert("Wizard".to_string(), 3);
		
		let result = getHighestLevels(getTestClassLevels().clone());
		assert_eq!(expected, result);
	}
	
	#[test]
	fn fn_getSpellSlots_5()
	{
		let mut expected = BTreeMap::<usize, usize>::new();
		expected.insert(1, 4);
		expected.insert(2, 3);
		expected.insert(3, 2);
		expected.insert(4, 0);
		expected.insert(5, 0);
		expected.insert(6, 0);
		expected.insert(7, 0);
		expected.insert(8, 0);
		expected.insert(9, 0);
		
		let casterLevel = 5;
		let result = getSpellSlots(casterLevel);
		
		assert_eq!(expected, result);
	}
	
	#[test]
	fn fn_getSpellSlots_20()
	{
		let mut expected = BTreeMap::<usize, usize>::new();
		expected.insert(1, 4);
		expected.insert(2, 3);
		expected.insert(3, 3);
		expected.insert(4, 3);
		expected.insert(5, 3);
		expected.insert(6, 2);
		expected.insert(7, 2);
		expected.insert(8, 1);
		expected.insert(9, 1);
		
		let casterLevel = 20;
		let result = getSpellSlots(casterLevel);
		
		assert_eq!(expected, result);
	}
}
