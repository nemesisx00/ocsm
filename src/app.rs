#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::HashMap;
use dioxus::prelude::*;
use dioxus::desktop::use_window;
use strum_macros::{
	AsRefStr,
	EnumCount,
	EnumIter,
};
use crate::{
	core::{
		components::menu::{
			MainMenu,
			Menu,
			MenuItem,
			MenuItemProps,
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
pub static Sheets: AtomRef<HashMap<GameSystems, String>> = |_| HashMap::<GameSystems, String>::new();

#[derive(AsRefStr, Clone, Copy, Debug, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Ord)]
pub enum GameSystems
{
	//CodBeast,
	CodChangeling2e,
	//CodDemon,
	//CodGeist,
	//CodHunter,
	CodMage2e,
	//CodMummy,
	//CodPromethean,
	CodVampire2e,
	//CodWerewolf,
	Dnd5e,
	WodVampireV5,
}

fn loadSheets(cx: &Scope)
{
	let sheetsRef = use_atom_ref(cx, Sheets);
	let mut sheets = sheetsRef.write();
	
	if sheets.len() == 0
	{
		sheets.insert(GameSystems::CodChangeling2e, "Changeling: The Lost 2e".to_string());
		sheets.insert(GameSystems::CodMage2e, "Mage: The Awakening 2e".to_string());
		sheets.insert(GameSystems::CodVampire2e, "Vampire: The Requiem 2e".to_string());
		sheets.insert(GameSystems::Dnd5e, "Dungeons & Dragons 5E".to_string());
		sheets.insert(GameSystems::WodVampireV5, "Vampire: The Masquerade V5".to_string());
	}
}

pub fn App(cx: Scope) -> Element
{
	loadSheets(&cx);
	
	let sheetsRef = use_atom_ref(&cx, Sheets);
	let sheets = sheetsRef.read();
	
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
					label: "File".to_string(),
					
					Menu { child: true, label: "New".to_string(), sheets.iter().map(|(_, l)| rsx!(cx, MenuItem { label: l.clone(), handler: newSheetHandler })) }
					MenuItem { label: "Open".to_string(), handler: openSheetHandler }
					Menu {  child: true, label: "Recent Characters".to_string(), MenuItem { label: "Character 1".to_string() }, MenuItem { label: "Character 2".to_string() } }
					MenuItem { label: "Save".to_string(), handler: saveSheetHandler }
					MenuItem { label: "Exit".to_string(), handler: exitHandler }
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

fn newSheetHandler(cx: &Scope<MenuItemProps>)
{
	let sheetsRef = use_atom_ref(cx, Sheets);
	let sheets = sheetsRef.read();
	
	match sheets.iter().filter(|(_, label)| *label == &cx.props.label.to_string()).next()
	{
		Some((st, _)) =>
		{
			match st
			{
				GameSystems::CodVampire2e => { let mut sheet = Kindred::default(); sheet.push(cx); }
				
				_ => {}
			}
		}
		None => {}
	}
}

fn openSheetHandler<T>(cx: &Scope<T>)
{
	match loadFromFile::<Kindred>(&"./test/Sheet.json".to_string())
	{
		Ok(data) =>
		{
			let mut sheet: Kindred = data;
			sheet.push(&cx);
		}
		Err(e) => { println!("Failed to loadFromFile: {:?}", e.to_string()); }
	}
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
