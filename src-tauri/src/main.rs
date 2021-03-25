#![allow(non_snake_case,unused_variables)]
#![cfg_attr(
	all(not(debug_assertions), target_os = "windows"),
	windows_subsystem = "windows"
)]

mod cmd;

use std::{
	fs::{read_to_string},
	path::{Path}
};

use tauri::api::{
	dialog::{
		Response,
		select
	}
};

fn main()
{
	tauri::AppBuilder::new()
		.invoke_handler(|_webview, arg| {
			use cmd::Cmd::*;
			match serde_json::from_str(arg)
			{
				Err(e) => {
					Err(e.to_string())
				}
				Ok(command) => {
					match command
					{
						ExitApp { } => { _webview.terminate() }
						LoadData { target } => doLoadData(target)
					}
					Ok(())
				}
			}
		})
		.build()
		.run();
}

fn doLoadData(target: String)
{
	let filter = Option::from("*.json");
	let p = Path::new(&target);
	let defaultPath = Option::from(p);
	
	match select(filter, defaultPath)
	{
		Err(e) => {
			println!("Error opening select dialog: {:#?}", e.to_string());
		}
		Ok(resp) => {
			match resp {
				Response::Cancel => {
					//Cancelled
					//Close the select dialog (assuming that it doesn't happen automatically)
				}
				Response::Okay(path) => {
					match read_to_string(&path)
					{
						Err(e) => {
							println!("Error loading file: {:#?}", e.to_string());
						}
						Ok(data) => {
							//pass the data to the frontend
							println!("Succeeded in loading file: {:#?}", data);
						}
					}
				}
				Response::OkayMultiple(paths) => {
					//Multiple paths
					//Probably not going to use this
					//Just show an error to the user and re-open the select dialog
				}
			}
		}
	}
}
