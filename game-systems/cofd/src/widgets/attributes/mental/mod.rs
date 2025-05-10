mod implementation;

use gtk4::glib::object::ObjectExt;
use gtk4::glib::{self, Object, RustClosure, SignalHandlerId};
use gtk4::glib::subclass::types::ObjectSubclassIsExt;

glib::wrapper!
{
	pub struct AttributesCofdMental(ObjectSubclass<implementation::AttributesCofdMental>)
		@extends gtk4::Box, gtk4::Widget,
		@implements gtk4::Accessible, gtk4::Actionable,
			gtk4::Buildable, gtk4::ConstraintTarget;
}

impl AttributesCofdMental
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
	
	pub fn connectIntelligence(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().intelligenceTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectResolve(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().resolveTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectWits(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().witsTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn intelligence(&self) -> u32
	{
		return self.imp().intelligenceTrack.value().one;
	}
	
	pub fn resolve(&self) -> u32
	{
		return self.imp().resolveTrack.value().one;
	}
	
	pub fn setMaximum(&self, max: u32)
	{
		self.imp().setMaximum(max);
	}
	
	pub fn setRowLength(&self, length: u32)
	{
		self.imp().setRowLength(length);
	}
	
	pub fn wits(&self) -> u32
	{
		return self.imp().witsTrack.value().one;
	}
}
