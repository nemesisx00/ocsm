mod implementation;

use gtk4::glib::subclass::types::ObjectSubclassIsExt;
use gtk4::{Accessible, Actionable, Box, Buildable, ConstraintTarget, Widget};
use gtk4::glib::{self, Object};
use widgets::traits::CharacterSheet;

glib::wrapper!
{
	pub struct SheetCofdMortal(ObjectSubclass<implementation::SheetCofdMortal>)
		@extends Box, Widget,
		@implements Accessible, Actionable, Buildable, ConstraintTarget;
}

impl CharacterSheet for SheetCofdMortal
{
	fn characterName(&self) -> String
	{
		return self.imp().characterName();
	}
	
	fn pageName(&self) -> String
	{
		return self.imp().pageName();
	}
	
	fn setPageName(&self, name: String)
	{
		return self.imp().setPageName(name);
	}
}

impl SheetCofdMortal
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
}
