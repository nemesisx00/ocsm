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
		Attributes,
		AttributeType,
		Skills,
		SkillType,
	},
	state::{
		updateAttribute,
		updateSkill,
	},
};

#[derive(PartialEq, Props)]
pub struct AttributesProps
{
	attributes: Attributes,
	label: String,
}

pub fn Attributes(scope: Scope<AttributesProps>) -> Element
{
	let dotsClass = "dots row".to_string();
	
	return scope.render(rsx!
	{
		div
		{
			class: "attributesWrapper column",
			
			div { class: "attributesLabel", "{scope.props.label}" },
			
			div
			{
				class: "attributes row",
				
				div
				{
					class: "column",
					
					Dots { class: dotsClass.clone(), label: scope.props.attributes.intelligence.name.clone(), max: 5, value: scope.props.attributes.intelligence.value, handler: attributeHandler, handlerKey: AttributeType::Intelligence }
					Dots { class: dotsClass.clone(), label: scope.props.attributes.wits.name.clone(), max: 5, value: scope.props.attributes.wits.value, handler: attributeHandler, handlerKey: AttributeType::Wits }
					Dots { class: dotsClass.clone(), label: scope.props.attributes.resolve.name.clone(), max: 5, value: scope.props.attributes.resolve.value, handler: attributeHandler, handlerKey: AttributeType::Resolve }
				}
				
				div
				{
					class: "column",
					
					Dots { class: dotsClass.clone(), label: scope.props.attributes.strength.name.clone(), max: 5, value: scope.props.attributes.strength.value, handler: attributeHandler, handlerKey: AttributeType::Strength }
					Dots { class: dotsClass.clone(), label: scope.props.attributes.dexterity.name.clone(), max: 5, value: scope.props.attributes.dexterity.value, handler: attributeHandler, handlerKey: AttributeType::Dexterity }
					Dots { class: dotsClass.clone(), label: scope.props.attributes.stamina.name.clone(), max: 5, value: scope.props.attributes.stamina.value, handler: attributeHandler, handlerKey: AttributeType::Stamina }
				}
				
				div
				{
					class: "column",
					
					Dots { class: dotsClass.clone(), label: scope.props.attributes.presence.name.clone(), max: 5, value: scope.props.attributes.presence.value, handler: attributeHandler, handlerKey: AttributeType::Presence }
					Dots { class: dotsClass.clone(), label: scope.props.attributes.manipulation.name.clone(), max: 5, value: scope.props.attributes.manipulation.value, handler: attributeHandler, handlerKey: AttributeType::Manipulation }
					Dots { class: dotsClass.clone(), label: scope.props.attributes.composure.name.clone(), max: 5, value: scope.props.attributes.composure.value, handler: attributeHandler, handlerKey: AttributeType::Composure }
				}
			}
		}
	});
}

#[derive(PartialEq, Props)]
pub struct SkillsProps
{
	label: String,
	skills: Skills,
}

