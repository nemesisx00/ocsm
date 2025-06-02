use gtk4::glib::object::ObjectExt;
use gtk4::{Box, CompositeTemplate, SpinButton, TemplateChild};
use gtk4::glib::{self, closure_local};
use gtk4::glib::subclass::InitializingObject;
use gtk4::glib::subclass::types::ObjectSubclass;
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::glib::types::StaticTypeExt;
use gtk4::subclass::box_::BoxImpl;
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetClassExt, WidgetImpl};
use widgets::statefultrack::StatefulTrack;
use crate::widgets::advantages::CombatAdvantagesCofd;
use crate::widgets::attributes::mental::AttributesCofdMental;
use crate::widgets::attributes::physical::AttributesCofdPhysical;
use crate::widgets::attributes::social::AttributesCofdSocial;
use crate::widgets::equipment::EquipmentCofd;
use crate::widgets::experiences::ExperiencesCofd;
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
	combatAdvantages: TemplateChild<CombatAdvantagesCofd>,
	
	#[template_child]
	healthTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	integrityTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	sizeButton: TemplateChild<SpinButton>,
	
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
		
		self.integrityTrack.setValue(7.into());
		self.sizeButton.set_value(5.0);
		
		self.connectHandlers();
		
		self.updateCombatAdvantages();
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
		CombatAdvantagesCofd::ensure_type();
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
		
		self.attributesMental.connectWits(
			StatefulTrack::Signal_ValueUpdated,
			false,
			closure_local!(
				#[weak] me,
				move |_: StatefulTrack, _: u32, _: u32, _: u32| me.updateCombatAdvantages()
			)
		);
		
		self.attributesPhysical.connectDexterity(
			StatefulTrack::Signal_ValueUpdated,
			false,
			closure_local!(
				#[weak] me,
				move |_: StatefulTrack, _: u32, _: u32, _: u32| me.updateCombatAdvantages()
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
		
		self.attributesPhysical.connectStrength(
			StatefulTrack::Signal_ValueUpdated,
			false,
			closure_local!(
				#[weak] me,
				move |_: StatefulTrack, _: u32, _: u32, _: u32| me.updateCombatAdvantages()
			)
		);
		
		self.attributesSocial.connectComposure(
			StatefulTrack::Signal_ValueUpdated,
			false,
			closure_local!(
				#[weak] me,
				move |_: StatefulTrack, _: u32, _: u32, _: u32| {
					me.updateCombatAdvantages();
					me.updateWillpowerMaximum();
				}
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
		
		self.skillsPhysical.connectAthletics(
			StatefulTrack::Signal_ValueUpdated,
			false,
			closure_local!(
				#[weak] me,
				move |_: StatefulTrack, _: u32, _: u32, _: u32| me.updateCombatAdvantages()
			)
		);
	}
	
	fn updateCombatAdvantages(&self)
	{
		self.combatAdvantages.updateAdvantages(
			self.skillsPhysical.athletics(),
			self.attributesSocial.composure(),
			self.attributesPhysical.dexterity(),
			self.attributesPhysical.strength(),
			self.attributesMental.wits()
		);
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
