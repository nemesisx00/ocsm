use cofd::sheet::SheetCofdMortal;
use gtk4::CompositeTemplate;
use glib::subclass::InitializingObject;
use gtk4::glib::{self};
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::glib::subclass::types::{ObjectSubclass, ObjectSubclassExt};
use gtk4::glib::types::StaticTypeExt;
use gtk4::prelude::GtkWindowExt;
use gtk4::subclass::prelude::ApplicationWindowImpl;
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetImpl};
use gtk4::subclass::window::WindowImpl;
use crate::ui::nav::LeftNav;

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/window.ui")]
pub struct OcsmWindow {}

impl ApplicationWindowImpl for OcsmWindow {}

#[glib::object_subclass]
impl ObjectSubclass for OcsmWindow
{
	// `NAME` needs to match `class` attribute of template
	const NAME: &'static str = "OcsmWindow";
	type Type = super::OcsmWindow;
	type ParentType = gtk4::ApplicationWindow;
	
	fn class_init(klass: &mut Self::Class)
	{
		LeftNav::ensure_type();
		SheetCofdMortal::ensure_type();
		klass.bind_template();
	}
	
	fn instance_init(obj: &InitializingObject<Self>)
	{
		obj.init_template();
	}
}

impl ObjectImpl for OcsmWindow
{
	fn constructed(&self)
	{
		self.parent_constructed();
		
		self.obj().set_default_size(640, 480);
	}
}

impl WidgetImpl for OcsmWindow {}
impl WindowImpl for OcsmWindow {}
