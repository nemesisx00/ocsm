use std::cell::RefCell;
use std::sync::OnceLock;
use gtk4::prelude::EditableExt;
use gtk4::{Box, CompositeTemplate, Entry, SpinButton, TemplateChild};
use gtk4::glib::{self, closure_local};
use gtk4::glib::object::ObjectExt;
use gtk4::glib::subclass::{InitializingObject, Signal};
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::glib::subclass::types::{ObjectSubclass, ObjectSubclassExt};
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
use cofd::widgets::weapons::WeaponsCofd;
use widgets::traits::{CharacterSheet, Signal_SheetUpdated};

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/ctl2e/sheet.ui")]
pub struct SheetCofdCtl2e
{
	#[template_child]
	attributesMental: TemplateChild<AttributesCofdMental>,
	
	#[template_child]
	attributesPhysical: TemplateChild<AttributesCofdPhysical>,
	
	#[template_child]
	attributesSocial: TemplateChild<AttributesCofdSocial>,
	
	#[template_child]
	characterNameEntry: TemplateChild<Entry>,
	
	#[template_child]
	clarityTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	glamourTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	healthTrack: TemplateChild<StatefulTrack>,
	
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
	wyrdTrack: TemplateChild<StatefulTrack>,
}

impl BoxImpl for SheetCofdCtl2e {}

impl CharacterSheet for SheetCofdCtl2e
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

impl ObjectImpl for SheetCofdCtl2e
{
	fn constructed(&self)
	{
		self.parent_constructed();
		
		self.sizeButton.set_value(5.0);
		self.wyrdTrack.setValue(StateValue { one: 1, ..Default::default() });
		
		let rowLength = 5;
		
		self.attributesMental.setRowLength(rowLength);
		self.attributesPhysical.setRowLength(rowLength);
		self.attributesSocial.setRowLength(rowLength);
		self.skillsMental.setRowLength(rowLength);
		self.skillsPhysical.setRowLength(rowLength);
		self.skillsSocial.setRowLength(rowLength);
		
		self.connectHandlers();
		
		self.handleWyrdChanged();
		self.updateClarityMaximum();
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
impl ObjectSubclass for SheetCofdCtl2e
{
	const NAME: &'static str = "SheetCofdCtl2e";
	type ParentType = Box;
	type Type = super::SheetCofdCtl2e;
	
	fn class_init(klass: &mut Self::Class)
	{
		AttributesCofdMental::ensure_type();
		AttributesCofdPhysical::ensure_type();
		AttributesCofdSocial::ensure_type();
		EquipmentCofd::ensure_type();
		ExperiencesCofd::ensure_type();
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

impl WidgetImpl for SheetCofdCtl2e {}

impl SheetCofdCtl2e
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
				move |_: StatefulTrack, _: u32, _: u32, _: u32| me.updateClarityMaximum()
			)
		);
		
		self.attributesPhysical.connectStamina(
			StatefulTrack::Signal_ValueUpdated,
			false,
			closure_local!(
				#[weak] me,
				move |_: StatefulTrack, _: u32, _: u32, _: u32| {
					me.handleWyrdChanged();
					me.updateHealthMaximum();
				}
			)
		);
		
		self.attributesSocial.connectComposure(
			StatefulTrack::Signal_ValueUpdated,
			false,
			closure_local!(
				#[weak] me,
				move |_: StatefulTrack, _: u32, _: u32, _: u32| {
					me.updateClarityMaximum();
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
		
		self.sizeButton.connect_closure(
			"value-changed",
			false,
			closure_local!(
				#[weak] me,
				move |_: SpinButton| me.updateHealthMaximum()
			)
		);
		
		self.wyrdTrack.connect_closure(
			StatefulTrack::Signal_ValueUpdated,
			false,
			closure_local!(
				#[weak] me,
				move |_: StatefulTrack, _: u32, _: u32, _: u32| me.handleWyrdChanged()
			)
		);
	}
	
	fn handleWyrdChanged(&self)
	{
		let value = self.wyrdTrack.get().value().one;
		
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
		
		let glamourMax = match value
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
		
		self.glamourTrack.set_maximum(glamourMax);
	}
	
	fn updateClarityMaximum(&self)
	{
		self.clarityTrack.set_maximum(
			self.attributesMental.wits()
			+ self.attributesSocial.composure()
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
