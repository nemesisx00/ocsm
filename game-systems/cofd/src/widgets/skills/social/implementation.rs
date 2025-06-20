use gtk4::glib::subclass::InitializingObject;
use gtk4::glib::types::StaticTypeExt;
use gtk4::subclass::box_::BoxImpl;
use gtk4::{Box, CompositeTemplate, TemplateChild};
use gtk4::glib::{self};
use gtk4::glib::subclass::types::ObjectSubclass;
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetClassExt, WidgetImpl};
use widgets::statefultrack::StatefulTrack;

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/skillsSocial.ui")]
pub struct SkillsCofdSocial
{
	#[template_child]
	pub animalKenTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub empathyTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub expressionTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub intimidationTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub persuasionTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub socializeTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub streetwiseTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub subterfugeTrack: TemplateChild<StatefulTrack>,
}

impl BoxImpl for SkillsCofdSocial {}

impl ObjectImpl for SkillsCofdSocial
{
	fn constructed(&self)
	{
		self.parent_constructed();
	}
}

#[glib::object_subclass]
impl ObjectSubclass for SkillsCofdSocial
{
	const NAME: &'static str = "SkillsCofdSocial";
	type ParentType = Box;
	type Type = super::SkillsCofdSocial;
	
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

impl WidgetImpl for SkillsCofdSocial {}

impl SkillsCofdSocial
{
	pub fn setMaximum(&self, max: u32)
	{
		self.animalKenTrack.set_maximum(max);
		self.empathyTrack.set_maximum(max);
		self.expressionTrack.set_maximum(max);
		self.intimidationTrack.set_maximum(max);
		self.persuasionTrack.set_maximum(max);
		self.socializeTrack.set_maximum(max);
		self.streetwiseTrack.set_maximum(max);
		self.subterfugeTrack.set_maximum(max);
	}
	
	pub fn setRowLength(&self, length: u32)
	{
		self.animalKenTrack.set_rowLength(length);
		self.empathyTrack.set_rowLength(length);
		self.expressionTrack.set_rowLength(length);
		self.intimidationTrack.set_rowLength(length);
		self.persuasionTrack.set_rowLength(length);
		self.socializeTrack.set_rowLength(length);
		self.streetwiseTrack.set_rowLength(length);
		self.subterfugeTrack.set_rowLength(length);
	}
}
