mod implementation;

use gtk4::glib::{self, Object, RustClosure, SignalHandlerId};
use gtk4::glib::object::ObjectExt;
use gtk4::glib::subclass::types::ObjectSubclassIsExt;

glib::wrapper!
{
	pub struct AttributesCofdPhysical(ObjectSubclass<implementation::AttributesCofdPhysical>)
		@extends gtk4::Box, gtk4::Widget,
		@implements gtk4::Accessible, gtk4::Actionable,
			gtk4::Buildable, gtk4::ConstraintTarget;
}

impl AttributesCofdPhysical
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
	
	pub fn connectDexterity(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().dexterityTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectStamina(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().staminaTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectStrength(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().strengthTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn dexterity(&self) -> u32
	{
		return self.imp().dexterityTrack.value().one;
	}
	
	pub fn setMaximum(&self, max: u32)
	{
		self.imp().setMaximum(max);
	}
	
	pub fn setRowLength(&self, length: u32)
	{
		self.imp().setRowLength(length);
	}
	
	pub fn stamina(&self) -> u32
	{
		return self.imp().staminaTrack.value().one;
	}
	
	pub fn strength(&self) -> u32
	{
		return self.imp().strengthTrack.value().one;
	}
}
