use gtk4::{Box, CompositeTemplate};
use gtk4::glib::{self};
use gtk4::glib::subclass::InitializingObject;
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::glib::subclass::types::ObjectSubclass;
use gtk4::subclass::box_::BoxImpl;
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetImpl};

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/equipment.ui")]
pub struct EquipmentCofd {}

impl BoxImpl for EquipmentCofd {}

impl ObjectImpl for EquipmentCofd
{
	fn constructed(&self)
	{
		self.parent_constructed();
	}
}

#[glib::object_subclass]
impl ObjectSubclass for EquipmentCofd
{
	const NAME: &'static str = "EquipmentCofd";
	type ParentType = Box;
	type Type = super::EquipmentCofd;
	
	fn class_init(klass: &mut Self::Class)
	{
		klass.bind_template();
	}
	
	fn instance_init(obj: &InitializingObject<Self>)
	{
		obj.init_template();
	}
}

impl WidgetImpl for EquipmentCofd {}

impl EquipmentCofd {}
