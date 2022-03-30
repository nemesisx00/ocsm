#![allow(non_snake_case, non_upper_case_globals)]

use std::{
	collections::BTreeMap,
	path::Path,
};
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
		components::{
			menu::{
				MainMenu,
				Menu,
				MenuItem,
				MenuItemProps,
			},
		},
		io::{
			getFilePath,
			loadFromFile,
			saveToFile,
		},
		state::{
			CurrentFilePath,
			StatefulTemplate,
			resetGlobalState,
		},
	},
	cod::{
		ctl2e::{
			components::sheet::ChangelingSheet,
			structs::Changeling,
		},
		vtr2e::{
			components::sheet::VampireSheet,
			structs::Vampire,
		},
	},
	WindowTitle,
};

/// Control switch used to signal all Menu components to hide their submenus.
/// 
/// Default value is `true`. Set to `false` to close all Menus.
pub static MainMenuState: Atom<bool> = |_| true;
/// The active `GameSystem` determining which character sheet is rendered.
pub static CurrentGameSystem: Atom<GameSystem> = |_| GameSystem::CodChangeling2e;

/// Game systems for which character sheets have been implemented.
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
	/// Generate a collection mapping GameSystem to their full human-readable titles.
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
	
	/// Generate a collection mapping GameSystem to a boolean value.
	/// 
	/// Used in tandem with `CurrentGameSystem` to determine whether or not to display
	/// UI elements in the `App` component.
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

/// The application's top-level UI component.
pub fn App(cx: Scope) -> Element
{
	let setMenuState = use_set(&cx, MainMenuState);
	let currentGameSystem = use_read(&cx, CurrentGameSystem);
	
	let sheets = GameSystem::asMap();
	let mut show = GameSystem::showMap();
	match show.get_mut(currentGameSystem)
	{
		Some(showGs) => *showGs = true,
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
					MenuItem { label: "New".to_string(), handler: newSheetHandler }
					MenuItem { label: "Open".to_string(), handler: menuFileOpenHandler }
					MenuItem { label: "Save".to_string(), handler: menuSaveHandler }
					MenuItem { label: "Exit".to_string(), handler: exitHandler }
				}
				
				Menu
				{
					label: "Game Systems".to_string(),
					sheets.iter().map(|(_, l)| rsx!(cx, MenuItem { label: l.clone(), handler: gameSystemHandler }))
				}
			}
			
			show[&GameSystem::CodChangeling2e].then(|| rsx! { ChangelingSheet {} })
			show[&GameSystem::CodVampire2e].then(|| rsx! { VampireSheet {} })
		}
	});
}

/// Event handler triggered by clicking the Exit `MenuItem` in the File `Menu`.
/// 
/// Closes the main `Window`.
fn exitHandler<T>(cx: &Scope<T>)
{
	let window = use_window(&cx);
	window.close();
}

/// Event handler triggered by clicking one of the `MenuItem`s in the Game Systems `Menu`.
/// 
/// Updates the value of `CurrentGameSystem`, the `Window` title, and calls `newSheetHandler`.
fn gameSystemHandler(cx: &Scope<MenuItemProps>)
{
	let setCurrentGameSystem = use_set(cx, CurrentGameSystem);
	let setMenuState = use_set(&cx, MainMenuState);
	let window = use_window(cx);
	
	match GameSystem::asMap().iter().filter(|(_, l)| *l.clone() == cx.props.label).next()
	{
		Some((gs, l)) =>
		{
			setMenuState(false);
			window.set_title(format!("{} - {}", WindowTitle, l).as_ref());
			setCurrentGameSystem(*gs);
			newSheetHandler(cx);
		},
		None => setMenuState(false)
	}
}

/// Event handler triggered by clicking the Open `MenuItem` in the File `Menu`.
/// 
/// Presents the user with am open `FileDialog` to choose which file to open. Updates
/// the value of `CurrentFilePath` and attempts to load the file as a character sheet.
fn menuFileOpenHandler(cx: &Scope<MenuItemProps>)
{
	let currentFilePath = use_read(&cx, CurrentFilePath);
	let currentGameSystem = use_read(&cx, CurrentGameSystem);
	let setCurrentFilePath = use_set(&cx, CurrentFilePath);
	let setMenuState = use_set(&cx, MainMenuState);
	
	setMenuState(false);
	setCurrentFilePath(getFilePath(false, currentFilePath.clone()));
	
	match currentGameSystem
	{
		GameSystem::CodChangeling2e => loadSheet::<Changeling>(cx),
		GameSystem::CodVampire2e => loadSheet::<Vampire>(cx),
	}
}

/// Event handler triggered by clicking the Save `MenuItem` in the File `Menu`.
/// 
/// Presents the user with a save `FileDialog` to choose the file to which to
/// write. Updates the value of `CurrentFilePath` and attempts to write the
/// character sheet to file.
fn menuSaveHandler(cx: &Scope<MenuItemProps>)
{
	let currentFilePath = use_read(&cx, CurrentFilePath);
	let currentGameSystem = use_read(&cx, CurrentGameSystem);
	let setCurrentFilePath = use_set(&cx, CurrentFilePath);
	let setMenuState = use_set(&cx, MainMenuState);
	
	setMenuState(false);
	setCurrentFilePath(getFilePath(true, currentFilePath.clone()));
	
	match currentGameSystem
	{
		GameSystem::CodChangeling2e => saveSheet::<Changeling>(cx),
		GameSystem::CodVampire2e => saveSheet::<Vampire>(cx),
	}
}

/// Event handler triggered by clicking the New `MenuItem` in the File `Menu`.
/// 
/// Resets the global state, which results in a "new" sheet.
fn newSheetHandler(cx: &Scope<MenuItemProps>)
{
	let setMenuState = use_set(&cx, MainMenuState);
	
	resetGlobalState(cx);
	setMenuState(false);
}

// -----

/// Deserialize a character sheet from a file and push it into the global state.
fn loadSheet<T: DeserializeOwned + Serialize + StatefulTemplate>(cx: &Scope<MenuItemProps>)
{
	let currentFilePath = use_read(&cx, CurrentFilePath);
	if let Some(pathStr) = currentFilePath
	{
		match loadFromFile::<T>(&Path::new(pathStr))
		{
			Ok(data) =>
			{
				resetGlobalState(cx);
				let mut sheet: T = data;
				sheet.push(&cx);
			},
			Err(e) => println!("Failed to loadFromFile: {:?}", e.to_string())
		}
	}
}

/// Pull the current character sheet from the global state and serialize it into a file.
fn saveSheet<T: Default + DeserializeOwned + Serialize + StatefulTemplate>(cx: &Scope<MenuItemProps>)
{
	let currentFilePath = use_read(&cx, CurrentFilePath);
	if let Some(pathStr) = currentFilePath
	{
		let mut sheet = T::default();
		sheet.pull(&cx);
		
		match saveToFile(&Path::new(pathStr), &sheet)
		{
			Ok(json) => println!("Saved to json file! {}", json),
			Err(e) => println!("Error saving to file: {}", e)
		}
	}
}
