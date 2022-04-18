#![allow(non_snake_case, non_upper_case_globals)]

pub(crate) mod cod;
pub(crate) mod core;
pub(crate) mod components;
pub(crate) mod dnd;

use dioxus::desktop::{
	tao::{
		dpi::LogicalSize,
		menu::{
			//AboutMetadata,
			MenuBar,
			MenuId,
			MenuItemAttributes,
		},
	},
	launch_cfg
};
use crate::components::core::app::App;

/// The string 
pub const AppVersion: &str = "0.3.0";
/// The base application title applied to the `Window`
pub const WindowTitle: &str = "Open Character Sheet Manager";

/// The entry point for the application
fn main()
{
    launch_cfg(App, |cfg|
	{
		cfg
			//.with_disable_context_menu(true)
			.with_window(|w|
			{
				w.with_title(format!("{} {}", WindowTitle, AppVersion))
					//.with_menu(generateMainMenu())
					.with_resizable(true)
					.with_inner_size(LogicalSize::new(950, 800))
			})
			//.with_event_handler(move |eventLoop, webView| {})
	});
}

#[allow(dead_code)]
/// Generates the main menu.
/// 
/// Applied to the `Window` in `main`.
fn generateMainMenu() -> MenuBar
{
	let mut fileMenu = MenuBar::new();
	fileMenu.add_item(MenuItemAttributes::new("New").with_id(MenuId(1)));
	fileMenu.add_item(MenuItemAttributes::new("Open").with_id(MenuId(2)));
	fileMenu.add_item(MenuItemAttributes::new("Save").with_id(MenuId(3)));
	fileMenu.add_item(MenuItemAttributes::new("Exit").with_id(MenuId(4)));
	
	let helpMenu = MenuBar::new();
	//helpMenu.add_native_item(MenuItem::About("".to_string(), AboutMetadata { version: "0.2.0".to_string(), license: "MIT".to_string(), ..Default::default() }));
	
	let mut mainMenu = MenuBar::new();
	mainMenu.add_submenu("File", true, fileMenu);
	//mainMenu.add_submenu("Game Systems", true, gameSystems);
	mainMenu.add_submenu("Help", true, helpMenu);
	
	return mainMenu;
}
