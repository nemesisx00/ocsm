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
	
	let mentalAttributeTypes = BaseAttributeType::mental();
	let physicalAttributeTypes = BaseAttributeType::physical();
	let socialAttributeTypes = BaseAttributeType::social();
	
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
					
					mentalAttributeTypes.iter().enumerate().map(|(i, at)| {
						let attr = attributes.clone();
						return rsx!(cx, Dots { dioxusKey: i, class: "dots row".to_string(), label: attr[at].name.clone(), max: 5, value: attr[at].value, handler: attributeHandler, handlerKey: *at });
					})
				}
				
				div
				{
					class: "column",
					
					physicalAttributeTypes.iter().enumerate().map(|(i, at)| {
						let attr = attributes.clone();
						return rsx!(cx, Dots { dioxusKey: i, class: "dots row".to_string(), label: attr[at].name.clone(), max: 5, value: attr[at].value, handler: attributeHandler, handlerKey: *at });
					})
				}
				
				div
				{
					class: "column",
					
					socialAttributeTypes.iter().enumerate().map(|(i, at)| {
						let attr = attributes.clone();
						return rsx!(cx, Dots { dioxusKey: i, class: "dots row".to_string(), label: attr[at].name.clone(), max: 5, value: attr[at].value, handler: attributeHandler, handlerKey: *at });
					})
				}
			}
		}
	});
}

pub fn Skills(cx: Scope) -> Element
{
	let skillsRef = use_atom_ref(&cx, CharacterSkills);
	let skills = skillsRef.read();
	
	let mentalSkillTypes = BaseSkillType::mental();
	let physicalSkillTypes = BaseSkillType::physical();
	let socialSkillTypes = BaseSkillType::social();
	
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
					
					mentalSkillTypes.iter().enumerate().map(|(i, st)| {
						let ski = skills.clone();
						return rsx!(cx, Dots { dioxusKey: i, class: "dots row".to_string(), label: ski[st].name.clone(), max: 5, value: ski[st].value, handler: skillHandler, handlerKey: *st });
					})
				}
				
				div
				{
					class: "column",
					
					physicalSkillTypes.iter().enumerate().map(|(i, st)| {
						let ski = skills.clone();
						return rsx!(cx, Dots { dioxusKey: i, class: "dots row".to_string(), label: ski[st].name.clone(), max: 5, value: ski[st].value, handler: skillHandler, handlerKey: *st });
					})
				}
				
				div
				{
					class: "column",
					
					socialSkillTypes.iter().enumerate().map(|(i, st)| {
						let ski = skills.clone();
						return rsx!(cx, Dots { dioxusKey: i, class: "dots row".to_string(), label: ski[st].name.clone(), max: 5, value: ski[st].value, handler: skillHandler, handlerKey: *st });
					})
				}
			}
		}
	});
}

fn attributeHandler(cx: &Scope<DotsProps<BaseAttributeType>>, clickedValue: usize)
{
	match &cx.props.handlerKey
	{
		Some(attributeType) => {
			let mut next = clickedValue;
			
			if clickedValue == cx.props.value { next -= 1; }
			if next > 5 { next = 5; }
			if next < 1 { next = 1; }
			
			updateBaseAttribute(cx, attributeType, next);
		},
		None => {}
	}
}

fn skillHandler(cx: &Scope<DotsProps<BaseSkillType>>, clickedValue: usize)
{
	match &cx.props.handlerKey
	{
		Some(skillType) => {
			let mut next = clickedValue;
			
			if clickedValue == cx.props.value { next -= 1; }
			if next > 5 { next = 5; }
			
			updateBaseSkill(cx, skillType, next);
		},
		None => {}
	}
}