pub fn Skills(scope: Scope<SkillsProps>) -> Element
{
	let dotsClass = "dots row".to_string();
	
	return scope.render(rsx!
	{
		div
		{
			class: "skillsWrapper cod column",
			
			div { class: "skillsLabel", "{scope.props.label}" },
			
			div
			{
				class: "skills row",
				
				div
				{
					class: "column",
					
					Dots { class: dotsClass.clone(), label: scope.props.skills.academics.name.clone(), max: 5, value: scope.props.skills.academics.value, handler: skillHandler, handlerKey: SkillType::Academics }
					Dots { class: dotsClass.clone(), label: scope.props.skills.computer.name.clone(), max: 5, value: scope.props.skills.computer.value, handler: skillHandler, handlerKey: SkillType::Computer }
					Dots { class: dotsClass.clone(), label: scope.props.skills.crafts.name.clone(), max: 5, value: scope.props.skills.crafts.value, handler: skillHandler, handlerKey: SkillType::Crafts }
					Dots { class: dotsClass.clone(), label: scope.props.skills.investigation.name.clone(), max: 5, value: scope.props.skills.investigation.value, handler: skillHandler, handlerKey: SkillType::Investigation }
					Dots { class: dotsClass.clone(), label: scope.props.skills.medicine.name.clone(), max: 5, value: scope.props.skills.medicine.value, handler: skillHandler, handlerKey: SkillType::Medicine }
					Dots { class: dotsClass.clone(), label: scope.props.skills.occult.name.clone(), max: 5, value: scope.props.skills.occult.value, handler: skillHandler, handlerKey: SkillType::Occult }
					Dots { class: dotsClass.clone(), label: scope.props.skills.politics.name.clone(), max: 5, value: scope.props.skills.politics.value, handler: skillHandler, handlerKey: SkillType::Politics }
					Dots { class: dotsClass.clone(), label: scope.props.skills.science.name.clone(), max: 5, value: scope.props.skills.science.value, handler: skillHandler, handlerKey: SkillType::Science }
				}
				
				div
				{
					class: "column",
					
					Dots { class: dotsClass.clone(), label: scope.props.skills.athletics.name.clone(), max: 5, value: scope.props.skills.athletics.value, handler: skillHandler, handlerKey: SkillType::Athletics }
					Dots { class: dotsClass.clone(), label: scope.props.skills.brawl.name.clone(), max: 5, value: scope.props.skills.brawl.value, handler: skillHandler, handlerKey: SkillType::Brawl }
					Dots { class: dotsClass.clone(), label: scope.props.skills.drive.name.clone(), max: 5, value: scope.props.skills.drive.value, handler: skillHandler, handlerKey: SkillType::Drive }
					Dots { class: dotsClass.clone(), label: scope.props.skills.firearms.name.clone(), max: 5, value: scope.props.skills.firearms.value, handler: skillHandler, handlerKey: SkillType::Firearms }
					Dots { class: dotsClass.clone(), label: scope.props.skills.larceny.name.clone(), max: 5, value: scope.props.skills.larceny.value, handler: skillHandler, handlerKey: SkillType::Larceny }
					Dots { class: dotsClass.clone(), label: scope.props.skills.stealth.name.clone(), max: 5, value: scope.props.skills.stealth.value, handler: skillHandler, handlerKey: SkillType::Stealth }
					Dots { class: dotsClass.clone(), label: scope.props.skills.survival.name.clone(), max: 5, value: scope.props.skills.survival.value, handler: skillHandler, handlerKey: SkillType::Survival }
					Dots { class: dotsClass.clone(), label: scope.props.skills.weaponry.name.clone(), max: 5, value: scope.props.skills.weaponry.value, handler: skillHandler, handlerKey: SkillType::Weaponry }
				}
				
				div
				{
					class: "column",
					
					Dots { class: dotsClass.clone(), label: scope.props.skills.animalKen.name.clone(), max: 5, value: scope.props.skills.animalKen.value, handler: skillHandler, handlerKey: SkillType::AnimalKen }
					Dots { class: dotsClass.clone(), label: scope.props.skills.empathy.name.clone(), max: 5, value: scope.props.skills.empathy.value, handler: skillHandler, handlerKey: SkillType::Empathy }
					Dots { class: dotsClass.clone(), label: scope.props.skills.expression.name.clone(), max: 5, value: scope.props.skills.expression.value, handler: skillHandler, handlerKey: SkillType::Expression }
					Dots { class: dotsClass.clone(), label: scope.props.skills.intimidation.name.clone(), max: 5, value: scope.props.skills.intimidation.value, handler: skillHandler, handlerKey: SkillType::Intimidation }
					Dots { class: dotsClass.clone(), label: scope.props.skills.persuasion.name.clone(), max: 5, value: scope.props.skills.persuasion.value, handler: skillHandler, handlerKey: SkillType::Persuasion }
					Dots { class: dotsClass.clone(), label: scope.props.skills.socialize.name.clone(), max: 5, value: scope.props.skills.socialize.value, handler: skillHandler, handlerKey: SkillType::Socialize }
					Dots { class: dotsClass.clone(), label: scope.props.skills.streetwise.name.clone(), max: 5, value: scope.props.skills.streetwise.value, handler: skillHandler, handlerKey: SkillType::Streetwise }
					Dots { class: dotsClass.clone(), label: scope.props.skills.subterfuge.name.clone(), max: 5, value: scope.props.skills.subterfuge.value, handler: skillHandler, handlerKey: SkillType::Subterfuge }
				}
			}
		}
	});
}

fn attributeHandler(scope: &Scope<DotsProps<AttributeType>>, clickedValue: usize)
{
	match &scope.props.handlerKey
	{
		Some(at) => {
			let mut next = clickedValue;
			
			if clickedValue == scope.props.value { next -= 1; }
			if next > 5 { next = 5; }
			if next < 1 { next = 1; }
			
			updateAttribute(scope, *at, next);
		},
		None => {}
	}
}

fn skillHandler(scope: &Scope<DotsProps<SkillType>>, clickedValue: usize)
{
	match &scope.props.handlerKey
	{
		Some(st) => {
			let mut next = clickedValue;
			
			if clickedValue == scope.props.value { next -= 1; }
			if next > 5 { next = 5; }
			
			updateSkill(scope, *st, next);
		},
		None => {}
	}
}
