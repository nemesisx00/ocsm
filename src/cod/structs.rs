#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::{
	Scope,
	use_atom_ref,
	use_read,
	use_set,
};
use serde::{
	Serialize,
	Deserialize,
};
use std::collections::BTreeMap;
use strum::IntoEnumIterator;
use crate::{
	cod::{
		enums::{
			CoreAttribute,
			CoreDetail,
			CoreSkill,
			TrackerState,
		},
		state::{
			CharacterAdvantages,
			CharacterAspirations,
			CharacterAttributes,
			CharacterBeats,
			CharacterDetails,
			CharacterExperience,
			CharacterMerits,
			CharacterSkills,
			CharacterSpecialties,
		},
	},
	core::state::StatefulTemplate,
};

/// Data structure defining the Advantages of a Chronicles of Darkness character.
/// 
/// While the specific names may vary, every Chronicles of Darkness
/// game system has stats corresponding to Integrity, Power,
/// and Resource. The only exceptions are Chronicles of Darkness
/// Mortals who do not possess a Power or Resource stat.
#[derive(Clone, Debug, Deserialize, Eq, PartialEq, PartialOrd, Serialize, Ord)]
pub struct CoreAdvantages
{
	#[serde(default)]
	pub defense: usize,
	
	#[serde(default)]
	pub health: Tracker,
	
	#[serde(default)]
	pub initiative: usize,
	
	#[serde(default)]
	pub integrity: usize,
	
	#[serde(default)]
	pub power: Option<usize>,
	
	#[serde(default)]
	pub resource: Option<Tracker>,
	
	#[serde(default)]
	pub size: usize,
	
	#[serde(default)]
	pub speed: usize,
	
	#[serde(default)]
	pub willpower: Tracker,
}

impl Default for CoreAdvantages
{
	fn default() -> Self
	{
		Self
		{
			defense: 1,
			health: Tracker::new(6),
			initiative: 2,
			integrity: 7,
			power: Some(1),
			resource: Some(Tracker::new(10)),
			size: 5,
			speed: 7,
			willpower: Tracker::new(2),
		}
	}
}

/// Data structure defining a Chronicles of Darkness core character.
#[derive(Clone, Debug, Deserialize, Eq, PartialEq, PartialOrd, Serialize, Ord)]
pub struct CoreCharacter
{
	#[serde(default)]
	pub advantages: CoreAdvantages,
	
	#[serde(default)]
	pub aspirations: Vec<String>,
	
	#[serde(default)]
	pub attributes: BTreeMap<CoreAttribute, usize>,
	
	#[serde(default)]
	pub details: BTreeMap<CoreDetail, String>,
	
	#[serde(default)]
	pub beats: Tracker,
	
	#[serde(default)]
	pub experience: usize,
	
	#[serde(default)]
	pub merits: Vec<(String, usize)>,
	
	#[serde(default)]
	pub skills: BTreeMap<CoreSkill, usize>,
	
	#[serde(default)]
	pub specialties: Vec<String>,
}

impl Default for CoreCharacter
{
	fn default() -> Self
	{
		return Self
		{
			advantages: CoreAdvantages::default(),
			aspirations: Vec::<String>::new(),
			attributes: CoreAttribute::asMap(),
			details: CoreDetail::asMap(),
			beats: Tracker::new(5),
			experience: 0,
			merits: Vec::<(String, usize)>::new(),
			skills: CoreSkill::asMap(),
			specialties: Vec::<String>::new(),
		};
	}
}

impl StatefulTemplate for CoreCharacter
{
	fn pull<T>(&mut self, cx: &Scope<T>)
	{
		let advantages = use_atom_ref(cx, CharacterAdvantages);
		let aspirations = use_atom_ref(cx, CharacterAspirations);
		let attributes = use_atom_ref(cx, CharacterAttributes);
		let details = use_atom_ref(cx, CharacterDetails);
		let beats = use_atom_ref(cx, CharacterBeats);
		let experience = use_read(cx, CharacterExperience);
		let merits = use_atom_ref(cx, CharacterMerits);
		let skills = use_atom_ref(cx, CharacterSkills);
		let specialties = use_atom_ref(cx, CharacterSpecialties);
		
		self.advantages = advantages.read().clone();
		self.aspirations = aspirations.read().clone();
		self.attributes = attributes.read().clone();
		self.beats = beats.read().clone();
		self.details = details.read().clone();
		self.experience = *experience;
		self.merits = merits.read().clone();
		self.skills = skills.read().clone();
		self.specialties = specialties.read().clone();
		
		self.validate();
	}
	
