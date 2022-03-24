#![allow(non_snake_case, non_upper_case_globals)]

use dioxus::prelude::*;
use dioxus::desktop::use_window;
use crate::{
	core::{
		components::menu::{
			MainMenu,
			Menu,
			MenuItem,
		},
		template::StatefulTemplate,
		io::{
			loadFromFile,
			saveToFile,
		}
	},
	cod::{
		vtr2e::{
			components::sheet::Sheet,
			template::Kindred,
		},
	},
};

pub static MainMenuState: Atom<bool> = |_| true;

pub fn App(cx: Scope) -> Element
{
	let setMenuState = use_set(&cx, MainMenuState);
	
	return cx.render(rsx!
	{
		style { [include_str!("../static/app.css")] }
		
		div
		{
			class: "app column",
			onclick: move |e| { e.cancel_bubble(); setMenuState(false); },
			oncontextmenu: move |e| e.cancel_bubble(),
			prevent_default: "oncontextmenu",
			
			MainMenu
			{
				Menu
				{
					label: "File",
					
					MenuItem { label: "New", handler: newSheetHandler }
					MenuItem { label: "Open", handler: openSheetHandler }
					Menu { class: "childMenu", label: "Recent Characters", MenuItem { label: "Character 1" }, MenuItem { label: "Character 2" } }
					MenuItem { label: "Save", handler: saveSheetHandler }
					MenuItem { label: "Exit", handler: exitHandler }
				}
			}
			
			Sheet { }
		}
	});
}

fn exitHandler<T>(cx: &Scope<T>)
{
	let window = use_window(&cx);
	window.close();
}

fn newSheetHandler<T>(cx: &Scope<T>)
{
	let sheet = Kindred::default();
	sheet.push(cx);
}

fn openSheetHandler<T>(cx: &Scope<T>)
{
	let sheet: Kindred = loadFromFile::<Kindred>(&"./test/Sheet.json".to_string()).unwrap();
	sheet.push(&cx);
}

fn saveSheetHandler<T>(cx: &Scope<T>)
{
	let mut sheet = Kindred::default();
	sheet.pull(&cx);
	match saveToFile(&"./test/Sheet.json".to_string(), &sheet)
	{
		Ok(json) => { println!("Saved to json file! {}", json); }
		Err(e) => { println!("Error saving to file: {}", e); }
	}
}
