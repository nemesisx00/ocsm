pub mod button;
pub mod statefultrack;

use std::path::Path;
use gtk4::{gio, CssProvider};
use gtk4::gdk::Display;

pub fn loadResources()
{
	gio::resources_register_include!("templates.gresource")
		.expect("Failed to register template resources.");
}

pub fn loadCss()
{
	let provider = CssProvider::new();
	let filePath = format!("{}/{}", env!("CARGO_MANIFEST_DIR"), "css/stateful.css");
	let p = Path::new(&filePath);
	provider.load_from_path(p);
	
	gtk4::style_context_add_provider_for_display(
		&Display::default().expect("Could not connect to a display"),
		&provider,
		gtk4::STYLE_PROVIDER_PRIORITY_APPLICATION
	);
}
