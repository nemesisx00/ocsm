pub mod sheet;
pub mod widgets;

use std::path::Path;
use gtk4::CssProvider;
use gtk4::gdk::Display;
use gtk4::gio;

pub const GameSystemId: &str = "cofd-mortal";

pub fn initializeResources()
{
	gio::resources_register_include!("templates.gresource")
		.expect("Failed to register template resources.");
}

pub fn initializeCss()
{
	let provider = CssProvider::new();
	let filePath = format!("{}/{}", env!("CARGO_MANIFEST_DIR"), "css/combatAdvantages.css");
	let p = Path::new(&filePath);
	provider.load_from_path(p);
	
	gtk4::style_context_add_provider_for_display(
		&Display::default().expect("Could not connect to a display"),
		&provider,
		gtk4::STYLE_PROVIDER_PRIORITY_APPLICATION
	);
}
