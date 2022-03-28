#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	events::FormEvent,
	prelude::*,
};
use crate::{
	cod::{
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
			CharacterSpecialties,
			updateBaseAttribute,
			updateBaseSkill,
		},
	},
	core::util::{
		RemovePopUpXOffset,
		RemovePopUpYOffset,
	},
};

#[inline_props]
pub fn Attributes(cx: Scope, traitMax: usize) -> Element
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
					div { class: "row traitCategory", "Mental" }
					mentalAttributeTypes.iter().enumerate().map(|(i, at)| {
						let attr = attributes.clone();
						rsx!(cx, Dots { key: "{i}", class: "dots row".to_string(), label: at.as_ref().to_string(), max: *traitMax, value: attr[at], handler: attributeHandler, handlerKey: *at })
					})
				}
				
				div
				{
					class: "column",
					div { class: "row traitCategory", "Physical" }
					physicalAttributeTypes.iter().enumerate().map(|(i, at)| {
						let attr = attributes.clone();
						rsx!(cx, Dots { key: "{i}", class: "dots row".to_string(), label: at.as_ref().to_string(), max: *traitMax, value: attr[at], handler: attributeHandler, handlerKey: *at })
					})
				}
				
				div
				{
					class: "column",
					div { class: "row traitCategory", "Social" }
					socialAttributeTypes.iter().enumerate().map(|(i, at)| {
						let attr = attributes.clone();
						rsx!(cx, Dots { key: "{i}", class: "dots row".to_string(), label: at.as_ref().to_string(), max: *traitMax, value: attr[at], handler: attributeHandler, handlerKey: *at })
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
			if next > cx.props.max { next = cx.props.max; }
			if next < 1 { next = 1; }
			
			updateBaseAttribute(cx, attributeType, next);
		},
		None => {}
	}
}

// -----

#[inline_props]
pub fn Skills(cx: Scope, traitMax: usize) -> Element
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
					div { class: "row traitCategory", "Mental" }
					div { class: "row unskilled", "(-3 unskilled)" }
					mentalSkillTypes.iter().enumerate().map(|(i, st)| {
						let ski = skills.clone();
						rsx!(cx, Dots { key: "{i}", class: "dots row".to_string(), label: BaseSkillType::getSkillName(*st), max: *traitMax, value: ski[st], handler: skillHandler, handlerKey: *st })
					})
				}
				
				div
				{
					class: "column",
					div { class: "row traitCategory", "Physical" }
					div { class: "row unskilled", "(-1 unskilled)" }
					physicalSkillTypes.iter().enumerate().map(|(i, st)| {
						let ski = skills.clone();
						rsx!(cx, Dots { key: "{i}", class: "dots row".to_string(), label: BaseSkillType::getSkillName(*st), max: *traitMax, value: ski[st], handler: skillHandler, handlerKey: *st })
					})
				}
				
				div
				{
					class: "column",
					div { class: "row traitCategory", "Social" }
					div { class: "row unskilled", "(-1 unskilled)" }
					socialSkillTypes.iter().enumerate().map(|(i, st)| {
						let ski = skills.clone();
						rsx!(cx, Dots { key: "{i}", class: "dots row".to_string(), label: BaseSkillType::getSkillName(*st), max: *traitMax, value: ski[st], handler: skillHandler, handlerKey: *st })
					})
				}
			}
		}
	});
}

fn skillHandler(cx: &Scope<DotsProps<BaseSkillType>>, clickedValue: usize)
{
	match &cx.props.handlerKey
	{
		Some(skillType) => {
			let mut next = clickedValue;
			
			if clickedValue == cx.props.value { next -= 1; }
			if next > cx.props.max { next = cx.props.max; }
			
			updateBaseSkill(cx, skillType, next);
		},
		None => {}
	}
}

// -----

pub fn SkillSpecialties(cx: Scope) -> Element
{
	let specialtiesRef = use_atom_ref(&cx, CharacterSpecialties);
	let specialties = specialtiesRef.read();
	
	let clickedX = use_state(&cx, || 0);
	let clickedY = use_state(&cx, || 0);
	let lastIndex = use_state(&cx, || 0);
	let showRemove = use_state(&cx, || false);
	
	let posX = *clickedX.get() - RemovePopUpXOffset;
	let posY = *clickedY.get() - RemovePopUpYOffset;
	
	return cx.render(rsx!
	{
		div
		{
			class: "skillSpecialtiesWrapper cod column",
			
			div { class: "skillSpecialtiesLabel", "Specialties" },
			
			div
			{
				class: "column",
				
				specialties.iter().enumerate().map(|(i, specialty)| {
					rsx!(cx, div
					{
						class: "row",
						key: "{i}",
						oncontextmenu: move |e|
						{
							e.cancel_bubble();
							clickedX.set(e.data.client_x);
							clickedY.set(e.data.client_y);
							lastIndex.set(i);
							showRemove.set(true);
						},
						prevent_default: "oncontextmenu",
						
						input
						{
							r#type: "text",
							value: "{specialty}",
							onchange: move |e| skillSpecialtyHandler(e, &cx, Some(i)),
							oncontextmenu: move |e|
							{
								e.cancel_bubble();
								clickedX.set(e.data.client_x);
								clickedY.set(e.data.client_y);
								lastIndex.set(i);
								showRemove.set(true);
							},
							prevent_default: "oncontextmenu"
						}
					})
				})
				
				div
				{
					class: "row",
					input { r#type: "text", value: "", placeholder: "Enter new a Specialty", onchange: move |e| skillSpecialtyHandler(e, &cx, None), oncontextmenu: move |e| e.cancel_bubble(), prevent_default: "oncontextmenu" }
				}
					
				showRemove.then(|| rsx!{
					div
					{
						class: "removePopUpWrapper column",
						style: "left: {posX}px; top: {posY}px;",
						onclick: move |e| { e.cancel_bubble(); showRemove.set(false); },
						prevent_default: "onclick",
						
						div
						{
							class: "removePopUp column",
							
							div { class: "row", "Are you sure you want to remove this Specialty?" }
							div
							{
								class: "row",
								
								button { onclick: move |e| { e.cancel_bubble(); removeClickHandler(&cx, *(lastIndex.get())); showRemove.set(false); }, prevent_default: "onclick", "Remove" }
								button { onclick: move |e| { e.cancel_bubble(); showRemove.set(false); }, prevent_default: "onclick", "Cancel" }
							}
						}
					}
				})
			}
		}
	});
}

fn removeClickHandler(cx: &Scope, index: usize)
{
	let specialtiesRef = use_atom_ref(&cx, CharacterSpecialties);
	let mut specialties = specialtiesRef.write();
	
	if index < specialties.len()
	{
		specialties.remove(index);
	}
}

fn skillSpecialtyHandler(e: FormEvent, cx: &Scope, index: Option<usize>)
{
	let specialtiesRef = use_atom_ref(&cx, CharacterSpecialties);
	let mut specialties = specialtiesRef.write();
	
	match index
	{
		Some(i) => { specialties[i] = e.value.to_string(); }
		None => { specialties.push(e.value.to_string()); }
	}
}
