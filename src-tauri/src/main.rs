#![allow(non_snake_case,non_upper_case_globals,unused_variables)]
#![cfg_attr(
	all(not(debug_assertions), target_os = "windows"),
	windows_subsystem = "windows"
)]

use std::{
	fs::{
		OpenOptions,
		read_to_string
	}
};
use tauri::{
	api::dialog::{FileDialogBuilder},
	command,
	CustomMenuItem,
	Menu,
	Submenu,
	Window
};
use serde::{
	Deserialize,
	Serialize
};

#[derive(Clone, Serialize)]
struct ContextPayload
{
	context: String
}

#[derive(Serialize, Deserialize)]
struct SaveData
{
	context: String,
	sheetState: String
}

#[non_exhaustive]
struct SheetContexts;
impl SheetContexts {
	pub const ChangelingTheLost: &'static str = "ChangelingTheLost";
	pub const MageTheAwakening: &'static str = "MageTheAwakening";
	pub const VampireTheMasquerade: &'static str = "VampireTheMasquerade";
}

fn main()
{
	tauri::Builder::default()
		.menu(BuildMenu())
		.on_menu_event(|event| {
			match event.menu_item_id() {
				"exit" => {
					//Closing the last window exits the application
					event.window().close().unwrap();
				}
				"clear" => { ClearSheet(event.window())}
				"contextCtl" => { NewSheet(event.window().to_owned(), SheetContexts::ChangelingTheLost.into()); }
				"contextMta" => { NewSheet(event.window().to_owned(), SheetContexts::MageTheAwakening.into()); }
				"contextVtm" => { NewSheet(event.window().to_owned(), SheetContexts::VampireTheMasquerade.into()); }
				"load" => { LoadFromFile(event.window()); }
				"save" => { EmitSave(event.window()); }
				_ => {}
			}
		})
		.invoke_handler(tauri::generate_handler![NewSheet, SaveSheet])
		.run(tauri::generate_context!())
		.expect("error while running tauri application");
}

#[command]
fn NewSheet(window: Window, context: String)
{
	match window.emit("newSheet", ContextPayload { context: context }) {
		Err(e) => { println!("Error emitting `newSheet` event: {:#?}", e.to_string()); }
		Ok(result) => { println!("Succeeded in emitting `newSheet` event: {:#?}", result); }
	}
}

#[command]
fn SaveSheet(window: Window, context: String, state: String)
{
	println!("Received the SaveSheet event!");
	let data = SaveData { context: context.to_owned(), sheetState: state.to_owned() };
	SaveToFile(data);
}

fn BuildMenu() -> Menu
{
	let newMenu = Submenu::new("New", Menu::new()
					.add_item(CustomMenuItem::new("contextCtl".to_string(), "Changeling: The Lost"))
					.add_item(CustomMenuItem::new("contextMta".to_string(), "Mage: The Awakening"))
					.add_item(CustomMenuItem::new("contextVtm".to_string(), "Vampire: The Masquerade")));
	
	let fileMenu = Submenu::new("File", Menu::new()
					.add_submenu(newMenu)
					.add_item(CustomMenuItem::new("save".to_string(), "Save"))
					.add_item(CustomMenuItem::new("clear".to_string(), "Clear Sheet"))
					.add_item(CustomMenuItem::new("load".to_string(), "Load"))
					.add_item(CustomMenuItem::new("exit".to_string(), "Exit")));
	
	let menu = Menu::new()
				.add_submenu(fileMenu);
	
	return menu;
}

fn ClearSheet(window: &Window)
{
	match window.emit("clearSheet", "") {
		Err(e) => { println!("Error clearing sheet: {:#?}", e.to_string()); }
		Ok(result) => { println!("Succeeded in clearing sheet: {:#?}", result); }
	}
}

fn EmitSave(window: &Window)
{
	match window.emit("saveSheet", true) {
		Err(e) => { println!("Error emitting `saveSheet` event: {:#?}", e.to_string()); }
		Ok(result) => { println!("Succeeded in emitting `saveSheet` event: {:#?}", result); }
	}
}

fn LoadFromFile(window: &Window)
{
	match FileDialogBuilder::default()
		.add_filter("JSON", &["json"])
		.pick_file()
	{
		None => {
			println!("Either no file was selected or there was a problem with the file dialog.");
		}
		Some(p) => {
			match read_to_string(p) {
				Err(e) => { println!("Error loading file: {:#?}", e.to_string()); }
				Ok(data) => {
					println!("Succeeded in loading file: {:#?}", data);
					match window.emit("loadSheet", data) {
						Err(e) => { println!("Error emitting `loadSheet` event: {:#?}", e.to_string()); }
						Ok(result) => { println!("Succeeded in emitting `loadSheet` event: {:#?}", result); }
					}
				}
			}
		}
	}
}

fn SaveToFile(data: SaveData)
{
	match FileDialogBuilder::default()
		.add_filter("JSON", &["json"])
		.save_file()
	{
		None => {
			println!("Either no file was selected or there was a problem with the file dialog.");
		}
		Some(path) => {
			let file = OpenOptions::new()
						.create(true)
						.write(true)
						.truncate(true)
						.open(path)
						.unwrap();
			
			match serde_json::to_writer(&file, &data) {
				Err(e) => { println!("Error writing to file: {:#?}", e.to_string()); }
				Ok(result) => { println!("Succeeded in writing to file: {:#?}", result); }
			}
		}
	}
}