	fn push<T>(&mut self, cx: &Scope<T>)
	{
		self.validate();
		
		let advantages = use_atom_ref(cx, CharacterAdvantages);
		let aspirations = use_atom_ref(cx, CharacterAspirations);
		let attributes = use_atom_ref(cx, CharacterAttributes);
		let beats = use_atom_ref(cx, CharacterBeats);
		let details = use_atom_ref(cx, CharacterDetails);
		let experience = use_set(cx, CharacterExperience);
		let merits = use_atom_ref(cx, CharacterMerits);
		let skills = use_atom_ref(cx, CharacterSkills);
		let specialties = use_atom_ref(cx, CharacterSpecialties);
		
		(*advantages.write()) = self.advantages.clone();
		(*aspirations.write()) = self.aspirations.clone();
		(*attributes.write()) = self.attributes.clone();
		(*beats.write()) = self.beats.clone();
		(*details.write()) = self.details.clone();
		experience(self.experience);
		(*merits.write()) = self.merits.clone();
		(*skills.write()) = self.skills.clone();
		(*specialties.write()) = self.specialties.clone();
	}
	
	fn validate(&mut self)
	{
		for ca in CoreAttribute::iter()
		{
			if self.attributes.get(&ca) == None { self.attributes.insert(ca, 1); }
		}
		
		for cd in CoreDetail::iter()
		{
			if self.details.get(&cd) == None { self.details.insert(cd, "".to_string()); }
		}
		
		for cs in CoreSkill::iter()
		{
			if self.skills.get(&cs) == None { self.skills.insert(cs, 0); }
		}
	}
}

/// Data structure representing the current state of a single Track
/// (i.e. Health Track) within a Chronicles of Darkness character sheet.
#[derive(Clone, Debug, Default, Deserialize, Eq, PartialEq, PartialOrd, Serialize, Ord)]
pub struct Tracker
{
	#[serde(default)]
	max: usize,
	
	#[serde(default)]
	pub values: Vec<TrackerState>
}

impl Tracker
{
	/// Create a new `Tracker` with a max of `max`.
	pub fn new(max: usize) -> Tracker
	{
		return Tracker
		{
			max: max,
			values: Vec::new()
		};
	}
	
	/// Add a new TrackerState to the `Tracker`.
	/// - Will not exceed the `max` capacity.
	/// - The values are sorted every time a new value is added.
	pub fn add(&mut self, state: TrackerState)
	{
		if self.values.len() < self.max
		{
			self.values.push(state);
		}
		
		self.values.sort();
	}
	
	/// Get the allowed maximum value.
	pub fn getMax(self) -> usize
	{
		return self.max;
	}
	
	/// Get the state of the designated value.
	pub fn getValue(self, index: usize) -> Option<TrackerState>
	{
		return match self.values.get(index)
		{
			Some(ts) => { Some(*ts) }
			None => { None }
		};
	}
	
	/// Remove one instance of the designated state from the `Tracker`'s values.
	pub fn remove(&mut self, state: TrackerState)
	{
		match self.values.iter().position(|ts| *ts == state)
		{
			Some(index) => { self.values.remove(index); }
			None => {}
		}
		
		self.values.sort();
	}
	
