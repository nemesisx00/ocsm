#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::cod::{
	components::{
		dots::{
			Dots,
			DotsProps,
		},
	},
	traits::{
		BaseAttributeType,
		BaseSkillType,
	},
	state::{
		CharacterAttributes,
		CharacterSkills,
		updateBaseAttribute,
		updateBaseSkill,
	},
};

pub fn Attributes(cx: Scope) -> Element
{
	let attributesRef = use_atom_ref(&cx, CharacterAttributes);
	let attributes = attributesRef.read();
	let dotsClass = "dots row".to_string();
	
	return cx.render(rsx!
	{
		div
		{
			class: "attributesWrapper column",
			
			div { class: "attributesLabel", "Attributes" },
			
			div
			{
				class: "attributes row",
				
				div
				{
					class: "column",
					
					Dots { class: dotsClass.clone(), label: attributes.intelligence.name.clone(), max: 5, value: attributes.intelligence.value, handler: attributeHandler, handlerKey: BaseAttributeType::Intelligence }
					Dots { class: dotsClass.clone(), label: attributes.wits.name.clone(), max: 5, value: attributes.wits.value, handler: attributeHandler, handlerKey: BaseAttributeType::Wits }
					Dots { class: dotsClass.clone(), label: attributes.resolve.name.clone(), max: 5, value: attributes.resolve.value, handler: attributeHandler, handlerKey: BaseAttributeType::Resolve }
				}
				
				div
				{
					class: "column",
					
					Dots { class: dotsClass.clone(), label: attributes.strength.name.clone(), max: 5, value: attributes.strength.value, handler: attributeHandler, handlerKey: BaseAttributeType::Strength }
					Dots { class: dotsClass.clone(), label: attributes.dexterity.name.clone(), max: 5, value: attributes.dexterity.value, handler: attributeHandler, handlerKey: BaseAttributeType::Dexterity }
					Dots { class: dotsClass.clone(), label: attributes.stamina.name.clone(), max: 5, value: attributes.stamina.value, handler: attributeHandler, handlerKey: BaseAttributeType::Stamina }
				}
				
				div
				{
					class: "column",
					
					Dots { class: dotsClass.clone(), label: attributes.presence.name.clone(), max: 5, value: attributes.presence.value, handler: attributeHandler, handlerKey: BaseAttributeType::Presence }
					Dots { class: dotsClass.clone(), label: attributes.manipulation.name.clone(), max: 5, value: attributes.manipulation.value, handler: attributeHandler, handlerKey: BaseAttributeType::Manipulation }
					Dots { class: dotsClass.clone(), label: attributes.composure.name.clone(), max: 5, value: attributes.composure.value, handler: attributeHandler, handlerKey: BaseAttributeType::Composure }
				}
			}
		}
	});
}

