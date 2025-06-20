use gtk4::{Box, CompositeTemplate, SpinButton, TemplateChild};
use gtk4::glib::{self, closure_local};
use gtk4::glib::object::ObjectExt;
use gtk4::glib::subclass::InitializingObject;
use gtk4::glib::subclass::types::ObjectSubclass;
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::glib::types::StaticTypeExt;
use gtk4::subclass::box_::BoxImpl;
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetClassExt, WidgetImpl};
use widgets::statefultrack::StatefulTrack;
use widgets::statefultrack::data::StateValue;
use cofd::widgets::attributes::mental::AttributesCofdMental;
use cofd::widgets::attributes::physical::AttributesCofdPhysical;
use cofd::widgets::attributes::social::AttributesCofdSocial;
use cofd::widgets::equipment::EquipmentCofd;
use cofd::widgets::experiences::ExperiencesCofd;
use cofd::widgets::skills::mental::SkillsCofdMental;
use cofd::widgets::skills::physical::SkillsCofdPhysical;
use cofd::widgets::skills::social::SkillsCofdSocial;
use cofd::widgets::skills::SkillsCofd;
use cofd::widgets::weapons::WeaponsCofd;

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/vtr2e/sheet.ui")]
pub struct SheetCofdVtr2e
{
	#[template_child]
	attributesMental: TemplateChild<AttributesCofdMental>,
	
	#[template_child]
	attributesPhysical: TemplateChild<AttributesCofdPhysical>,
	
	#[template_child]
	attributesSocial: TemplateChild<AttributesCofdSocial>,
	
	#[template_child]
	bloodPotencyTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	healthTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	humanityTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	sizeButton: TemplateChild<SpinButton>,
	
	#[template_child]
	skillsMental: TemplateChild<SkillsCofdMental>,
	
	#[template_child]
	skillsPhysical: TemplateChild<SkillsCofdPhysical>,
	
	#[template_child]
	skillsSocial: TemplateChild<SkillsCofdSocial>,
	
	#[template_child]
	vitaeTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	willpowerTrack: TemplateChild<StatefulTrack>,
}

impl BoxImpl for SheetCofdVtr2e {}

impl ObjectImpl for SheetCofdVtr2e
{
	fn constructed(&self)
	{
		self.parent_constructed();
		
		self.bloodPotencyTrack.setValue(StateValue { one: 1, ..Default::default() });
		self.humanityTrack.setValue(StateValue { one: 7, ..Default::default() });
		self.sizeButton.set_value(5.0);
		
		let rowLength = 5;
		
		self.attributesMental.setRowLength(rowLength);
		self.attributesPhysical.setRowLength(rowLength);
		self.attributesSocial.setRowLength(rowLength);
		self.skillsMental.setRowLength(rowLength);
		self.skillsPhysical.setRowLength(rowLength);
		self.skillsSocial.setRowLength(rowLength);
		
		self.connectHandlers();
		
		self.handleBloodPotencyChanged();
		self.updateHealthMaximum();
		self.updateWillpowerMaximum();
	}
}

#[glib::object_subclass]
impl ObjectSubclass for SheetCofdVtr2e
{
	const NAME: &'static str = "SheetCofdVtr2e";
	type ParentType = Box;
	type Type = super::SheetCofdVtr2e;
	
	fn class_init(klass: &mut Self::Class)
	{
		AttributesCofdMental::ensure_type();
		AttributesCofdPhysical::ensure_type();
		AttributesCofdSocial::ensure_type();
		EquipmentCofd::ensure_type();
		ExperiencesCofd::ensure_type();
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

impl WidgetImpl for SheetCofdVtr2e {}

impl SheetCofdVtr2e
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
					me.handleBloodPotencyChanged();
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
		
		self.bloodPotencyTrack.connect_closure(
			StatefulTrack::Signal_ValueUpdated,
			false,
			closure_local!(
				#[weak] me,
				move |_: StatefulTrack, _: u32, _: u32, _: u32| me.handleBloodPotencyChanged()
			)
		);
		
		self.sizeButton.connect_closure(
			"value-changed",
			false,
			closure_local!(
				#[weak] me,
				move |_: SpinButton| me.updateHealthMaximum()
			)
		);
	}
	
	fn handleBloodPotencyChanged(&self)
	{
		let value = self.bloodPotencyTrack.get().value().one;
		
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
		
		let stamina = self.attributesPhysical.stamina();
		
		let glamourMax = match value
		{
			1 => 10,
			2 => 11,
			3 => 12,
			4 => 13,
			5 => 15,
			6 => 20,
			7 => 25,
			8 => 30,
			9 => 50,
			10 => 75,
			_ => match stamina
			{
				0 => 1,
				_ => stamina,
			},
		};
		
		self.vitaeTrack.set_maximum(glamourMax);
	}
	
	fn updateHealthMaximum(&self)
	{
		self.healthTrack.set_maximum(
			self.sizeButton.value_as_int() as u32
			+ self.attributesPhysical.stamina()
		);
	}
	
	fn updateWillpowerMaximum(&self)
	{
		self.willpowerTrack.set_maximum(
			self.attributesMental.resolve()
			+ self.attributesSocial.composure()
		);
	}
}
