#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::HashMap;
use dioxus::prelude::*;
use crate::{
	cod::{
		advantages::{
			BaseAdvantages,
			BaseAdvantageType,
		},
		merits::Merit,
		tracks::{
			Tracker,
			TrackerState,
		},
		traits::{
			BaseAttributeType,
			BaseSkillType,
		}
	},
};

pub static CharacterAdvantages: AtomRef<BaseAdvantages> = |_| BaseAdvantages::default();
pub static CharacterAspirations: AtomRef<Vec<String>> = |_| Vec::<String>::new();
pub static CharacterAttributes: AtomRef<HashMap<BaseAttributeType, usize>> = |_| BaseAttributeType::asMap();
pub static CharacterBeats: AtomRef<Tracker> = |_| Tracker::new(5);
pub static CharacterExperience: Atom<usize> = |_| 0;
pub static CharacterMerits: AtomRef<Vec<Merit>> = |_| Vec::<Merit>::new();
pub static CharacterSkills: AtomRef<HashMap<BaseSkillType, usize>> = |_| BaseSkillType::asMap();
pub static CharacterSpecialties: AtomRef<Vec<String>> = |_| Vec::<String>::new();

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
			let healthMax = attributes[&BaseAttributeType::Stamina] + finalValue;
			
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
	
	match attributes.get_mut(attributeType)
	{
		Some(attr) => { *attr = value; }
		None => {}
	}
	
	match attributeType
	{
		BaseAttributeType::Composure =>
		{
			advantages.initiative = attributes[&BaseAttributeType::Dexterity] + attributes[&BaseAttributeType::Composure];
			advantages.willpower.updateMax(attributes[&BaseAttributeType::Resolve] + attributes[&BaseAttributeType::Composure]);
		}
		
		BaseAttributeType::Dexterity =>
		{
			let defense = match attributes[&BaseAttributeType::Dexterity] <= attributes[&BaseAttributeType::Wits]
			{
				true => { attributes[&BaseAttributeType::Dexterity] }
				false => { attributes[&BaseAttributeType::Wits] }
			} + skills[&BaseSkillType::Athletics];
			
			advantages.defense = defense;
			advantages.initiative = attributes[&BaseAttributeType::Dexterity] + attributes[&BaseAttributeType::Composure];
			advantages.speed = advantages.size + attributes[&BaseAttributeType::Dexterity] + attributes[&BaseAttributeType::Strength];
		}
		
		BaseAttributeType::Resolve => { advantages.willpower.updateMax(attributes[&BaseAttributeType::Composure] + attributes[&BaseAttributeType::Resolve]); }
		
		BaseAttributeType::Stamina =>
		{
			let size = advantages.size;
			advantages.health.updateMax(size + attributes[&BaseAttributeType::Stamina]);
		}
		
		BaseAttributeType::Strength => { advantages.speed = advantages.size + attributes[&BaseAttributeType::Dexterity] + attributes[&BaseAttributeType::Strength]; }
		
		BaseAttributeType::Wits =>
		{
			let defense = match attributes[&BaseAttributeType::Dexterity] <= attributes[&BaseAttributeType::Wits]
			{
				true => { attributes[&BaseAttributeType::Dexterity] }
				false => { attributes[&BaseAttributeType::Wits]}
			} + skills[&BaseSkillType::Athletics];
			
			advantages.defense = defense;
		}
		
		_ => {}
	}
}

pub fn updateBaseBeats<T>(cx: &Scope<T>, index: usize, overrideValues: bool)
{
	let beatsRef = use_atom_ref(&cx, CharacterBeats);
	let mut beats = beatsRef.write();
	
	updateTrackerState_SingleState(&mut beats, index, TrackerState::Two, overrideValues);
}

pub fn updateBaseSkill<T>(cx: &Scope<T>, skillType: &BaseSkillType, value: usize)
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let attributesRef = use_atom_ref(cx, CharacterAttributes);
	let skillsRef = use_atom_ref(&cx, CharacterSkills);
	
	let mut advantages = advantagesRef.write();
	let attributes = attributesRef.read();
	let mut skills = skillsRef.write();
	
	match skills.get_mut(skillType)
	{
		Some(ski) => { *ski = value; }
		None => {}
	}
	
	// Handle 
	match skillType
	{
		BaseSkillType::Athletics =>
		{
			let attributeDefense = match attributes[&BaseAttributeType::Dexterity] <= attributes[&BaseAttributeType::Wits]
			{
				true => { attributes[&BaseAttributeType::Dexterity] }
				false => { attributes[&BaseAttributeType::Wits] }
			};
			
			advantages.defense = attributeDefense + skills[&BaseSkillType::Athletics];
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
	
	updateTrackerState_SingleState(&mut advantages.willpower, index, TrackerState::Two, false);
}

/// 
pub fn updateTrackerState_SingleState(tracker: &mut Tracker, index: usize, state: TrackerState, overrideValues: bool)
{
	let len = tracker.values.len();
	
	if overrideValues
	{
		tracker.values.truncate(0);
		(0..index).for_each(|_| tracker.add(state));
	}
	else
	{
		if index >= len
		{
			(len..index+1).for_each(|_| tracker.add(state));
		}
		else if index < len
		{
			(index..len).for_each(|_| tracker.remove(state));
			
			// If we're trying to "disable" more than one box, add one back in.
			// People naturally click where they want the checks to stop
			// not one above where they want them to stop.
			// However, this makes clicking the top most checked box act weird
			// thus... the if.
			if len - index > 1
			{
				tracker.add(state);
			}
		}
	}
}
