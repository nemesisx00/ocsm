#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::{
	cod::{
		tracks::{
			Tracker,
			TrackerState,
		},
		traits::{
			Attributes,
			AttributeType,
			Skills,
			SkillType,
		}
	},
};

pub static CharacterAttributes: AtomRef<Attributes> = |_| Attributes::default();
pub static CharacterHealth: AtomRef<Tracker> = |_| Tracker::new(6);
pub static CharacterSkills: AtomRef<Skills> = |_| Skills::default();
pub static CharacterWillpower: AtomRef<Tracker> = |_| Tracker::new(2);

pub fn updateAttribute<T>(scope: &Scope<T>, attribute: AttributeType, value: usize)
{
	let attributes = use_atom_ref(&scope, CharacterAttributes);
	let health = use_atom_ref(&scope, CharacterHealth);
	let willpower = use_atom_ref(&scope, CharacterWillpower);
	
	match attribute
	{
		AttributeType::Composure =>
		{
			willpower.write().max = attributes.read().resolve.value + value;
			attributes.write().composure.value = value;
		},
		AttributeType::Dexterity => { attributes.write().dexterity.value = value; },
		AttributeType::Intelligence => { attributes.write().intelligence.value = value; },
		AttributeType::Manipulation => { attributes.write().manipulation.value = value; },
		AttributeType::Presence => { attributes.write().presence.value = value; },
		AttributeType::Resolve =>
		{
			willpower.write().max = attributes.read().composure.value + value;
			attributes.write().resolve.value = value;
		},
		AttributeType::Stamina =>
		{
			health.write().max = 5 + value;
			attributes.write().stamina.value = value;
		},
		AttributeType::Strength => { attributes.write().strength.value = value; },
		AttributeType::Wits => { attributes.write().wits.value = value; },
	}
}

pub fn updateHealth<T>(scope: &Scope<T>, damageType: TrackerState, remove: bool, index: Option<usize>)
{
	let health = use_atom_ref(&scope, CharacterHealth);
	
	if remove
	{
		health.write().remove(damageType);
	}
	else
	{
		match index
		{
			Some(i) => { health.write().update(damageType, i); }
			None => { health.write().add(damageType); }
		}
	}
}

pub fn updateSkill<T>(scope: &Scope<T>, skill: SkillType, value: usize)
{
	let skills = use_atom_ref(&scope, CharacterSkills);
	
	match skill
	{
		SkillType::Academics => { skills.write().academics.value = value; }
		SkillType::AnimalKen => { skills.write().animalKen.value = value; }
		SkillType::Athletics => { skills.write().athletics.value = value; }
		SkillType::Brawl => { skills.write().brawl.value = value; }
		SkillType::Computer => { skills.write().computer.value = value; }
		SkillType::Crafts => { skills.write().crafts.value = value; }
		SkillType::Drive => { skills.write().drive.value = value; }
		SkillType::Empathy => { skills.write().empathy.value = value; }
		SkillType::Expression => { skills.write().expression.value = value; }
		SkillType::Firearms => { skills.write().firearms.value = value; }
		SkillType::Investigation => { skills.write().investigation.value = value; }
		SkillType::Intimidation => { skills.write().intimidation.value = value; }
		SkillType::Larceny => { skills.write().larceny.value = value; }
		SkillType::Medicine => { skills.write().medicine.value = value; }
		SkillType::Occult => { skills.write().occult.value = value; }
		SkillType::Persuasion => { skills.write().persuasion.value = value; }
		SkillType::Politics => { skills.write().politics.value = value; }
		SkillType::Science => { skills.write().science.value = value; }
		SkillType::Socialize => { skills.write().socialize.value = value; }
		SkillType::Stealth => { skills.write().stealth.value = value; }
		SkillType::Streetwise => { skills.write().streetwise.value = value; }
		SkillType::Subterfuge => { skills.write().subterfuge.value = value; }
		SkillType::Survival => { skills.write().survival.value = value; }
		SkillType::Weaponry => { skills.write().weaponry.value = value; }
	}
}

pub fn updateWillpower<T>(scope: &Scope<T>, damageType: TrackerState, index: Option<usize>)
{
	let willpower = use_atom_ref(&scope, CharacterWillpower);
	
	match index
	{
		Some(_) =>
		{
			match damageType
			{
				TrackerState::Two => { willpower.write().remove(TrackerState::Two); }
				_ => { willpower.write().add(TrackerState::Two); }
			}
		}
		None => { willpower.write().add(TrackerState::Two); }
	}
}
