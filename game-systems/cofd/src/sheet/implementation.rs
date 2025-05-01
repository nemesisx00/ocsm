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
use crate::widgets::attributes::mental::AttributesCofdMental;
use crate::widgets::attributes::physical::AttributesCofdPhysical;
use crate::widgets::attributes::social::AttributesCofdSocial;
use crate::widgets::skills::mental::SkillsCofdMental;
use crate::widgets::skills::physical::SkillsCofdPhysical;
use crate::widgets::skills::social::SkillsCofdSocial;
use crate::widgets::skills::SkillsCofd;

#[derive(CompositeTemplate, Default)]
//#[properties(wrapper_type = super::SheetCofdMortal)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/sheet.ui")]
pub struct SheetCofdMortal {}

impl BoxImpl for SheetCofdMortal {}

//#[glib::derived_properties]
impl ObjectImpl for SheetCofdMortal
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
impl ObjectSubclass for SheetCofdMortal
{
	const NAME: &'static str = "SheetCofdMortal";
	type ParentType = Box;
	type Type = super::SheetCofdMortal;
	
	fn class_init(klass: &mut Self::Class)
	{
		AttributesCofdMental::ensure_type();
		AttributesCofdPhysical::ensure_type();
		AttributesCofdSocial::ensure_type();
		SkillsCofd::ensure_type();
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

impl WidgetImpl for SheetCofdMortal {}

impl SheetCofdMortal {}
