mod implementation;

use gtk4::{Accessible, Actionable, Box, Buildable, ConstraintTarget, Widget};
use gtk4::glib::{self, Object};

glib::wrapper!
{
	pub struct SheetCofdVtr2e(ObjectSubclass<implementation::SheetCofdVtr2e>)
		@extends Box, Widget,
		@implements Accessible, Actionable, Buildable, ConstraintTarget;
}

impl SheetCofdVtr2e
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
}