	/// Update the state of the designated value.
	/// 
	/// States can be upgraded or downgraded. In either case, the state will
	/// "wrap around" when moving beyond the corresponding limit.
	/// 
	/// Follows the standard rules for a Chronicles of Darkness Health Track,
	/// progressing from Superficial (`TrackerState::One`) to Lethal
	/// (`TrackerState::Two`) to Aggravated (`TrackerState::Three`).
	pub fn update(&mut self, state: TrackerState, index: usize, downgrade: bool)
	{
		match self.values.get(index)
		{
			Some(ts) =>
			{
				match ts
				{
					TrackerState::One =>
					{
						match downgrade
						{
							true => { self.remove(TrackerState::One); }
							false => { self.replace(index, TrackerState::Two); }
						}
					}
					TrackerState::Two =>
					{
						match downgrade
						{
							true => { self.replace(index, TrackerState::One); }
							false => { self.replace(index, TrackerState::Three); }
						}
					}
					TrackerState::Three =>
					{
						match downgrade
						{
							true => { self.replace(index, TrackerState::Two); }
							false => { self.remove(TrackerState::Three); }
						}
					}
				}
			}
			None =>
			{
				match downgrade
				{
					true => { self.remove(state); }
					false => { self.add(state); }
				}
			}
		}
	}
	
	/// Update the allowed maximum value.
	/// 
	/// When the maximum is reduced, any excess values are removed.
	pub fn updateMax(&mut self, max: usize)
	{
		self.max = max;
		
		if self.values.len() > max
		{
			for _ in 0..(self.values.len() - max)
			{
				self.values.pop();
			}
		}
	}
	
	/// Replace the designated value with the designated state.
	fn replace(&mut self, index: usize, new: TrackerState)
	{
		self.values.remove(index);
		self.add(new);
	}
}

#[cfg(test)]
mod tests
{
	use super::*;
	
	#[test]
	fn CoreCharacter_validate()
	{
		let attributes = CoreAttribute::asMap();
		let skills = CoreSkill::asMap();
		
		let mut character = CoreCharacter::default();
		character.attributes = BTreeMap::new();
		character.skills = BTreeMap::new();
		
		character.attributes.iter().for_each(|(at, value)| assert_ne!(attributes[at], *value));
		character.skills.iter().for_each(|(st, value)| assert_ne!(skills[st], *value));
		
		character.validate();
		
		character.attributes.iter().for_each(|(at, value)| assert_eq!(attributes[at], *value));
		character.skills.iter().for_each(|(st, value)| assert_eq!(skills[st], *value));
	}
	
	#[test]
	fn Tracker_Add()
	{
		let max = 3;
		let mut t = Tracker::new(max);
		t.add(TrackerState::One);
		
		assert_eq!(1, t.values.len());
	}
	
	#[test]
	fn Tracker_Remove()
	{
		let max = 3;
		let mut t = Tracker::new(max);
		t.add(TrackerState::One);
		
		t.remove(TrackerState::Three);
		assert_eq!(1, t.values.len());
		
		t.remove(TrackerState::One);
		assert_eq!(0, t.values.len());
	}
	
	#[test]
	fn Tracker_Update()
	{
		let max = 3;
		let mut t = Tracker::new(max);
		
		t.update(TrackerState::One, 2, false);
		assert_eq!(TrackerState::One, t.clone().getValue(0).unwrap());
		t.update(TrackerState::Two, 0, false);
		assert_eq!(TrackerState::Two, t.clone().getValue(0).unwrap());
		t.update(TrackerState::Two, 0, true);
		assert_eq!(TrackerState::One, t.clone().getValue(0).unwrap());
		t.update(TrackerState::One, 0, true);
		assert_eq!(0, t.values.clone().len());
		t.update(TrackerState::Three, 2, false);
		assert_eq!(TrackerState::Three, t.clone().getValue(0).unwrap());
		t.update(TrackerState::Three, 0, true);
		assert_eq!(TrackerState::Two, t.clone().getValue(0).unwrap());
		t.update(TrackerState::Two, 0, false);
		t.update(TrackerState::Three, 0, false);
		assert_eq!(0, t.values.clone().len());
	}
	
	#[test]
	fn Tracker_UpdateMax()
	{
		let max = 3;
		let altMax = 2;
		
		assert_eq!(true, altMax < max);
		
		let mut t = Tracker::new(max);
		t.add(TrackerState::One);
		t.add(TrackerState::One);
		t.add(TrackerState::One);
		
		assert_eq!(max, t.values.len());
		t.updateMax(altMax);
		assert_eq!(altMax, t.values.len());
		t.updateMax(max);
		assert_eq!(altMax, t.values.len());
	}
}