pub fn Skills(cx: Scope) -> Element
{
	let skillsRef = use_atom_ref(&cx, CharacterSkills);
	let skills = skillsRef.read();
	let dotsClass = "dots row".to_string();
	
	return cx.render(rsx!
	{
		div
		{
			class: "skillsWrapper cod column",
			
			div { class: "skillsLabel", "Skills" },
			
			div
			{
				class: "skills row",
				
				div
				{
					class: "column",
					
					Dots { class: dotsClass.clone(), label: skills.academics.name.clone(), max: 5, value: skills.academics.value, handler: skillHandler, handlerKey: BaseSkillType::Academics }
					Dots { class: dotsClass.clone(), label: skills.computer.name.clone(), max: 5, value: skills.computer.value, handler: skillHandler, handlerKey: BaseSkillType::Computer }
					Dots { class: dotsClass.clone(), label: skills.crafts.name.clone(), max: 5, value: skills.crafts.value, handler: skillHandler, handlerKey: BaseSkillType::Crafts }
					Dots { class: dotsClass.clone(), label: skills.investigation.name.clone(), max: 5, value: skills.investigation.value, handler: skillHandler, handlerKey: BaseSkillType::Investigation }
					Dots { class: dotsClass.clone(), label: skills.medicine.name.clone(), max: 5, value: skills.medicine.value, handler: skillHandler, handlerKey: BaseSkillType::Medicine }
					Dots { class: dotsClass.clone(), label: skills.occult.name.clone(), max: 5, value: skills.occult.value, handler: skillHandler, handlerKey: BaseSkillType::Occult }
					Dots { class: dotsClass.clone(), label: skills.politics.name.clone(), max: 5, value: skills.politics.value, handler: skillHandler, handlerKey: BaseSkillType::Politics }
					Dots { class: dotsClass.clone(), label: skills.science.name.clone(), max: 5, value: skills.science.value, handler: skillHandler, handlerKey: BaseSkillType::Science }
				}
				
				div
				{
					class: "column",
					
					Dots { class: dotsClass.clone(), label: skills.athletics.name.clone(), max: 5, value: skills.athletics.value, handler: skillHandler, handlerKey: BaseSkillType::Athletics }
					Dots { class: dotsClass.clone(), label: skills.brawl.name.clone(), max: 5, value: skills.brawl.value, handler: skillHandler, handlerKey: BaseSkillType::Brawl }
					Dots { class: dotsClass.clone(), label: skills.drive.name.clone(), max: 5, value: skills.drive.value, handler: skillHandler, handlerKey: BaseSkillType::Drive }
					Dots { class: dotsClass.clone(), label: skills.firearms.name.clone(), max: 5, value: skills.firearms.value, handler: skillHandler, handlerKey: BaseSkillType::Firearms }
					Dots { class: dotsClass.clone(), label: skills.larceny.name.clone(), max: 5, value: skills.larceny.value, handler: skillHandler, handlerKey: BaseSkillType::Larceny }
					Dots { class: dotsClass.clone(), label: skills.stealth.name.clone(), max: 5, value: skills.stealth.value, handler: skillHandler, handlerKey: BaseSkillType::Stealth }
					Dots { class: dotsClass.clone(), label: skills.survival.name.clone(), max: 5, value: skills.survival.value, handler: skillHandler, handlerKey: BaseSkillType::Survival }
					Dots { class: dotsClass.clone(), label: skills.weaponry.name.clone(), max: 5, value: skills.weaponry.value, handler: skillHandler, handlerKey: BaseSkillType::Weaponry }
				}
				
				div
				{
					class: "column",
					
					Dots { class: dotsClass.clone(), label: skills.animalKen.name.clone(), max: 5, value: skills.animalKen.value, handler: skillHandler, handlerKey: BaseSkillType::AnimalKen }
					Dots { class: dotsClass.clone(), label: skills.empathy.name.clone(), max: 5, value: skills.empathy.value, handler: skillHandler, handlerKey: BaseSkillType::Empathy }
					Dots { class: dotsClass.clone(), label: skills.expression.name.clone(), max: 5, value: skills.expression.value, handler: skillHandler, handlerKey: BaseSkillType::Expression }
					Dots { class: dotsClass.clone(), label: skills.intimidation.name.clone(), max: 5, value: skills.intimidation.value, handler: skillHandler, handlerKey: BaseSkillType::Intimidation }
					Dots { class: dotsClass.clone(), label: skills.persuasion.name.clone(), max: 5, value: skills.persuasion.value, handler: skillHandler, handlerKey: BaseSkillType::Persuasion }
					Dots { class: dotsClass.clone(), label: skills.socialize.name.clone(), max: 5, value: skills.socialize.value, handler: skillHandler, handlerKey: BaseSkillType::Socialize }
					Dots { class: dotsClass.clone(), label: skills.streetwise.name.clone(), max: 5, value: skills.streetwise.value, handler: skillHandler, handlerKey: BaseSkillType::Streetwise }
					Dots { class: dotsClass.clone(), label: skills.subterfuge.name.clone(), max: 5, value: skills.subterfuge.value, handler: skillHandler, handlerKey: BaseSkillType::Subterfuge }
				}
			}
		}
	});
}

fn attributeHandler(cx: &Scope<DotsProps<BaseAttributeType>>, clickedValue: usize)
{
	match &cx.props.handlerKey
	{
		Some(at) => {
			let mut next = clickedValue;
			
			if clickedValue == cx.props.value { next -= 1; }
			if next > 5 { next = 5; }
			if next < 1 { next = 1; }
			
			updateBaseAttribute(cx, at, next);
		},
		None => {}
	}
}

fn skillHandler(cx: &Scope<DotsProps<BaseSkillType>>, clickedValue: usize)
{
	match &cx.props.handlerKey
	{
		Some(st) => {
			let mut next = clickedValue;
			
			if clickedValue == cx.props.value { next -= 1; }
			if next > 5 { next = 5; }
			
			updateBaseSkill(cx, st, next);
		},
		None => {}
	}
}
