#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::{
	cod::{
		enums::{
			CoreAttribute,
			CoreSkill,
		},
		state::{
			CharacterAttributes,
			CharacterSkills,
			updateCoreAttribute,
			updateCoreSkill,
		},
	},
	components::cod::dots::{
		Dots,
		DotsProps,
	},
};

/// The properties struct for `Attributes` and `Skills`.
#[derive(PartialEq, Props)]
pub struct TraitProps
{
	#[props(optional)]
	traitMax: Option<usize>,
}

/// The UI Component handling a Chronicles of Darkness character's Attribute values.
pub fn Attributes(cx: Scope<TraitProps>) -> Element
{
	let attributesRef = use_atom_ref(&cx, CharacterAttributes);
	let attributes = attributesRef.read();
	
	let mentalAttributeTypes = CoreAttribute::mental();
	let physicalAttributeTypes = CoreAttribute::physical();
	let socialAttributeTypes = CoreAttribute::social();
	
	let traitMax = match cx.props.traitMax
	{
		Some(tm) => tm,
		None => 5
	};
	
	return cx.render(rsx!
	{
		div
		{
			class: "attributesWrapper column justEven",
			
			div { class: "attributesLabel", "Attributes" },
			
			div
			{
				class: "attributes row justEven",
				
				div
				{
					class: "column justStart",
					div { class: "row justEven traitCategory", "Mental" }
					mentalAttributeTypes.iter().enumerate().map(|(i, at)| {
						let attr = attributes.clone();
						rsx!(cx, Dots { key: "{i}", class: "dots row justEven".to_string(), label: at.as_ref().to_string(), max: traitMax, value: attr[at], handler: attributeHandler, handlerKey: *at })
					})
				}
				
				div
				{
					class: "column justStart",
					div { class: "row justEven traitCategory", "Physical" }
					physicalAttributeTypes.iter().enumerate().map(|(i, at)| {
						let attr = attributes.clone();
						rsx!(cx, Dots { key: "{i}", class: "dots row justEven".to_string(), label: at.as_ref().to_string(), max: traitMax, value: attr[at], handler: attributeHandler, handlerKey: *at })
					})
				}
				
				div
				{
					class: "column justStart",
					div { class: "row justEven traitCategory", "Social" }
					socialAttributeTypes.iter().enumerate().map(|(i, at)| {
						let attr = attributes.clone();
						rsx!(cx, Dots { key: "{i}", class: "dots row justEven".to_string(), label: at.as_ref().to_string(), max: traitMax, value: attr[at], handler: attributeHandler, handlerKey: *at })
					})
				}
			}
		}
	});
}

/// Event handler triggered when a dot in an Attribute is clicked.
fn attributeHandler(cx: &Scope<DotsProps<CoreAttribute>>, clickedValue: usize)
{
	match &cx.props.handlerKey
	{
		Some(attributeType) => {
			let mut next = clickedValue;
			
			if clickedValue == cx.props.value { next -= 1; }
			if next > cx.props.max { next = cx.props.max; }
			if next < 1 { next = 1; }
			
			updateCoreAttribute(cx, attributeType, next);
		},
		None => {}
	}
}

// -----

/// The UI Component handling a Chronicles of Darkness character's Skill values.
pub fn Skills(cx: Scope<TraitProps>) -> Element
{
	let skillsRef = use_atom_ref(&cx, CharacterSkills);
	let skills = skillsRef.read();
	
	let mentalSkillTypes = CoreSkill::mental();
	let physicalSkillTypes = CoreSkill::physical();
	let socialSkillTypes = CoreSkill::social();
	
	let traitMax = match cx.props.traitMax
	{
		Some(tm) => tm,
		None => 5
	};
	
	return cx.render(rsx!
	{
		div
		{
			class: "skillsWrapper cod column justEven",
			
			div { class: "skillsLabel", "Skills" },
			
			div
			{
				class: "skills row justEven",
				
				div
				{
					class: "column justStart",
					div { class: "row justEven traitCategory", "Mental" }
					div { class: "row justEven unskilled", "(-3 unskilled)" }
					mentalSkillTypes.iter().enumerate().map(|(i, st)| {
						let ski = skills.clone();
						rsx!(cx, Dots { key: "{i}", class: "dots row justEven".to_string(), label: CoreSkill::getSkillName(*st), max: traitMax, value: ski[st], handler: skillHandler, handlerKey: *st })
					})
				}
				
				div
				{
					class: "column justStart",
					div { class: "row justEven traitCategory", "Physical" }
					div { class: "row justEven unskilled", "(-1 unskilled)" }
					physicalSkillTypes.iter().enumerate().map(|(i, st)| {
						let ski = skills.clone();
						rsx!(cx, Dots { key: "{i}", class: "dots row justEven".to_string(), label: CoreSkill::getSkillName(*st), max: traitMax, value: ski[st], handler: skillHandler, handlerKey: *st })
					})
				}
				
				div
				{
					class: "column justStart",
					div { class: "row justEven traitCategory", "Social" }
					div { class: "row justEven unskilled", "(-1 unskilled)" }
					socialSkillTypes.iter().enumerate().map(|(i, st)| {
						let ski = skills.clone();
						rsx!(cx, Dots { key: "{i}", class: "dots row justEven".to_string(), label: CoreSkill::getSkillName(*st), max: traitMax, value: ski[st], handler: skillHandler, handlerKey: *st })
					})
				}
			}
		}
	});
}

/// Event handler triggered when a dot in a Skill is clicked.
fn skillHandler(cx: &Scope<DotsProps<CoreSkill>>, clickedValue: usize)
{
	match &cx.props.handlerKey
	{
		Some(skillType) => {
			let mut next = clickedValue;
			
			if clickedValue == cx.props.value { next -= 1; }
			if next > cx.props.max { next = cx.props.max; }
			
			updateCoreSkill(cx, skillType, next);
		},
		None => {}
	}
}
