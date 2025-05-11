use gtk4::{Box, CompositeTemplate, Entry, TemplateChild};
use gtk4::glib::{self, closure_local};
use gtk4::glib::object::ObjectExt;
use gtk4::glib::subclass::InitializingObject;
use gtk4::glib::subclass::types::ObjectSubclass;
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::glib::types::StaticTypeExt;
use gtk4::prelude::EditableExt;
use gtk4::subclass::box_::BoxImpl;
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetClassExt, WidgetImpl};
use widgets::statefultrack::StatefulTrack;
use widgets::statefultrack::data::StateValue;
use cofd::widgets::attributes::mental::AttributesCofdMental;
use cofd::widgets::attributes::physical::AttributesCofdPhysical;
use cofd::widgets::attributes::social::AttributesCofdSocial;
use cofd::widgets::equipment::EquipmentCofd;
use cofd::widgets::skills::mental::SkillsCofdMental;
use cofd::widgets::skills::physical::SkillsCofdPhysical;
use cofd::widgets::skills::social::SkillsCofdSocial;
use cofd::widgets::skills::SkillsCofd;
use cofd::widgets::weapons::WeaponsCofd;

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/mta2e/sheet.ui")]
pub struct SheetCofdMta2e
{
	#[template_child]
	attributesMental: TemplateChild<AttributesCofdMental>,
	
	#[template_child]
	attributesPhysical: TemplateChild<AttributesCofdPhysical>,
	
	#[template_child]
	attributesSocial: TemplateChild<AttributesCofdSocial>,
	
	#[template_child]
	gnosisTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	healthTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	manaTrack: TemplateChild<StatefulTrack>,
	
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
	
	#[template_child]
	wisdomTrack: TemplateChild<StatefulTrack>,
}

impl BoxImpl for SheetCofdMta2e {}

impl ObjectImpl for SheetCofdMta2e
{
	fn constructed(&self)
	{
		self.parent_constructed();
		
		self.gnosisTrack.setValue(StateValue { one: 1, ..Default::default() });
		self.wisdomTrack.setValue(StateValue { one: 7, ..Default::default() });
		
		let rowLength = 5;
		
		self.attributesMental.setRowLength(rowLength);
		self.attributesPhysical.setRowLength(rowLength);
		self.attributesSocial.setRowLength(rowLength);
		self.skillsMental.setRowLength(rowLength);
		self.skillsPhysical.setRowLength(rowLength);
		self.skillsSocial.setRowLength(rowLength);
		
		self.connectHandlers();
		
		self.handleGnosisChanged();
		self.updateHealthMaximum();
		self.updateWillpowerMaximum();
	}
}

#[glib::object_subclass]
impl ObjectSubclass for SheetCofdMta2e
{
	const NAME: &'static str = "SheetCofdMta2e";
	type ParentType = Box;
	type Type = super::SheetCofdMta2e;
	
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

impl WidgetImpl for SheetCofdMta2e {}

impl SheetCofdMta2e
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
				move |_: StatefulTrack, _: u32, _: u32, _: u32| {
					me.handleGnosisChanged();
					me.updateHealthMaximum();
				}
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
		
		self.gnosisTrack.connect_closure(
			StatefulTrack::Signal_ValueUpdated,
			false,
			closure_local!(
				#[weak] me,
				move |_: StatefulTrack, _: u32, _: u32, _: u32| me.handleGnosisChanged()
			)
		);
	}
	
	fn handleGnosisChanged(&self)
	{
		let value = self.gnosisTrack.get().value().one;
		
		let max = match value
		{
			6 => 6,
			7 => 7,
			8 => 8,
			9 => 9,
			10 => 10,
			_ => 5,
		};
		
		self.attributesMental.setMaximum(max);
		self.attributesPhysical.setMaximum(max);
		self.attributesSocial.setMaximum(max);
		self.skillsMental.setMaximum(max);
		self.skillsPhysical.setMaximum(max);
		self.skillsSocial.setMaximum(max);
		
		let manaMax = match value
		{
			2 => 11,
			3 => 12,
			4 => 13,
			5 => 15,
			6 => 20,
			7 => 25,
			8 => 30,
			9 => 50,
			10 => 75,
			_ => 10,
		};
		
		self.manaTrack.set_maximum(manaMax);
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
