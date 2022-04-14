#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use std::collections::HashMap;
use crate::{
	components::core::check::CheckCircle,
	dnd::fifth::{
		enums::{
			Ability,
			Proficiency,
			Skill,
		},
		util::{
			DefaultProficiencyBonus,
			calculateAbilityModifier,
			displayModifier,
			getProficiencyModifier,
		}
	},
};

#[derive(PartialEq, Props)]
pub struct AbilityScoresProps
{
	pub abilityScores: HashMap<Ability, isize>,
}

/// The UI Component defining the layout of a D&D5e Adventurer's Ability Scores.
pub fn AbilityScores(cx: Scope<AbilityScoresProps>) -> Element
{
	return cx.render(rsx!
	{
		div
		{
			class: "column justStart abilityScoresWrapper",
			
			h3 { class: "row justEven", "Ability Scores" }
			
			div
			{
				class: "row justEven abilityScores",
				
				div
				{
					class: "column justEven",
					
					AbilityScore { label: "Strength".to_string(), score: cx.props.abilityScores[&Ability::Strength] }
					AbilityScore { label: "Intelligence".to_string(), score: cx.props.abilityScores[&Ability::Intelligence] }
				}
				
				div
				{
					class: "column justEven",
					
					AbilityScore { label: "Dexterity".to_string(), score: cx.props.abilityScores[&Ability::Dexterity] }
					AbilityScore { label: "Wisdom".to_string(), score: cx.props.abilityScores[&Ability::Wisdom] }
				}
				
				div
				{
					class: "column justEven",
					
					AbilityScore { label: "Constitution".to_string(), score: cx.props.abilityScores[&Ability::Constitution] }
					AbilityScore { label: "Charisma".to_string(), score: cx.props.abilityScores[&Ability::Charisma] }
				}
			}
		}
	});
}

// --------------------------------------------------

#[derive(PartialEq, Props)]
pub struct AbilityScoreProps
{
	pub label: String,
	pub score: isize,
}

