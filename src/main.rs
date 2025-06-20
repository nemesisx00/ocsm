mod ui;

use std::path::Path;
use gtk4::gdk::Display;
use gtk4::prelude::{ApplicationExt, ApplicationExtManual, GtkWindowExt};
use gtk4::{gio, Application, CssProvider};
use gtk4::glib::ExitCode;
use ui::window::OcsmWindow;

const AppId: &str = "io.github.nemesisx00.OCSM";

fn main() -> ExitCode
{
	gio::resources_register_include!("templates.gresource")
		.expect("Failed to register template resources.");
	
	let app = Application::builder()
		.application_id(AppId)
		.build();
	
	app.connect_startup(startup);
	app.connect_activate(generateUi);
	
	return app.run();
}

fn generateUi(app: &Application)
{
	let window = OcsmWindow::new(app);
	window.present();
}

fn loadAllCss()
{
	loadCss();
	widgets::loadCss();
	cofd::loadCss();
	ctl2e::loadCss();
	mta2e::loadCss();
	vtr2e::loadCss();
}

fn loadAllResources()
{
	widgets::loadResources();
	cofd::loadResources();
	ctl2e::loadResources();
	mta2e::loadResources();
	vtr2e::loadResources();
}

fn loadCss()
{
	let provider = CssProvider::new();
	let filePath = format!("{}/{}", env!("CARGO_MANIFEST_DIR"), "css/window.css");
	let p = Path::new(&filePath);
	provider.load_from_path(p);
	
	gtk4::style_context_add_provider_for_display(
		&Display::default().expect("Could not connect to a display"),
		&provider,
		gtk4::STYLE_PROVIDER_PRIORITY_APPLICATION
	);
}

fn startup(_: &Application)
{
	loadAllCss();
	loadAllResources();
}
