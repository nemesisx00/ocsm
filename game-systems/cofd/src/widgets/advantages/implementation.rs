use gtk4::prelude::WidgetExt;
use gtk4::{Box, CompositeTemplate, Label, TemplateChild};
use gtk4::glib::{self};
use gtk4::glib::subclass::InitializingObject;
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::glib::subclass::types::ObjectSubclass;
use gtk4::subclass::box_::BoxImpl;
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetClassExt, WidgetImpl};

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/combatAdvantages.ui")]
pub struct CombatAdvantagesCofd
{
	#[template_child]
	defense: TemplateChild<Label>,
	
	#[template_child]
	initiative: TemplateChild<Label>,
	
	#[template_child]
	speed: TemplateChild<Label>,
}

impl BoxImpl for CombatAdvantagesCofd {}

impl ObjectImpl for CombatAdvantagesCofd
{
	fn constructed(&self)
	{
		self.parent_constructed();
		
		self.defense.add_css_class("labelBorder");
		self.initiative.add_css_class("labelBorder");
		self.speed.add_css_class("labelBorder");
	}
}

#[glib::object_subclass]
impl ObjectSubclass for CombatAdvantagesCofd
{
	const NAME: &'static str = "CombatAdvantagesCofd";
	type ParentType = Box;
	type Type = super::CombatAdvantagesCofd;
	
	fn class_init(klass: &mut Self::Class)
	{
		klass.bind_template();
	}
	
	fn instance_init(obj: &InitializingObject<Self>)
	{
		obj.init_template();
	}
}

impl WidgetImpl for CombatAdvantagesCofd {}

impl CombatAdvantagesCofd
{
	pub fn calculateDefense(&self, athletics: u32, dexterity: u32, wits: u32)
	{
		let mut def = dexterity;
		
		if wits < def
		{
			def = wits;
		}
		
		def = def + athletics;
		
		self.defense.set_label(def.to_string().as_ref());
	}
	
	pub fn calculateInitiative(&self, composure: u32, dexterity: u32)
	{
		self.initiative.set_label((composure + dexterity).to_string().as_ref());
	}
	
	pub fn calculateSpeed(&self, dexterity: u32, strength: u32)
	{
		self.speed.set_label((dexterity + strength + 5).to_string().as_ref());
	}
}
