#![allow(non_snake_case, non_upper_case_globals)]

use std::collections::BTreeMap;
use dioxus::{
	desktop::use_window,
	prelude::*,
};
use serde::{
	de::DeserializeOwned,
	Serialize,
};
use strum::IntoEnumIterator;
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
		},
		state::resetGlobalState,
	},
	cod::{
		ctl2e::{
			components::sheet::ChangelingSheet,
			template::Changeling,
		},
		vtr2e::{
			components::sheet::VampireSheet,
			template::Vampire,
		},
	},
};

pub static MainMenuState: Atom<bool> = |_| true;
pub static CurrentGameSystem: Atom<GameSystem> = |_| GameSystem::CodChangeling2e;

#[derive(AsRefStr, Clone, Copy, Debug, EnumCount, EnumIter, Eq, Hash, PartialEq, PartialOrd, Ord)]
pub enum GameSystem
{
		//CodBeast,
	CodChangeling2e,
		//CodDemon,
		//CodGeist,
		//CodHunter,
	//CodMage2e,
		//CodMummy,
		//CodPromethean,
	CodVampire2e,
		//CodWerewolf,
	//Dnd5e,
	//WodVampireV5,
}

impl GameSystem
{
	pub fn asMap() -> BTreeMap<Self, String>
	{
		let mut map = BTreeMap::new();
		map.insert(GameSystem::CodChangeling2e, "Changeling: The Lost 2e".to_string());
		//map.insert(GameSystem::CodMage2e, "Mage: The Awakening 2e".to_string());
		map.insert(GameSystem::CodVampire2e, "Vampire: The Requiem 2e".to_string());
		//map.insert(GameSystem::Dnd5e, "Dungeons & Dragons 5E".to_string());
		//map.insert(GameSystem::WodVampireV5, "Vampire: The Masquerade V5".to_string());
		return map;
	}
	
	pub fn showMap() -> BTreeMap<Self, bool>
	{
		let mut map = BTreeMap::new();
		for gs in GameSystem::iter()
		{
			map.insert(gs, false);
		}
		return map;
	}
}

pub fn App(cx: Scope) -> Element
{
	let setMenuState = use_set(&cx, MainMenuState);
	let currentGameSystem = use_read(&cx, CurrentGameSystem);
	
	let sheets = GameSystem::asMap();
	let mut show = GameSystem::showMap();
	match show.get_mut(currentGameSystem)
	{
		Some(showGs) => { *showGs = true; }
		None => {}
	}
	
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
					show[&GameSystem::CodChangeling2e].then(|| rsx!
					{
						MenuItem { label: "Open".to_string(), handler: openSheetHandler::<Changeling> }
						MenuItem { label: "Save".to_string(), handler: saveSheetHandler::<Changeling> }
					})
					show[&GameSystem::CodVampire2e].then(|| rsx!
					{
						MenuItem { label: "Open".to_string(), handler: openSheetHandler::<Vampire> }
						MenuItem { label: "Save".to_string(), handler: saveSheetHandler::<Vampire> }
					})
					MenuItem { label: "Exit".to_string(), handler: exitHandler }
				}
			}
			
			show[&GameSystem::CodChangeling2e].then(|| rsx! { ChangelingSheet {} })
			show[&GameSystem::CodVampire2e].then(|| rsx! { VampireSheet {} })
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
	let setCurrentGameSystem = use_set(&cx, CurrentGameSystem);
	
	match GameSystem::asMap().iter().filter(|(_, label)| *label == &cx.props.label.to_string()).next()
	{
		Some((gs, _)) =>
		{
			setCurrentGameSystem(*gs);
			match gs
			{
				GameSystem::CodChangeling2e => { pushSheet::<Changeling>(cx); }
				GameSystem::CodVampire2e => { pushSheet::<Vampire>(cx); }
			}
		}
		None => {}
	}
}

fn pushSheet<T: Default + StatefulTemplate>(cx: &Scope<MenuItemProps>)
{
	resetGlobalState(cx);
	let mut sheet = T::default();
	sheet.push(cx);
}

fn openSheetHandler<T: DeserializeOwned + Serialize + StatefulTemplate>(cx: &Scope<MenuItemProps>)
{
	match loadFromFile::<T>(&"./test/Sheet.json".to_string())
	{
		Ok(data) =>
		{
			resetGlobalState(cx);
			let mut sheet: T = data;
			sheet.push(&cx);
		}
		Err(e) => { println!("Failed to loadFromFile: {:?}", e.to_string()); }
	}
}

fn saveSheetHandler<T: Default + DeserializeOwned + Serialize + StatefulTemplate>(cx: &Scope<MenuItemProps>)
{
	let mut sheet = T::default();
	sheet.pull(&cx);
	
	match saveToFile(&"./test/Sheet.json".to_string(), &sheet)
	{
		Ok(json) => { println!("Saved to json file! {}", json); }
		Err(e) => { println!("Error saving to file: {}", e); }
	}
}
