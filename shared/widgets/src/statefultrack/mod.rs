pub mod data;
mod implementation;

use data::StateValue;
use gtk4::{Accessible, Buildable, ConstraintTarget, Grid, Orientable, Widget};
use gtk4::glib::{self, Object};
use gtk4::subclass::prelude::ObjectSubclassIsExt;

glib::wrapper!
{
	pub struct StatefulTrack(ObjectSubclass<implementation::StatefulTrack>)
		@extends Grid, Widget,
		@implements Accessible, Buildable, ConstraintTarget, Orientable;
}

impl StatefulTrack
{
	pub const Signal_ValueUpdated: &'static str = "valueUpdated";
	
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
	
	pub fn value(&self) -> StateValue
	{
		return self.imp().getValue();
	}
	
	pub fn setValue(&self, value: StateValue)
	{
		self.imp().setValue(value);
	}
}
