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
	components::cod::dots::{
		Dots,
		DotsProps,
	},
	core::util::{
		RemovePopUpXOffset,
		RemovePopUpYOffset,
		singularize,
	},
};

/// The properties struct for `SimpleEntryList`.
/// 
/// `label` needs to be singular for when it's displayed in the remove pop up.
/// It will be pluralized for the component label.
#[derive(Props)]
pub struct SimpleEntryListProps
{
	data: Vec<String>,
	label: String,
	entryRemoveHandler: fn(&Scope<SimpleEntryListProps>, usize),
	entryUpdateHandler: fn(FormEvent, &Scope<SimpleEntryListProps>, Option<usize>),
	
	#[props(optional)]
	class: Option<String>,
}

impl PartialEq for SimpleEntryListProps
{
	fn eq(&self, other: &Self) -> bool
	{
		return self.class == other.class
			&& self.data == other.data
			&& self.label == other.label;
	}
}

/// A generic UI Component for an editable list of text inputs.
pub fn SimpleEntryList(cx: Scope<SimpleEntryListProps>) -> Element
{
	let clickedX = use_state(&cx, || 0);
	let clickedY = use_state(&cx, || 0);
	let lastIndex = use_state(&cx, || 0);
	let showRemove = use_state(&cx, || false);
	
	let posX = *clickedX.get() - RemovePopUpXOffset;
	let posY = *clickedY.get() - RemovePopUpYOffset;
	let singularLabel = singularize(cx.props.label.clone());
	
	let className = match &cx.props.class
	{
		Some(c) => c.clone(),
		None => "".to_string()
	};
	
	return cx.render(rsx!
	{
		div
		{
			class: "simpleEntryListWrapper column {className}",
			
			div { class: "simpleEntryListLabel", "{cx.props.label}" },
			
			div
			{
				class: "simpleEntryList column",
				
				cx.props.data.iter().enumerate().map(|(i, entry)| {
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
							value: "{entry}",
							onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, Some(i)),
							oncontextmenu: move |e|
							{
								e.cancel_bubble();
								clickedX.set(e.data.client_x);
								clickedY.set(e.data.client_y);
								lastIndex.set(i);
								showRemove.set(true);
							},
							prevent_default: "oncontextmenu",
						}
					})
				})
				
				div
				{
					class: "row",
					input { r#type: "text", value: "", placeholder: "Enter new a {singularLabel}", onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, None), oncontextmenu: move |e| e.cancel_bubble(), prevent_default: "oncontextmenu" }
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
							
							div { class: "row", "Are you sure you want to remove this {singularLabel}?" }
							div
							{
								class: "row",
								
								button { onclick: move |e| { e.cancel_bubble(); (cx.props.entryRemoveHandler)(&cx, *(lastIndex.get())); showRemove.set(false); }, prevent_default: "onclick", "Remove" }
								button { onclick: move |e| { e.cancel_bubble(); showRemove.set(false); }, prevent_default: "onclick", "Cancel" }
							}
						}
					}
				})
			}
		}
	});
}

// --------------------------------------------------

/// The properties struct for `DotEntryList`.
/// 
/// `label` needs to be singular for when it's displayed in the remove pop up.
/// It will be pluralized for the component label.
#[derive(Props)]
pub struct DotEntryListProps
{
	data: Vec<(String, usize)>,
	label: String,
	entryDotHandler: fn(&Scope<DotsProps<usize>>, usize),
	entryRemoveHandler: fn(&Scope<DotEntryListProps>, usize),
	entryUpdateHandler: fn(FormEvent, &Scope<DotEntryListProps>, Option<usize>),
	
	#[props(optional)]
	class: Option<String>,
}

impl PartialEq for DotEntryListProps
{
	fn eq(&self, other: &Self) -> bool
	{
		return self.class == other.class
			&& self.data == other.data
			&& self.label == other.label;
	}
}

/// The UI Component handling a Chronicles of Darkness character's list of Merits.
pub fn DotEntryList(cx: Scope<DotEntryListProps>) -> Element
{
	let clickedX = use_state(&cx, || 0);
	let clickedY = use_state(&cx, || 0);
	let lastIndex = use_state(&cx, || 0);
	let showRemove = use_state(&cx, || false);
	
	let posX = *clickedX.get() - RemovePopUpXOffset;
	let posY = *clickedY.get() - RemovePopUpYOffset;
	let singularLabel = singularize(cx.props.label.clone());
	
	let className = match &cx.props.class
	{
		Some(c) => c.clone(),
		None => "".to_string()
	};
	
	return cx.render(rsx!
	{
		div
		{
			class: "entryListWrapper dots column {className}",
			
			div { class: "entryListLabel", "{cx.props.label}" }
			
			div
			{
				class: "entryList column",
				
				cx.props.data.iter().enumerate().map(|(i, (name, value))| rsx!(cx, div
				{
					key: "{i}",
					class: "entry row",
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
						value: "{name}",
						onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, Some(i)),
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
					Dots { max: 5, value: *value, handler: cx.props.entryDotHandler, handlerKey: i }
				}))
				
				div
				{
					class: "entry row",
					input { r#type: "text", value: "", placeholder: "Enter new a {singularLabel}", onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, None), oncontextmenu: move |e| e.cancel_bubble(), prevent_default: "oncontextmenu" }
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
							
							div { class: "row", "Are you sure you want to remove this {singularLabel}?" }
							div
							{
								class: "row",
								
								button { onclick: move |e| { e.cancel_bubble(); (cx.props.entryRemoveHandler)(&cx, *(lastIndex.get())); showRemove.set(false); }, prevent_default: "onclick", "Remove" }
								button { onclick: move |e| { e.cancel_bubble(); showRemove.set(false); }, prevent_default: "onclick", "Cancel" }
							}
						}
					}
				})
			}
		}
	});
}

