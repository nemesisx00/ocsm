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
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/attributesMental.ui")]
pub struct AttributesCofdMental
{
	#[template_child]
	pub intelligenceTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub resolveTrack: TemplateChild<StatefulTrack>,
	
	#[template_child]
	pub witsTrack: TemplateChild<StatefulTrack>,
}

impl BoxImpl for AttributesCofdMental {}

impl ObjectImpl for AttributesCofdMental
{
	fn constructed(&self)
	{
		self.parent_constructed();
		
		let initialValue = StateValue { one: 1, ..Default::default() };
		self.intelligenceTrack.setValue(initialValue);
		self.resolveTrack.setValue(initialValue);
		self.witsTrack.setValue(initialValue);
	}
}

#[glib::object_subclass]
impl ObjectSubclass for AttributesCofdMental
{
	const NAME: &'static str = "AttributesCofdMental";
	type ParentType = Box;
	type Type = super::AttributesCofdMental;
	
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

impl WidgetImpl for AttributesCofdMental {}

impl AttributesCofdMental
{
	pub fn setMaximum(&self, max: u32)
	{
		self.intelligenceTrack.set_maximum(max);
		self.resolveTrack.set_maximum(max);
		self.witsTrack.set_maximum(max);
	}
	
	pub fn setRowLength(&self, length: u32)
	{
		self.intelligenceTrack.set_rowLength(length);
		self.resolveTrack.set_rowLength(length);
		self.witsTrack.set_rowLength(length);
	}
}
