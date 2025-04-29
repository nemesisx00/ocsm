mod ui;

use gtk4::prelude::{ApplicationExt, ApplicationExtManual, GtkWindowExt};
use gtk4::{gio, Application};
use gtk4::glib::ExitCode;
use ui::window::OcsmWindow;

const AppId: &str = "io.github.nemesisx00.OCSM";

fn main() -> ExitCode
{
	gio::resources_register_include!("templates.gresource")
		.expect("Failed to register template resources.");
	
	widgets::loadResources();
	
	let app = Application::builder()
		.application_id(AppId)
		.build();
	
	app.connect_startup(loadAllCss);
	app.connect_activate(generateUi);
	
	return app.run();
}

fn generateUi(app: &Application)
{
	let window = OcsmWindow::new(app);
	window.present();
}

fn loadAllCss(_: &Application)
{
	widgets::loadCss();
}
