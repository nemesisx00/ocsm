mod implementation;

use gtk4::{Accessible, Actionable, Buildable, ConstraintTarget, Grid, Orientable, Widget};
use gtk4::glib::{self, Object};

glib::wrapper!
{
	pub struct DotLabelList(ObjectSubclass<implementation::DotLabelList>)
		@extends Grid, Widget,
		@implements Accessible, Actionable, Buildable, ConstraintTarget, Orientable;
}

impl DotLabelList
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
}
