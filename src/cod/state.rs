#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use std::collections::HashMap;
use crate::{
	cod::{
		advantages::{
			BaseAdvantages,
			BaseAdvantageType,
		},
		merits::{
			Merit,
		},
		tracks::{
			TrackerState,
		},
		traits::{
			BaseAttribute,
			BaseAttributeType,
			BaseSkill,
			BaseSkillType,
		}
	},
};

pub static CharacterAdvantages: AtomRef<BaseAdvantages> = |_| BaseAdvantages::default();
pub static CharacterAttributes: AtomRef<HashMap<BaseAttributeType, BaseAttribute>> = |_| BaseAttribute::newAllAttributes();
pub static CharacterMerits: AtomRef<Vec<Merit>> = |_| Vec::<Merit>::new();
pub static CharacterSkills: AtomRef<HashMap<BaseSkillType, BaseSkill>> = |_| BaseSkill::newAllSkills();

pub fn updateBaseAdvantage<T>(cx: &Scope<T>, advantage: BaseAdvantageType, value: usize)
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let attributesRef = use_atom_ref(&cx, CharacterAttributes);
	
	let mut advantages = advantagesRef.write();
	let attributes = attributesRef.read();
	
	match advantage
	{
		BaseAdvantageType::Defense => { advantages.defense = value; }
		BaseAdvantageType::Health => { advantages.health.updateMax(value); }
		BaseAdvantageType::Initiative => { advantages.initiative = value; }
		
		BaseAdvantageType::Size =>
		{
			let finalValue = match value < 1
			{
				true => { 1 }
				false => match value > 10
				{
					true => { 10 }
					false => { value }
				}
			};
			let healthMax = attributes[&BaseAttributeType::Stamina].value + finalValue;
			
			advantages.size = finalValue;
			advantages.health.updateMax(healthMax);
		}
		
		BaseAdvantageType::Speed => { advantages.speed = value; }
		BaseAdvantageType::Willpower => { advantages.willpower.updateMax(value); }
	}
}

pub fn updateBaseAttribute<T>(cx: &Scope<T>, attributeType: &BaseAttributeType, value: usize)
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let attributesRef = use_atom_ref(&cx, CharacterAttributes);
	let skillsRef = use_atom_ref(&cx, CharacterSkills);
	
	let mut advantages = advantagesRef.write();
	let mut attributes = attributesRef.write();
	let skills = skillsRef.read();
	
	attributes.get_mut(attributeType).unwrap().value = value;
	
	match attributeType
	{
		BaseAttributeType::Composure =>
		{
			advantages.initiative = attributes[&BaseAttributeType::Dexterity].value + attributes[&BaseAttributeType::Composure].value;
			advantages.willpower.updateMax(attributes[&BaseAttributeType::Resolve].value + attributes[&BaseAttributeType::Composure].value);
		}
		
		BaseAttributeType::Dexterity =>
		{
			let defense = match attributes[&BaseAttributeType::Dexterity].value <= attributes[&BaseAttributeType::Wits].value
			{
				true => { attributes[&BaseAttributeType::Dexterity].value }
				false => { attributes[&BaseAttributeType::Wits].value }
			} + skills[&BaseSkillType::Athletics].value;
			
			advantages.defense = defense;
			advantages.initiative = attributes[&BaseAttributeType::Dexterity].value + attributes[&BaseAttributeType::Composure].value;
			advantages.speed = advantages.size + attributes[&BaseAttributeType::Dexterity].value + attributes[&BaseAttributeType::Strength].value;
		}
		
		BaseAttributeType::Resolve => { advantages.willpower.updateMax(attributes[&BaseAttributeType::Composure].value + attributes[&BaseAttributeType::Resolve].value); }
		
		BaseAttributeType::Stamina =>
		{
			let size = advantages.size;
			advantages.health.updateMax(size + attributes[&BaseAttributeType::Stamina].value);
		}
		
		BaseAttributeType::Strength => { advantages.speed = advantages.size + attributes[&BaseAttributeType::Dexterity].value + attributes[&BaseAttributeType::Strength].value; }
		
		BaseAttributeType::Wits =>
		{
			let defense = match attributes[&BaseAttributeType::Dexterity].value <= attributes[&BaseAttributeType::Wits].value
			{
				true => { attributes[&BaseAttributeType::Dexterity].value }
				false => { attributes[&BaseAttributeType::Wits].value }
			} + skills[&BaseSkillType::Athletics].value;
			
			advantages.defense = defense;
		}
		
		_ => {}
	}
}

pub fn updateBaseSkill<T>(cx: &Scope<T>, skillType: &BaseSkillType, value: usize)
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let attributesRef = use_atom_ref(cx, CharacterAttributes);
	let skillsRef = use_atom_ref(&cx, CharacterSkills);
	
	let mut advantages = advantagesRef.write();
	let attributes = attributesRef.read();
	let mut skills = skillsRef.write();
	
	skills.get_mut(skillType).unwrap().value = value;
	
	// Handle 
	match skillType
	{
		BaseSkillType::Athletics =>
		{
			let attributeDefense = match attributes[&BaseAttributeType::Dexterity].value <= attributes[&BaseAttributeType::Wits].value
			{
				true => { attributes[&BaseAttributeType::Dexterity].value }
				false => { attributes[&BaseAttributeType::Wits].value }
			};
			
			advantages.defense = attributeDefense + skills[&BaseSkillType::Athletics].value;
		}
		
		_ => {}
	}
}

pub fn updateBaseHealth<T>(cx: &Scope<T>, damageType: TrackerState, remove: bool, index: Option<usize>)
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let mut advantages = advantagesRef.write();
	
	if remove
	{
		advantages.health.remove(damageType);
	}
	else
	{
		match index
		{
			Some(i) => { advantages.health.update(damageType, i, false); }
			None => { advantages.health.add(damageType); }
		}
	}
}

pub fn updateBaseWillpower<T>(cx: &Scope<T>, index: usize)
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let mut advantages = advantagesRef.write();
	
	let len = advantages.willpower.values.len();
	
	if index >= len
	{
		for _ in len..index+1 { advantages.willpower.add(TrackerState::Two) }
	}
	else if index < len
	{
		for _ in index..len { advantages.willpower.remove(TrackerState::Two) }
		
		// If we're trying to "disable" more than one box, add one back in.
		// People naturally click where they want the checks to stop
		// not one above where they want them to stop.
		// However, this makes clicking the top most checked box act weird
		// thus... the if.
		if len - index > 1
		{
			advantages.willpower.add(TrackerState::Two);
		}
	}
}
