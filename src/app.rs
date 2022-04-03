#![allow(non_snake_case, non_upper_case_globals)]

use std::path::Path;
use dioxus::{
	desktop::use_window,
	prelude::*,
};
use serde::{
	de::DeserializeOwned,
	Serialize,
};
use crate::{
	cod::{
		structs::CoreCharacter,
		ctl2e::structs::Changeling,
		mta2e::structs::Mage,
		vtr2e::structs::Vampire,
	},
	components::{
		cod::{
			sheet::MortalSheet,
			ctl2e::sheet::ChangelingSheet,
			mta2e::sheet::MageSheet,
			vtr2e::sheet::VampireSheet,
		},
		core::{
			menu::{
				MainMenu,
				Menu,
				MenuItem,
				MenuItemProps,
			},
		},
	},
	core::{
		enums::GameSystem,
		io::{
			deserializeSheet,
			getFilePath,
			getUserDocumentsDir,
			loadFromFile,
			saveToFile,
			serializeSheet,
		},
		state::{
			MainMenuState,
			CurrentGameSystem,
			CurrentFilePath,
			StatefulTemplate,
			resetGlobalState,
		},
		structs::SaveData,
	},
	WindowTitle,
};

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
			class: "app column justEven",
			onclick: move |e| { e.cancel_bubble(); setMenuState(false); },
			prevent_default: "oncontextmenu",
			
			MainMenu
			{
				Menu
				{
					label: "File".to_string(),
					Menu
					{
						child: true,
						label: "New".to_string(),
						sheets.iter().map(|(_, l)| rsx!(cx, MenuItem { label: l.clone(), handler: newSheetHandler }))
					}
					MenuItem { label: "Open".to_string(), handler: menuFileOpenHandler }
					MenuItem { label: "Save".to_string(), handler: menuSaveHandler }
					MenuItem { label: "Exit".to_string(), handler: exitHandler }
				}
			}
			
			show[&GameSystem::CodMortal].then(|| rsx! { MortalSheet {} })
			show[&GameSystem::CodChangeling2e].then(|| rsx! { ChangelingSheet {} })
			show[&GameSystem::CodMage2e].then(|| rsx! { MageSheet {} })
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

/// Event handler triggered by clicking the Open `MenuItem` in the File `Menu`.
/// 
/// Presents the user with am open `FileDialog` to choose which file to open. Updates
/// the value of `CurrentFilePath` and attempts to load the file as a character sheet.
fn menuFileOpenHandler(cx: &Scope<MenuItemProps>)
{
	let currentFilePath = use_read(&cx, CurrentFilePath);
	let setCurrentFilePath = use_set(&cx, CurrentFilePath);
	let setMenuState = use_set(&cx, MainMenuState);
	
	setMenuState(false);
	setCurrentFilePath(getFilePath(false, currentFilePath.clone()));
	loadSheet(cx);
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
		GameSystem::CodMage2e => saveSheet::<Mage>(cx),
		GameSystem::CodMortal => saveSheet::<CoreCharacter>(cx),
		GameSystem::CodVampire2e => saveSheet::<Vampire>(cx),
	}
}

/// Event handler triggered by clicking one of the `MenuItem`s in the New submenu.
/// 
/// Resets the global state, the `Window` title, and updates the value of `CurrentGameSystem`.
fn newSheetHandler(cx: &Scope<MenuItemProps>)
{
	let setCurrentGameSystem = use_set(cx, CurrentGameSystem);
	let setMenuState = use_set(&cx, MainMenuState);
	
	if let Some((gs, _)) = GameSystem::asMap().iter().filter(|(_, l)| *l.clone() == cx.props.label).next()
	{
		resetGlobalState(cx);
		updateWindowTitle(cx, *gs);
		setCurrentGameSystem(*gs);
		setMenuState(false);
		
		if gs == &GameSystem::CodMortal
		{
			let mut mortal = CoreCharacter::mortal();
			mortal.push(cx);
		}
	}
}

// -----

/// Deserialize a character sheet from a file and push it into the global state.
fn loadSheet(cx: &Scope<MenuItemProps>)
{
	let currentFilePath = use_read(&cx, CurrentFilePath);
	let setCurrentFilePath = use_set(&cx, CurrentFilePath);
	if let None = currentFilePath
	{
		setCurrentFilePath(Some(getUserDocumentsDir()));
	}
	
	let setCurrentGameSystem = use_set(&cx, CurrentGameSystem);
	if let Some(pathStr) = currentFilePath
	{
		match loadFromFile::<SaveData>(&Path::new(pathStr))
		{
			Ok(data) =>
			{
				let saveData = data;
				setCurrentGameSystem(saveData.game);
				updateWindowTitle(cx, saveData.game);
				
				match saveData.game
				{
					GameSystem::CodChangeling2e => pushLoadedSheet::<Changeling>(cx, deserializeSheet::<Changeling>(saveData.sheet)),
					GameSystem::CodMage2e => pushLoadedSheet::<Mage>(cx, deserializeSheet::<Mage>(saveData.sheet)),
					GameSystem::CodMortal => pushLoadedSheet::<CoreCharacter>(cx, deserializeSheet::<CoreCharacter>(saveData.sheet)),
					GameSystem::CodVampire2e => pushLoadedSheet::<Vampire>(cx, deserializeSheet::<Vampire>(saveData.sheet)),
				}
			},
			Err(e) => println!("Failed to loadFromFile: {:?}", e.to_string())
		}
	}
}

/// Generic method to push a StatefulTemplate into the global state.
fn pushLoadedSheet<T: StatefulTemplate>(cx: &Scope<MenuItemProps>, deserialized: Result<T, String>)
{
	if let Ok(mut sheet) = deserialized
	{
		sheet.push(&cx);
	}
}

/// Pull the current character sheet from the global state and serialize it into a file.
fn saveSheet<T: Default + DeserializeOwned + Serialize + StatefulTemplate>(cx: &Scope<MenuItemProps>)
{
	let currentFilePath = use_read(&cx, CurrentFilePath);
	let currentGameSystem = use_read(&cx, CurrentGameSystem);
	if let Some(pathStr) = currentFilePath
	{
		let mut sheet = T::default();
		sheet.pull(&cx);
		
		match serializeSheet::<T>(sheet)
		{
			Ok(sheetJson) =>
			{
				let saveData = SaveData::new(*currentGameSystem, sheetJson);
				match saveToFile(&Path::new(pathStr), &saveData)
				{
					Ok(json) => println!("Saved to json file! {}", json),
					Err(e) => println!("Error saving to file: {}", e)
				}
			},
			Err(e) => println!("Error saving to file: {}", e)
		}
	}
}

fn updateWindowTitle<T>(cx: &Scope<T>, gameSystem: GameSystem)
{
	let window = use_window(cx);
	if let Some((_, title)) = GameSystem::asMap().iter().filter(|(gs, _)| *gs == &gameSystem).next()
	{
		window.set_title(format!("{} - {}", WindowTitle, title).as_ref());
	}
}
