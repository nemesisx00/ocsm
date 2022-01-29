#![allow(non_snake_case,non_upper_case_globals,unused_variables)]
#![cfg_attr(
	all(not(debug_assertions), target_os = "windows"),
	windows_subsystem = "windows"
)]

mod rolls;

pub use crate::rolls::DieRoller;

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
	Window,
	WindowMenuEvent
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

#[derive(Clone, Serialize, Deserialize)]
struct SaveData
{
	context: String,
	sheetState: String
}

#[non_exhaustive]
struct SheetContexts;
impl SheetContexts {
	pub const DnD5e: &'static str = "DnD5e";
	pub const ChangelingTheLost: &'static str = "ChangelingTheLost";
	pub const MageTheAwakening: &'static str = "MageTheAwakening";
	pub const VampireTheMasquerade: &'static str = "VampireTheMasquerade";
}

fn main()
{
	tauri::Builder::default()
		.menu(BuildMenu())
		.on_menu_event(MainMenuEventHandler)
		.invoke_handler(tauri::generate_handler![NewSheet, RollDice, SaveSheet])
		.run(tauri::generate_context!())
		.expect("error while running tauri application");
}

#[command]
fn NewSheet(window: Window, context: String)
{
	match window.emit("newSheet", ContextPayload { context: context })
	{
		Err(e) => { println!("Error emitting `newSheet` event: {:#?}", e.to_string()); }
		
		Ok(result) => { println!("Succeeded in emitting `newSheet` event: {:#?}", result); }
	}
}

#[command]
fn RollDice(window: Window, sides: u64, quantity: u64)
{
	let roller = DieRoller::default();
	let roll = roller.rollMultiple(sides, quantity);
	
	match window.emit("rolledDice", serde_json::to_string(&roll).unwrap())
	{
		Err(e) => { println!("Error emitting `rolledDice` event: {:#?}", e.to_string()); }
		
		Ok(result) => { println!("Succeeded in emitting `rolledDice` event: {:#?}", result); }
	}
}

#[command]
fn SaveSheet(window: Window, context: String, state: String)
{
	println!("Received the SaveSheet event!");
	let data = Box::leak(Box::new(SaveData { context: context.to_owned(), sheetState: state.to_owned() })) as &'static SaveData;
	SaveToFile(&data);
}

fn BuildMenu() -> Menu
{
	let dndMenu = Submenu::new("Dungeons && Dragons", Menu::new()
					.add_item(CustomMenuItem::new("contextDnd5e".to_string(), "5th Edition"))
				);
	
	let wodMenu = Submenu::new("World of Darkness", Menu::new()
					.add_item(CustomMenuItem::new("contextCtl".to_string(), "Changeling: The Lost"))
					.add_item(CustomMenuItem::new("contextMta".to_string(), "Mage: The Awakening"))
					.add_item(CustomMenuItem::new("contextVtm".to_string(), "Vampire: The Masquerade"))
				);
	
	let newMenu = Submenu::new("New", Menu::new()
					.add_submenu(dndMenu)
					.add_submenu(wodMenu)
				);
	
	let fileMenu = Submenu::new("File", Menu::new()
					.add_submenu(newMenu)
					.add_item(CustomMenuItem::new("save".to_string(), "Save"))
					.add_item(CustomMenuItem::new("clear".to_string(), "Clear Sheet"))
					.add_item(CustomMenuItem::new("load".to_string(), "Load"))
					.add_item(CustomMenuItem::new("exit".to_string(), "Exit"))
				);
	
	let helpMenu = Submenu::new("Help", Menu::new()
					.add_item(CustomMenuItem::new("about".to_string(), "About"))
				);
	
	let menu = Menu::new()
				.add_submenu(fileMenu)
				.add_submenu(helpMenu);
	
	return menu;
}

fn CloseWindow(window: &Window)
{
	window.close().unwrap();
}

fn DisplayAbout(parent: Window)
{
	
}

fn EmitClearSheet(window: Window)
{
	match window.emit("clearSheet", "")
	{
		Err(e) => { println!("Error clearing sheet: {:#?}", e.to_string()); }
		
		Ok(result) => { println!("Succeeded in clearing sheet: {:#?}", result); }
	}
}

fn EmitSave(window: Window)
{
	match window.emit("saveSheet", true)
	{
		Err(e) => { println!("Error emitting `saveSheet` event: {:#?}", e.to_string()); }
		
		Ok(result) => { println!("Succeeded in emitting `saveSheet` event: {:#?}", result); }
	}
}

fn LoadFromFile(window: &'static Window)
{
	FileDialogBuilder::default()
		.add_filter("JSON", &["json"])
		.pick_file(move |p|
		{
			match p
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
		});
}

fn MainMenuEventHandler(event: WindowMenuEvent)
{
	let e = Box::leak(Box::new(event)) as &'static WindowMenuEvent;
	
	match e.menu_item_id() {
		"about" => { DisplayAbout(e.window().to_owned()); }
		"clear" => { EmitClearSheet(e.window().to_owned()); }
		"contextDnd5e" => { NewSheet(e.window().to_owned(), SheetContexts::DnD5e.into()); }
		"contextCtl" => { NewSheet(e.window().to_owned(), SheetContexts::ChangelingTheLost.into()); }
		"contextMta" => { NewSheet(e.window().to_owned(), SheetContexts::MageTheAwakening.into()); }
		"contextVtm" => { NewSheet(e.window().to_owned(), SheetContexts::VampireTheMasquerade.into()); }
		"exit" => { CloseWindow(e.window()); } //Closing the last window exits the application
		"load" => { LoadFromFile(e.window()); }
		"save" => { EmitSave(e.window().to_owned()); }
		_ => {}
	}
}

fn SaveToFile(data: &'static SaveData)
{
	FileDialogBuilder::default()
		.add_filter("JSON", &["json"])
		.save_file(move |p|
		{
			match p
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
		});
}
