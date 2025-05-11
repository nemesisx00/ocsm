use gtk4::{Box, CompositeTemplate};
use gtk4::glib::{self};
use gtk4::glib::subclass::InitializingObject;
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::glib::subclass::types::ObjectSubclass;
use gtk4::subclass::box_::BoxImpl;
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetImpl};

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/weapons.ui")]
pub struct WeaponsCofd {}

impl BoxImpl for WeaponsCofd {}

impl ObjectImpl for WeaponsCofd
{
	fn constructed(&self)
	{
		self.parent_constructed();
	}
}

#[glib::object_subclass]
impl ObjectSubclass for WeaponsCofd
{
	const NAME: &'static str = "WeaponsCofd";
	type ParentType = Box;
	type Type = super::WeaponsCofd;
	
	fn class_init(klass: &mut Self::Class)
	{
		klass.bind_template();
	}
	
	fn instance_init(obj: &InitializingObject<Self>)
	{
		obj.init_template();
	}
}

impl WidgetImpl for WeaponsCofd {}

impl WeaponsCofd {}
