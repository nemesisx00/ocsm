mod implementation;

use gtk4::glib::{self, Object, RustClosure, SignalHandlerId};
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
	
	pub fn connectClosure_dexterity(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().connectClosure_dexterity(signalName, after, closure);
	}
	
	pub fn connectClosure_stamina(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().connectClosure_stamina(signalName, after, closure);
	}
	
	pub fn connectClosure_strength(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().connectClosure_strength(signalName, after, closure);
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
