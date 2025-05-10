mod implementation;

use gtk4::glib::{self, Object, RustClosure, SignalHandlerId};
use gtk4::glib::subclass::types::ObjectSubclassIsExt;

glib::wrapper!
{
	pub struct AttributesCofdSocial(ObjectSubclass<implementation::AttributesCofdSocial>)
		@extends gtk4::Box, gtk4::Widget,
		@implements gtk4::Accessible, gtk4::Actionable,
			gtk4::Buildable, gtk4::ConstraintTarget;
}

impl AttributesCofdSocial
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
	
	pub fn composure(&self) -> u32
	{
		return self.imp().composureTrack.value().one;
	}
	
	pub fn connectClosure_composure(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().connectClosure_composure(signalName, after, closure);
	}
	
	pub fn connectClosure_manipulation(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().connectClosure_manipulation(signalName, after, closure);
	}
	
	pub fn connectClosure_presence(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().connectClosure_presence(signalName, after, closure);
	}
	
	pub fn manipulation(&self) -> u32
	{
		return self.imp().manipulationTrack.value().one;
	}
	
	pub fn presence(&self) -> u32
	{
		return self.imp().presenceTrack.value().one;
	}
	
	pub fn setMaximum(&self, max: u32)
	{
		self.imp().setMaximum(max);
	}
	
	pub fn setRowLength(&self, length: u32)
	{
		self.imp().setRowLength(length);
	}
}
