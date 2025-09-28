use std::cell::RefCell;
use std::sync::OnceLock;
use gtk4::prelude::EditableExt;
use gtk4::{Box, CompositeTemplate, Entry, SpinButton, TemplateChild};
use gtk4::glib::{self, closure_local};
use gtk4::glib::object::ObjectExt;
use gtk4::glib::subclass::{InitializingObject, Signal};
use gtk4::glib::subclass::types::{ObjectSubclass, ObjectSubclassExt};
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::glib::types::StaticTypeExt;
use gtk4::subclass::box_::BoxImpl;
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetClassExt, WidgetImpl};
use widgets::statefultrack::StatefulTrack;
use cofd::widgets::advantages::CombatAdvantagesCofd;
use cofd::widgets::attributes::mental::AttributesCofdMental;
use cofd::widgets::attributes::physical::AttributesCofdPhysical;
use cofd::widgets::attributes::social::AttributesCofdSocial;
use cofd::widgets::equipment::EquipmentCofd;
use cofd::widgets::experiences::ExperiencesCofd;
use cofd::widgets::list::dotLabelList::DotLabelList;
use cofd::widgets::skills::mental::SkillsCofdMental;
use cofd::widgets::skills::physical::SkillsCofdPhysical;
use cofd::widgets::skills::social::SkillsCofdSocial;
use cofd::widgets::skills::SkillsCofd;
use cofd::widgets::weapons::WeaponsCofd;
use widgets::traits::{CharacterSheet, Signal_SheetUpdated};

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/mta2e/sheet.ui")]
pub struct SheetCofdMta2e
{
	#[template_child]
	arcanaList: TemplateChild<DotLabelList>,
	
	#[template_child]
	arcaneExperiences: TemplateChild<ExperiencesCofd>,
	
	#[template_child]
	attributesMental: TemplateChild<AttributesCofdMental>,
	
	#[template_child]
	attributesPhysical: TemplateChild<AttributesCofdPhysical>,
	
	#[template_child]
	attributesSocial: TemplateChild<AttributesCofdSocial>,
	
	#[template_child]
	characterNameEntry: TemplateChild<Entry>,
	
	#[template_child]
	combatAdvantages: TemplateChild<CombatAdvantagesCofd>,
	
	#[template_child]
	gnosisTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	healthTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	manaTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	meritsList: TemplateChild<DotLabelList>,
	
	pageName: RefCell<String>,
	
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
	
	#[template_child]
	wisdomTrack: TemplateChild<StatefulTrack>,
}

impl BoxImpl for SheetCofdMta2e {}

impl CharacterSheet for SheetCofdMta2e
{
	fn characterName(&self) -> String
	{
		return self.characterNameEntry.text().into();
	}
	
	fn pageName(&self) -> String
	{
		return self.pageName.borrow().clone();
	}
	
	fn setPageName(&self, name: String)
	{
		*self.pageName.borrow_mut() = name;
	}
}

impl ObjectImpl for SheetCofdMta2e
{
	fn constructed(&self)
	{
		self.parent_constructed();
		
		self.arcaneExperiences.setLabel("Arcane Experiences".into());
		
		self.gnosisTrack.setValue(1.into());
		self.sizeButton.set_value(5.0);
		self.wisdomTrack.setValue(7.into());
		
		let rowLength = 5;
		
		self.attributesMental.setRowLength(rowLength);
		self.attributesPhysical.setRowLength(rowLength);
		self.attributesSocial.setRowLength(rowLength);
		self.skillsMental.setRowLength(rowLength);
		self.skillsPhysical.setRowLength(rowLength);
		self.skillsSocial.setRowLength(rowLength);
		
		self.connectHandlers();
		
		self.handleGnosisChanged();
		self.updateCombatAdvantages();
		self.updateHealthMaximum();
		self.updateWillpowerMaximum();
	}
	
	fn signals() -> &'static [Signal]
	{
		static SIGNALS: OnceLock<Vec<Signal>> = OnceLock::new();
		return SIGNALS.get_or_init(|| {
			vec![
				Signal::builder(Signal_SheetUpdated)
					.build()
			]
		});
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
		CombatAdvantagesCofd::ensure_type();
		DotLabelList::ensure_type();
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
				move |_: StatefulTrack, _: u32, _: u32, _: u32| {
					me.handleGnosisChanged();
					me.updateHealthMaximum();
				}
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
		
		self.characterNameEntry.connect_closure(
			"changed",
			false,
			closure_local!(
				#[weak] me,
				move |_: Entry| me.obj().emit_by_name::<()>(Signal_SheetUpdated, &[])
			)
		);
		
		self.gnosisTrack.connect_closure(
			StatefulTrack::Signal_ValueUpdated,
			false,
			closure_local!(
				#[weak] me,
				move |_: StatefulTrack, _: u32, _: u32, _: u32| {
					me.handleGnosisChanged();
					me.updateCombatAdvantages();
					me.updateHealthMaximum();
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
			self.sizeButton.value() as u32
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
