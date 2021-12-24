#![allow(non_snake_case,non_upper_case_globals,unused_variables)]
#![cfg_attr(
	all(not(debug_assertions), target_os = "windows"),
	windows_subsystem = "windows"
)]

use std::{
	fs::{
		read_to_string,
		write
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
	Serialize
};

#[derive(Clone, Serialize)]
struct ContextPayload {
	context: String
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
				"contextCtl" => { NewSheet(event.window(), "ChangelingTheLost".into()); }
				"contextMta" => { NewSheet(event.window(), "MageTheAwakening".into()); }
				"contextVtm" => { NewSheet(event.window(), "VampireTheMasquerade".into()); }
				"load" => { LoadFromFile(event.window()); }
				"save" => { EmitSave(event.window()); }
				_ => {}
			}
		})
		.invoke_handler(tauri::generate_handler![SaveSheet])
		.run(tauri::generate_context!())
		.expect("error while running tauri application");
}

#[command]
fn SaveSheet(window: Window, state: String)
{
	println!("Received the SaveSheet event!");
	SaveToFile(state);
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
	match window.emit("doSaveSheet", true) {
		Err(e) => { println!("Error emitting `doSaveSheet` event: {:#?}", e.to_string()); }
		Ok(result) => { println!("Succeeded in emitting `doSaveSheet` event: {:#?}", result); }
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

fn NewSheet(window: &Window, context: String)
{
	match window.emit("newSheet", ContextPayload { context: context }) {
		Err(e) => { println!("Error emitting `newSheet` event: {:#?}", e.to_string()); }
		Ok(result) => { println!("Succeeded in emitting `newSheet` event: {:#?}", result); }
	}
}

fn SaveToFile(state: String)
{
	match FileDialogBuilder::default()
		.add_filter("JSON", &["json"])
		.save_file()
	{
		None => {
			println!("Either no file was selected or there was a problem with the file dialog.");
		}
		Some(p) => {
			match write(p, state) {
				Err(e) => { println!("Error writing to file: {:#?}", e.to_string()); }
				Ok(result) => { println!("Succeeded in writing to file: {:#?}", result); }
			}
		}
	}
}
