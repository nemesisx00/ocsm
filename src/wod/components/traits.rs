#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::wod::traits::Attributes;

#[derive(PartialEq, Props)]
pub struct AttributesProps
{
	attributes: Attributes,
}

pub fn Attributes(cx: Scope<AttributesProps>) -> Element
{
	let labelClass = "label";
	let dotsClass = "extended dots";
	
	return cx.render(rsx!
	{
		div
		{
			class: "attributes",
			
			div
			{
				class: "column",
				
				div { class: "{labelClass}", "{cx.props.attributes.strength.name}:" }
				div { class: "{labelClass}", "{cx.props.attributes.dexterity.name}:" }
				div { class: "{labelClass}", "{cx.props.attributes.stamina.name}:" }
			}
			
			div
			{
				class: "column",
				
				div { class: "{dotsClass}", "{cx.props.attributes.strength.value}" }
				div { class: "{dotsClass}", "{cx.props.attributes.dexterity.value}" }
				div { class: "{dotsClass}", "{cx.props.attributes.stamina.value}" }
			},
			
			div
			{
				class: "column",
				
				div { class: "{labelClass}", "{cx.props.attributes.presence.name}:" }
				div { class: "{labelClass}", "{cx.props.attributes.manipulation.name}:" }
				div { class: "{labelClass}", "{cx.props.attributes.composure.name}:" }
			}
			
			div
			{
				class: "column",
				
				div { class: "{dotsClass}", "{cx.props.attributes.presence.value}" }
				div { class: "{dotsClass}", "{cx.props.attributes.manipulation.value}" }
				div { class: "{dotsClass}", "{cx.props.attributes.composure.value}" }
			},
			
			div
			{
				class: "column",
				
				div { class: "{labelClass}", "{cx.props.attributes.intelligence.name}:" }
				div { class: "{labelClass}", "{cx.props.attributes.wits.name}:" }
				div { class: "{labelClass}", "{cx.props.attributes.resolve.name}:" }
			}
			
			div
			{
				class: "column",
				
				div { class: "{dotsClass}", "{cx.props.attributes.intelligence.value}" }
				div { class: "{dotsClass}", "{cx.props.attributes.wits.value}" }
				div { class: "{dotsClass}", "{cx.props.attributes.resolve.value}" }
			}
		}
	});
}
