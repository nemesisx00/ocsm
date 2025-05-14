mod implementation;

use gtk4::{Accessible, Actionable, Box, Buildable, ConstraintTarget, Widget};
use gtk4::glib::{self, Object};

glib::wrapper!
{
	pub struct ExperiencesCofd(ObjectSubclass<implementation::ExperiencesCofd>)
		@extends Box, Widget,
		@implements Accessible, Actionable, Buildable, ConstraintTarget;
}

impl ExperiencesCofd
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
}
