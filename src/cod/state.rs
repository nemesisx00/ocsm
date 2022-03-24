#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
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
			BaseAttributes,
			BaseAttributeType,
			BaseSkills,
			BaseSkillType,
		}
	},
};

pub static CharacterAdvantages: AtomRef<BaseAdvantages> = |_| BaseAdvantages::default();
pub static CharacterAttributes: AtomRef<BaseAttributes> = |_| BaseAttributes::default();
pub static CharacterMerits: AtomRef<Vec<Merit>> = |_| Vec::<Merit>::new();
pub static CharacterSkills: AtomRef<BaseSkills> = |_| BaseSkills::default();

pub fn updateBaseAdvantage<T>(scope: &Scope<T>, advantage: BaseAdvantageType, value: usize)
{
	let advantagesRef = use_atom_ref(&scope, CharacterAdvantages);
	let attributesRef = use_atom_ref(&scope, CharacterAttributes);
	
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
			let healthMax = attributes.stamina.value + finalValue;
			
			advantages.size = finalValue;
			advantages.health.updateMax(healthMax);
		}
		
		BaseAdvantageType::Speed => { advantages.speed = value; }
		BaseAdvantageType::Willpower => { advantages.willpower.updateMax(value); }
	}
}

pub fn updateBaseAttribute<T>(scope: &Scope<T>, attribute: &BaseAttributeType, value: usize)
{
	let advantagesRef = use_atom_ref(&scope, CharacterAdvantages);
	let attributesRef = use_atom_ref(&scope, CharacterAttributes);
	let skillsRef = use_atom_ref(&scope, CharacterSkills);
	
	let mut advantages = advantagesRef.write();
	let mut attributes = attributesRef.write();
	let skills = skillsRef.read();
	
	match attribute
	{
		BaseAttributeType::Composure =>
		{
			attributes.composure.value = value;
			advantages.initiative = attributes.dexterity.value + value;
			advantages.willpower.updateMax(attributes.resolve.value + value);
		}
		
		BaseAttributeType::Dexterity =>
		{
			let defense = match value <= attributes.wits.value
			{
				true => { value }
				false => { attributes.wits.value }
			} + skills.athletics.value;
			
			attributes.dexterity.value = value;
			advantages.defense = defense;
			advantages.initiative = attributes.composure.value + value;
			advantages.speed = advantages.size + attributes.strength.value + value;
		}
		
		BaseAttributeType::Intelligence => { attributes.intelligence.value = value; }
		BaseAttributeType::Manipulation => { attributes.manipulation.value = value; }
		BaseAttributeType::Presence => { attributes.presence.value = value; }
		
		BaseAttributeType::Resolve =>
		{
			attributes.resolve.value = value;
			advantages.willpower.updateMax(attributes.composure.value + value);
		}
		
		BaseAttributeType::Stamina =>
		{
			let healthMax = advantages.size + value;
			
			attributes.stamina.value = value;
			advantages.health.updateMax(healthMax);
		}
		
		BaseAttributeType::Strength =>
		{
			attributes.strength.value = value;
			advantages.speed = advantages.size + attributes.dexterity.value + value;
		}
		
		BaseAttributeType::Wits =>
		{
			let defense = match value <= attributes.dexterity.value
			{
				true => { value }
				false => { attributes.dexterity.value }
			} + skills.athletics.value;
			
			attributes.wits.value = value;
			advantages.defense = defense;
		}
	}
}

// For now, just updating directly since this is such a simple data structure
/*
pub fn updateBaseMerit<T>(scope: &Scope<T>, merit: &mut Merit, index: usize)
{
	let meritsRef = use_atom_ref(&scope, CharacterMerits);
	let mut merits = meritsRef.write();
	
	match merits.get_mut(index)
	{
		Some(m) => { *m = merit.clone(); }
		None => {}
	}
}
*/

pub fn updateBaseSkill<T>(scope: &Scope<T>, skill: &BaseSkillType, value: usize)
{
	let advantagesRef = use_atom_ref(&scope, CharacterAdvantages);
	let attributesRef = use_atom_ref(scope, CharacterAttributes);
	let skillsRef = use_atom_ref(&scope, CharacterSkills);
	
	let mut advantages = advantagesRef.write();
	let attributes = attributesRef.read();
	let mut skills = skillsRef.write();
	
	match skill
	{
		BaseSkillType::Academics => { skills.academics.value = value; }
		BaseSkillType::AnimalKen => { skills.animalKen.value = value; }
		
		BaseSkillType::Athletics =>
		{
			let attributeDefense = match attributes.dexterity.value <= attributes.wits.value
			{
				true => { attributes.dexterity.value }
				false => { attributes.wits.value }
			};
			
			skills.athletics.value = value;
			advantages.defense = attributeDefense + value;
		}
		
		BaseSkillType::Brawl => { skills.brawl.value = value; }
		BaseSkillType::Computer => { skills.computer.value = value; }
		BaseSkillType::Crafts => { skills.crafts.value = value; }
		BaseSkillType::Drive => { skills.drive.value = value; }
		BaseSkillType::Empathy => { skills.empathy.value = value; }
		BaseSkillType::Expression => { skills.expression.value = value; }
		BaseSkillType::Firearms => { skills.firearms.value = value; }
		BaseSkillType::Investigation => { skills.investigation.value = value; }
		BaseSkillType::Intimidation => { skills.intimidation.value = value; }
		BaseSkillType::Larceny => { skills.larceny.value = value; }
		BaseSkillType::Medicine => { skills.medicine.value = value; }
		BaseSkillType::Occult => { skills.occult.value = value; }
		BaseSkillType::Persuasion => { skills.persuasion.value = value; }
		BaseSkillType::Politics => { skills.politics.value = value; }
		BaseSkillType::Science => { skills.science.value = value; }
		BaseSkillType::Socialize => { skills.socialize.value = value; }
		BaseSkillType::Stealth => { skills.stealth.value = value; }
		BaseSkillType::Streetwise => { skills.streetwise.value = value; }
		BaseSkillType::Subterfuge => { skills.subterfuge.value = value; }
		BaseSkillType::Survival => { skills.survival.value = value; }
		BaseSkillType::Weaponry => { skills.weaponry.value = value; }
	}
}

pub fn updateBaseHealth<T>(scope: &Scope<T>, damageType: TrackerState, remove: bool, index: Option<usize>)
{
	let advantagesRef = use_atom_ref(&scope, CharacterAdvantages);
	let mut advantages = advantagesRef.write();
	
	if remove
	{
		advantages.health.remove(damageType);
	}
	else
	{
		match index
		{
			Some(i) => { advantages.health.update(damageType, i); }
			None => { advantages.health.add(damageType); }
		}
	}
}

pub fn updateBaseWillpower<T>(scope: &Scope<T>, damageType: TrackerState, index: Option<usize>)
{
	let advantagesRef = use_atom_ref(&scope, CharacterAdvantages);
	let mut advantages = advantagesRef.write();
	
	match index
	{
		Some(_) =>
		{
			match damageType
			{
				TrackerState::Two => { advantages.willpower.remove(TrackerState::Two); }
				_ => { advantages.willpower.add(TrackerState::Two); }
			}
		}
		None => { advantages.willpower.add(TrackerState::Two); }
	}
}
