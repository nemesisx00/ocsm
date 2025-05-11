mod implementation;

use gtk4::{Accessible, Actionable, Box, Buildable, ConstraintTarget, Widget};
use gtk4::glib::{self, Object};

glib::wrapper!
{
	pub struct SheetCofdMortal(ObjectSubclass<implementation::SheetCofdMortal>)
		@extends Box, Widget,
		@implements Accessible, Actionable, Buildable, ConstraintTarget;
}

impl SheetCofdMortal
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
}
