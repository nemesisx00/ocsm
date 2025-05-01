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
use super::mental::SkillsCofdMental;
use super::physical::SkillsCofdPhysical;
use super::social::SkillsCofdSocial;

#[derive(CompositeTemplate, Default)]
//#[properties(wrapper_type = super::SkillsCofd)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/skills.ui")]
pub struct SkillsCofd
{
}

impl BoxImpl for SkillsCofd {}

//#[glib::derived_properties]
impl ObjectImpl for SkillsCofd
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
impl ObjectSubclass for SkillsCofd
{
	const NAME: &'static str = "SkillsCofd";
	type ParentType = Box;
	type Type = super::SkillsCofd;
	
	fn class_init(klass: &mut Self::Class)
	{
		SkillsCofdMental::ensure_type();
		SkillsCofdPhysical::ensure_type();
		SkillsCofdSocial::ensure_type();
		StatefulTrack::ensure_type();
		klass.bind_template();
	}
	
	fn instance_init(obj: &InitializingObject<Self>)
	{
		obj.init_template();
	}
}

impl WidgetImpl for SkillsCofd {}

impl SkillsCofd {}
