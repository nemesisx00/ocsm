mod implementation;
pub mod mental;
pub mod physical;
pub mod social;

use gtk4::{Accessible, Actionable, Box, Buildable, ConstraintTarget, Widget};
use gtk4::glib::{self, Object};

glib::wrapper!
{
	pub struct AttributesCofd(ObjectSubclass<implementation::AttributesCofd>)
		@extends Box, Widget,
		@implements Accessible, Actionable, Buildable, ConstraintTarget;
}

impl AttributesCofd
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
}
