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

use tauri::{
	api::{
		dialog::{
			Response,
			select
		}
	},
	event::emit
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
						LoadData { target } => { doLoadData(target, &mut _webview.as_mut()) }
						NewSheet {} => { emitNewSheet(&mut _webview.as_mut()) }
					}
					Ok(())
				}
			}
		})
		.build()
		.run();
}

fn doLoadData(target: String, _webview: &mut tauri::WebviewMut)
{
	let filter = Some("json");
	let p = Path::new(&target);
	let defaultPath = Some(p);
	
	//Right now it seems like all directory/file selector dialogs error on the first attempt.
	//See https://github.com/tauri-apps/tauri/issues/1054
	match select(filter, defaultPath)
	{
		Err(e) => {
			println!("Right now this always errors on the first attempt. See https://github.com/tauri-apps/tauri/issues/1054");
			println!("Error opening select dialog: {:#?}", e.to_string());
			
			match select(filter, defaultPath)
			{
				Err(e) => {
					println!("Error opening select dialog #2: {:#?}", e.to_string());
				}
				Ok(resp) => {
					emitLoadedData(resp, _webview);
				}
			}
		}
		Ok(resp) => {
			emitLoadedData(resp, _webview);
		}
	}
}

fn emitLoadedData(resp: Response, _webview: &mut tauri::WebviewMut)
{
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
					
					match emit(_webview, "loadSheet", Some(data))
					{
						Err(e) => {
							println!("Error emitting loadSheet event to frontend: {:#?}", e.to_string());
						}
						Ok(res) => {
							println!("Succeeded in emitting loadSheet event to frontend!");
						}
					}
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

fn emitNewSheet(_webview: &mut tauri::WebviewMut)
{
	match emit(_webview, "newSheet", Some(""))
	{
		Err(e) => {
			println!("Error emitting newSheet event to frontend: {:#?}", e.to_string());
		}
		Ok(res) => {
			println!("Succeeded in emitting newSheet event to frontend!");
		}
	}
}
