#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::{
	events::FormEvent,
	prelude::*,
};
use crate::{
	cod::{
		enums::ActiveAbilityField,
		structs::ActiveAbility,
	},
	components::{
		core::{
			events::{
				hideRemovePopUp,
				showRemovePopUpWithIndex,
			},
		},
	},
	core::util::{
		RemovePopUpXOffset,
		RemovePopUpYOffset,
		singularize,
	},
};

/// The properties struct for `ActiveAbilities`.
#[derive(Props)]
pub struct ActiveAbilitiesProps
{
	data: Vec<ActiveAbility>,
	label: String,
	entryRemoveHandler: fn(&Scope<ActiveAbilitiesProps>, usize),
	entryUpdateHandler: fn(FormEvent, &Scope<ActiveAbilitiesProps>, Option<usize>, ActiveAbilityField),
	
	#[props(optional)]
	class: Option<String>,
	
	#[props(optional)]
	singularOverride: Option<String>,
}

impl PartialEq for ActiveAbilitiesProps
{
	fn eq(&self, other: &Self) -> bool
	{
		return self.class == other.class
			&& self.data == other.data
			&& self.label == other.label;
	}
}

/// The UI Component handling a Chronicles of Darkness character's list of Active Abilities.
pub fn ActiveAbilities(cx: Scope<ActiveAbilitiesProps>) -> Element
{
	let clickedX = use_state(&cx, || 0);
	let clickedY = use_state(&cx, || 0);
	let lastIndex = use_state(&cx, || 0);
	let showRemove = use_state(&cx, || false);
	
	let posX = *clickedX.get() - RemovePopUpXOffset;
	let posY = *clickedY.get() - RemovePopUpYOffset;
	
	let singularLabel = match &cx.props.singularOverride
	{
		Some(so) => so.clone(),
		None => singularize(cx.props.label.clone())
	};
	
	return cx.render(rsx!
	{
		div
		{
			class: "simpleEntryListWrapper abilities column justEven",
			
			div { class: "simpleEntryListLabel", "{cx.props.label}" }
			
			div
			{
				class: "simpleEntryList column justEven",
				
				cx.props.data.iter().enumerate().map(|(i, ability)| rsx!(cx,
					(i > 0).then(|| rsx!(cx, hr { class: "row justEven thin" }))
					div
					{
						class: "entry column justStart",
						oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
						prevent_default: "oncontextmenu",
						
						div
						{
							class: "row justEven",
							oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
							prevent_default: "oncontextmenu",
							
							div { class: "label first", "Name:" }
							input
							{
								r#type: "text",
								value: "{ability.name}",
								onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, Some(i), ActiveAbilityField::Name),
								oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
								prevent_default: "oncontextmenu"
							}
							div { class: "label second", "Cost:" }
							input
							{
								r#type: "text",
								value: "{ability.cost}",
								onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, Some(i), ActiveAbilityField::Cost),
								oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
								prevent_default: "oncontextmenu"
							}
						}
						
						div
						{
							class: "row",
							
							div { class: "label first", "Dice Pool:" }
							input
							{
								r#type: "text",
								value: "{ability.dicePool}",
								onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, Some(i), ActiveAbilityField::DicePool),
								oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
								prevent_default: "oncontextmenu"
							}
							div { class: "label second", "Action:" }
							input
							{
								r#type: "text",
								value: "{ability.action}",
								onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, Some(i), ActiveAbilityField::Action),
								oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
								prevent_default: "oncontextmenu"
							}
						}
						
						div
						{
							class: "row justEven",
							
							div { class: "label first", "Requirements:" }
							input
							{
								r#type: "text",
								value: "{ability.requirements}",
								onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, Some(i), ActiveAbilityField::Requirements),
								oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
								prevent_default: "oncontextmenu"
							}
							div { class: "label second", "Duration:" }
							input
							{
								r#type: "text",
								value: "{ability.duration}",
								onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, Some(i), ActiveAbilityField::Duration),
								oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
								prevent_default: "oncontextmenu"
							}
						}
						
						div
						{
							class: "column justEven",
							div { class: "label", "Description:" }
							textarea
							{
								onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, Some(i), ActiveAbilityField::Description),
								oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
								prevent_default: "oncontextmenu",
								
								"{ability.description}"
							}
						}
						
						div
						{
							class: "column justEven",
							div { class: "label", "Effects:" }
							textarea
							{
								onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, Some(i), ActiveAbilityField::Effects),
								oncontextmenu: move |e| showRemovePopUpWithIndex(e, &clickedX, &clickedY, &showRemove, &lastIndex, i),
								prevent_default: "oncontextmenu",
								
								"{ability.effects}"
							}
						}
					}
				))
				
				div
				{
					class: "new row justEven",
					input { r#type: "text", value: "", placeholder: "Enter a new {singularLabel}", onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, None, ActiveAbilityField::Name), prevent_default: "oncontextmenu" }
				}
			}
			
			showRemove.then(|| rsx!
			{
				div
				{
					class: "removePopUpWrapper column justEven",
					style: "left: {posX}px; top: {posY}px;",
					onclick: move |e| hideRemovePopUp(e, &showRemove),
					prevent_default: "oncontextmenu",
					
					div
					{
						class: "removePopUp column justEven",
						
						div { class: "row justEven", "Are you sure you want to remove this {singularLabel}?" }
						div
						{
							class: "row justEven",
							
							button { onclick: move |e| { hideRemovePopUp(e, &showRemove); (cx.props.entryRemoveHandler)(&cx, *(lastIndex.get())); }, prevent_default: "oncontextmenu", "Remove" }
							button { onclick: move |e| hideRemovePopUp(e, &showRemove), prevent_default: "oncontextmenu", "Cancel" }
						}
					}
				}
			})
		}
	});
}
