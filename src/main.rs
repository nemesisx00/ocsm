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

/// The base application title applied to the `Window`
pub const WindowTitle: &str = "Open Character Sheet Manager v0.3.0";

/// The entry point for the application
fn main()
{
    launch_cfg(App, |cfg|
	{
		cfg
			//.with_disable_context_menu(true)
			.with_window(|w|
			{
				w.with_title(format!("{} - {}", WindowTitle, "Chronicles of Darkness"))
					//.with_menu(generateMainMenu())
					.with_resizable(true)
					.with_inner_size(LogicalSize::new(900, 800))
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
	
	/*
	let mut gameSystems = MenuBar::new();
	gameSystems.add_item(MenuItemAttributes::new("Chronicles of Darkness Core"));
	gameSystems.add_item(MenuItemAttributes::new("Changeling: The Lost 2e"));
	gameSystems.add_item(MenuItemAttributes::new("Mage: The Awakening 2e"));
	gameSystems.add_item(MenuItemAttributes::new("Vampire: The Requiem 2e"));
	gameSystems.add_item(MenuItemAttributes::new("Dungeons & Dragons 5th Edition"));
	gameSystems.add_item(MenuItemAttributes::new("Vampire: The Masquerade V5"));
	*/
	
	let helpMenu = MenuBar::new();
	//helpMenu.add_native_item(MenuItem::About("".to_string(), AboutMetadata { version: "0.2.0".to_string(), license: "MIT".to_string(), ..Default::default() }));
	
	let mut mainMenu = MenuBar::new();
	mainMenu.add_submenu("File", true, fileMenu);
	//mainMenu.add_submenu("Game Systems", true, gameSystems);
	mainMenu.add_submenu("Help", true, helpMenu);
	
	return mainMenu;
}
