#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::BTreeMap;
use dioxus::prelude::*;
use crate::{
	cod::{
		advantages::{
			CoreAdvantages,
			CoreAdvantageType,
		},
		merits::Merit,
		tracks::{
			Tracker,
			TrackerState,
		},
		traits::{
			CoreAttributeType,
			CoreSkillType,
		}
	},
};

/// A Chronicles of Darkness character's Advantages common across all games, such as Health, Willpower, and Size.
pub static CharacterAdvantages: AtomRef<CoreAdvantages> = |_| CoreAdvantages::default();
/// A Chronicles of Darkness character's list of Aspirations.
pub static CharacterAspirations: AtomRef<Vec<String>> = |_| Vec::<String>::new();
/// A Chronicles of Darkness character's Attributes, such as Strength and Presence.
pub static CharacterAttributes: AtomRef<BTreeMap<CoreAttributeType, usize>> = |_| CoreAttributeType::asMap();
/// A Chronicles of Darkness character's Beats Track.
pub static CharacterBeats: AtomRef<Tracker> = |_| Tracker::new(5);
/// A Chronicles of Darkness character's current Experience.
pub static CharacterExperience: Atom<usize> = |_| 0;
/// A Chronicles of Darkness character's list of Merits.
pub static CharacterMerits: AtomRef<Vec<Merit>> = |_| Vec::<Merit>::new();
/// A Chronicles of Darkness character's list of Skills, such as Athletics and Investigation.
pub static CharacterSkills: AtomRef<BTreeMap<CoreSkillType, usize>> = |_| CoreSkillType::asMap();
/// A Chronicles of Darkness character's list of Skill Specialties.
pub static CharacterSpecialties: AtomRef<Vec<String>> = |_| Vec::<String>::new();

/// Update the designated Advantage's value.
/// 
/// Automaticaly updates any affected Traits.
pub fn updateBaseAdvantage<T>(cx: &Scope<T>, advantage: CoreAdvantageType, value: usize)
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let attributesRef = use_atom_ref(&cx, CharacterAttributes);
	
	let mut advantages = advantagesRef.write();
	let attributes = attributesRef.read();
	
	match advantage
	{
		CoreAdvantageType::Defense => { advantages.defense = value; }
		CoreAdvantageType::Health => { advantages.health.updateMax(value); }
		CoreAdvantageType::Initiative => { advantages.initiative = value; }
		
		CoreAdvantageType::Size =>
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
			let healthMax = attributes[&CoreAttributeType::Stamina] + finalValue;
			
			advantages.size = finalValue;
			advantages.health.updateMax(healthMax);
		}
		
		CoreAdvantageType::Speed => { advantages.speed = value; }
		CoreAdvantageType::Willpower => { advantages.willpower.updateMax(value); }
	}
}

/// Update the designated Attribute's value.
/// 
/// Automaticaly updates any affected Traits.
pub fn updateBaseAttribute<T>(cx: &Scope<T>, attributeType: &CoreAttributeType, value: usize)
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
		CoreAttributeType::Composure =>
		{
			advantages.initiative = attributes[&CoreAttributeType::Dexterity] + attributes[&CoreAttributeType::Composure];
			advantages.willpower.updateMax(attributes[&CoreAttributeType::Resolve] + attributes[&CoreAttributeType::Composure]);
		}
		
		CoreAttributeType::Dexterity =>
		{
			let defense = match attributes[&CoreAttributeType::Dexterity] <= attributes[&CoreAttributeType::Wits]
			{
				true => { attributes[&CoreAttributeType::Dexterity] }
				false => { attributes[&CoreAttributeType::Wits] }
			} + skills[&CoreSkillType::Athletics];
			
			advantages.defense = defense;
			advantages.initiative = attributes[&CoreAttributeType::Dexterity] + attributes[&CoreAttributeType::Composure];
			advantages.speed = advantages.size + attributes[&CoreAttributeType::Dexterity] + attributes[&CoreAttributeType::Strength];
		}
		
		CoreAttributeType::Resolve => { advantages.willpower.updateMax(attributes[&CoreAttributeType::Composure] + attributes[&CoreAttributeType::Resolve]); }
		
		CoreAttributeType::Stamina =>
		{
			let size = advantages.size;
			advantages.health.updateMax(size + attributes[&CoreAttributeType::Stamina]);
		}
		
		CoreAttributeType::Strength => { advantages.speed = advantages.size + attributes[&CoreAttributeType::Dexterity] + attributes[&CoreAttributeType::Strength]; }
		
		CoreAttributeType::Wits =>
		{
			let defense = match attributes[&CoreAttributeType::Dexterity] <= attributes[&CoreAttributeType::Wits]
			{
				true => { attributes[&CoreAttributeType::Dexterity] }
				false => { attributes[&CoreAttributeType::Wits]}
			} + skills[&CoreSkillType::Athletics];
			
			advantages.defense = defense;
		}
		
		_ => {}
	}
}

