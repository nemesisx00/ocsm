mod implementation;

use gtk4::glib::{self, Object};

glib::wrapper!
{
	pub struct SkillsCofdSocial(ObjectSubclass<implementation::SkillsCofdSocial>)
		@extends gtk4::Box, gtk4::Widget,
		@implements gtk4::Accessible, gtk4::Actionable,
			gtk4::Buildable, gtk4::ConstraintTarget;
}

impl SkillsCofdSocial
{
	pub fn new() -> Self
	{
		return Object::builder()
			.build();
	}
}
