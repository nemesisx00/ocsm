#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::wod::traits::Attributes;

#[derive(PartialEq, Props)]
pub struct AttributesProps
{
	attributes: Attributes,
}

pub fn Attributes(scope: Scope<AttributesProps>) -> Element
{
	let labelClass = "label";
	let dotsClass = "extended dots";
	
	return scope.render(rsx!
	{
		div
		{
			class: "attributes",
			
			div
			{
				class: "column",
				
				div { class: "{labelClass}", "{scope.props.attributes.strength.name}:" }
				div { class: "{labelClass}", "{scope.props.attributes.dexterity.name}:" }
				div { class: "{labelClass}", "{scope.props.attributes.stamina.name}:" }
			}
			
			div
			{
				class: "column",
				
				div { class: "{dotsClass}", "{scope.props.attributes.strength.value}" }
				div { class: "{dotsClass}", "{scope.props.attributes.dexterity.value}" }
				div { class: "{dotsClass}", "{scope.props.attributes.stamina.value}" }
			},
			
			div
			{
				class: "column",
				
				div { class: "{labelClass}", "{scope.props.attributes.presence.name}:" }
				div { class: "{labelClass}", "{scope.props.attributes.manipulation.name}:" }
				div { class: "{labelClass}", "{scope.props.attributes.composure.name}:" }
			}
			
			div
			{
				class: "column",
				
				div { class: "{dotsClass}", "{scope.props.attributes.presence.value}" }
				div { class: "{dotsClass}", "{scope.props.attributes.manipulation.value}" }
				div { class: "{dotsClass}", "{scope.props.attributes.composure.value}" }
			},
			
			div
			{
				class: "column",
				
				div { class: "{labelClass}", "{scope.props.attributes.intelligence.name}:" }
				div { class: "{labelClass}", "{scope.props.attributes.wits.name}:" }
				div { class: "{labelClass}", "{scope.props.attributes.resolve.name}:" }
			}
			
			div
			{
				class: "column",
				
				div { class: "{dotsClass}", "{scope.props.attributes.intelligence.value}" }
				div { class: "{dotsClass}", "{scope.props.attributes.wits.value}" }
				div { class: "{dotsClass}", "{scope.props.attributes.resolve.value}" }
			}
		}
	});
}