// --------------------------------------------------

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
	let singularLabel = singularize(cx.props.label.clone());
	
	return cx.render(rsx!
	{
		div
		{
			class: "entryListWrapper abilities column",
			
			div { class: "entryListLabel", "{cx.props.label}" }
			
			div
			{
				class: "entryList column",
				
				cx.props.data.iter().enumerate().map(|(i, ability)| rsx!(cx,
					(i > 0).then(|| rsx!(cx, hr { class: "row" }))
					div
					{
						class: "entry column",
						oncontextmenu: move |e|
						{
							e.cancel_bubble();
							clickedX.set(e.data.client_x);
							clickedY.set(e.data.client_y);
							lastIndex.set(i);
							showRemove.set(true);
						},
						prevent_default: "oncontextmenu",
						
						div
						{
							class: "row",
							oncontextmenu: move |e|
							{
								e.cancel_bubble();
								clickedX.set(e.data.client_x);
								clickedY.set(e.data.client_y);
								lastIndex.set(i);
								showRemove.set(true);
							},
							prevent_default: "oncontextmenu",
							
							div { class: "label first", "Name:" }
							input
							{
								r#type: "text",
								value: "{ability.name}",
								onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, Some(i), ActiveAbilityField::Name),
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
							div { class: "label second", "Cost:" }
							input
							{
								r#type: "text",
								value: "{ability.cost}",
								onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, Some(i), ActiveAbilityField::Cost),
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
						}
						
						div
						{
							class: "column",
							div { class: "label", "Description:" }
							textarea
							{
								onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, Some(i), ActiveAbilityField::Description),
								oncontextmenu: move |e|
								{
									e.cancel_bubble();
									clickedX.set(e.data.client_x);
									clickedY.set(e.data.client_y);
									lastIndex.set(i);
									showRemove.set(true);
								},
								prevent_default: "oncontextmenu",
								
								"{ability.description}"
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
							div { class: "label second", "Action:" }
							input
							{
								r#type: "text",
								value: "{ability.action}",
								onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, Some(i), ActiveAbilityField::Action),
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
						}
						
						div
						{
							class: "row",
							
							div { class: "label first", "Requirements:" }
							input
							{
								r#type: "text",
								value: "{ability.requirements}",
								onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, Some(i), ActiveAbilityField::Requirements),
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
							div { class: "label second", "Duration:" }
							input
							{
								r#type: "text",
								value: "{ability.duration}",
								onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, Some(i), ActiveAbilityField::Duration),
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
						}
						
						div
						{
							class: "column",
							div { class: "label", "Effects:" }
							textarea
							{
								onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, Some(i), ActiveAbilityField::Effects),
								oncontextmenu: move |e|
								{
									e.cancel_bubble();
									clickedX.set(e.data.client_x);
									clickedY.set(e.data.client_y);
									lastIndex.set(i);
									showRemove.set(true);
								},
								prevent_default: "oncontextmenu",
								
								"{ability.effects}"
							}
						}
					}
				))
				
				div
				{
					class: "new entry row",
					input { r#type: "text", value: "", placeholder: "Enter a new {singularLabel}", onchange: move |e| (cx.props.entryUpdateHandler)(e, &cx, None, ActiveAbilityField::Name), oncontextmenu: move |e| e.cancel_bubble(), prevent_default: "oncontextmenu" }
				}
			}
			
			showRemove.then(|| rsx!
			{
				div
				{
					class: "removePopUpWrapper column",
					style: "left: {posX}px; top: {posY}px;",
					onclick: move |e| { e.cancel_bubble(); showRemove.set(false); },
					prevent_default: "onclick",
					
					div
					{
						class: "removePopUp column",
						
						div { class: "row", "Are you sure you want to remove this {singularLabel}?" }
						div
						{
							class: "row",
							
							button { onclick: move |e| { e.cancel_bubble(); (cx.props.entryRemoveHandler)(&cx, *(lastIndex.get())); showRemove.set(false); }, prevent_default: "onclick", "Remove" }
							button { onclick: move |e| { e.cancel_bubble(); showRemove.set(false); }, prevent_default: "onclick", "Cancel" }
						}
					}
				}
			})
		}
	});
}
