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
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/skillsPhysical.ui")]
pub struct SkillsCofdPhysical
{
	#[template_child]
	pub athleticsTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub brawlTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub driveTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub firearmsTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub larcenyTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub stealthTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub survivalTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub weaponryTrack: TemplateChild<StatefulTrack>,
}

impl BoxImpl for SkillsCofdPhysical {}

impl ObjectImpl for SkillsCofdPhysical
{
	fn constructed(&self)
	{
		self.parent_constructed();
	}
}

#[glib::object_subclass]
impl ObjectSubclass for SkillsCofdPhysical
{
	const NAME: &'static str = "SkillsCofdPhysical";
	type ParentType = Box;
	type Type = super::SkillsCofdPhysical;
	
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

impl WidgetImpl for SkillsCofdPhysical {}

impl SkillsCofdPhysical
{
	pub fn setMaximum(&self, max: u32)
	{
		self.athleticsTrack.set_maximum(max);
		self.brawlTrack.set_maximum(max);
		self.driveTrack.set_maximum(max);
		self.firearmsTrack.set_maximum(max);
		self.larcenyTrack.set_maximum(max);
		self.stealthTrack.set_maximum(max);
		self.survivalTrack.set_maximum(max);
		self.weaponryTrack.set_maximum(max);
	}
	
	pub fn setRowLength(&self, length: u32)
	{
		self.athleticsTrack.set_rowLength(length);
		self.brawlTrack.set_rowLength(length);
		self.driveTrack.set_rowLength(length);
		self.firearmsTrack.set_rowLength(length);
		self.larcenyTrack.set_rowLength(length);
		self.stealthTrack.set_rowLength(length);
		self.survivalTrack.set_rowLength(length);
		self.weaponryTrack.set_rowLength(length);
	}
}
