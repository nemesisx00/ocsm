use gtk4::glib::subclass::InitializingObject;
use gtk4::glib::types::StaticTypeExt;
use gtk4::prelude::WidgetExt;
use gtk4::subclass::box_::BoxImpl;
use gtk4::{Box, CompositeTemplate};
use gtk4::glib::{self};
use gtk4::glib::subclass::types::{ObjectSubclass, ObjectSubclassExt};
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetImpl};
use widgets::statefultrack::StatefulTrack;

#[derive(CompositeTemplate, Default)]
//#[properties(wrapper_type = super::SkillsCofdMental)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/skillsMental.ui")]
pub struct SkillsCofdMental {}

impl BoxImpl for SkillsCofdMental {}

//#[glib::derived_properties]
impl ObjectImpl for SkillsCofdMental
{
	fn constructed(&self)
	{
		self.parent_constructed();
	}
	
	fn dispose(&self)
	{
		if let Some(child) = self.obj().first_child()
		{
			child.unparent();
		}
	}
}

#[glib::object_subclass]
impl ObjectSubclass for SkillsCofdMental
{
	const NAME: &'static str = "SkillsCofdMental";
	type ParentType = Box;
	type Type = super::SkillsCofdMental;
	
	fn class_init(klass: &mut Self::Class)
	{
		StatefulTrack::ensure_type();
		klass.bind_template();
	}
	
	fn instance_init(obj: &InitializingObject<Self>)
	{
		obj.init_template();
	}
}

impl WidgetImpl for SkillsCofdMental {}

impl SkillsCofdMental {}
