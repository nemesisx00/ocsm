mod implementation;

use gtk4::{Accessible, Actionable, Box, Buildable, ConstraintTarget, Widget};
use gtk4::glib::{self, Object, RustClosure, SignalHandlerId};
use gtk4::glib::object::ObjectExt;
use gtk4::glib::subclass::types::ObjectSubclassIsExt;

glib::wrapper!
{
	pub struct SkillsCofdSocial(ObjectSubclass<implementation::SkillsCofdSocial>)
		@extends Box, Widget,
		@implements Accessible, Actionable, Buildable, ConstraintTarget;
}

impl SkillsCofdSocial
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
	
	pub fn animalKen(&self) -> u32
	{
		return self.imp().animalKenTrack.value().one;
	}
	
	pub fn connectAnimalKen(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().animalKenTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectEmpathy(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().empathyTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectExpression(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().expressionTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectIntimidation(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().intimidationTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectPersuasion(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().persuasionTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectSocialize(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().socializeTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectStreetwise(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().streetwiseTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectSubterfuge(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().subterfugeTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn empathy(&self) -> u32
	{
		return self.imp().empathyTrack.value().one;
	}
	
	pub fn expression(&self) -> u32
	{
		return self.imp().expressionTrack.value().one;
	}
	
	pub fn intimidation(&self) -> u32
	{
		return self.imp().intimidationTrack.value().one;
	}
	
	pub fn persuasion(&self) -> u32
	{
		return self.imp().persuasionTrack.value().one;
	}
	
	pub fn setMaximum(&self, max: u32)
	{
		self.imp().setMaximum(max);
	}
	
	pub fn setRowLength(&self, length: u32)
	{
		self.imp().setRowLength(length);
	}
	
	pub fn socialize(&self) -> u32
	{
		return self.imp().socializeTrack.value().one;
	}
	
	pub fn streetwise(&self) -> u32
	{
		return self.imp().streetwiseTrack.value().one;
	}
	
	pub fn subterfuge(&self) -> u32
	{
		return self.imp().subterfugeTrack.value().one;
	}
}
