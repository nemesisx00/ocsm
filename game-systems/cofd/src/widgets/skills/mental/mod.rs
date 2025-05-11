mod implementation;

use gtk4::{Accessible, Actionable, Box, Buildable, ConstraintTarget, Widget};
use gtk4::glib::{self, Object, RustClosure, SignalHandlerId};
use gtk4::glib::object::ObjectExt;
use gtk4::glib::subclass::types::ObjectSubclassIsExt;

glib::wrapper!
{
	pub struct SkillsCofdMental(ObjectSubclass<implementation::SkillsCofdMental>)
		@extends Box, Widget,
		@implements Accessible, Actionable, Buildable, ConstraintTarget;
}

impl SkillsCofdMental
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
	
	pub fn academics(&self) -> u32
	{
		return self.imp().academicsTrack.value().one;
	}
	
	pub fn computer(&self) -> u32
	{
		return self.imp().computerTrack.value().one;
	}
	
	pub fn connectAcademics(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().academicsTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectComputer(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().computerTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectCrafts(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().craftsTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectInvestigation(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().investigationTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectMedicine(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().medicineTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectOccult(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().occultTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectPolitics(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().politicsTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectScience(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().scienceTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn crafts(&self) -> u32
	{
		return self.imp().craftsTrack.value().one;
	}
	
	pub fn investigation(&self) -> u32
	{
		return self.imp().investigationTrack.value().one;
	}
	
	pub fn medicine(&self) -> u32
	{
		return self.imp().medicineTrack.value().one;
	}
	
	pub fn occult(&self) -> u32
	{
		return self.imp().occultTrack.value().one;
	}
	
	pub fn politics(&self) -> u32
	{
		return self.imp().politicsTrack.value().one;
	}
	
	pub fn science(&self) -> u32
	{
		return self.imp().scienceTrack.value().one;
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
