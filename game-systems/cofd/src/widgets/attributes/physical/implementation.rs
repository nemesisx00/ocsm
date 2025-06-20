use gtk4::{Box, CompositeTemplate, TemplateChild};
use gtk4::glib::{self};
use gtk4::glib::subclass::InitializingObject;
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::glib::subclass::types::ObjectSubclass;
use gtk4::glib::types::StaticTypeExt;
use gtk4::subclass::box_::BoxImpl;
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetClassExt, WidgetImpl};
use widgets::statefultrack::StatefulTrack;
use widgets::statefultrack::data::StateValue;

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/attributesPhysical.ui")]
pub struct AttributesCofdPhysical
{
	#[template_child]
	pub dexterityTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub staminaTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub strengthTrack: TemplateChild<StatefulTrack>,
}

impl BoxImpl for AttributesCofdPhysical {}

impl ObjectImpl for AttributesCofdPhysical
{
	fn constructed(&self)
	{
		self.parent_constructed();
		
		let initialValue = StateValue { one: 1, ..Default::default() };
		self.dexterityTrack.setValue(initialValue);
		self.staminaTrack.setValue(initialValue);
		self.strengthTrack.setValue(initialValue);
	}
}

#[glib::object_subclass]
impl ObjectSubclass for AttributesCofdPhysical
{
	const NAME: &'static str = "AttributesCofdPhysical";
	type ParentType = Box;
	type Type = super::AttributesCofdPhysical;
	
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

impl WidgetImpl for AttributesCofdPhysical {}

impl AttributesCofdPhysical
{
	pub fn setMaximum(&self, max: u32)
	{
		self.dexterityTrack.set_maximum(max);
		self.staminaTrack.set_maximum(max);
		self.strengthTrack.set_maximum(max);
	}
	
	pub fn setRowLength(&self, length: u32)
	{
		self.dexterityTrack.set_rowLength(length);
		self.staminaTrack.set_rowLength(length);
		self.strengthTrack.set_rowLength(length);
	}
}
