mod implementation;

use gtk4::glib::{self, Object, RustClosure, SignalHandlerId};
use gtk4::glib::object::ObjectExt;
use gtk4::glib::subclass::types::ObjectSubclassIsExt;

glib::wrapper!
{
	pub struct SkillsCofdPhysical(ObjectSubclass<implementation::SkillsCofdPhysical>)
		@extends gtk4::Box, gtk4::Widget,
		@implements gtk4::Accessible, gtk4::Actionable,
			gtk4::Buildable, gtk4::ConstraintTarget;
}

impl SkillsCofdPhysical
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
	
	pub fn athletics(&self) -> u32
	{
		return self.imp().athleticsTrack.value().one;
	}
	
	pub fn brawl(&self) -> u32
	{
		return self.imp().brawlTrack.value().one;
	}
	
	pub fn connectAthletics(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().athleticsTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectBrawl(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().brawlTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectDrive(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().driveTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectFirearms(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().firearmsTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectLarceny(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().larcenyTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectStealth(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().stealthTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectSurvival(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().survivalTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn connectWeaponry(&self, signalName: &str, after: bool, closure: RustClosure) -> SignalHandlerId
	{
		return self.imp().weaponryTrack.connect_closure(signalName, after, closure);
	}
	
	pub fn drive(&self) -> u32
	{
		return self.imp().driveTrack.value().one;
	}
	
	pub fn firearms(&self) -> u32
	{
		return self.imp().firearmsTrack.value().one;
	}
	
	pub fn larceny(&self) -> u32
	{
		return self.imp().larcenyTrack.value().one;
	}
	
	pub fn setMaximum(&self, max: u32)
	{
		self.imp().setMaximum(max);
	}
	
	pub fn setRowLength(&self, length: u32)
	{
		self.imp().setRowLength(length);
	}
	
	pub fn stealth(&self) -> u32
	{
		return self.imp().stealthTrack.value().one;
	}
	
	pub fn survival(&self) -> u32
	{
		return self.imp().survivalTrack.value().one;
	}
	
	pub fn weaponry(&self) -> u32
	{
		return self.imp().weaponryTrack.value().one;
	}
}
