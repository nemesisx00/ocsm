#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::{
	cod::{
		advantages::{
			Advantages,
			AdvantageType,
		},
		tracks::{
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

pub static CharacterAdvantages: AtomRef<Advantages> = |_| Advantages::default();
pub static CharacterAttributes: AtomRef<Attributes> = |_| Attributes::default();
pub static CharacterSkills: AtomRef<Skills> = |_| Skills::default();

pub fn updateCharacterAdvantage<T>(scope: &Scope<T>, advantage: AdvantageType, value: usize)
{
	let advantagesRef = use_atom_ref(&scope, CharacterAdvantages);
	let attributesRef = use_atom_ref(&scope, CharacterAttributes);
	
	let mut advantages = advantagesRef.write();
	let attributes = attributesRef.read();
	
	match advantage
	{
		AdvantageType::Defense => { advantages.defense = value; }
		AdvantageType::Health => { advantages.health.updateMax(value); }
		AdvantageType::Initiative => { advantages.initiative = value; }
		
		AdvantageType::Size =>
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
		
		AdvantageType::Speed => { advantages.speed = value; }
		AdvantageType::Willpower => { advantages.willpower.updateMax(value); }
	}
}

pub fn updateAttribute<T>(scope: &Scope<T>, attribute: AttributeType, value: usize)
{
	let advantagesRef = use_atom_ref(&scope, CharacterAdvantages);
	let attributesRef = use_atom_ref(&scope, CharacterAttributes);
	let skillsRef = use_atom_ref(&scope, CharacterSkills);
	
	let mut advantages = advantagesRef.write();
	let mut attributes = attributesRef.write();
	let skills = skillsRef.read();
	
	match attribute
	{
		AttributeType::Composure =>
		{
			attributes.composure.value = value;
			advantages.initiative = attributes.dexterity.value + value;
			advantages.willpower.updateMax(attributes.resolve.value + value);
		}
		
		AttributeType::Dexterity =>
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
		
		AttributeType::Resolve =>
		{
			attributes.resolve.value = value;
			advantages.willpower.updateMax(attributes.composure.value + value);
		}
		
		AttributeType::Stamina =>
		{
			let healthMax = advantages.size + value;
			
			attributes.stamina.value = value;
			advantages.health.updateMax(healthMax);
		}
		
		AttributeType::Strength =>
		{
			attributes.strength.value = value;
			advantages.speed = advantages.size + attributes.dexterity.value + value;
		}
		
		AttributeType::Wits =>
		{
			let defense = match value <= attributes.dexterity.value
			{
				true => { value }
				false => { attributes.dexterity.value }
			} + skills.athletics.value;
			
			attributes.wits.value = value;
			advantages.defense = defense;
		}
		_ => {}
	}
}

pub fn updateSkill<T>(scope: &Scope<T>, skill: SkillType, value: usize)
{
	let advantagesRef = use_atom_ref(&scope, CharacterAdvantages);
	let attributesRef = use_atom_ref(scope, CharacterAttributes);
	let skillsRef = use_atom_ref(&scope, CharacterSkills);
	
	let mut advantages = advantagesRef.write();
	let attributes = attributesRef.read();
	let mut skills = skillsRef.write();
	
	match skill
	{
		SkillType::Academics => { skills.academics.value = value; }
		SkillType::AnimalKen => { skills.animalKen.value = value; }
		
		SkillType::Athletics =>
		{
			let attributeDefense = match attributes.dexterity.value <= attributes.wits.value
			{
				true => { attributes.dexterity.value }
				false => { attributes.wits.value }
			};
			
			skills.athletics.value = value;
			advantages.defense = attributeDefense + value;
		}
		
		SkillType::Brawl => { skills.brawl.value = value; }
		SkillType::Computer => { skills.computer.value = value; }
		SkillType::Crafts => { skills.crafts.value = value; }
		SkillType::Drive => { skills.drive.value = value; }
		SkillType::Empathy => { skills.empathy.value = value; }
		SkillType::Expression => { skills.expression.value = value; }
		SkillType::Firearms => { skills.firearms.value = value; }
		SkillType::Investigation => { skills.investigation.value = value; }
		SkillType::Intimidation => { skills.intimidation.value = value; }
		SkillType::Larceny => { skills.larceny.value = value; }
		SkillType::Medicine => { skills.medicine.value = value; }
		SkillType::Occult => { skills.occult.value = value; }
		SkillType::Persuasion => { skills.persuasion.value = value; }
		SkillType::Politics => { skills.politics.value = value; }
		SkillType::Science => { skills.science.value = value; }
		SkillType::Socialize => { skills.socialize.value = value; }
		SkillType::Stealth => { skills.stealth.value = value; }
		SkillType::Streetwise => { skills.streetwise.value = value; }
		SkillType::Subterfuge => { skills.subterfuge.value = value; }
		SkillType::Survival => { skills.survival.value = value; }
		SkillType::Weaponry => { skills.weaponry.value = value; }
	}
}

pub fn updateHealth<T>(scope: &Scope<T>, damageType: TrackerState, remove: bool, index: Option<usize>)
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

pub fn updateWillpower<T>(scope: &Scope<T>, damageType: TrackerState, index: Option<usize>)
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
