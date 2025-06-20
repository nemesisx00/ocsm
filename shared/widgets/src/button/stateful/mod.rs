mod implementation;
pub mod mode;

use gtk4::glib::subclass::types::ObjectSubclassIsExt;
use gtk4::{Accessible, Actionable, Buildable, ConstraintTarget, Widget};
use gtk4::glib::{self, Object};
use implementation::{StatefulButton_Mode, StatefulButton_State};
pub use implementation::Signal_StateToggled;
pub use mode::StatefulMode;

glib::wrapper!
{
	pub struct StatefulButton(ObjectSubclass<implementation::StatefulButton>)
		@extends Widget,
		@implements Accessible, Actionable, Buildable, ConstraintTarget;
}

impl StatefulButton
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
	
	pub fn withMode(mode: u32) -> Self
	{
		return Object::builder()
			.property(StatefulButton_Mode, mode)
			.build();
	}
	
	pub fn withState(mode: u32, state: u32) -> Self
	{
		return Object::builder()
			.property(StatefulButton_Mode, mode)
			.property(StatefulButton_State, state)
			.build();
	}
	
	pub fn refresh(&self)
	{
		self.imp().updateImage();
	}
}
