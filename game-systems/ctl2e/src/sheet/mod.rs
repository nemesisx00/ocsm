mod implementation;

use gtk4::glib::subclass::types::ObjectSubclassIsExt;
use gtk4::{Accessible, Actionable, Box, Buildable, ConstraintTarget, Widget};
use gtk4::glib::{self, Object};
use widgets::traits::CharacterSheet;

glib::wrapper!
{
	pub struct SheetCofdCtl2e(ObjectSubclass<implementation::SheetCofdCtl2e>)
		@extends Box, Widget,
		@implements Accessible, Actionable, Buildable, ConstraintTarget;
}

impl CharacterSheet for SheetCofdCtl2e
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

impl SheetCofdCtl2e
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
}
