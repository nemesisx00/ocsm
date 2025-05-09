use gtk4::{Box, CompositeTemplate, TemplateChild};
use gtk4::glib::{self};
use gtk4::glib::subclass::InitializingObject;
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::glib::subclass::types::{ObjectSubclass, ObjectSubclassExt};
use gtk4::glib::types::StaticTypeExt;
use gtk4::prelude::WidgetExt;
use gtk4::subclass::box_::BoxImpl;
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetClassExt, WidgetImpl};
use widgets::statefultrack::StatefulTrack;

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/attributesSocial.ui")]
pub struct AttributesCofdSocial
{
	#[template_child]
	composureTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	manipulationTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	presenceTrack: TemplateChild<StatefulTrack>,
}

impl BoxImpl for AttributesCofdSocial {}

impl ObjectImpl for AttributesCofdSocial
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
impl ObjectSubclass for AttributesCofdSocial
{
	const NAME: &'static str = "AttributesCofdSocial";
	type ParentType = Box;
	type Type = super::AttributesCofdSocial;
	
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

impl WidgetImpl for AttributesCofdSocial {}

impl AttributesCofdSocial
{
	pub fn setMaximum(&self, max: u32)
	{
		self.composureTrack.set_maximum(max);
		self.manipulationTrack.set_maximum(max);
		self.presenceTrack.set_maximum(max);
	}
	
	pub fn setRowLength(&self, length: u32)
	{
		self.composureTrack.set_rowLength(length);
		self.manipulationTrack.set_rowLength(length);
		self.presenceTrack.set_rowLength(length);
	}
}
