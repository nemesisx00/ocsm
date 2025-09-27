mod implementation;

use gtk4::{Accessible, Actionable, Box, Buildable, ConstraintTarget, Widget};
use gtk4::glib::{self, Object};

glib::wrapper!
{
	pub struct DotLabel(ObjectSubclass<implementation::DotLabel>)
		@extends Box, Widget,
		@implements Accessible, Actionable, Buildable, ConstraintTarget;
}

impl DotLabel
{
	pub const Signal_DotLabelChanged: &'static str = "dotLabelChanged";
	
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
	
	pub fn isEmpty(&self) -> bool
	{
		return self.label().is_empty() && self.value() < 1;
	}
}
