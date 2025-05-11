mod implementation;

use gtk4::{Accessible, Actionable, Box, Buildable, ConstraintTarget, Widget};
use gtk4::glib::{self, Object};

glib::wrapper!
{
	pub struct EquipmentCofd(ObjectSubclass<implementation::EquipmentCofd>)
		@extends Box, Widget,
		@implements Accessible, Actionable, Buildable, ConstraintTarget;
}

impl EquipmentCofd
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
}
