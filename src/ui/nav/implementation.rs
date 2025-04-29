use glib::subclass::InitializingObject;
use gtk4::subclass::prelude::*;
use gtk4::{glib, CompositeTemplate};

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/leftNav.ui")]
pub struct LeftNav
{
}

impl BoxImpl for LeftNav {}

#[glib::object_subclass]
impl ObjectSubclass for LeftNav
{
	// `NAME` needs to match `class` attribute of template
	const NAME: &'static str = "LeftNav";
	type Type = super::LeftNav;
	type ParentType = gtk4::Box;
	
	fn class_init(klass: &mut Self::Class)
	{
		klass.bind_template();
	}
	
	fn instance_init(obj: &InitializingObject<Self>)
	{
		obj.init_template();
	}
}

impl ObjectImpl for LeftNav
{
	fn constructed(&self)
	{
		self.parent_constructed();
	}
}

impl WidgetImpl for LeftNav {}
