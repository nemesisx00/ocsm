mod implementation;

use glib::Object;
use gtk4::glib;
pub use implementation::Signal_NewSheet;

glib::wrapper!
{
	pub struct HomeScreen(ObjectSubclass<implementation::HomeScreen>)
		@extends gtk4::Box, gtk4::Widget,
		@implements gtk4::Accessible, gtk4::Buildable, gtk4::ConstraintTarget;
}

impl HomeScreen
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
}
