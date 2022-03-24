#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use crate::app::MainMenuState;

#[inline_props]
pub fn MainMenu<'a>(cx: Scope, children: Element<'a>) -> Element<'a>
{
	return cx.render(rsx!
	{
		div
		{
			class: "mainMenu row",
			oncontextmenu: move |e| e.cancel_bubble(),
			prevent_default: "oncontextmenu",
			
			children
		}
	});
}

#[derive(Props)]
pub struct MenuProps<'a>
{
	label: &'a str,
    children: Element<'a>,
	
	#[props(optional)]
	class: Option<&'a str>,
}

pub fn Menu<'a>(cx: Scope<'a, MenuProps<'a>>) -> Element<'a>
{
	let state = use_state(&cx, || false);
	
	//Check if the App is telling us to close all menus
	let appMenuState = use_read(&cx, MainMenuState);
	let setAppMenuState = use_set(&cx, MainMenuState);
	match appMenuState
	{
		false => {
			state.set(false);
			setAppMenuState(true);
		}
		true => {}
	}
	
	let mut class = "".to_string();
	match &cx.props.class
	{
		Some(c) => { class += &format!(" {}", c); }
		None => {}
	}
	
	return cx.render(rsx!
	{
		div
		{
			class: "menu column{class}",
			
			div
			{
				class: "label",
				onclick: move |e| { e.cancel_bubble(); state.set(!state.get()); },
				oncontextmenu: move |e| e.cancel_bubble(),
				prevent_default: "oncontextmenu",
				"{cx.props.label}"
			}
			
			state.then(|| rsx!{
				div
				{
					class: "subMenu column",
					onclick: move |_| { state.set(!state.get()); },
					oncontextmenu: move |e| e.cancel_bubble(),
					prevent_default: "oncontextmenu",
					
					&cx.props.children
				}
			})
		}
	});
}

#[derive(Props)]
pub struct MenuItemProps<'a>
{
	label: &'a str,
	
	#[props(optional)]
	handler: Option<fn(&Scope<'a, MenuItemProps<'a>>)>,
}

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
