use gtk4::glib::subclass::InitializingObject;
use gtk4::glib::types::StaticTypeExt;
use gtk4::prelude::WidgetExt;
use gtk4::subclass::box_::BoxImpl;
use gtk4::{Box, CompositeTemplate, TemplateChild};
use gtk4::glib::{self};
use gtk4::glib::subclass::types::{ObjectSubclass, ObjectSubclassExt};
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetClassExt, WidgetImpl};
use widgets::statefultrack::StatefulTrack;

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/skillsMental.ui")]
pub struct SkillsCofdMental
{
	#[template_child]
	pub academicsTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub computerTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub craftsTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub investigationTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub medicineTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub occultTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub politicsTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub scienceTrack: TemplateChild<StatefulTrack>,
}

impl BoxImpl for SkillsCofdMental {}

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

impl SkillsCofdMental
{
	pub fn setMaximum(&self, max: u32)
	{
		self.academicsTrack.set_maximum(max);
		self.computerTrack.set_maximum(max);
		self.craftsTrack.set_maximum(max);
		self.investigationTrack.set_maximum(max);
		self.medicineTrack.set_maximum(max);
		self.occultTrack.set_maximum(max);
		self.politicsTrack.set_maximum(max);
		self.scienceTrack.set_maximum(max);
	}
	
	pub fn setRowLength(&self, length: u32)
	{
		self.academicsTrack.set_rowLength(length);
		self.computerTrack.set_rowLength(length);
		self.craftsTrack.set_rowLength(length);
		self.investigationTrack.set_rowLength(length);
		self.medicineTrack.set_rowLength(length);
		self.occultTrack.set_rowLength(length);
		self.politicsTrack.set_rowLength(length);
		self.scienceTrack.set_rowLength(length);
	}
}
