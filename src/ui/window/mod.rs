mod implementation;

use gtk4::{Accessible, Application, ApplicationWindow, Buildable, ConstraintTarget, Native, Root, ShortcutManager, Widget, Window};
use gtk4::gio::{ActionGroup, ActionMap};
use gtk4::glib::{self, Object};

glib::wrapper!
{
	pub struct OcsmWindow(ObjectSubclass<implementation::OcsmWindow>)
		@extends ApplicationWindow, Window, Widget,
		@implements ActionGroup, ActionMap, Accessible,
			Buildable, ConstraintTarget, Native,
			Root, ShortcutManager;
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