/// Update the value of the Beats Track.
/// 
/// When the Track is filled, automatically clears the Beats Track and adds one Experience.
pub fn updateBaseBeats<T>(cx: &Scope<T>, index: usize, overrideValues: bool)
{
	let beatsRef = use_atom_ref(&cx, CharacterBeats);
	let mut beats = beatsRef.write();
	
	updateTrackerState_SingleState(&mut beats, index, TrackerState::Two, overrideValues);
}

/// Update the designated Skill's value.
/// 
/// Automaticaly updates any affected Traits.
pub fn updateBaseSkill<T>(cx: &Scope<T>, skillType: &CoreSkillType, value: usize)
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
		CoreSkillType::Athletics =>
		{
			let attributeDefense = match attributes[&CoreAttributeType::Dexterity] <= attributes[&CoreAttributeType::Wits]
			{
				true => { attributes[&CoreAttributeType::Dexterity] }
				false => { attributes[&CoreAttributeType::Wits] }
			};
			
			advantages.defense = attributeDefense + skills[&CoreSkillType::Athletics];
		}
		
		_ => {}
	}
}

/// Update the value of the Health Track.
/// 
/// Handles adding, upgrading, and removing damage.
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

/// Update the value of the Willpower Track.
pub fn updateBaseWillpower<T>(cx: &Scope<T>, index: usize)
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let mut advantages = advantagesRef.write();
	
	updateTrackerState_SingleState(&mut advantages.willpower, index, TrackerState::Two, false);
}

/// Update the value of a single-state Track, such as Willpower.
/// 
/// This Track only requires a single state, as opposed to the default of three.
/// Clicking on a box should fill the Track up to that point, empty
/// the Track down to that point, or empty the clicked box.
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

/// Reset all `cod::state` global state values.
pub fn resetGlobalStateCod<T>(cx: &Scope<T>)
{
	let characterAdvantages = use_atom_ref(cx, CharacterAdvantages);
	let characterAspirations = use_atom_ref(cx, CharacterAspirations);
	let characterAttributes = use_atom_ref(cx, CharacterAttributes);
	let characterBeats = use_atom_ref(cx, CharacterBeats);
	let characterExperience = use_set(cx, CharacterExperience);
	let characterMerits = use_atom_ref(cx, CharacterMerits);
	let characterSkills = use_atom_ref(cx, CharacterSkills);
	let characterSpecialties = use_atom_ref(cx, CharacterSpecialties);
	
	(*characterAdvantages.write()) = CoreAdvantages::default();
	(*characterAspirations.write()) = Vec::<String>::new();
	(*characterAttributes.write()) = CoreAttributeType::asMap();
	(*characterBeats.write()) = Tracker::new(5);
	characterExperience(0);
	(*characterMerits.write()) = Vec::<Merit>::new();
	(*characterSkills.write()) = CoreSkillType::asMap();
	(*characterSpecialties.write()) = Vec::<String>::new();
}
