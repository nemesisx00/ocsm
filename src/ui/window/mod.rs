mod implementation;

use glib::Object;
use gtk4::{gio, glib, Application};

glib::wrapper!
{
	pub struct OcsmWindow(ObjectSubclass<implementation::OcsmWindow>)
		@extends gtk4::ApplicationWindow, gtk4::Window, gtk4::Widget,
		@implements gio::ActionGroup, gio::ActionMap, gtk4::Accessible,
			gtk4::Buildable, gtk4::ConstraintTarget, gtk4::Native,
			gtk4::Root, gtk4::ShortcutManager;
}

impl OcsmWindow
{
	pub fn new(app: &Application) -> Self
	{
		return Object::builder()
			.property("application", app)
			.build();
	}
}
