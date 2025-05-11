mod implementation;

use gtk4::{Accessible, Actionable, Box, Buildable, ConstraintTarget, Widget};
use gtk4::glib::{self, Object};

glib::wrapper!
{
	pub struct WeaponsCofd(ObjectSubclass<implementation::WeaponsCofd>)
		@extends Box, Widget,
		@implements Accessible, Actionable, Buildable, ConstraintTarget;
}

impl WeaponsCofd
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
}
