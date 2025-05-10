mod implementation;

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
	
	pub fn connectClosure_intelligence(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().connectClosure_intelligence(signalName, after, closure);
	}
	
	pub fn connectClosure_resolve(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().connectClosure_resolve(signalName, after, closure);
	}
	
	pub fn connectClosure_wits(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().connectClosure_wits(signalName, after, closure);
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
