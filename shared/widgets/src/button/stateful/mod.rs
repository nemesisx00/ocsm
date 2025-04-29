mod implementation;
pub mod mode;

use gtk4::glib::{self, Object};
use implementation::{StatefulButton_Mode, StatefulButton_State};
pub use implementation::Signal_StateToggled;
pub use mode::StatefulMode;

glib::wrapper!
{
	pub struct StatefulButton(ObjectSubclass<implementation::StatefulButton>)
		@extends gtk4::Widget,
		@implements gtk4::Accessible, gtk4::Actionable,
			gtk4::Buildable, gtk4::ConstraintTarget;
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
}
