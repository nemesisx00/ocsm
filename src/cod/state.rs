#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::BTreeMap;
use dioxus::prelude::{
	Atom,
	AtomRef,
	Scope,
	use_atom_ref,
	use_set,
};
use crate::{
	cod::{
		enums::{
			CoreAdvantage,
			CoreAttribute,
			CoreDetail,
			CoreSkill,
			TrackerState,
		},
		structs::{
			CoreAdvantages,
			Tracker,
		},
	},
};

/// The base speed value of a Chronicles of Darkness character before applying Trait values.
pub const BaseSpeed: usize = 5;

/// A Chronicles of Darkness character's Advantages common across all games, such as Health, Willpower, and Size.
pub static CharacterAdvantages: AtomRef<CoreAdvantages> = |_| CoreAdvantages::default();
/// A Chronicles of Darkness character's list of Aspirations.
pub static CharacterAspirations: AtomRef<Vec<String>> = |_| Vec::<String>::new();
/// A Chronicles of Darkness character's Attributes, such as Strength and Presence.
pub static CharacterAttributes: AtomRef<BTreeMap<CoreAttribute, usize>> = |_| CoreAttribute::asMap();
/// A Chronicles of Darkness character's Beats Track.
pub static CharacterBeats: AtomRef<Tracker> = |_| Tracker::new(5);
/// A Chronicles of Darkness character's list of Conditions.
pub static CharacterConditions: AtomRef<Vec<String>> = |_| Vec::<String>::new();
/// A Chronicles of Darkness character's core Details.
pub static CharacterDetails: AtomRef<BTreeMap<CoreDetail, String>> = |_| CoreDetail::asMap();
/// A Chronicles of Darkness character's current Experience.
pub static CharacterExperience: Atom<usize> = |_| 0;
/// A Chronicles of Darkness character's list of Merits.
pub static CharacterMerits: AtomRef<Vec<(String, usize)>> = |_| Vec::<(String, usize)>::new();
/// A Chronicles of Darkness character's list of Skills, such as Athletics and Investigation.
pub static CharacterSkills: AtomRef<BTreeMap<CoreSkill, usize>> = |_| CoreSkill::asMap();
/// A Chronicles of Darkness character's list of Skill Specialties.
pub static CharacterSpecialties: AtomRef<Vec<String>> = |_| Vec::<String>::new();

// --------------------------------------------------

/// Update the designated Advantage's value.
/// 
/// Automaticaly updates any affected Traits.
pub fn updateCoreAdvantage<T>(cx: &Scope<T>, advantage: CoreAdvantage, value: usize)
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let attributesRef = use_atom_ref(&cx, CharacterAttributes);
	
	let mut advantages = advantagesRef.write();
	let attributes = attributesRef.read();
	
	match advantage
	{
		CoreAdvantage::Defense => advantages.defense = value,
		CoreAdvantage::Health => advantages.health.updateMax(value),
		CoreAdvantage::Initiative => advantages.initiative = value,
		CoreAdvantage::Integrity => advantages.integrity = value,
		CoreAdvantage::Power =>
		{
			advantages.power = Some(value);
			if let Some(ref mut resource) = advantages.resource
			{
				resource.updateMax(getResourceMax(value))
			}
		},
		CoreAdvantage::Resource => if let Some(ref mut resource) = advantages.resource { resource.updateMax(value) },
		CoreAdvantage::Size =>
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
			let healthMax = attributes[&CoreAttribute::Stamina] + finalValue;
			
			advantages.size = finalValue;
			advantages.health.updateMax(healthMax);
		},
		CoreAdvantage::Speed => advantages.speed = value,
		CoreAdvantage::Willpower => advantages.willpower.updateMax(value),
	}
}

/// Update the designated Attribute's value.
/// 
/// Automaticaly updates any affected Traits.
pub fn updateCoreAttribute<T>(cx: &Scope<T>, attributeType: &CoreAttribute, value: usize)
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
		CoreAttribute::Composure =>
		{
			advantages.initiative = attributes[&CoreAttribute::Dexterity] + attributes[&CoreAttribute::Composure];
			advantages.willpower.updateMax(attributes[&CoreAttribute::Resolve] + attributes[&CoreAttribute::Composure]);
		},
		CoreAttribute::Dexterity =>
		{
			let defense = match attributes[&CoreAttribute::Dexterity] <= attributes[&CoreAttribute::Wits]
			{
				true => { attributes[&CoreAttribute::Dexterity] }
				false => { attributes[&CoreAttribute::Wits] }
			} + skills[&CoreSkill::Athletics];
			
			advantages.defense = defense;
			advantages.initiative = attributes[&CoreAttribute::Dexterity] + attributes[&CoreAttribute::Composure];
			advantages.speed = advantages.size + attributes[&CoreAttribute::Dexterity] + attributes[&CoreAttribute::Strength];
		},
		CoreAttribute::Resolve => advantages.willpower.updateMax(attributes[&CoreAttribute::Composure] + attributes[&CoreAttribute::Resolve]),
		CoreAttribute::Stamina =>
		{
			let size = advantages.size;
			advantages.health.updateMax(size + attributes[&CoreAttribute::Stamina]);
		},
		CoreAttribute::Strength => advantages.speed = advantages.size + attributes[&CoreAttribute::Dexterity] + attributes[&CoreAttribute::Strength],
		CoreAttribute::Wits =>
		{
			let defense = match attributes[&CoreAttribute::Dexterity] <= attributes[&CoreAttribute::Wits]
			{
				true => { attributes[&CoreAttribute::Dexterity] }
				false => { attributes[&CoreAttribute::Wits]}
			} + skills[&CoreSkill::Athletics];
			
			advantages.defense = defense;
		},
		_ => {}
	}
}

