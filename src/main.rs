#![allow(non_snake_case, non_upper_case_globals)]

pub(crate) mod app;
pub(crate) mod core;
pub(crate) mod cod;

use dioxus::desktop::{
	tao::{
		dpi::LogicalSize,
		menu::{
			MenuBar,
			MenuItemAttributes,
		},
	},
	launch_cfg
};
use crate::app::App;

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
				w.with_title(WindowTitle)
					//.with_menu(generateMainMenu())
					.with_resizable(true)
					.with_inner_size(LogicalSize::new(900.0, 800.0))
			})
	});
}

#[allow(dead_code)]
/// Generates the main menu.
/// 
/// Applied to the `Window` in `main`.
fn generateMainMenu() -> MenuBar
{
	let mut fileMenu = MenuBar::new();
	fileMenu.add_item(MenuItemAttributes::new("New"));
	fileMenu.add_item(MenuItemAttributes::new("Open"));
	fileMenu.add_item(MenuItemAttributes::new("Save"));
	fileMenu.add_item(MenuItemAttributes::new("Exit"));
	
	let mut gameSystems = MenuBar::new();
	gameSystems.add_item(MenuItemAttributes::new("Chronicles of Darkness Core"));
	gameSystems.add_item(MenuItemAttributes::new("Changeling: The Lost 2e"));
	gameSystems.add_item(MenuItemAttributes::new("Mage: The Awakening 2e"));
	gameSystems.add_item(MenuItemAttributes::new("Vampire: The Requiem 2e"));
	gameSystems.add_item(MenuItemAttributes::new("Dungeons & Dragons 5th Edition"));
	gameSystems.add_item(MenuItemAttributes::new("Vampire: The Masquerade V5"));
	
	let mut mainMenu = MenuBar::new();
	mainMenu.add_submenu("File", true, fileMenu);
	mainMenu.add_submenu("Game Systems", true, gameSystems);
	
	return mainMenu;
}
