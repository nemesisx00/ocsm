pub mod data;
mod implementation;

use data::StateValue;
use gtk4::glib::{self, Object};
use gtk4::subclass::prelude::ObjectSubclassIsExt;

glib::wrapper!
{
	pub struct StatefulTrack(ObjectSubclass<implementation::StatefulTrack>)
		@extends gtk4::Grid, gtk4::Widget,
		@implements gtk4::Accessible, gtk4::Buildable,
			gtk4::ConstraintTarget, gtk4::Orientable;
}

impl StatefulTrack
{
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
