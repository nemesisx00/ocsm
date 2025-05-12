mod implementation;

use gtk4::{Accessible, Actionable, Box, Buildable, ConstraintTarget, Widget};
use gtk4::glib::{self, Object};
use gtk4::glib::subclass::types::ObjectSubclassIsExt;

glib::wrapper!
{
	pub struct CombatAdvantagesCofd(ObjectSubclass<implementation::CombatAdvantagesCofd>)
		@extends Box, Widget,
		@implements Accessible, Actionable, Buildable, ConstraintTarget;
}

impl CombatAdvantagesCofd
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
	
	pub fn updateAdvantages(&self, athletics: u32, composure: u32, dexterity: u32, strength: u32, wits: u32)
	{
		self.imp().calculateDefense(athletics, dexterity, wits);
		self.imp().calculateInitiative(composure, dexterity);
		self.imp().calculateSpeed(dexterity, strength);
	}
}
