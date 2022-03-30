#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::core::state::MainMenuState;

/// The properties struct for the `MainMenu` component.
#[derive(Props)]
pub struct MainMenuProps<'a>
{
    children: Element<'a>
}

/// The UI component defining the main menu.
/// 
/// There should only ever be one of these at a time.
pub fn MainMenu<'a>(cx: Scope<'a, MainMenuProps<'a>>) -> Element<'a>
{
	return cx.render(rsx!
	{
		div
		{
			class: "mainMenu row",
			oncontextmenu: move |e| e.cancel_bubble(),
			prevent_default: "oncontextmenu",
			
			&cx.props.children
		}
	});
}

/// The properties struct for the `Menu` component.
#[derive(Props)]
pub struct MenuProps<'a>
{
	label: String,
    children: Element<'a>,
	
	#[props(optional)]
	class: Option<String>,
	
	#[props(optional)]
	child: Option<bool>,
}

/// UI component defining a single menu within the main menu.
/// 
/// Can contain `Menu`s and `MenuItem`s as children.
pub fn Menu<'a>(cx: Scope<'a, MenuProps<'a>>) -> Element<'a>
{
	let mainMenuState = use_read(&cx, MainMenuState);
	let setMainMenuState = use_set(&cx, MainMenuState);
	
	//Per instance state
	let state = use_state(&cx, || false);
	let submenu = use_state(&cx, || false);
	
	//Check if the App is telling us to close all menus
	if !mainMenuState
	{
		state.set(false);
	}
	
	if !state.get()
	{
		setMainMenuState(true);
	}
	
	let mut class = "".to_string();
	match &cx.props.class
	{
		Some(c) => { class += &format!(" {}", c); }
		None => {}
	}
	
	match &cx.props.child
	{
		Some(isChild) =>
		{
			submenu.set(*isChild);
			class += " childMenu";
		}
		None => { submenu.set(false); }
	}
	
	return cx.render(rsx!
	{
		div
		{
			class: "menu column{class}",
			onclick: move |e| { e.cancel_bubble(); state.set(!state.get()); },
			oncontextmenu: move |e| e.cancel_bubble(),
			//onmouseover: move |_| { if *submenu.get() { state.set(true); } }, //TODO: Figure out how to give this a bit of a delay/buffer for a second or two
			prevent_default: "oncontextmenu",
			
			div
			{
				class: "label",
				"{cx.props.label}"
			}
			
			state.then(|| rsx!{
				div
				{
					class: "subMenu column",
					//Right now this means we only support one level of depth for child menus. Will need more complexity if it becomes necessary to support more than one level.
					onclick: move |_| { setMainMenuState(false); },
					oncontextmenu: move |e| e.cancel_bubble(),
					//onmouseout: move |_| { if *submenu.get() { state.set(false); } }, //TODO: Figure out how to give this a bit of a delay/buffer for a second or two
					prevent_default: "oncontextmenu",
					
					&cx.props.children
				}
			})
		}
	});
}

/// The properties struct for `MenuItem`.
#[derive(Props)]
pub struct MenuItemProps<'a>
{
	pub label: String,
	
	#[props(optional)]
	handler: Option<fn(&Scope<'a, MenuItemProps<'a>>)>,
}

/// UI component defining a single item within a `Menu`.
pub fn MenuItem<'a>(cx: Scope<'a, MenuItemProps<'a>>) -> Element<'a>
{
	return cx.render(rsx!
	{
		div
		{
			class: "menuItem",
			onclick: move |_| {
				//Don't cancel bubble here so the submenu closes automatically
				match &cx.props.handler
				{
					Some(h) => { h(&cx); }
					None => {}
				}
			},
			oncontextmenu: move |e| e.cancel_bubble(),
			prevent_default: "oncontextmenu",
			"{cx.props.label}"
		}
	});
}
