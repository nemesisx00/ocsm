#![allow(non_snake_case,non_upper_case_globals,unused_variables)]
#![cfg_attr(
	all(not(debug_assertions), target_os = "windows"),
	windows_subsystem = "windows"
)]

use std::{
	fs::{read_to_string}
};
use tauri::{
	api::dialog::{FileDialogBuilder},
	command,
	CustomMenuItem,
	Menu,
	Submenu,
	Window
};

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
				"load" => { LoadFromFile(event.window()); }
				"new" => { NewSheet(event.window()); }
				_ => {}
			}
		})
		.invoke_handler(tauri::generate_handler![SaveSheet])
		.run(tauri::generate_context!())
		.expect("error while running tauri application");
}

#[command]
fn SaveSheet()
{
	//Accept json payload and save to file
}

fn BuildMenu() -> Menu
{
	let fileMenu = Submenu::new("File", Menu::new()
					.add_item(CustomMenuItem::new("new".to_string(), "New"))
					.add_item(CustomMenuItem::new("load".to_string(), "Load"))
					.add_item(CustomMenuItem::new("exit".to_string(), "Exit")));
	
	let menu = Menu::new()
				.add_submenu(fileMenu);
	
	return menu;
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

fn NewSheet(window: &Window)
{
	match window.emit("newSheet", "{}") {
		Err(e) => { println!("Error emitting `newSheet` event: {:#?}", e.to_string()); }
		Ok(result) => { println!("Succeeded in emitting `newSheet` event: {:#?}", result); }
	}
}