/// The UI Component defining the layout of one of a D&D5e Adventurer's Ability Scores.
pub fn AbilityScore(cx: Scope<AbilityScoreProps>) -> Element
{
	let modifier = calculateAbilityModifier(cx.props.score);
	let modifierDisplay = displayModifier(modifier);
	
	return cx.render(rsx!
	{
		div
		{
			class: "column justEven abilityScore",
			
			div { class: "label", "{cx.props.label}"}
			input { r#type: "text", value: "{cx.props.score}" }
			div { class: "modifier", "{modifierDisplay}" }
		}
	});
}

// --------------------------------------------------

#[derive(PartialEq, Props)]
pub struct SavingThrowsProps
{
	pub abilityScores: HashMap<Ability, isize>,
	pub proficiencies: HashMap<Ability, Proficiency>,
}

/// The UI Component defining the layout of a D&D5e Adventurer's Saving Throws.
pub fn SavingThrows(cx: Scope<SavingThrowsProps>) -> Element
{
	return cx.render(rsx!
	{div
		{
			class: "column justStart savingThrowsWrapper",
			
			h3 { class: "row justEven", "Saving Throws" }
			
			div
			{
				class: "row justEven savingThrows",
				
				div
				{
					class: "column justEven",
					
					SavingThrow { label: "Strength".to_string(), score: cx.props.abilityScores[&Ability::Strength], proficiency: cx.props.proficiencies[&Ability::Strength] }
					SavingThrow { label: "Dexterity".to_string(), score: cx.props.abilityScores[&Ability::Dexterity], proficiency: cx.props.proficiencies[&Ability::Dexterity] }
					SavingThrow { label: "Constitution".to_string(), score: cx.props.abilityScores[&Ability::Constitution], proficiency: cx.props.proficiencies[&Ability::Constitution] }
				}
				
				div
				{
					class: "column justEven",
					
					SavingThrow { label: "Intelligence".to_string(), score: cx.props.abilityScores[&Ability::Intelligence], proficiency: cx.props.proficiencies[&Ability::Intelligence] }
					SavingThrow { label: "Wisdom".to_string(), score: cx.props.abilityScores[&Ability::Wisdom], proficiency: cx.props.proficiencies[&Ability::Wisdom] }
					SavingThrow { label: "Charisma".to_string(), score: cx.props.abilityScores[&Ability::Charisma], proficiency: cx.props.proficiencies[&Ability::Charisma] }
				}
			}
		}
	});
}

// --------------------------------------------------

#[derive(PartialEq, Props)]
pub struct SavingThrowProps
{
	pub label: String,
	pub score: isize,
	pub proficiency: Proficiency,
}

/// The UI Component defining the layout of one of a D&D5e Adventurer's Saving Throws.
pub fn SavingThrow(cx: Scope<SavingThrowProps>) -> Element
{
	let saveBonus = calculateAbilityModifier(cx.props.score) + getProficiencyModifier(DefaultProficiencyBonus, cx.props.proficiency);
	let saveDisplay = displayModifier(saveBonus);
	let proficient = cx.props.proficiency != Proficiency::None;
	let proficiencyClass = cx.props.proficiency.as_ref().to_lowercase();
	
	return cx.render(rsx!
	{
		div
		{
			class: "row justStart savingThrow",
			
			CheckCircle { class: proficiencyClass, checked: proficient, tooltip: cx.props.proficiency.asTitle() }
			div { class: "value", "{saveDisplay}" }
			div { class: "label", "{cx.props.label}" }
		}
	});
}

// --------------------------------------------------

#[derive(PartialEq, Props)]
pub struct SkillsProps
{
	pub abilityScores: HashMap<Ability, isize>,
	pub proficiencies: HashMap<Skill, Proficiency>,
}

/// The UI Component defining the layout of a D&D5e Adventurer's Skills.
pub fn Skills(cx: Scope<SkillsProps>) -> Element
{
	return cx.render(rsx!
	{div
		{
			class: "column justStart skillsWrapper",
			
			h3 { class: "row justEven", "Skills" }
			
			div
			{
				class: "row justEven skills",
				
				div
				{
					class: "column justStart",
					
					div { class: "label", "Strength" }
					Skill { label: "Athletics".to_string(), score: cx.props.abilityScores[&Ability::Strength], proficiency: cx.props.proficiencies[&Skill::Athletics] }
				}
				
				div
				{
					class: "column justStart",
					
					div { class: "label", "Dexterity" }
					Skill { label: "Acrobatics".to_string(), score: cx.props.abilityScores[&Ability::Dexterity], proficiency: cx.props.proficiencies[&Skill::Acrobatics] }
					Skill { label: "Sleight of Hand".to_string(), score: cx.props.abilityScores[&Ability::Dexterity], proficiency: cx.props.proficiencies[&Skill::SleightOfHand] }
					Skill { label: "Stealth".to_string(), score: cx.props.abilityScores[&Ability::Dexterity], proficiency: cx.props.proficiencies[&Skill::Stealth] }
				}
				
				div
				{
					class: "column justStart",
					
					div { class: "label", "Intelligence" }
					Skill { label: "Arcana".to_string(), score: cx.props.abilityScores[&Ability::Intelligence], proficiency: cx.props.proficiencies[&Skill::Arcana] }
					Skill { label: "History".to_string(), score: cx.props.abilityScores[&Ability::Intelligence], proficiency: cx.props.proficiencies[&Skill::History] }
					Skill { label: "Investigation".to_string(), score: cx.props.abilityScores[&Ability::Intelligence], proficiency: cx.props.proficiencies[&Skill::Investigation] }
					Skill { label: "Nature".to_string(), score: cx.props.abilityScores[&Ability::Intelligence], proficiency: cx.props.proficiencies[&Skill::Nature] }
					Skill { label: "Religion".to_string(), score: cx.props.abilityScores[&Ability::Intelligence], proficiency: cx.props.proficiencies[&Skill::Religion] }
				}
				
				div
				{
					class: "column justStart",
					
					div { class: "label", "Wisdom" }
					Skill { label: "Animal Handling".to_string(), score: cx.props.abilityScores[&Ability::Wisdom], proficiency: cx.props.proficiencies[&Skill::AnimalHandling] }
					Skill { label: "Insight".to_string(), score: cx.props.abilityScores[&Ability::Wisdom], proficiency: cx.props.proficiencies[&Skill::Insight] }
					Skill { label: "Medicine".to_string(), score: cx.props.abilityScores[&Ability::Wisdom], proficiency: cx.props.proficiencies[&Skill::Medicine] }
					Skill { label: "Perception".to_string(), score: cx.props.abilityScores[&Ability::Wisdom], proficiency: cx.props.proficiencies[&Skill::Perception] }
					Skill { label: "Survival".to_string(), score: cx.props.abilityScores[&Ability::Wisdom], proficiency: cx.props.proficiencies[&Skill::Survival] }
				}
				
				div
				{
					class: "column justStart",
					
					div { class: "label", "Charisma" }
					Skill { label: "Deception".to_string(), score: cx.props.abilityScores[&Ability::Charisma], proficiency: cx.props.proficiencies[&Skill::Deception] }
					Skill { label: "Intimidation".to_string(), score: cx.props.abilityScores[&Ability::Charisma], proficiency: cx.props.proficiencies[&Skill::Intimidation] }
					Skill { label: "Performance".to_string(), score: cx.props.abilityScores[&Ability::Charisma], proficiency: cx.props.proficiencies[&Skill::Performance] }
					Skill { label: "Persuasion".to_string(), score: cx.props.abilityScores[&Ability::Charisma], proficiency: cx.props.proficiencies[&Skill::Persuasion] }
				}
			}
		}
	});
}

// --------------------------------------------------

#[derive(PartialEq, Props)]
pub struct SkillProps
{
	pub label: String,
	pub score: isize,
	pub proficiency: Proficiency,
}

/// The UI Component defining the layout of one of a D&D5e Adventurer's Saving Throws.
pub fn Skill(cx: Scope<SkillProps>) -> Element
{
	let skillBonus = calculateAbilityModifier(cx.props.score) + getProficiencyModifier(DefaultProficiencyBonus, cx.props.proficiency);
	let skillDisplay = displayModifier(skillBonus);
	let proficient = cx.props.proficiency != Proficiency::None;
	let proficiencyClass = cx.props.proficiency.as_ref().to_lowercase();
	
	return cx.render(rsx!
	{
		div
		{
			class: "row justStart skill",
			
			CheckCircle { class: proficiencyClass, checked: proficient, tooltip: cx.props.proficiency.asTitle() }
			div { class: "value", "{skillDisplay}"}
			div { class: "label", "{cx.props.label}" }
		}
	});
}
