use gtk4::{Box, CompositeTemplate, Entry, TemplateChild};
use gtk4::glib::{self, closure_local};
use gtk4::glib::subclass::InitializingObject;
use gtk4::glib::subclass::types::ObjectSubclass;
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::glib::types::StaticTypeExt;
use gtk4::prelude::EditableExt;
use gtk4::subclass::box_::BoxImpl;
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetClassExt, WidgetImpl};
use widgets::statefultrack::data::StateValue;
use widgets::statefultrack::StatefulTrack;
use crate::widgets::attributes::mental::AttributesCofdMental;
use crate::widgets::attributes::physical::AttributesCofdPhysical;
use crate::widgets::attributes::social::AttributesCofdSocial;
use crate::widgets::equipment::EquipmentCofd;
use crate::widgets::skills::SkillsCofd;
use crate::widgets::skills::mental::SkillsCofdMental;
use crate::widgets::skills::physical::SkillsCofdPhysical;
use crate::widgets::skills::social::SkillsCofdSocial;
use crate::widgets::weapons::WeaponsCofd;

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/sheetMortal.ui")]
pub struct SheetCofdMortal
{
	#[template_child]
	attributesMental: TemplateChild<AttributesCofdMental>,
	
	#[template_child]
	attributesPhysical: TemplateChild<AttributesCofdPhysical>,
	
	#[template_child]
	attributesSocial: TemplateChild<AttributesCofdSocial>,
	
	#[template_child]
	healthTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	integrityTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	sizeEntry: TemplateChild<Entry>,
	
	#[template_child]
	skillsMental: TemplateChild<SkillsCofdMental>,
	
	#[template_child]
	skillsPhysical: TemplateChild<SkillsCofdPhysical>,
	
	#[template_child]
	skillsSocial: TemplateChild<SkillsCofdSocial>,
	
	#[template_child]
	willpowerTrack: TemplateChild<StatefulTrack>,
}

impl BoxImpl for SheetCofdMortal {}

impl ObjectImpl for SheetCofdMortal
{
	fn constructed(&self)
	{
		self.parent_constructed();
		
		self.integrityTrack.setValue(StateValue { one: 7, ..Default::default() });
		self.sizeEntry.set_text("5");
		
		self.connectHandlers();
		
		self.updateHealthMaximum();
		self.updateWillpowerMaximum();
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
		EquipmentCofd::ensure_type();
		SkillsCofd::ensure_type();
		SkillsCofdMental::ensure_type();
		SkillsCofdPhysical::ensure_type();
		SkillsCofdSocial::ensure_type();
		StatefulTrack::ensure_type();
		WeaponsCofd::ensure_type();
		
		klass.bind_template();
	}
	
	fn instance_init(obj: &InitializingObject<Self>)
	{
		obj.init_template();
	}
}

impl WidgetImpl for SheetCofdMortal {}

impl SheetCofdMortal
{
	fn connectHandlers(&self)
	{
		let me = self;
		
		self.attributesMental.connectResolve(
			StatefulTrack::Signal_ValueUpdated,
			false,
			closure_local!(
				#[weak] me,
				move |_: StatefulTrack, _: u32, _: u32, _: u32| me.updateWillpowerMaximum()
			)
		);
		
		self.attributesPhysical.connectStamina(
			StatefulTrack::Signal_ValueUpdated,
			false,
			closure_local!(
				#[weak] me,
				move |_: StatefulTrack, _: u32, _: u32, _: u32| me.updateHealthMaximum()
			)
		);
		
		self.attributesSocial.connectComposure(
			StatefulTrack::Signal_ValueUpdated,
			false,
			closure_local!(
				#[weak] me,
				move |_: StatefulTrack, _: u32, _: u32, _: u32| me.updateWillpowerMaximum()
			)
		);
	}
	
	fn updateHealthMaximum(&self)
	{
		let size = match self.sizeEntry.text().parse::<u32>()
		{
			Ok(value) => value,
			Err(_) => 5,
		};
		
		self.healthTrack.set_maximum(size + self.attributesPhysical.stamina());
	}
	
	fn updateWillpowerMaximum(&self)
	{
		self.willpowerTrack.set_maximum(
			self.attributesMental.resolve()
			+ self.attributesSocial.composure()
		);
	}
}
