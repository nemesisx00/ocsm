mod implementation;

use gtk4::{Accessible, Actionable, Box, Buildable, ConstraintTarget, Widget};
use gtk4::glib::{self, Object};

glib::wrapper!
{
	pub struct SheetCofdMta2e(ObjectSubclass<implementation::SheetCofdMta2e>)
		@extends Box, Widget,
		@implements Accessible, Actionable, Buildable, ConstraintTarget;
}

impl SheetCofdMta2e
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
}
