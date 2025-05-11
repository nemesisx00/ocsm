mod implementation;

use gtk4::{Accessible, Box, Buildable, ConstraintTarget, Widget};
use gtk4::glib::{self, Object};
pub use implementation::Signal_NewSheet;

glib::wrapper!
{
	pub struct HomeScreen(ObjectSubclass<implementation::HomeScreen>)
		@extends Box, Widget,
		@implements Accessible, Buildable, ConstraintTarget;
}

impl HomeScreen
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
}
