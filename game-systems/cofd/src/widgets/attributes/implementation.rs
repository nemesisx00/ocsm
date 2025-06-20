use gtk4::glib::subclass::InitializingObject;
use gtk4::glib::types::StaticTypeExt;
use gtk4::subclass::box_::BoxImpl;
use gtk4::{Box, CompositeTemplate};
use gtk4::glib::{self};
use gtk4::glib::subclass::types::ObjectSubclass;
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetImpl};
use widgets::statefultrack::StatefulTrack;
use super::mental::AttributesCofdMental;
use super::physical::AttributesCofdPhysical;
use super::social::AttributesCofdSocial;

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/attributes.ui")]
pub struct AttributesCofd {}

impl BoxImpl for AttributesCofd {}

impl ObjectImpl for AttributesCofd
{
	fn constructed(&self)
	{
		self.parent_constructed();
	}
}

#[glib::object_subclass]
impl ObjectSubclass for AttributesCofd
{
	const NAME: &'static str = "AttributesCofd";
	type ParentType = Box;
	type Type = super::AttributesCofd;
	
	fn class_init(klass: &mut Self::Class)
	{
		AttributesCofdMental::ensure_type();
		AttributesCofdPhysical::ensure_type();
		AttributesCofdSocial::ensure_type();
		StatefulTrack::ensure_type();
		klass.bind_template();
	}
	
	fn instance_init(obj: &InitializingObject<Self>)
	{
		obj.init_template();
	}
}

impl WidgetImpl for AttributesCofd {}

impl AttributesCofd {}
