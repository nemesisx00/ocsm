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

pub const WindowTitle: &str = "Open Character Sheet Manager";

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
fn generateMainMenu() -> MenuBar
{
	let mut fileMenu = MenuBar::new();
	fileMenu.add_item(MenuItemAttributes::new("New"));
	fileMenu.add_item(MenuItemAttributes::new("Open"));
	fileMenu.add_item(MenuItemAttributes::new("Save"));
	fileMenu.add_item(MenuItemAttributes::new("Exit"));
	
	let mut gameSystems = MenuBar::new();
	gameSystems.add_item(MenuItemAttributes::new("Changeling: The Lost - Second Edition"));
	gameSystems.add_item(MenuItemAttributes::new("Mage: The Awakening - Second Edition"));
	gameSystems.add_item(MenuItemAttributes::new("Vampire: The Requiem - Second Edition"));
	gameSystems.add_item(MenuItemAttributes::new("Dungeons & Dragons 5th Edition"));
	gameSystems.add_item(MenuItemAttributes::new("Vampire: The Masquerade 5th edition"));
	
	let mut mainMenu = MenuBar::new();
	mainMenu.add_submenu("File", true, fileMenu);
	mainMenu.add_submenu("Game Systems", true, gameSystems);
	
	return mainMenu;
}
