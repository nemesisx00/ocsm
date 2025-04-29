mod implementation;

use glib::Object;
use gtk4::glib;

glib::wrapper!
{
	pub struct LeftNav(ObjectSubclass<implementation::LeftNav>)
		@extends gtk4::Box, gtk4::Widget,
		@implements gtk4::Accessible, gtk4::Buildable, gtk4::ConstraintTarget;
}

impl LeftNav
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
}