/// Update the value of the Beats Track.
/// 
/// When the Track is filled, automatically clears the Beats Track and adds one Experience.
pub fn updateCoreBeats<T>(cx: &Scope<T>, index: usize, overrideValues: bool)
{
	let beatsRef = use_atom_ref(&cx, CharacterBeats);
	let mut beats = beatsRef.write();
	
	updateTrackerState_SingleState(&mut beats, index, TrackerState::Two, overrideValues);
}

/// Update the designated Detail.
pub fn updateCoreDetail<T>(cx: &Scope<T>, detailType: CoreDetail, value: String)
{
	let detailsRef = use_atom_ref(&cx, CharacterDetails);
	let mut details = detailsRef.write();
	
	match details.get_mut(&detailType)
	{
		Some(detail) => *detail = value,
		None => {}
	}
}

/// Update the designated Skill's value.
/// 
/// Automaticaly updates any affected Traits.
pub fn updateCoreSkill<T>(cx: &Scope<T>, skillType: &CoreSkill, value: usize)
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
		CoreSkill::Athletics =>
		{
			let attributeDefense = match attributes[&CoreAttribute::Dexterity] <= attributes[&CoreAttribute::Wits]
			{
				true => attributes[&CoreAttribute::Dexterity],
				false => attributes[&CoreAttribute::Wits]
			};
			
			advantages.defense = attributeDefense + skills[&CoreSkill::Athletics];
		},
		_ => {}
	}
}

/// Update the value of the Health Track.
/// 
/// Handles adding, upgrading, and removing damage.
pub fn updateCoreHealth<T>(cx: &Scope<T>, damageType: TrackerState, remove: bool, index: Option<usize>)
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
			Some(i) => advantages.health.update(damageType, i, false),
			None => advantages.health.add(damageType)
		}
	}
}

/// Update the value of the Resource Track.
pub fn updateCoreResource<T>(cx: &Scope<T>, index: usize)
{
	let advantagesRef = use_atom_ref(&cx, CharacterAdvantages);
	let mut advantages = advantagesRef.write();
	
	if let Some(ref mut resource) = advantages.resource
	{
		updateTrackerState_SingleState(resource, index, TrackerState::Two, false);
	}
}

/// Update the value of the Willpower Track.
pub fn updateCoreWillpower<T>(cx: &Scope<T>, index: usize)
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
	let characterConditions = use_atom_ref(cx, CharacterConditions);
	let characterDetails = use_atom_ref(cx, CharacterDetails);
	let characterExperience = use_set(cx, CharacterExperience);
	let characterMerits = use_atom_ref(cx, CharacterMerits);
	let characterSkills = use_atom_ref(cx, CharacterSkills);
	let characterSpecialties = use_atom_ref(cx, CharacterSpecialties);
	
	(*characterAdvantages.write()) = CoreAdvantages::default();
	(*characterAspirations.write()) = Vec::<String>::new();
	(*characterAttributes.write()) = CoreAttribute::asMap();
	(*characterBeats.write()) = Tracker::new(5);
	(*characterConditions.write()) = Vec::<String>::new();
	(*characterDetails.write()) = CoreDetail::asMap();
	characterExperience(0);
	(*characterMerits.write()) = Vec::<(String, usize)>::new();
	(*characterSkills.write()) = CoreSkill::asMap();
	(*characterSpecialties.write()) = Vec::<String>::new();
}

/// Get the maximum allowed Trait value based on Power.
pub fn getTraitMax(power: usize) -> usize
{
	return match power
	{
		0 => { 5 }
		1 => { 5 }
		2 => { 5 }
		3 => { 5 }
		4 => { 5 }
		5 => { 5 }
		6 => { 6 }
		7 => { 7 }
		8 => { 8 }
		9 => { 9 }
		10 => { 10 }
		_ => { 10 }
	};
}

/// Get the maximum Resource capacity based on Power.
pub fn getResourceMax(power: usize) -> usize
{
	return match power
	{
		0 => { 5 }
		1 => { 10 }
		2 => { 11 }
		3 => { 12 }
		4 => { 13 }
		5 => { 15 }
		6 => { 20 }
		7 => { 25 }
		8 => { 30 }
		9 => { 50 }
		10 => { 75 }
		_ => { 75 }
	};
}

#[allow(dead_code)]
/// Get the maximum allowed Resource per turn based on Power.
pub fn getResourcePerTurn(power: usize) -> usize
{
	return match power
	{
		0 => { 1 }
		1 => { 1 }
		2 => { 2 }
		3 => { 3 }
		4 => { 4 }
		5 => { 5 }
		6 => { 6 }
		7 => { 7 }
		8 => { 8 }
		9 => { 10 }
		10 => { 15 }
		_ => { 15 }
	};
}
