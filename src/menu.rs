use anyhow::{anyhow, Result};
use freya::prelude::{dioxus_elements, fc_to_builder, rsx, spawn_forever, Element,
	GlobalSignal, Menu, MenuButton, Readable, SubMenu};
use serde_json::Value;
use crate::data::{AppState, CharacterSheet, SheetId};
use crate::gamesystems::{CofdSheet, GameSystem, Vtr2eSheet};
use crate::io::{chooseLoadFile, chooseSaveFile, loadFromFile, saveToFile};

pub fn MainMenu() -> Element
{
	return rsx!(
		Menu
		{
			onclose: move |_| AppState.write().menuOpen = false,
			
			SubMenu
			{
				menu: AddSheetMenu(),
				
				label { "Add" }
			}
			
			MenuButton
			{
				onpress: move |_| loadHandler(),
				label { "Open" }
			}
			
			MenuButton
			{
				onpress: move |_| saveHandler(),
				label { "Save" }
			}
		}
	);
}

pub fn loadHandler()
{
	spawn_forever(async move {
		if let Some(path) = chooseLoadFile().await
		{
			match loadFromFile(path)
			{
				Err(e) => println!("{:?}", e),
				Ok(json) => match loadJson(json)
				{
					Err(e) => println!("{:?}", e),
					Ok(_) => println!("Loaded sheet!"),
				}
			}
		}
	});
}

pub fn saveHandler()
{
	if let Some(id) = AppState().activeId
	{
		match getJson(id)
		{
			Err(e) => println!("{:?}", e),
			Ok(json) => doSave(json),
		}
	}
}

fn AddSheetMenu() -> Element
{
	return rsx!(
		MenuButton
		{
			onpress: move |_| addSheet(GameSystem::Cofd),
			label { "Chronicles of Darkness 2nd Edition" }
		}
		
		MenuButton
		{
			onpress: move |_| addSheet(GameSystem::Vtr2e),
			label { "Vampire: The Requiem 2nd Edition" }
		}
	);
}

fn addSheet(game: GameSystem)
{
	AppState.write().sheets.addSheet(game);
	AppState.write().activeId = match AppState().sheets.len()
	{
		1 => AppState().sheets.getLastSheetId(game),
		_ => None,
	};
}

fn doSave(json: String)
{
	spawn_forever(async move {
		if let Some(path) = chooseSaveFile().await
		{
			match saveToFile(json, path)
			{
				Err(e) => println!("Failed to save sheet: {:?}", e),
				Ok(()) => println!("Sheet saved successfully!")
			}
		}
	});
}

fn getJson(id: SheetId) -> Result<String>
{
	return match id.game
	{
		GameSystem::Cofd => match AppState().sheets.cofd.iter_mut()
			.find(|s| s.id == id)
		{
			None => Err(anyhow!("Failed to serialize sheet data for id: {:?}", id)),
			Some(s) => s.serialize(),
		},
		
		GameSystem::Vtr2e => match AppState().sheets.vtr2e.iter()
			.find(|s| s.id == id)
		{
			None => Err(anyhow!("Failed to serialize sheet data for id: {:?}", id)),
			Some(s) => s.save(),
			Some(s) => s.serialize(),
		},
		
		GameSystem::None => Err(anyhow!("No GameSystem: {:?}", id)),
	};
}

fn loadJson(json: String) -> Result<()>
{
	let value: Value = serde_json::from_str(json.as_str())?;
	let id: SheetId = serde_json::from_value(value["id"].clone())?;
	
	return match id.game
	{
		GameSystem::Cofd => {
			let mut sheet: CofdSheet = serde_json::from_value(value.clone())?;
			sheet.id.index = AppState().sheets.cofd.len() as u32;
			AppState.write().sheets.cofd.push(sheet);
			Ok(())
		},
		
		GameSystem::Vtr2e => {
			let mut sheet: Vtr2eSheet = serde_json::from_value(value.clone())?;
			sheet.id.index = AppState().sheets.vtr2e.len() as u32;
			AppState.write().sheets.vtr2e.push(sheet);
			Ok(())
		},
		
		GameSystem::None => Err(anyhow!("No GameSystem: {:?}", id)),
	};
}
